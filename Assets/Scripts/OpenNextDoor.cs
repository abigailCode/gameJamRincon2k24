using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OpenNextDoor : MonoBehaviour {
    [SerializeField] TextMeshProUGUI numOfEnemiesText;
    [SerializeField] GameObject Door;

    private List<GameObject> EnemyList;
    private int _numOfEnemies;
    private int _deadEnemies;

    void Start() {
        // Buscar todos los objetos con el tag "Enemy"
        GameObject[] items = GameObject.FindGameObjectsWithTag("Enemy");
        _numOfEnemies = items.Length;

        Door.SetActive(false);
    }


    void Update() {
        
    }

    private void OnCollectedGem() {
        _deadEnemies++;

        // Actualiza el texto de gemas faltantes
        TextGemsLeft();

        CheckWin();
    }

    private void CheckWin() {
        if (_deadEnemies == _numOfEnemies) {
            Door.SetActive(true);
        }
    }
}
