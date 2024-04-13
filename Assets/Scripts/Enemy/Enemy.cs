using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public Transform player;
    
    private Rigidbody rb;
    private Collider collider;

    //EVENTO (DELEGADO)   --> Avisa de que un enemigo ha muerto
    public delegate void DeadEnemy();
    public static event DeadEnemy OnDeadEnemy;  //(EVENTO)
    private int distanciaToque = 5;

    [Header("Estadisticas")]
    public int vidas = 3;
    public int salud = 100;
    public int saludActual;
    public int damageFromPlayer = 10;
    public float speed = 5f;

    [Header("Booleanos")]
    public bool tocaJugador;
    private bool puedeDañar = true;

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
    }


    void Update() {
        float distanciaEnemyPlayer = Vector3.Distance(transform.position, player.position);

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
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(RecibirDaño());
            collider.enabled = false;
            StartCoroutine(ReactivaCollider());
            tocaJugador = true;
            /*Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                RecibeDaño(damageFromPlayer);
            }*/
        }
        else
        {
            tocaJugador = false;
        }
    }

    public void RecibeDaño(int daño)
    {
        saludActual -= daño;

        if (saludActual <= 0)
        {
            Morir();
        }
    }

    

    private void Morir()
    {
        if (vidas <= 0)
        {
            speed = 0;
            rb.velocity = Vector2.zero;
            Destroy(this.gameObject, 0.2f);
        }
    }

    IEnumerator RecibirDaño()
    {
        puedeDañar = false;
        {
            RecibeDaño(damageFromPlayer);
        }
        yield return new WaitForSeconds(0.1f);
        puedeDañar = true;
    }

    IEnumerator ReactivaCollider()
    {
        yield return new WaitForSeconds(0.1f);
        collider.enabled = true;
    }

    /* Poner cuando muere el enemigo
    //Evento Aumenta la cantidad de Gemas recogidas
    if (OnDeadEnemy != null)
        OnDeadEnemy();
    break;*/
}
