using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
    public Transform player;

    //EVENTO (DELEGADO)   --> Avisa de que un enemigo ha muerto
    public delegate void DeadEnemy(GameObject enemyObj);
    public static event DeadEnemy OnDeadEnemy;  //(EVENTO)
    //EVENTO (DELEGADO)   --> Avisa para hacer dano al player
    public delegate void damagePlayer(float damage);
    public static event damagePlayer OnDamagePlayer;  //(EVENTO)

    [Header("Damage")]
    [SerializeField] float damagetime = 0.1f;
    [SerializeField] float amountOfDamageToPlayer = 10f;
    [SerializeField] float amountOfDamageToSummon = 10f;
    private bool canAttack;

    private NavMeshAgent navMeshAgent;
    private float closestDistance;
    private Transform closestTarget;

    [Header("Estadisticas")]
    public int salud = 100;
    public int saludActual;
    public int damageFromPlayer = 10;

    private void Awake()
    {
        //anim = GetComponent<Animator>();
    }

    void Start() {
        saludActual = salud;
        navMeshAgent = GetComponent<NavMeshAgent>();

        canAttack = true;
    }


    void Update() {
        SetNavDestination();

        // Comprueba si se ha muerto
        DestroyOnDead();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Summon"))
            other.GetComponent<InvocationHealthController>().SetDamage(amountOfDamageToSummon);

        if (other.CompareTag("Player"))
            StartCoroutine(DamageToPlayer(amountOfDamageToPlayer));
    }

    private void OnTriggerExit(Collider other) {
        StopCoroutine(DamageToPlayer(amountOfDamageToPlayer));
    }

    public void RecibeDano(int dano)
    {
        saludActual -= dano;
    }

    private void SetNavDestination() {
        GameObject[] summons = GameObject.FindGameObjectsWithTag("Summon");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        closestTarget = null;
        closestDistance = Mathf.Infinity;

        if (summons.Length != 0) {
            SetDestinationList(summons);
        } else {
            SetDestinationList(players);
        }        

        if (closestTarget != null) {
            navMeshAgent.SetDestination(closestTarget.position);
        }
        else {
            Debug.LogWarning("No se encontró ningún jugador en la escena.");
        }
    }
    private void SetDestinationList(GameObject[] targets) {
        foreach (GameObject target in targets) {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < closestDistance) {
                closestTarget = target.transform;
                closestDistance = distance;
            }
        }
    }

    IEnumerator DamageToPlayer(float damage) {
        Debug.Log("DamageToPlayer " + damage);
        while (true) {
            if (canAttack) {
                //Evento hace dano al jugador
                if (OnDamagePlayer != null)
                    OnDamagePlayer(damage);
            }


            canAttack = false;
            yield return new WaitForSeconds(damagetime);
            canAttack = true;
        }        
    }

    private void DestroyOnDead() {
        if (CheckedIsDead()) {
            Debug.Log("muere");
            //Evento Aumenta la cantidad de Gemas recogidas
            if (OnDeadEnemy != null)
                OnDeadEnemy(this.gameObject);

            Destroy(this.gameObject);
        }
    }
    public bool CheckedIsDead() {
        if (saludActual <= 0) return true;
        else return false;
    }
}
