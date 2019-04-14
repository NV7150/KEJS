using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

/// <summary>
/// マイコンのシリアルポートを示すオブジェクト
/// 起動処理とかめんどいから入ってる
/// </summary>
[Serializable]
public class MiconPort {
    /// <summary>
    /// ポートのID
    /// </summary>
    [SerializeField]private int id;
    /// <summary>
    /// ポートのローカル的な名前
    /// 別にポートの名前(tty.SLAT~とか)と同じ必要はない
    /// </summary>
    [SerializeField]private String localName;
    /// <summary>
    /// 通信速度
    /// </summary>
    [SerializeField]private int baudRate;
    /// <summary>
    /// こちらから識別用に送るシリアル数値
    /// </summary>
    [SerializeField]private byte sendMessage;
    /// <summary>
    /// マイコンポートからの応答数値
    /// </summary>
    [SerializeField]private byte replyMessage;
    /// <summary>
    /// 起動時の返答待ちをどれくらいにするか(ms)
    /// </summary>
    [SerializeField]private int startReplyLimit = 500;
    
    
    /// <summary>
    /// シリアルポートオブジェクト
    /// </summary>
    private SerialPort port;
    
    /// <summary>
    /// ポート側から送られてきたメッセージ
    /// </summary>
    private List<String> recivedMessage = new List<String>();
    
    /// <summary>
    /// メッセージ受け付け用スレッド
    /// </summary>
    private Thread readThread;
    
    /// <summary>
    /// スレッドをまだ動かすか
    /// </summary>
    private bool threadRunning = false;
    
    /// <summary>
    /// ポートの準備ができているか
    /// </summary>
    private bool ready = false;

    
    /// <summary>
    /// マイコンからメッセージが送られてきた時の処理のデリゲート
    /// </summary>
    /// <param name="message">送られてきたメッセージ</param>
    public delegate void MaiconReciveEvent(String message);
    
    /// <summary>
    /// マイコンからメッセージ送られてきた時に呼ばれるイベント
    /// </summary>
    public event MaiconReciveEvent OnRecived;

    public int Id {
        get { return id; }
    }
    public string LocalName {
        get { return localName; }
    }
    
    public bool Ready {
        get { return ready; }
    }
    
    /// <summary>
    /// 一応動的生成にも対応するためのコンスラクタ
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="localName">識別用のローカルな名前</param>
    /// <param name="baudRate">通信速度</param>
    /// <param name="sendMessage">問い合わせ時に送る数値</param>
    /// <param name="replyMessage">正常なポートであった場合の返答</param>
    public MiconPort(int id, string localName, int baudRate, byte sendMessage, byte replyMessage) {
        this.id = id;
        this.localName = localName;
        this.baudRate = baudRate;
        this.sendMessage = sendMessage;
        this.replyMessage = replyMessage;
    }
    
