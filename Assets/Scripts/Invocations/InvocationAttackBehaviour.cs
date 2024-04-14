using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvocationAttackBehaviour : MonoBehaviour
{
	//TODO: Avisar al InvocationBehaviour cuando todos los enemigos están muertos.
	[SerializeField] InvocationModel invocation;

	//ATTACK
	int livingTarget = 0;
	bool canAttack = false;
	bool cooldownIsActive;
	float coolDownDuration = 0f;
	readonly List<GameObject> enemiesInAttackArea = new();
	GameObject target;

	public UnityEvent OnAttackEnemy;
	public UnityEvent OnDestroyAllEnemiesInAttackArea;
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
		//Si no tienes target y hay enemigos rodeando
		if (!target && enemiesInAttackArea.Count > 0)
			ChoseTargetToAttack();

		//Si no hay enemigos en el área y está atacando, deja de atacar:
		if (canAttack && livingTarget<1)
		{
			Debug.Log("TODOS LOS ENEMIGOS EN EL ÁREA DESTRUÍDOS");
			target = null;
			canAttack = false;
			OnDestroyAllEnemiesInAttackArea.Invoke();
		}

		if( canAttack && livingTarget > 0)
		{
			TryToAttackTarget();
		}

	}

	private void OnTriggerEnter(Collider other)
	{
		if (CheckIfColliderIsEnemy(other))
		{
			canAttack = true;
			livingTarget++;
			enemiesInAttackArea.Add(other.gameObject);
			OnEnemyEntryArea.Invoke();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (CheckIfColliderIsEnemy(other))
		{
			livingTarget--;
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
		if (target && target.activeSelf && canAttack && !cooldownIsActive)
		{
			StartCoroutine(StartCooldown());
			Attack(target);
		}
	}

	void Attack(GameObject target)
	{

		Debug.Log("Attacking");
		target = enemiesInAttackArea.Find(enemy => enemy.activeSelf);
		Enemy enemy = target.GetComponent<Enemy>();
		int currentDamage = (int)Random.Range(invocation.MaxDamage, invocation.MinDamage);
		enemy.RecibeDano(currentDamage);
		OnAttackEnemy.Invoke();
        AudioManager.instance.PlaySFX("summonAttack");

        //TODO: Check with enemy if he think is dead;
        if (enemy.saludActual <= 0)
		{
			Debug.Log("ENEMIGO MUERTO");
			HandleDeadEnemy(target);
			if (enemiesInAttackArea.Count > 0) ChoseTargetToAttack();
		}
	}

	GameObject ChoseTargetToAttack()
	{
		target = enemiesInAttackArea.Find(enemy => enemy.activeSelf);
		if(target && !target.activeSelf) target = null;
		if(target == null) livingTarget = 0;
		return enemiesInAttackArea.Find(enemy => enemy.activeSelf);

		/* //Si sólo hay un enemigo en el area -> Ese es el enemigo
		if (enemiesInAttackArea.Count == 1 && enemiesInAttackArea[0].activeSelf)
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
			if (enemy.activeSelf)
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
			}
		});

		//Si sólo hay un enemigo débil, atácalo
		if (weakEnemies.Count == 1 && weakEnemies[0].activeSelf)
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
		return GetClosestEnemy(enemiesInAttackArea); */
	}

	GameObject GetClosestEnemy(List<GameObject> enemies)
	{
		Vector3 currentPosition = gameObject.transform.position;
		float lowerDistance = Vector3.Distance(enemies[0].transform.position, currentPosition);
		GameObject closestEnemy = enemies[0];

		enemies.ForEach(enemy =>
		{
			if (enemy.activeSelf)
			{
				float enemyDistance = Vector3.Distance(enemy.transform.position, currentPosition);
				if (enemyDistance < lowerDistance)
					lowerDistance = enemyDistance;
				closestEnemy = enemy;
			}
		});

		return closestEnemy;
	}

	void HandleDeadEnemy(GameObject enemy)
	{
		Debug.Log("Target dead. Lo seteamos a null");
		target = null;
		livingTarget--;
		//enemiesInAttackArea.Remove(enemy);
	}

	#endregion


}
