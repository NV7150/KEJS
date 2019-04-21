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
    
    // Start is called before the first frame update
    void Start() {
        hitForm.OnSelected += hitChoosen;
        ipponForm.OnSelected += ipponChosen;
    }

    void hitChoosen(String url) {
        StartCoroutine(loadImage(ImageDuty.HIT, url));
    }

    void ipponChosen(String url) {
        StartCoroutine(loadImage(ImageDuty.IPPON,url));
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
