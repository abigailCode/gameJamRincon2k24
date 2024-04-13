using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSummon : MonoBehaviour {
    [Header("Lists")]
    [SerializeField] List<KeyCode> summonKey;
    [SerializeField] List<GameObject> summon;
    [SerializeField] GameObject manaObj;

    [Header("Instance Distance")]
    [SerializeField] float minDistance = 2f;    // Distancia mínima desde el jugador
    [SerializeField] float maxDistance = 5f;    // Distancia máxima desde el jugador
    [SerializeField] float yOffset = 1f;        // Offset de altura en el eje Y

    [Header("Cooldown")]
    [SerializeField] float[] summonCooldowns;   // Tiempo de cooldown para cada tipo de summon

    private bool[] summonOnCooldown;            // Bandera para indicar si un tipo de summon está en cooldown

    void Start() {
        summonOnCooldown = new bool[summon.Count];

    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(summonKey[0]) && !summonOnCooldown[0]) {
            Instantiate(summon[0], CalculateSummonPosition(), Quaternion.identity);
            StartCoroutine(SummonCooldown(0));
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(summonKey[1]) && !summonOnCooldown[1]) {
            Instantiate(summon[1], CalculateSummonPosition(), Quaternion.identity);
            StartCoroutine(SummonCooldown(1));
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(summonKey[2]) && !summonOnCooldown[2]) {
            Instantiate(summon[2], CalculateSummonPosition(), Quaternion.identity);
            StartCoroutine(SummonCooldown(2));
        }
    }

    private Vector3 CalculateSummonPosition() {
        // Obtener la posición del jugador
        Vector3 playerPosition = transform.position;

        // Generar una posición aleatoria dentro del círculo
        float randomAngle = Random.Range(0f, 360f);
        float randomDistance = Random.Range(minDistance, maxDistance);

        float x = playerPosition.x + randomDistance * Mathf.Cos(randomAngle * Mathf.Deg2Rad);
        float z = playerPosition.z + randomDistance * Mathf.Sin(randomAngle * Mathf.Deg2Rad);

        // Crear el vector de posición del summon
        Vector3 summonPosition = new Vector3(x, playerPosition.y+ yOffset, z);

        return summonPosition;
    }

    IEnumerator SummonCooldown(int index) {
        summonOnCooldown[index] = true;
        yield return new WaitForSeconds(summonCooldowns[index]);
        summonOnCooldown[index] = false;
    }
}
