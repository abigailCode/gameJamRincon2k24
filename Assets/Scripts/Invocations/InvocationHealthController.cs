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


	public UnityEvent OnInvocationDead;

	void Awake()
	{
		currentHealth = invocation.Health;
	}

	public void SetDamage(float damage){
		if(isDead) return;

		currentHealth -= damage;
		if(currentHealth <= 0) {
			isDead = true;
			OnInvocationDead.Invoke();
		}
	}

}
