using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvocationHealthController : MonoBehaviour
{
	[SerializeField] InvocationModel invocation;
	
	//LIFE
	float currentHealth;
	bool isDead;
	[Tooltip("El tiempo que transcurre entre que muere y que desaparece de la escena")]
	[SerializeField] float seccondsToDestroyOnDead = 5;

	public UnityEvent OnInvocationDead;

	void Awake()
	{
		currentHealth = invocation.Health;
	}

	public void SetDamage(float damage){
		Debug.Log("RECIBIENDO DAÑO: " + damage);
		if(isDead) return;

		currentHealth -= damage;
		if(currentHealth <= 0) {
			isDead = true;
			OnInvocationDead.Invoke();
			DestroyOnDeath();
		}
	}

	void DestroyOnDeath()
	{
		GameObject.Destroy(this, seccondsToDestroyOnDead);
	}

}
