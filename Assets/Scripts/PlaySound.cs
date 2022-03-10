using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    private AudioSource Source;

    void Start() {
        Source = GetComponent<AudioSource>();
    }

    void Update() {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Source.PlayOneShot(Source.clip);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Source.PlayOneShot(Source.clip);
    }
}
