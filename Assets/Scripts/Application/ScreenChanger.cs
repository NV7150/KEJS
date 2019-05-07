using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenChanger : MonoBehaviour {
	[SerializeField] private KendoManager kendoMan;

	private GameObject imageObjPrefab;

	[SerializeField] private GameObject canvas;

	private GameObject ipponScreenObj;
	private GameObject hitScreenObj;

	private PlNumText ipponPlText;
	private PlNumText hitPlText;
	
	// Use this for initialization
	void Start () {
		imageObjPrefab = Resources.Load<GameObject>("ImageObject");
		
		ipponScreenObj = Instantiate(imageObjPrefab);
		hitScreenObj = Instantiate(imageObjPrefab);

		ipponScreenObj.transform.parent = canvas.transform;
		hitScreenObj.transform.parent = canvas.transform;
		ipponScreenObj.transform.position = new Vector3(Screen.width / 2f ,Screen.height / 2f);
		hitScreenObj.transform.position = new Vector3(Screen.width / 2f ,Screen.height / 2f);

		ipponPlText = ipponScreenObj.GetComponent<PlNumText>();
		hitPlText = hitScreenObj.GetComponent<PlNumText>();

		if (SelectImages.HitSprite != null) {
			setSpriteToImageObj(hitScreenObj,SelectImages.HitSprite);
		}

		if (SelectImages.IpponSprite != null) {
			setSpriteToImageObj(ipponScreenObj,SelectImages.IpponSprite);
		}

		ipponScreenObj.SetActive(false);
		hitScreenObj.SetActive(false);

		kendoMan.OnHit += toHit;
		kendoMan.OnIppon += toIppon;
		kendoMan.OnBack += back;
	}

	private void OnDestroy() {
		kendoMan.OnHit -= toHit;
		kendoMan.OnIppon -= toIppon;
		kendoMan.OnBack -= back;
	}

	void setSpriteToImageObj(GameObject imageObj,Sprite sprite) {
		var imageCom = imageObj.GetComponent<Image>();
		var aspRat = imageCom.GetComponent<AspectRatioFitter>();
		imageCom.sprite = sprite;
		imageCom.SetNativeSize();
		aspRat.aspectRatio = 
			imageCom.rectTransform.rect.width / imageCom.rectTransform.rect.height;
		aspRat.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
		imageObj.transform.localScale = new Vector3(1,1,1);
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
