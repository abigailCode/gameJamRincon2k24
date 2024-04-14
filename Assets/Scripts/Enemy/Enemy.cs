using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
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
    private bool canAttackSummon;

    private NavMeshAgent navMeshAgent;
	private float closestDistance;
	private Transform closestTarget;

	[Header("Estadisticas")]
	public int salud = 100;
	public int saludActual;
	public int damageFromPlayer = 10;

	void Start()
	{
		saludActual = salud;
		navMeshAgent = GetComponent<NavMeshAgent>();

		canAttack = true;
	}


	void Update()
	{
		SetNavDestination();

		// Comprueba si se ha muerto
		DestroyOnDead();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (canAttack)
		{
			if (other.CompareTag("Summon"))
                StartCoroutine(DamageToSummon(other, amountOfDamageToSummon));            

			if (other.CompareTag("Player"))
				StartCoroutine(DamageToPlayer(amountOfDamageToPlayer));
		}
	}

	private void OnTriggerExit(Collider other) {
        StopCoroutine(DamageToSummon(other, amountOfDamageToSummon));

        StopCoroutine(DamageToPlayer(amountOfDamageToPlayer));
	}

	public void RecibeDano(int dano)
	{
		saludActual -= dano;
	}

	private void SetNavDestination()
	{
		GameObject[] summons = GameObject.FindGameObjectsWithTag("Summon");
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		closestTarget = null;
		closestDistance = Mathf.Infinity;

		if (summons.Length != 0)
		{
			SetDestinationList(summons);
		}
		else
		{
			SetDestinationList(players);
		}

		if (closestTarget != null)
		{
			navMeshAgent.SetDestination(closestTarget.position);
		}
		else
		{
			Debug.LogWarning("No se encontr� ning�n jugador en la escena.");
		}
	}
	private void SetDestinationList(GameObject[] targets)
	{
		foreach (GameObject target in targets)
		{
			float distance = Vector3.Distance(transform.position, target.transform.position);
			if (distance < closestDistance)
			{
				closestTarget = target.transform;
				closestDistance = distance;
			}
		}
	}

    IEnumerator DamageToPlayer(float damage) {
        Debug.Log("DamageToPlayer " + damage);
        while (true) {
            if (canAttack) {
				AudioManager.instance.PlaySFX("enemyAttack");
                //Evento hace dano al jugador
                if (OnDamagePlayer != null)
                    OnDamagePlayer(damage);
            }


            canAttack = false;
            yield return new WaitForSeconds(damagetime);
            canAttack = true;
        }
    }
    IEnumerator DamageToSummon(Collider summonCol, float damage) {
        Debug.Log("DamageToSummon " + damage);
        while (true) {
            if (canAttackSummon) {
                AudioManager.instance.PlaySFX("enemyAttack");
                //Evento hace dano al summon
                summonCol.GetComponent<InvocationHealthController>().SetDamage(amountOfDamageToSummon);
            }
			if (!summonCol.gameObject.activeSelf)
                StopCoroutine(DamageToSummon(summonCol, damage));

            canAttackSummon = false;
            yield return new WaitForSeconds(damagetime);
            canAttackSummon = true;
        }
    }

    private void DestroyOnDead()
	{
		if (CheckedIsDead()) {
            AudioManager.instance.PlaySFX("enemyDeath");

            canAttack = false;
			Debug.Log("muere");
			//Evento Aumenta la cantidad de Gemas recogidas
			if (OnDeadEnemy != null)
				OnDeadEnemy(this.gameObject);

			this.gameObject.SetActive(false);
			//Destroy(this.gameObject);
		}
	}
	public bool CheckedIsDead()
	{
		if (saludActual <= 0) return true;
		else return false;
	}

}
