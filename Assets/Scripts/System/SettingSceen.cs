using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using SimpleFileBrowser;
using UnityEngine.Networking;

public class SettingSceen : MonoBehaviour {
	private bool isWindowEnabled = false;
	[SerializeField]private AudioSource source;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.anyKeyDown)
			chooseSound();
	}

	public void chooseSound() {
		if (!isWindowEnabled) {
			isWindowEnabled = true;
			FileBrowser.ShowLoadDialog(onSoundChoosed, () => { }, title: "Choose Sound");
		}

//		StartCoroutine(FileBrowser.WaitForLoadDialog());
	}

	void onSoundChoosed(String path) {
		isWindowEnabled = false;
		StartCoroutine(get(path));
		Debug.Log(path);
	}

	IEnumerator get(String path) {
		var webReq = UnityWebRequestMultimedia.GetAudioClip("file://" + path,AudioType.MPEG);
		yield return webReq.SendWebRequest();
		
		var mp3Raw = ((DownloadHandlerAudioClip) webReq.downloadHandler).data;
		
//		source.clip = clip;
		source.Play();
	}
}
