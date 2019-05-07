using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectWaring : MonoBehaviour {
    [SerializeField] private GameObject waringObject;
    
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        if (!SerialReciver.INSTANCE.IsAllPortReady) {
            
            waringObject.SetActive(true);
        }
    }
}
