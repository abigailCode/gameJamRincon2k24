using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvocationAttackBehaviour : MonoBehaviour
{
	//TODO: Avisar al InvocationBehaviour cuando todos los enemigos están muertos.
	[SerializeField] InvocationModel invocation;

	//ATTACK
	bool canAttack = true;
	bool cooldownIsActive;
	float coolDownDuration = 0f;
	readonly List<GameObject> enemiesInAttackArea = new();
	GameObject target;

	public UnityEvent OnAttackEnemy;
	public UnityEvent OnEnemyEntryArea;
	public UnityEvent OnEnemyExitArea;
	public UnityEvent OnEnemyStayArea;

	#region UNITY METHODS
	private void Start()
	{
		coolDownDuration = invocation.AttackCoolDown;
	}

	private void Update()
	{
		TryToAttackTarget();
		if (!target && enemiesInAttackArea.Count > 0)
			ChoseTargetToAttack();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Enemy"))
		{
			enemiesInAttackArea.Add(other.gameObject);
			OnEnemyEntryArea.Invoke();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Enemy"))
		{
			enemiesInAttackArea.Remove(other.gameObject);
			OnEnemyExitArea.Invoke();
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (CheckIfColliderIsEnemy(other))
			OnEnemyStayArea.Invoke();

	}


	#endregion

	#region PUBLIC METHODS

	public void SetDead()
	{
		canAttack = false;
	}
	#endregion

	#region PRIVATE METHODS

	bool CheckIfColliderIsEnemy(Collider other)
	{
		//El otro es un enemigo y está vivo y coleando
		return other.CompareTag("Enemy"); //&& !other.GetComponent<Enemy>().GetIsDead();
	}
	IEnumerator StartCooldown()
	{
		cooldownIsActive = true;

		yield return new WaitForSeconds(coolDownDuration);
		cooldownIsActive = false;
	}
	void TryToAttackTarget()
	{
		if (target && canAttack && !cooldownIsActive)
		{
			StartCoroutine(StartCooldown());
			Attack(target);
		}
	}

	void Attack(GameObject target)
	{

		Debug.Log("Attacking");
		Enemy enemy = target.GetComponent<Enemy>();
		int currentDamage = (int)Random.Range(invocation.MaxDamage, invocation.MinDamage);
		enemy.RecibeDano(currentDamage);
		OnAttackEnemy.Invoke();

		//TODO: Check with enemy if he think is dead;
		if (enemy.saludActual <= 0)
		{
			HandleDeadEnemy(target);
			if(enemiesInAttackArea.Count > 0) ChoseTargetToAttack();
		}
	}

	GameObject ChoseTargetToAttack()
	{
		
		//Si sólo hay un enemigo en el area -> Ese es el enemigo
		if (enemiesInAttackArea.Count == 1)
		{
			target = enemiesInAttackArea[0];
			return enemiesInAttackArea[0];
		}

		//Si hay más de un enemigo en el área y no hay target
		//Busca qué enemigo tiene menos vida:
		List<GameObject> weakEnemies = new();

		float lowestHealth = enemiesInAttackArea[0].GetComponent<EnemyHealth>().Health;

		enemiesInAttackArea.ForEach(enemy =>
		{
			float enemyHealth = enemy.GetComponent<EnemyHealth>().Health;
			if (enemyHealth < lowestHealth)
			{
				weakEnemies.Clear();
				weakEnemies.Add(enemy);
			}
			else if (enemyHealth == lowestHealth)
			{
				weakEnemies.Add(enemy);
			}
		});

		//Si sólo hay un enemigo débil, atácalo
		if (weakEnemies.Count == 1)
		{
			target = weakEnemies[0];
			return weakEnemies[0];
		}
		//Si hay más de un enemigo con la misma vida, ataca al más cercano
		if (weakEnemies.Count > 1)
		{
			target = GetClosestEnemy(weakEnemies);
			return GetClosestEnemy(weakEnemies);
		}
		//Como último recurso, ataca al más cercano
		target = GetClosestEnemy(enemiesInAttackArea);
		return GetClosestEnemy(enemiesInAttackArea);
	}

	GameObject GetClosestEnemy(List<GameObject> enemies)
	{
		Vector3 currentPosition = gameObject.transform.position;
		float lowerDistance = Vector3.Distance(enemies[0].transform.position, currentPosition);
		GameObject closestEnemy = enemies[0];

		enemies.ForEach(enemy =>
		{
			float enemyDistance = Vector3.Distance(enemy.transform.position, currentPosition);
			if (enemyDistance < lowerDistance)
				lowerDistance = enemyDistance;
			closestEnemy = enemy;
		});

		return closestEnemy;
	}

	void HandleDeadEnemy(GameObject enemy)
	{
		target = null;
		enemiesInAttackArea.Remove(enemy);
	}
	#endregion


}
