using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour {
    public AudioClip[] Tracks;
    private AudioSource AudioSource;

    void Start() {
        AudioSource = GetComponent<AudioSource>();
    }

    public void Play(int id) {
        if(id < 0 || id >= Tracks.Length) {
            Debug.LogError("Cannot load audio track #" + id);
            return;
        }

        AudioSource.Stop();
        AudioSource.clip = Tracks[id];
        AudioSource.Play();
    }
}
