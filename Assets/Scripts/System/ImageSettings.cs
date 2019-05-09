using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

enum ImageDuty{
    HIT,
    IPPON
}

public class ImageSettings : MonoBehaviour {
    [SerializeField] private FileForm hitForm;
    [SerializeField] private FileForm ipponForm;

    private string hitImageSaveName = "I_HIT";
    private string ipponImageSaveName = "I_IPPON";

    private void Awake() {
        load(hitImageSaveName,ImageDuty.HIT);
        load(ipponImageSaveName,ImageDuty.IPPON);
    }

    void load(string name,ImageDuty duty) {
        string path = SettingSaveManger.load(name);
        if(path == SettingSaveManger.NULL_NAME)
            return;
        StartCoroutine(loadImage(duty, path));
        switch (duty) {
                case ImageDuty.HIT:
                    hitForm.onSoundChoosed(path);
                    break;
                case ImageDuty.IPPON:
                    ipponForm.onSoundChoosed(path);
                    break;
        }
    }

    // Start is called before the first frame update
    void Start() {
        hitForm.OnSelected += choose;
        ipponForm.OnSelected += choose;
    }

    void hitChoosen(String url) {
    }

    void ipponChosen(String url) {
        StartCoroutine(loadImage(ImageDuty.IPPON,url));
    }

    void choose(string title,string url) {
        if (title == hitForm.Title) {
            StartCoroutine(loadImage(ImageDuty.HIT, url));
            SettingSaveManger.save(SelectImages.HIT_FILE_TAG,url);
            SettingSaveManger.saveTitle(SelectImages.HIT_FILE_TAG,title);
        }else if (title == ipponForm.Title) {
            StartCoroutine(loadImage(ImageDuty.IPPON, url));
            SettingSaveManger.save(SelectImages.IPPON_FILE_TAG,url);
            SettingSaveManger.saveTitle(SelectImages.IPPON_FILE_TAG,title);
        }
    }

    public void back() {
        SceneManager.LoadScene("Setting");
    }
    

    IEnumerator loadImage(ImageDuty duty,String url) {
        var request = UnityWebRequestTexture.GetTexture("file://" + url);
        yield return request.SendWebRequest();
        
        if (request.isNetworkError) {
            Debug.LogError("Erorr!");
            
            yield break;
        }

        var image = ((DownloadHandlerTexture) request.downloadHandler).texture;
        var sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), Vector2.zero);
        switch (duty) {
                case ImageDuty.HIT:
                    SelectImages.HitSprite = sprite;
                    Debug.Log(SelectImages.HitSprite);
                    break;
                case ImageDuty.IPPON:
                    SelectImages.IpponSprite = sprite;
                    break;
        }
    }
    
}
