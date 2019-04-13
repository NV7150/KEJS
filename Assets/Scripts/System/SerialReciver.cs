using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEditor.VersionControl;
using UnityEngine;

/// <summary>
/// シリアルを取得します
/// 起動時、接続されているシリアルポートにメッセージを飛ばし、正当な返答があった場合、そこに繋ぎます
/// </summary>
public class SerialReciver : MonoBehaviour {
	/// <summary>
	/// シリアルの周期
	/// デフォは9600
	/// </summary>
	[SerializeField] private int frequency = 9600;
	
	/// <summary>
	/// シリアルの起動時にマイコンに送るメッセージ
	/// </summary>
	[SerializeField]private string serialBootMessage = "EKJS_BOOT";
	
	/// <summary>
	/// serialBootMessageをマイコンに送った時の返答
	/// </summary>
	[SerializeField] private string serialBootReply = "EKJS_REPLY";
	
	/// <summary>
	/// 返答を待つ時間（秒）
	/// </summary>
	[SerializeField] private float replyLimit = 3f;
	
	/// <summary>
	/// シリアルポート
	/// </summary>
	private SerialPort port;

	/// <summary>
	/// 正常なシリアルポートに接続されたかどうか
	/// </summary>
	private bool serialBootted = false;
	
	/// <summary>
	/// シリアル読みするか
	/// </summary>
	private bool serialRunning = false;
	
	/// <summary>
	/// メッセージを読み込んでキューに追加するメソッド
	/// </summary>
	private Thread readThread;
	
	/// <summary>
	/// シリアルに届いたメッセージのキュー
	/// </summary>
	private List<string> readQue = new List<string>();
	
	/// <summary>
	/// シリアルでメッセージを受信した時の処理のデリゲード
	/// </summary>
	/// <param name="message"></param>
	public delegate void SerialReciveHandler(string message);
	
	/// <summary>
	/// メッセージ受信時のイベント
	/// </summary>
	public event SerialReciveHandler OnRead;
	

	private void Awake() {
		findPort();
		serialRunning = true;
		setupReadPort();
		port.DiscardInBuffer();
	}

	
	/// <summary>
	/// シリアルポートを見つけます
	/// </summary>
	/// <exception cref="PortNotFoundException"></exception>
	private void findPort() {
		string[] ports = Directory.GetFiles("/dev/","cu.SLAB*");
		int notBoot = 0;
		foreach (var portName in ports) {
			Debug.Log(portName + "init");
			if (bootPort(portName)) {
				break;
			} else {
				notBoot++;
			}
		}

		if (notBoot >= ports.Length) {
			port = null;
			throw new PortNotFoundException();
		} else {
			Debug.Log("Port found " + port.PortName);
		}
	}
	
	
	/// <summary>
	/// 指定した名前のポートを起動し、ブートメッセージを送り、返答があればそれにポートを設定します
	/// 返答が設定時間以上ない場合、ポートを閉じて戻ります
	/// </summary>
	/// <param name="portName">起動するポート</param>
	/// <returns>返答があったらtrue</returns>
	private bool bootPort(string portName) {
		port = new SerialPort(portName,frequency,Parity.None, 8, StopBits.One);
		port.Open();
		
		Debug.Log(port.PortName);
		Debug.Log(port.BaudRate);
		Debug.Log(port.IsOpen);
		
		try {
			port.Write(new byte[] {0xAA}, 0, 1);
		} catch (TimeoutException) {
			return false;
		}

		if (!reciveReply()) {
			port.Close();
			port.Dispose();
			return false;
		}

		return true;
	}
	
	/// <summary>
	/// 現在portに入っているシリアルポートからの返答を待ちます
	/// </summary>
	/// <returns>返答があったかどうか</returns>
	private bool reciveReply() {
		if (port != null && port.IsOpen) {
			for(float time = 0; time <= replyLimit; time += Time.deltaTime){
				//deltaTimeはs、ReadTimeoutはmsなので * 1000
				int limit = (int) ((replyLimit - time) * 1000f);
				limit = (limit > 0) ? limit : 1;
				Debug.Log(limit);
				port.ReadTimeout = limit;
				try {
					var b = port.ReadByte();
					Debug.Log(b.ToString("X"));
					if (b == 0x55) {
						//タイムアウトしないようになる
						port.ReadTimeout = SerialPort.InfiniteTimeout;
						return true;
					}
				}catch(TimeoutException) {
					//タイムアウトしたらfalse
					return false;
				}
			}
		}
		return false;
	}

	/// <summary>
	/// 読み込み系を立ち上げます
	/// </summary>
	/// <exception cref="PortNotFoundException"></exception>
	private void setupReadPort() {
		if (!serialRunning || port == null || !port.IsOpen)
			throw new PortNotFoundException("Cannot setup port");
		
		readThread = new Thread(readSerial);
		readThread.Start();
	}
	
	/// <summary>
	/// メッセージが届いていたらキューに追加します
	/// </summary>
	private void readSerial() {
		port.ReadTimeout = 3000;
		while (port != null && port.IsOpen) {
			try {
				string message = port.ReadLine();
				Debug.Log("thread recived" + message);
				readQue.Add(message);
			}catch(TimeoutException){
				
			}
			if (!serialRunning)
				break;
		}
	}
	
	
	// Update is called once per frame
	void Update () {
		if (readQue.Count > 0) {
			//キューから一個取り出して実行
			if(OnRead != null)
				OnRead(readQue[0]);
			readQue.RemoveAt(0);
		}
	}
	
	/// <summary>
	/// 読み込みスレッドを終了します
	/// </summary>
	public void endThread() {
		serialRunning = false;
		if (readThread != null && readThread.IsAlive)
			readThread.Abort();

		if (port != null && port.IsOpen) {
			port.Dispose();
			port.Close();
		}

		foreach (var message in readQue) {
			//キューから一個取り出して実行
			if(OnRead != null)
				OnRead(message);
		}

		readQue.Clear();
	}

	private void OnDestroy() {
		endThread();
	}
}


public class PortNotFoundException : System.Exception {
	public PortNotFoundException() : base("Port Not Found") {}
	public PortNotFoundException(string message) : base(message){}
}