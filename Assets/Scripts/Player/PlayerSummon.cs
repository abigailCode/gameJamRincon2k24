using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSummon : MonoBehaviour {

    void Start() {
        
    }


    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            Debug.Log("Se ha pulsado la tecla 1");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            Debug.Log("Se ha pulsado la tecla 2");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            Debug.Log("Se ha pulsado la tecla 3");
        }


        if (Input.GetKeyDown("1")) {
            Debug.Log("Se ha pulsado la tecla 1");
        }
    }
}
