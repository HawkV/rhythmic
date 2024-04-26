using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    [SerializeField] 
    private AudioPlayer audioPlayer;

    Button playButton, stopButton, muteButton;
    Slider seekbar;

    void OnEnable() {
        var audioPlayer = GetComponent<UIDocument>().rootVisualElement;

        playButton = audioPlayer?.Q<Button>("play");
        stopButton = audioPlayer?.Q<Button>("stop");
        muteButton = audioPlayer?.Q<Button>("mute");
        seekbar = audioPlayer?.Q<Slider>("seekbar");
        
        playButton?.RegisterCallback<ClickEvent>(Play);
        stopButton?.RegisterCallback<ClickEvent>(Stop);
        muteButton?.RegisterCallback<ClickEvent>(Mute);

        seekbar?.RegisterCallback<ChangeEvent<float>>(Seek);
        seekbar?.RegisterCallback<MouseCaptureOutEvent>(UnPause);
        seekbar?.RegisterCallback<MouseCaptureEvent>(Pause);
    }

    void Play(ClickEvent evt) {
        audioPlayer.Play();
    }

    void Stop(ClickEvent evt) {
        audioPlayer.Stop();
        SyncSeekbar();
    }

    void Mute(ClickEvent evt) {
        if (audioPlayer.IsMuted()) {
            audioPlayer.UnMute();
        } else {
            audioPlayer.Mute();
        }
    }

    void UnPause(MouseCaptureOutEvent evt) {
        audioPlayer.UnPause();
    }

    void Pause(MouseCaptureEvent evt) {
        audioPlayer.Pause();
    }

    void Seek(ChangeEvent<float> evt) { 
        audioPlayer.SeekNormalized(evt.newValue/seekbar.highValue);
    }

    void SyncSeekbar() {
        seekbar.SetValueWithoutNotify(audioPlayer.TimeNormalized() * seekbar.highValue);
    }

    void Update() {
        if (audioPlayer.IsPlaying()) {
            SyncSeekbar();
        }
    }
}
