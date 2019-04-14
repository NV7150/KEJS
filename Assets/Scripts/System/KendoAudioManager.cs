using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KendoAudioManager : MonoBehaviour {
    [SerializeField] private KendoManager kendoMan;
    private AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start() {
        audioSource = GetComponent<AudioSource>();
        kendoMan.OnBack += bgmPlay;
        kendoMan.OnHit += hitPlay;
        kendoMan.OnIppon += ipponPlay;
        bgmPlay();
    }

    void bgmPlay() {
        playAudio(SelectAudios.Bgm);
    }

    void hitPlay(int player) {
        playAudio(SelectAudios.Hit);
    }

    void ipponPlay(int player) {
        playAudio(SelectAudios.Ippon);
    }

    public void playAudio(AudioClip clip) {
        audioSource.Stop();
        if (clip != null) {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
