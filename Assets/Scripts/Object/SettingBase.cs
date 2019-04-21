using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingBase : MonoBehaviour{
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void toAudio() {
        SceneManager.LoadScene("AudioSetting");
    }

    public void toImg() {
        SceneManager.LoadScene("ImageSetting");
    }

    public void back() {
        SceneManager.LoadScene("Title");
    }
}
