using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
    public Transform player;
    
    private Rigidbody rb;
    private Collider collider;

    //EVENTO (DELEGADO)   --> Avisa de que un enemigo ha muerto
    public delegate void DeadEnemy();
    public static event DeadEnemy OnDeadEnemy;  //(EVENTO)
    //EVENTO (DELEGADO)   --> Avisa para hacer dano al player
    public delegate void damagePlayer(float damage);
    public static event damagePlayer OnDamagePlayer;  //(EVENTO)

    [Header("Damage")]
    [SerializeField] float damagetime = 0.1f;
    [SerializeField] float amountOfDamage = 10f;

    private NavMeshAgent navMeshAgent;
    private float closestDistance;
    private Transform closestTarget;

    [Header("Estadisticas")]
    public int salud = 100;
    public int saludActual;
    public int damageFromPlayer = 10;
    public float speed = 5f;

    [Header("Booleanos")]
    public bool tocaJugador;
    private bool puedeDanar = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        //anim = GetComponent<Animator>();
    }

    void Start() {
        saludActual = salud;
        tocaJugador = false;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }


    void Update() {
        /*float distanciaEnemyPlayer = Vector3.Distance(transform.position, player.position);

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            Transform playerTransform = playerObject.transform;
            Vector3 direction = playerTransform.position - transform.position;
            direction.Normalize();
            transform.position += direction * speed * Time.deltaTime;
        }
        else
        {
            Debug.LogWarning("No se encontró al jugador en la escena.");
        }*/
        SetNavDestination();
    }

    private void OnTriggerEnter(Collider other) {
        while (true) {
            StartCoroutine(RecibirDano());

            StartCoroutine(DamageToPlayer(amountOfDamage));
        }
    }

    private void OnTriggerExit(Collider other) {
        StopCoroutine(RecibirDano());

        StopCoroutine(DamageToPlayer(amountOfDamage));
    }
    /*private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(RecibirDano());
            tocaJugador = true;
            /*Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                RecibeDano(damageFromPlayer);
            }*/

            /*StartCoroutine(DamageToPlayer(amountOfDamage));
        }
        else
        {
            tocaJugador = false;
        }
    }*/

    public void RecibeDano(int dano)
    {
        saludActual -= dano;

        if (saludActual <= 0)
        {
            Morir();
        }
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

    private void Morir()
    {
        if (saludActual <= 0)
        {
            speed = 0;
            rb.velocity = Vector2.zero;

            //Evento Aumenta la cantidad de Gemas recogidas
            if (OnDeadEnemy != null)
                OnDeadEnemy();

            Destroy(this.gameObject, 0.2f);
        }
    }

    IEnumerator RecibirDano() {
        puedeDanar = false;

        RecibeDano(damageFromPlayer);
        yield return new WaitForSeconds(damagetime);
        puedeDanar = true;
    }

    IEnumerator DamageToPlayer(float damage) {
        Debug.Log("DamageToPlayer " + damage);
        //Evento hace dano al jugador
        if (OnDamagePlayer != null)
            OnDamagePlayer(damage);

        yield return new WaitForSeconds(damagetime);
    }

    public bool CheckedIsDead() {
        if (saludActual <= 0) return true;
        else return false;
    }
}
