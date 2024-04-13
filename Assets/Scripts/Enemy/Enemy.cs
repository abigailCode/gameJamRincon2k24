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
    
    private NavMeshAgent navMeshAgent;
    private float closestDistance;
    private Transform closestTarget;

    [Header("Estadisticas")]
    public int vidas = 3;
    public int salud = 100;
    public int saludActual;
    public int damageFromPlayer = 10;
    public float speed = 5f;

    [Header("Booleanos")]
    public bool tocaJugador;
    private bool puedeDanar = true;

    private void Awake()
    {
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        //sp = GetComponent<SpriteRenderer>();
        //anim = GetComponent<Animator>();
        //cm = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
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
    private void OnTriggerStay(Collider collision)
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
        }
        else
        {
            tocaJugador = false;
        }
    }

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
        if (vidas <= 0)
        {
            speed = 0;
            rb.velocity = Vector2.zero;

            //Evento Aumenta la cantidad de Gemas recogidas
            if (OnDeadEnemy != null)
                OnDeadEnemy();

            Destroy(this.gameObject, 0.2f);
        }
    }

    IEnumerator RecibirDano()
    {
        puedeDanar = false;
        {
            RecibeDano(damageFromPlayer);
        }
        yield return new WaitForSeconds(0.1f);
        puedeDanar = true;
    }

    /* Poner cuando muere el enemigo
    //Evento Aumenta la cantidad de Gemas recogidas
    if (OnDeadEnemy != null)
        OnDeadEnemy();
    break;*/
}
