using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyReciver : MonoBehaviour {
	[SerializeField] private KendoManager kendoMan;

	private int currentIppon = 0;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		switch (kendoMan.CurrState) {
				case KendoState.GAME:
					if (Input.GetButtonDown("Hit1")) {
						currentIppon = 1;
						kendoMan.changeState(KendoState.HIT,currentIppon);
					} else if(Input.GetButtonDown("Hit2")){
						currentIppon = 2;
						kendoMan.changeState(KendoState.HIT,currentIppon);
					}
					break;
				
				case KendoState.HIT:
					if (Input.GetButtonDown("Ippon")) {
						kendoMan.changeState(KendoState.IPPON, currentIppon);
					}
					//戻れる
					goto case KendoState.IPPON;
					
				case KendoState.IPPON:
					if (Input.GetButtonDown("BackGame"))
						kendoMan.changeState(KendoState.GAME,-1);
					break;
		}
	}
}
