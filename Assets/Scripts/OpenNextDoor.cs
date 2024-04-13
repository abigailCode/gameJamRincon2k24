using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OpenNextDoor : MonoBehaviour {
    [SerializeField] TextMeshProUGUI numOfEnemiesText;
    [SerializeField] GameObject Door;

    private List<GameObject> EnemyList;
    private int _numOfEnemies;
    private int _deadEnemies = 0;

    //SUSCRIPCIÓN al EVENTO
    void OnEnable() {
        Enemy.OnDeadEnemy += OnDeadEnemy;
    }
    //DESUSCRIPCIÓN al EVENTO
    void OnDisable() {
        Enemy.OnDeadEnemy -= OnDeadEnemy;
    }

    void Start() {
        // Buscar todos los objetos con el tag "Enemy"
        GameObject[] items = GameObject.FindGameObjectsWithTag("Enemy");
        _numOfEnemies = items.Length;

        Door.SetActive(false);

        TextEnemiesLeft();
    }


    void Update() {
        
    }

    private void TextEnemiesLeft() {
        int enemiesLeft = _numOfEnemies - _deadEnemies;
        numOfEnemiesText.text = enemiesLeft.ToString();
    }
    private void OnDeadEnemy() {
        _deadEnemies++;

        // Actualiza el texto de gemas faltantes
        TextEnemiesLeft();

        CheckWin();
    }

    private void CheckWin() {
        if (_deadEnemies == _numOfEnemies) {
            Door.SetActive(true);
        }
    }
}
