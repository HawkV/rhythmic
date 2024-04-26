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
    private AudioPlayer audioPlayer;

    [SerializeField]
    private TrackController trackController;

    [SerializeField]
    private NotePlacer noteSpawner;

    private PlayerInput playerInput;

    private Note[] notes = new Note[] {
        new("A"),
        new("B"),
        new("C"),
        new("D"),
        new("E"),
    };

    private Dictionary<Note, bool> notePressed = new();

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        playerInput.actions["Fret"].started += (InputAction.CallbackContext ctx) => Debug.Log(NotePressToString());

        foreach (var note in notes) {
            playerInput.actions[note.Name].started += (InputAction.CallbackContext ctx) => RegisterNotePress(ctx, note);
            playerInput.actions[note.Name].canceled += (InputAction.CallbackContext ctx) => RegisterNotePress(ctx, note);

            notePressed[note] = false;          
        }
    }

    void OnEnable() {
        audioPlayer.LoadingStatusChanged += OnLoadingStatusChanged;   
    }

    void OnDisable() {
        audioPlayer.LoadingStatusChanged -= OnLoadingStatusChanged;
    }

    void OnLoadingStatusChanged(LoadingStatus status) {
        Debug.Log("bigly changed status to " + status);
    }

    void RegisterNotePress(InputAction.CallbackContext ctx, Note note) {
        notePressed[note] = ctx.phase switch {
            InputActionPhase.Started => true,
            InputActionPhase.Canceled => false,
            _ => false
        };

        noteSpawner.notes[note.Name].SetHighlighted(notePressed[note]);
    }

    string NotePressToString() {
        var s = "test: ";

        foreach(var note in notePressed) {
            s += note.Key + " " + note.Value + "; ";
        }

        return s;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
