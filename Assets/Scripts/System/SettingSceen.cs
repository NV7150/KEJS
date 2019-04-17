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

public class SettingSceen : MonoBehaviour {
	[SerializeField] private FileForm bgmForm;
	[SerializeField] private FileForm hitForm;
	[SerializeField] private FileForm ipponForm;
	
	// Use this for initialization
	void Start () {
		bgmForm.OnSelected += chooseBgm;
		hitForm.OnSelected += chooseHit;
		ipponForm.OnSelected += chooseIppon;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void chooseBgm(String url) {
		onSoundChoosed(url,AudioDuty.BGM);
	}

	void chooseHit(String url) {
		onSoundChoosed(url,AudioDuty.HIT);
	}

	void chooseIppon(String url) {
		onSoundChoosed(url,AudioDuty.IPPON);
	}

	public void back() {
		SceneManager.LoadScene("Title");
	}
	
	/// <summary>
	/// 選択が選ばれた時に呼ばれるデリゲードメソッド
	/// </summary>
	/// <param name="path">オーディオへの絶対パス</param>
	void onSoundChoosed(String path,AudioDuty duty) {
		StartCoroutine(get(path,duty));
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
