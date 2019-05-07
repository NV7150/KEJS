using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
    [SerializeField] private Text player1Text;
    [SerializeField] private Text player2Text;
    [SerializeField] private GameObject scoreScreen;

    [SerializeField] private KendoManager kendoMan;
    
    private int player1Point = 0;
    private int player2Point = 0;
    
    // Start is called before the first frame update
    void Start() {
        kendoMan.OnIppon += ippon;
        kendoMan.OnHit += hit;
        kendoMan.OnBack += back;
    }

    private void OnDestroy() {
        kendoMan.OnIppon -= ippon;
        kendoMan.OnHit -= hit;
        kendoMan.OnBack -= back;
    }

    void ippon(int player) {
        switch (player) {
                case 1:increase1();
                    break;
                case 2: increase2();
                    break;
        }
    }

    void hit(int player) {
        scoreScreen.SetActive(false);
    }

    void back() {
        scoreScreen.SetActive(true);
    }

    void increase1() {
        player1Point++;
        player1Text.text = "" + player1Point;
    }

    void increase2() {
        player2Point++;
        player2Text.text = "" + player2Point;
    }

    // Update is called once per frame
    void Update(){
        
    }
}
