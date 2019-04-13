using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenChanger : MonoBehaviour {
	[SerializeField] private KendoManager kendoMan;
	
	[SerializeField] private GameObject ipponScreenPrefab;
	[SerializeField] private GameObject hitScreenPrefab;

	[SerializeField] private GameObject canvas;

	private GameObject ipponScreenObj;
	private GameObject hitScreenObj;

	private PlNumText ipponPlText;
	private PlNumText hitPlText;
	
	// Use this for initialization
	void Start () {
		ipponScreenObj = Instantiate(ipponScreenPrefab);
		hitScreenObj = Instantiate(hitScreenPrefab);

		ipponScreenObj.transform.parent = canvas.transform;
		hitScreenObj.transform.parent = canvas.transform;
		ipponScreenObj.transform.position = new Vector3(Screen.width / 2f ,Screen.height / 2f);
		hitScreenObj.transform.position = new Vector3(Screen.width / 2f ,Screen.height / 2f);

		ipponPlText = ipponScreenObj.GetComponent<PlNumText>();
		hitPlText = hitScreenObj.GetComponent<PlNumText>();
		
		ipponScreenObj.SetActive(false);
		hitScreenObj.SetActive(false);

		kendoMan.OnHit += toHit;
		kendoMan.OnIppon += toIppon;
		kendoMan.OnBack += back;
	}

	void toHit(int player) {
		ipponScreenObj.SetActive(false);
		hitScreenObj.SetActive(true);
		hitPlText.setText("Player " + player);
	}

	void toIppon(int player) {
		hitScreenObj.SetActive(false);
		ipponScreenObj.SetActive(true);
		ipponPlText.setText("Player " + player);
	}

	void back() {
		ipponScreenObj.SetActive(false);
		hitScreenObj.SetActive(false);
	}
}
