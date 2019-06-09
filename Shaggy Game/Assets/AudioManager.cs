using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private AudioSource audioSource;
    public AudioClip music1;
    public AudioClip music2;

    private bool keepPlaying = true;

    private void Start() {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public IEnumerator playMusic1() {
        keepPlaying = true;
        while (keepPlaying) {
            audioSource.PlayOneShot(music1);
            yield return new WaitForSeconds(music1.length);
        }
    }

    public IEnumerator playMusic2() {
        keepPlaying = true;
        while (keepPlaying) {
            audioSource.PlayOneShot(music2);
            yield return new WaitForSeconds(music2.length);
        }
    }

    public IEnumerator FadeOut() {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / 1;
            yield return null;
        }
        keepPlaying = false;
        audioSource.Stop();
        audioSource.volume = startVolume;
        StopAllCoroutines();
    }

    public IEnumerator FadeOut(AudioSource audioSource, float time) {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / time;
            yield return null;
        }
        audioSource.Stop();
    }

    public IEnumerator FadeIn(AudioSource audioSource, float time) {
        audioSource.Play();
        audioSource.volume = 0;
        while (audioSource.volume < 0.04f) {
            audioSource.volume += 0.04f * Time.deltaTime / time;
            yield return null;
        }
    }
}
