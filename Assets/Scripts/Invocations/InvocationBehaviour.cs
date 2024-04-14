using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.AI;

public class InvocationBehaviour : MonoBehaviour
{
	[SerializeField] InvocationModel invocation;
	GameObject target;
	GameObject player;
	GameObject[] enemiesInScene;

	//MOVEMENT
	NavMeshAgent navAgent;
	Vector3 stopPoint;


	InvocationState state = InvocationState.Idle;

	void Awake()
	{
		navAgent = gameObject.GetComponent<NavMeshAgent>();
	}

	void OnEnable()
	{
		Enemy.OnDeadEnemy += HandleDeadEnemy;
	}
	void OnDisable()
	{
		Enemy.OnDeadEnemy -= HandleDeadEnemy;
	}

	private void Start()
	{
		navAgent.speed = invocation.MovementSpeed;
		enemiesInScene = GetAllEnemiesInScene();
		player = GameObject.FindGameObjectWithTag("Player");
		SetTaget();
	}

	private void FixedUpdate()
	{
		//Dirigirse al target
		switch (state)
		{
			case InvocationState.Idle:
				SetTaget();
				break;
			case InvocationState.ChasingEnemy:
				ChaseEnemy();
				break;
			case InvocationState.FollowingPlayer:
				FollowPlayer();
				break;
			case InvocationState.Attacking:
				StopMovements();
				break;
			case InvocationState.Dead:
			default:
				break;
		}
	}

	#region PUBLIC METHODS
	public void SetAttackTarget(GameObject target)
	{
		state = InvocationState.Attacking;
		this.target = target;
	}

	public void SetAttackEnd() => state = InvocationState.Idle;

	public void SetOnAttack() => state = InvocationState.Attacking;
	public void SetDead()
	{
		state = InvocationState.Dead;
	}

	#endregion

	#region PRIVATE METHODS
	void SetTaget()
	{
		if (GetAllEnemiesInScene().Length < 1)
		{
			state = InvocationState.FollowingPlayer;
		}
		target = GetCloseEnemy();
		state = InvocationState.ChasingEnemy;

		if (!target)
		{
			target = player;
			state = InvocationState.FollowingPlayer;
		}

		//stopPoint = new Vector3(target.transform.position.x + attackRange, target.transform.position.y, gameObject.transform.position.z);
		stopPoint = target.transform.position;
	}
	GameObject GetCloseEnemy()
	{
		//enemiesInScene = GetAllEnemiesInScene();
		//Inicializamos la distancia más cercana con un enemigo aleatorio. El primero en la lista.
		if (enemiesInScene.Length > 0)
		{

			float closestDistance = Vector3.Distance(enemiesInScene[0].transform.position, gameObject.transform.position);
			GameObject closestEnemy = GetAllEnemiesInScene()[0];

			foreach (GameObject enemy in enemiesInScene)
			{
				// Calcular la distancia entre el jugador y el enemigo actual
				float distance = Vector3.Distance(enemy.transform.position, gameObject.transform.position);

				// Si la distancia actual es menor que la distancia más cercana encontrada hasta ahora
				if (distance < closestDistance)
				{
					// Actualizar el enemigo más cercano y su distancia
					closestEnemy = enemy;
					closestDistance = distance;
				}
			}
			return closestEnemy;
		}
		state = InvocationState.Idle;
		return null;
	}

	void ChaseEnemy()
	{
		enemiesInScene = GetAllEnemiesInScene();
		if (!target || !target.activeSelf)
		{
			target = null;
			state = InvocationState.Idle;
			return;
		}
		//Reducir la distancia entre él y el enemigo
		navAgent.SetDestination(stopPoint);
		if (gameObject.transform.position == stopPoint)
			state = InvocationState.Attacking;
	}
	//TODO
	void FollowPlayer()
	{
		Debug.Log("FOLLOWING PLAYER");
		navAgent.SetDestination(player.transform.position);
	}

	void StopMovements()
	{
		navAgent.isStopped = true;
	}

	GameObject[] GetAllEnemiesInScene()
	{
		GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

		return Array.FindAll(allEnemies, (enemy) => enemy.activeSelf);
	}

	void HandleDeadEnemy(GameObject enemy)
	{
		if (target == enemy)
		{
			target = null;
			state = InvocationState.Idle;
		}
		enemiesInScene = Array.FindAll(enemiesInScene, (enemy) => enemy.activeSelf);
	}

	#endregion
}




