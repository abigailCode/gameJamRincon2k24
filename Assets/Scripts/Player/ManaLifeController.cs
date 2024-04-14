using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Enemy;
public class ManaLifeController : MonoBehaviour {
    // Variable pública para modificar desde cualquier script
    public string nextLevel;

    public float life = 100;
    public float maxLife = 100;
    public float mana = 100;
    public float maxMana = 100;

    [SerializeField] GameObject HPBar;
    [SerializeField] GameObject MPBar;

    [SerializeField] float timePlayerDead;
    [SerializeField] float timeChargeMana;
    [SerializeField] float chargeManaAmount;

    private Image lifeImg;
    private Image manaImg;
    private bool isDead = false;

    private float deathRotationSpeed = 50f;


    void Start()
    {
        StartCoroutine(ChargeMana());
        lifeImg = HPBar.GetComponent<Image>();
        manaImg = MPBar.GetComponent<Image>();
    }

    //SUSCRIPCIÓN al EVENTO
    void OnEnable() {
        //Enemy.OnDoDamage += TakeDamage;
        Enemy.OnDamagePlayer += TakeDamage;
    }
    //DESUSCRIPCIÓN al EVENTO
    void OnDisable() {
        //Enemy.OnDoDamage -= TakeDamage;
        Enemy.OnDamagePlayer -= TakeDamage;
    }


    // Método para restar vida al Player
    private void TakeDamage(float damage)
    {
        life = Mathf.Clamp(life - damage, 0, maxLife);
        // Actualización de la barra de vida
        lifeImg.fillAmount = life / maxLife;

        CheckSelfDead();
    }

    // Método para restar mana al Player
    public void TakeMana(float invocationMana)
    {
        mana = Mathf.Clamp(mana - invocationMana, 0, maxMana);
        // Actualización de la barra de mana
        manaImg.fillAmount = mana / maxMana;
    }

    IEnumerator ChargeMana() {
        while (true) {
            yield return new WaitForSeconds(timeChargeMana);
            mana = Mathf.Clamp(mana + chargeManaAmount, 0, maxMana);
            // Actualización de la barra de mana
            manaImg.fillAmount = mana / maxMana;
        }
    }

    private void CheckSelfDead() {
        if (life <= 0 && !isDead) {
            //Animación de muerte
            Debug.Log("Rotando");
            float elapsedTime = 0f;
            Quaternion startRotation = transform.rotation;
            Quaternion endRotation = Quaternion.Euler(90f, 0f, 0f); // Rotación final cuando el jugador está muerto

            while (elapsedTime < 1f) {
                // Interpola entre la rotación inicial y final para crear una animación de muerte suave
                transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime);
                elapsedTime += Time.deltaTime * deathRotationSpeed;
            }

            //Espera un tiempo
            StartCoroutine(WaitDeadAndGameOver());
        }
    }

    IEnumerator WaitDeadAndGameOver() {
        // Desactiva el control del jugador
        PlayerController playerController = GetComponent<PlayerController>();
        playerController.enabled = false;

        yield return new WaitForSeconds(timePlayerDead);

        //Destruye al jugador
        Destroy(gameObject);

        //Cambia de escena
        SceneController.instance.LoadScene(nextLevel);
    }

    #region Getters
    public float getLife()
    {
        return life;
    }

    public float getMana()
    {
        return mana;
    }
    #endregion


}
