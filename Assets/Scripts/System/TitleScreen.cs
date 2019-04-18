using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour {
	[SerializeField] private Text logText;
	[SerializeField] private Button startButton;
	[SerializeField] private Button connectButton;
	[SerializeField] private bool debug = false;

	private const String MES_UNCONNECTED = "Not-Connected";
	private const String MES_CONNECTED = "Connected";

	// Use this for initialization
	void Start () {
		if (!debug) {
			if (SerialReciver.INSTANCE.IsAllPortReady) {
				changeConnected();
			} else {
				changeNotConnected();
			}
		} else {
			changeConnected();
		}
	}
	
	/// <summary>
	/// ゲーム画面に移行します
	/// </summary>
	/// <exception cref="Exception">異常な状態の時</exception>
	public void start() {
//		if (!debug && !SerialReciver.INSTANCE.IsAllPortReady)
//			throw new Exception("Tried to start game though port isn't ready");
		
		SceneManager.LoadScene("GameScene");
	}
	
	/// <summary>
	/// シリアルポートに接続します
	/// </summary>
	/// <exception cref="Exception">異常な状態の時</exception>
	public void connect() {
		if (SerialReciver.INSTANCE.IsAllPortReady)
			throw new Exception("Tried to connect port though port ready");
		try {
			SerialReciver.INSTANCE.startAllPorts();
		} catch (PortNotFoundException) {
			logText.text = "Failed";
		}
	}
	
	/// <summary>
	/// 設定画面に移動します
	/// </summary>
	public void setting() {
		SceneManager.LoadScene("Setting");
	}
	
	/// <summary>
	/// 画面をシリアル接続状態に変更します
	/// </summary>
	private void changeNotConnected() {
		logText.text = MES_CONNECTED;
//		startButton.interactable = false;
//		connectButton.interactable = true;
	}
	
	/// <summary>
	/// 画面をシリアル未接続状態に変更します
	/// </summary>
	private void changeConnected() {
		logText.text = MES_UNCONNECTED;
//		connectButton.interactable = false;
//		startButton.interactable = true;
	}
}
