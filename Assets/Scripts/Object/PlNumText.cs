using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//指定箇所に数字を入れるだけ
public class PlNumText : MonoBehaviour {
	[SerializeField] private Text text;

	public void setText(string message) {
		text.text =  message;
	}
}
