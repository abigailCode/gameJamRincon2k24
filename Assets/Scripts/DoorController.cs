using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("|----------------Next Level Scene Name----------------|")]
    public string nextLevel;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with " + other.name);
        if (other.CompareTag("Player"))
        {
            SceneController.instance.LoadScene(nextLevel);
        }
    }
}
