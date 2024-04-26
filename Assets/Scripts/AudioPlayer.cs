using System.Collections;
using System.Collections.Generic;
using Unity.Loading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class AudioPlayer: MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    public event LoadingStatusChangedDelegate LoadingStatusChanged; 
    public delegate void LoadingStatusChangedDelegate(LoadingStatus status);


    private LoadingStatus _loadingStatus;    
    private LoadingStatus loadingStatus {
        get{ return _loadingStatus; }
        set{
            _loadingStatus = value;
            LoadingStatusChanged?.Invoke(loadingStatus);
        }
    }

    void Start() {
        StartCoroutine(LoadFile("C:/Users/hawkv/Downloads/Tetris.mp3"));
    }

    IEnumerator LoadFile(string path) {
        loadingStatus = LoadingStatus.InProgress;

        if(!System.IO.File.Exists(path)) {
            loadingStatus = LoadingStatus.Failed;
            yield break;
        }

        using var uwr = UnityWebRequestMultimedia.GetAudioClip("file://" + path, AudioType.MPEG);

        DownloadHandlerAudioClip dlHandler = (DownloadHandlerAudioClip)uwr.downloadHandler;
        dlHandler.streamAudio = true;

        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError) {
            Debug.LogError(uwr.error);
            loadingStatus = LoadingStatus.Failed;
            yield break;
        }

        if (dlHandler.isDone && dlHandler.audioClip != null) {
            audioSource.clip = DownloadHandlerAudioClip.GetContent(uwr);
            loadingStatus = LoadingStatus.Completed;
        } else {
            loadingStatus = LoadingStatus.Failed;
        }
    }

    public bool IsPlaying() {
        return audioSource.isPlaying;
    }

    public void Play() {
        if (!IsPlaying()) {
            audioSource.Play();
        }
    }

    public void Stop() {
        audioSource.Stop();
    }

    public void Pause() {
        audioSource.Pause();
    }

    public void UnPause() {
        audioSource.UnPause();
    }

    public bool IsMuted() {
        return audioSource.mute;
    }

    public void Mute() {
        audioSource.mute = true;
    }
    
    public void UnMute() {
        audioSource.mute = false;
    }

    // Set new playing position in seconds
    public void Seek(float time) {
        audioSource.time = time;
    }

    // Set new playing position in range [0, 1] where 1 is end of track. Range out of bounds is clamped  
    public void SeekNormalized(float fraction) {
        fraction = Mathf.Clamp(fraction, 0, 1);

        audioSource.time = audioSource.clip.length * fraction;
    }

    public float Time() {
        return audioSource.time;
    }

    public float TimeNormalized() {
        return audioSource.time/audioSource.clip.length;
    }

    public void PlayFromStart() {
        Stop();
        Play();
    }
}
