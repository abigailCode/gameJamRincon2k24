using System.Collections;
using System.Collections.Generic;
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

	private void Start()
	{
		navAgent.speed = invocation.MovementSpeed;
		enemiesInScene = GameObject.FindGameObjectsWithTag("Enemy");
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
				//TODO
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

	#endregion

	#region PRIVATE METHODS
	void SetTaget()
	{
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
		//TODO: SI TODOS LOS ENEMIGOS ESTÁN MUERTOS (NO HAY ENEMIGOS) NO HACER ESTO
		//Inicializamos la distancia más cercana con un enemigo aleatorio. El primero en la lista.
		float closestDistance = Vector3.Distance(enemiesInScene[0].transform.position, gameObject.transform.position);
		GameObject closestEnemy = enemiesInScene[0];

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

	void ChaseEnemy()
	{
		//Reducir la distancia entre él y el enemigo
		navAgent.SetDestination(stopPoint);
		if (gameObject.transform.position == stopPoint)
			state = InvocationState.Attacking;

	}
	void FollowPlayer() { }

	void Dead() { }

	#endregion
}




