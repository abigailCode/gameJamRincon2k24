using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmenyPool : MonoBehaviour
{

    public GameObject enemyPrefab;
    public GameObject playerPrefab;
    [Header("|----------------Enemy Spawn Area----------------|")]
    public float maximumX;
    public float minimumX;
    public float maximumZ;
    public float minimumZ;
    public int enemyCount = 20;
    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {

            Vector3 randomDestination = new Vector3(Random.Range(minimumX, maximumX), 1.08f, Random.Range(minimumZ, maximumZ));


            //if (randomDestination.x < playerPrefab.transform.position.x + 10 && randomDestination.x > playerPrefab.transform.position.x - 10)
            //{
            //    randomDestination.x = Random.Range(187.63f, 219.2f);
            //}


            Instantiate(enemyPrefab, randomDestination, Quaternion.identity);

        }
    }

   
}
