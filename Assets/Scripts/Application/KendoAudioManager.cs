using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KendoAudioManager : MonoBehaviour {
    [SerializeField] private KendoManager kendoMan;
    [SerializeField]private AudioLoader loader;
    private AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start() {
        audioSource = GetComponent<AudioSource>();
        kendoMan.OnBack += bgmPlay;
        kendoMan.OnHit += hitPlay;
        kendoMan.OnIppon += ipponPlay;
        bgmPlay();
        
    }

    private void OnDestroy() {
        kendoMan.OnBack -= bgmPlay;
        kendoMan.OnHit -= hitPlay;
        kendoMan.OnIppon -= ipponPlay;
    }

    void bgmPlay() {
        playAudio(loader.Bgm);
    }

    void hitPlay(int player) {
        playAudio(loader.Hit);
    }

    void ipponPlay(int player) {
        playAudio(loader.Ippon);
    }

    public void playAudio(AudioClip clip) {
        audioSource.Stop();
        if (clip != null) {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
