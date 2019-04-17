﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

/// <summary>
/// シリアルを取得します
/// 起動時、接続されているシリアルポートにメッセージを飛ばし、正当な返答があった場合、そこに繋ぎます
/// シーン間を繋ぐ必要があるので、シングルトンにしてどこからでも取得できるようにしてあります
/// </summary>
public class SerialReciver : MonoBehaviour {
	/// <summary>
	/// 対象とするマイコンポート
	/// </summary>
	[SerializeField] private List<MiconPort> ports;
	
	/// <summary>
	/// 全てのポートがつながったか
	/// </summary>
	private bool isAllPortReady = false;
	
	/// <summary>
	/// シングルトンのインスタンス
	/// </summary>
	private static SerialReciver instance;
	
	/// <summary>
	/// 唯一のインスタンスを取得します
	/// </summary>
	public static SerialReciver INSTANCE {
		get {
			if (instance != null) {
				return instance;
			} else {
				return null;
			}
		}
	}

	public bool IsAllPortReady {
		get { return isAllPortReady; }
	}
	
	private void Awake() {
		//シングルトン
		if (instance == null) {
			DontDestroyOnLoad(this.gameObject);
			instance = this;
		} else {
			Destroy(this.gameObject);
		}
	}

	private void Update() {
		foreach (var port in ports) {
			port.solveRecive();
		}
	}
	
	/// <summary>
	/// 登録してある全てのポートを起動させます
	/// </summary>
	/// <exception cref="PortNotFoundException">一つでもポートが見つからない時に投げられます</exception>
	public void startAllPorts() {
		foreach (var port in ports) {
			Debug.Log("into");
			port.start();
		}

		isAllPortReady = true;
	}
	
	/// <summary>
	/// マイコンポートを取得します
	/// </summary>
	/// <param name="id">取得するポートのID</param>
	/// <returns>IDに対応したポート</returns>
	public MiconPort getPort(int id) {
		foreach (var port in ports) {
			if (port.Id == id)
				return port;
		}

		return null;
	}
	
	/// <summary>
	/// マイコンポートを取得します
	/// </summary>
	/// <param name="localName">取得するポートのローカルネーム</param>
	/// <returns>localNameに対応したポート</returns>
	public MiconPort getPort(String localName) {
		foreach (var port in ports) {
			if (port.LocalName.Equals(localName))
				return port;
		}

		return null;
	}
	
	/// <summary>
	/// 読み込みスレッドを終了します
	/// </summary>
	public void endAllPorts() {
		foreach (var port in ports) {
			port.end();
		}
	}

	private void OnDestroy() {
		endAllPorts();
	}
}