using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Invocation_", menuName = "Summoning/New Invocation", order = 0)]
public class InvocationModel : ScriptableObject {
	[SerializeField] private string invocationName = "Test";
	[SerializeField] private float maxHealth = 100;

	[Space]
	[Header("Invocation")]
	[Tooltip("Coste de invotar a esta criatura")]
	[SerializeField] float manaCost = 5;
	[Tooltip("Tiempo que hay que esperar hasta invocar otra criatura de este tipo")]
	[SerializeField] float invocationCooldown = 1;

	[Space]
	[Header("Attack")]
	[SerializeField] private float maxDamage = 10;
	[SerializeField] private float minDamage = 1;
	[Tooltip("Distancia desde la cual puede inflingir daño al enemigo. A partir de esa distancia, no seguirá acercándose al enemigo")]
	[SerializeField] private float attackRange = 0.3f;
	[SerializeField] private float cooldownAttack = 0.3f;

	[Space]
	[Header("Movement")]
	[SerializeField] private float movementSpeed = 5f;

	public string GetName => invocationName;
	public float Health => maxHealth;
	public float ManaCost => manaCost;
	public float MaxDamage => maxDamage;
	public float MinDamage => minDamage;
	public float MovementSpeed => movementSpeed;
	public float AttackCoolDown => cooldownAttack;
	public float AttackRange => attackRange;
}
