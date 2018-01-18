using UnityEngine;
using System.Collections;

public class SoundEffectEnemies : MonoBehaviour {

    public AudioClip[] sfx;

    static AudioClip[] _sfx;
    static AudioSource audioSource;

    void Awake() {
        _sfx = sfx;
        audioSource = GetComponent<AudioSource>();
    }

    public static void Play(int aux) {
        audioSource.clip = _sfx[aux];
        audioSource.pitch = Random.Range(0.9f,1.1f);
        audioSource.Play();
    }
}
