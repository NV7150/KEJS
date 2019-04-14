using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
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
	private bool isWindowEnabled = false;
	private AudioDuty choosingAudioDuty = AudioDuty.BGM;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void chooseBgm() {
		chooseSound(AudioDuty.BGM);
	}

	public void chooseHit() {
		chooseSound(AudioDuty.HIT);
	}

	public void chooseIppon() {
		chooseSound(AudioDuty.IPPON);
	}

	public void back() {
		SceneManager.LoadScene("Title");
	}
	
	/// <summary>
	/// dutyに指定された役割にオーディオファイルを登録します
	/// </summary>
	/// <param name="duty">オーディオファイルの役割</param>
	public void chooseSound(AudioDuty duty) {
		if (!isWindowEnabled) {
			FileBrowser.SetFilters(true,new FileBrowser.Filter("WAV File",".wav"));
			FileBrowser.SetDefaultFilter(".wav");
			isWindowEnabled = true;
			FileBrowser.ShowLoadDialog(onSoundChoosed, () => { isWindowEnabled = false;}, title: "Choose Sound");
			choosingAudioDuty = duty;
		}

	}
	
	/// <summary>
	/// 選択が選ばれた時に呼ばれるデリゲードメソッド
	/// </summary>
	/// <param name="path">オーディオへの絶対パス</param>
	void onSoundChoosed(String path) {
		isWindowEnabled = false;
		StartCoroutine(get(path));
	}
	
	/// <summary>
	/// パスに指定されたものをchoosingAudioDutyに指定されたものに登録します
	/// </summary>
	/// <param name="path">オーディオへの絶対パス</param>
	/// <returns>コルーチン</returns>
	IEnumerator get(String path) {
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
