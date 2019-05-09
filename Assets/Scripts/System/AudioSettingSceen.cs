using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Policy;
using UnityEngine;
using SimpleFileBrowser;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using AudioSettings = UnityEngine.AudioSettings;

public enum AudioDuty {
	BGM,
	HIT,
	IPPON
}

public class AudioSettingSceen : MonoBehaviour {
	[SerializeField] private FileForm bgmForm;
	[SerializeField] private FileForm hitForm;
	[SerializeField] private FileForm ipponForm;

	private void Awake() {
		bgmForm.OnSelected += choose;
		hitForm.OnSelected += choose;
		ipponForm.OnSelected += choose;
		
		bgmForm.OnDeleted += delete;
		hitForm.OnDeleted += delete;
		ipponForm.OnDeleted += delete;
	}
	
	// Use this for initialization
	void Start () {
//		Debug.Log(bgmForm);
		
	}

	// Update is called once per frame
	void Update () {
	}

	void choose(string title,string url) {
		if (title == bgmForm.Title) {
			onSoundChoosed(url,AudioDuty.BGM,SelectAudios.BGM_FILE_TAG,title);
		}else if (title == hitForm.Title) {
			onSoundChoosed(url,AudioDuty.HIT,SelectAudios.HIT_FILE_TAG,title);
		}else if (title == ipponForm.Title) {
			onSoundChoosed(url,AudioDuty.IPPON,SelectAudios.IPPON_FILE_TAG,title);
		}
	}

	void delete(string title) {
		if (title == bgmForm.Title) {
			SelectAudios.Bgm = null;
			SettingSaveManger.delete(SelectAudios.BGM_FILE_TAG);
		}else if (title == hitForm.Title) {
			SelectAudios.Hit = null;
			SettingSaveManger.delete(SelectAudios.HIT_FILE_TAG);
		}else if (title == ipponForm.Title) {
			SelectAudios.Ippon = null;
			SettingSaveManger.delete(SelectAudios.IPPON_FILE_TAG);
		}
	}

	public void back() {
		SceneManager.LoadScene("Setting");
	}
	
	/// <summary>
	/// 選択が選ばれた時に呼ばれるデリゲードメソッド
	/// </summary>
	/// <param name="path">オーディオへの絶対パス</param>
	void onSoundChoosed(String path,AudioDuty duty,string tag,string title) {
		StartCoroutine(get(path,duty));
		SettingSaveManger.save(tag,path);
		SettingSaveManger.saveTitle(tag,title);
	}
	
	/// <summary>
	/// パスに指定されたものをchoosingAudioDutyに指定されたものに登録します
	/// </summary>
	/// <param name="path">オーディオへの絶対パス</param>
	/// <returns>コルーチン</returns>
	IEnumerator get(String path,AudioDuty choosingAudioDuty) {
		if (path.Substring(path.Length - 4).Equals(".wav")) {
			var webReq = UnityWebRequestMultimedia.GetAudioClip("file://" + path, AudioType.WAV);
			yield return webReq.SendWebRequest();

			if (webReq.isNetworkError) {
				Debug.Log("error!");
				yield break;
			}

			var clip = ((DownloadHandlerAudioClip) webReq.downloadHandler).audioClip;
			switch (choosingAudioDuty) {
					case AudioDuty.BGM:
						SelectAudios.Bgm = clip;
						break;
					case AudioDuty.HIT:
						SelectAudios.Hit = clip;
						break;
					case AudioDuty.IPPON:
						SelectAudios.Ippon = clip;
						break;
			}
		}
	}
}
