using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSwap : MonoBehaviour
{
    [SerializeField]
    private int TargetTrack;
    private bool Triggered;

    private void Start() {
        Triggered = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(Triggered) {
            return;
        }

        Triggered = true;

        BackgroundMusicPlayer.Instance.Play(TargetTrack);
    }
}
