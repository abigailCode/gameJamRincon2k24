using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{	
	[SerializeField] float health = 100f;
	[SerializeField] bool isDead;

	public bool IsDead => isDead;
	public float Health => health;

	public void SetDamage(float damage){
		health -= damage;
		if(health <= 0) 
			isDead = true;
	}



}
