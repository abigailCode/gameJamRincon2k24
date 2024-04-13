using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    //EVENTO (DELEGADO)   --> Avisa de que un enemigo ha muerto
    public delegate void DeadEnemy();
    public static event DeadEnemy OnDeadEnemy;  //(EVENTO)

    void Start() {
        
    }


    void Update() {

    }

    /* Poner cuando muere el enemigo
    //Evento Aumenta la cantidad de Gemas recogidas
    if (OnDeadEnemy != null)
        OnDeadEnemy();
    break;*/
}
