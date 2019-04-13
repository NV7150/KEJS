using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KendoSerialReciver : MonoBehaviour {
	[SerializeField] private SerialReciver reciver;
//	[SerializeField] private string hitMes = "hit";
	[SerializeField] private KendoManager kendoMan;

	private int lastHit = -1;
	
	// Use this for initialization
	void Start () {
		reciver.OnRead += onReciveMessage;
	}
	
	// Update is called once per frame
	void Update () {
		switch (kendoMan.CurrState){ 
			case KendoState.HIT:
				if (Input.GetButtonDown("Ippon")) {
					kendoMan.changeState(KendoState.IPPON,lastHit);
				}

				goto case KendoState.IPPON;
			
			case KendoState.IPPON:
				if (Input.GetButtonDown("BackGame")) {
					kendoMan.changeState(KendoState.GAME,-1);
				}
				break;
		}
	}

	private void onReciveMessage(string message) {
		if (kendoMan.CurrState == KendoState.GAME) {
			int val = 0;
			if (int.TryParse(message,out val)) {
				//シリアル受信
				Debug.Log(val);
				lastHit = val;
				kendoMan.changeState(KendoState.HIT, lastHit);
			}
		}
	}
}