    /// <summary>
    /// シリアルを起動します
    /// <returns>成功したかどうか</returns>
    /// </summary>
    /// <exception cref="PortNotFoundException">ポートが見つからなかった時に投げられます</exception>
    public void start() {
        findPort();
        threadRunning = true;
        setupReadPort();
        port.DiscardInBuffer();
        ready = true;
    }

	
    /// <summary>
    /// シリアルポートを見つけます
    /// </summary>
    /// <exception cref="PortNotFoundException">ポートが見つからなかった時に投げられます</exception>
    private void findPort() {
        string[] ports;
        
        #if UNITY_EDITOR_OSX 
                ports = Directory.GetFiles("/dev/","cu.*");
        #elif UNITY_STANDALONE_OSX
			ports = Directory.GetFiles("/dev/","tty.*");
		#elif UNITY_STANDALONE_LINUX
			ports = Directory.GetFiles("/dev/","tty*");
		#elif UNITY_STANDALONE_WIN
			ports = SerialPort.GetPortNames();
		#else
			throw new Exception("OS you're using is not supported.");
		#endif
        
        int notBoot = 0;
        foreach (var portName in ports) {
            if (accessPort(portName)) {
                break;
            } else {
                notBoot++;
            }
        }

        if (notBoot >= ports.Length) {
            port = null;
            throw new PortNotFoundException();
        }
    }
	
	
    /// <summary>
    /// 指定した名前のポートに対して問い合わせを行い、返答があればそれにポートを設定します
    /// 返答が設定時間以上ない場合、ポートを閉じて戻ります
    /// </summary>
    /// <param name="portName">起動するポート</param>
    /// <returns>返答があったらtrue</returns>
    private bool accessPort(string portName) {
        port = new SerialPort(portName,baudRate,Parity.None, 8, StopBits.One);
        try {
            port.Open();
        } catch (Exception) {
            //ポートがなんか異常あったら
            if (port.IsOpen) {
                port.Dispose();
                port.Close();
            }
            return false;
        }

        try {
            port.Write(new byte[] {sendMessage}, 0, 1);
        } catch (TimeoutException) {
            return false;
        }

        if (!reciveReply()) {
            port.Dispose();
            port.Close();
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
            //startReplyLimitが0未満ならタイムアウトを行わない(非推奨)
            bool willTimeOut = startReplyLimit >= 0;
            if (!willTimeOut)
                port.ReadTimeout = SerialPort.InfiniteTimeout;
            
            for(float time = 0; time <= startReplyLimit; time += Time.deltaTime){
                
                //残りのタイムアウト時間の設定(タイムアウトする場合のみ)
                if (willTimeOut) {
                    //deltaTimeはs、ReadTimeoutはmsなので * 1000
                    int limit = (int) (startReplyLimit - time * 1000f);
                    limit = (limit > 0) ? limit : 1;
                    port.ReadTimeout = limit;
                }

                try {
                    var message = port.ReadByte();
                    if (message == replyMessage)
                        return true;
                    
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
        if (!threadRunning || port == null || !port.IsOpen)
            throw new PortNotFoundException("Cannot setup port");
		
        readThread = new Thread(readSerial);
        readThread.Start();
    }
	
    /// <summary>
    /// メッセージが届いていたらキューに追加します
    /// </summary>
    private void readSerial() {
        port.ReadTimeout = 500;
        while (port != null && port.IsOpen) {
            try {
                string message = port.ReadLine();
                recivedMessage.Add(message);
            }catch(TimeoutException){
                //定期的にタイムアウトして見る
                if (!threadRunning)
                    break;
            }
        }
    }
    
    /// <summary>
    /// 読み込みスレッドを終了します
    /// </summary>
    public void end() {
        if (!ready)
            return;
        
        ready = false;
        
        threadRunning = false;
        if (readThread != null && readThread.IsAlive)
            readThread.Abort();

        if (port != null && port.IsOpen) {
            port.Dispose();
            port.Close();
        }

        foreach (var message in recivedMessage) {
            //キューから一個取り出して実行
            if(OnRecived != null)
                OnRecived(message);
        }

        recivedMessage.Clear();

    }
    
    /// <summary>
    /// 到着しているメッセージを処理します
    /// </summary>
    public void solveRecive() {
        if (!ready)
            return;
        
        //fpsが遅くなってもいけないので一つづつ処理
        if (recivedMessage.Count > 0) {
            if (OnRecived != null)
                OnRecived(recivedMessage[0]);
            recivedMessage.RemoveAt(0);
        }
    }
    
    /// <summary>
    /// ポートにメッセージを書き込みます
    /// </summary>
    /// <param name="message">送るメッセージ</param>
    public void send(byte message) {
        if (!ready)
            return;
        
        port.Write(new[] {message}, 0, 1);
    }
}


public class PortNotFoundException : System.Exception {
    public PortNotFoundException() : base("port not found"){}
    public PortNotFoundException(String message) : base(message){}
}