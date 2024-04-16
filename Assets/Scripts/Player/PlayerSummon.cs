using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSummon : MonoBehaviour {
    [Header("Lists")]
    [SerializeField] List<KeyCode> summonKeyBoard;
    [SerializeField] List<GameObject> summon;
    [SerializeField] List<Image> imageList;
    [SerializeField] List<Image> imageDeactivatedList;

    [Header("Instance Distance")]
    [SerializeField] float minDistance = 2f;    // Distancia mínima desde el jugador
    [SerializeField] float maxDistance = 5f;    // Distancia máxima desde el jugador
    [SerializeField] float yOffset = 1f;        // Offset de altura en el eje Y

    [Header("Config")]
    [SerializeField] float[] summonCooldowns;       // Tiempo de cooldown para cada tipo de summon
    [SerializeField] ManaLifeController manaObj;    //Referencia al script ManaLifeController
    [SerializeField] List<float> manaCost;          //Coste de invocaciones

    [Header("Control Spawn")]
    public float maximumX;
    public float minimumX;
    public float maximumZ;
    public float minimumZ;

    private bool[] summonOnCooldown;    // Bool para indicar si un tipo de summon está en cooldown

    void Start() {
        summonOnCooldown = new bool[summon.Count];
        manaObj = GetComponent<ManaLifeController>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(summonKeyBoard[0]) && !summonOnCooldown[0]) {
            CheckAndInstance(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(summonKeyBoard[1]) && !summonOnCooldown[1]) {
            CheckAndInstance(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(summonKeyBoard[2]) && !summonOnCooldown[2]) {
            CheckAndInstance(2);
        }

        //Comprueba constantemente si activa o desactiva la imagen indicadora del summon
        ShownHideImageSummon();
    }

    private Vector3 CalculateSummonPosition() {
        // Obtener la posición del jugador
        Vector3 playerPosition = transform.position;

        // Generar una posición aleatoria dentro del círculo
        float randomAngle = Random.Range(0f, 360f);
        float randomDistance = Random.Range(minDistance, maxDistance);

        float x = playerPosition.x + randomDistance * Mathf.Cos(randomAngle * Mathf.Deg2Rad);
        float z = playerPosition.z + randomDistance * Mathf.Sin(randomAngle * Mathf.Deg2Rad);

        //Controla que no se salga del mapa
        if (x > maximumX) x = maximumX;
        if (x < minimumX) x = minimumX;

        if (z > maximumZ) z = maximumZ;
        if (z < minimumZ) z = minimumZ;

        // Crear el vector de posición del summon
        Vector3 summonPosition = new Vector3(x, playerPosition.y+ yOffset, z);

        return summonPosition;
    }

    IEnumerator SummonCooldown(int index) {
        // Oculta la imagen indicadora de spawn correspondiente
        //imageList[index].gameObject.SetActive(false);

        summonOnCooldown[index] = true;
        yield return new WaitForSeconds(summonCooldowns[index]);
        summonOnCooldown[index] = false;

        // Activa la imagen indicadora de spawn correspondiente
        //imageList[index].gameObject.SetActive(true);
    }

    private void CheckAndInstance(int numList) {
        if (manaObj.getMana() >= manaCost[numList]) {
            manaObj.TakeMana(manaCost[numList]);
            Instantiate(summon[numList], CalculateSummonPosition(), Quaternion.identity);
            Debug.Log("Summon " + numList + " instanciado");
            AudioManager.instance.PlaySFX("summonSpawn");
            StartCoroutine(SummonCooldown(numList));
        }
    }

    private void ShownHideImageSummon() {
        for (int i = 0; i < imageList.Count; i++) {
            if (manaObj.getMana() >= manaCost[i] && !summonOnCooldown[i]) {
                imageList[i].gameObject.SetActive(true);
                imageDeactivatedList[i].gameObject.SetActive(false);
            }
            else {
                imageList[i].gameObject.SetActive(false);
                imageDeactivatedList[i].gameObject.SetActive(true);
            }
        }
        /*foreach (Image img in imageList) {
            int index = imageList.IndexOf(img);
            if (manaObj.getMana() >= manaCost[index] && !summonOnCooldown[index]) {
                img.gameObject.SetActive(true);
            }
            else {
                img.gameObject.SetActive(false);
            }
        }*/
    }
}
