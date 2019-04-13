using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestReciver : MonoBehaviour {
	[SerializeField] private KendoManager kendoMan;
	
	// Use this for initialization
	void Start () {
		kendoMan.OnHit += hit;
		kendoMan.OnIppon += ippon;
		kendoMan.OnBack += back;
	}

	private void hit(int player) {
		Debug.Log("hit!");
	}

	private void ippon(int player) {
		Debug.Log("ippon!");
	}

	private void back() {
		Debug.Log("back to game");
	}
}
