using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour {

    AudioSource audio;
    public float PlayDelay;
    public float UnmuteDelay;
    public AudioSource MuteTarget;

    void Awake() {
        audio = GetComponent<AudioSource>();
        audio.Stop();
        audio.mute = true;
    }

    void FixedUpdate() {
        if (PlayDelay > 0) {
            PlayDelay -= Time.deltaTime;
            if (PlayDelay <= 0) {
                audio.Play();
            }
        }
        if (UnmuteDelay > 0) {
            UnmuteDelay -= Time.deltaTime;
            if (UnmuteDelay <= 0) {
                audio.mute = false;
                MuteTarget.mute = true;
            }
        }
    }
}