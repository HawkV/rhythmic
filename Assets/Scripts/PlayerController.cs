using System.Collections;
using System.Collections.Generic;
using Unity.Loading;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private NoteInfo noteInfo;

    private PlayerInput playerInput;

    private Chord pressedNotes = new();

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        playerInput.actions["Fret"].started += (InputAction.CallbackContext ctx) => 
            EventManager.TriggerEvent(EventManager.Event.ChordPlayed, pressedNotes);

        for (int i = 0; i < noteInfo.noteDescription.Length; i++) {
            var note = noteInfo.noteDescription[i];
            var noteActions = playerInput.actions[note.name];

            var capturedIndex = i;
            noteActions.started += (InputAction.CallbackContext ctx) => RegisterFretPress(ctx, capturedIndex);
            noteActions.canceled += (InputAction.CallbackContext ctx) => RegisterFretPress(ctx, capturedIndex   );
        }
    }

    void OnEnable() {
        EventManager.StartListening(EventManager.Event.LoadingStatusChanged, OnLoadingStatusChanged);   
    }

    void OnDisable() {
        EventManager.StopListening(EventManager.Event.LoadingStatusChanged, OnLoadingStatusChanged);
    }

    void OnLoadingStatusChanged(LoadingStatus status) {
        Debug.Log("bigly changed status to " + status);
    }

    void RegisterFretPress(InputAction.CallbackContext ctx, int noteID) {
        var notePressed = ctx.phase switch {
            InputActionPhase.Started => true,
            InputActionPhase.Canceled => false,
            _ => false
        };

        pressedNotes.SetNote(noteID, notePressed);

        EventManager.TriggerEvent(EventManager.Event.NotePressed, noteID, notePressed);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
