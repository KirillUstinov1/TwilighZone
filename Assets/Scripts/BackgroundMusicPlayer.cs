using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicPlayer : MonoBehaviour {
    [System.Serializable]
    struct MusicTrack {
        [SerializeField] public AudioClip Melody;
        [SerializeField] public AudioClip Atomos;
    };

    [SerializeField] private AudioSource MelodySource;
    [SerializeField] private AudioSource AtmosSource;
    [SerializeField] private MusicTrack[] Tracks;

    public static BackgroundMusicPlayer Instance;

    private void Start() {
        Instance = this;
        DontDestroyOnLoad(Instance);

        GetComponent<MusicPlayer>();
    }

    public void Play(int track) {
        MelodySource.Stop();
        AtmosSource.Stop();

        var t = Tracks[track];

        MelodySource.clip = t.Melody;
        AtmosSource.clip = t.Atomos;

        MelodySource.Play();
        AtmosSource.Play();
    }
}
