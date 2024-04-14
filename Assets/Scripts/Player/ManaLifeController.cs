using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Enemy;
public class ManaLifeController : MonoBehaviour {
    // Variable p�blica para modificar desde cualquier script
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

    [SerializeField] float deathRotationSpeed = 100f;

    private Image lifeImg;
    private Image manaImg;
    private bool isDead = false;



    void Start()
    {
        StartCoroutine(ChargeMana());
        lifeImg = HPBar.GetComponent<Image>();
        manaImg = MPBar.GetComponent<Image>();
    }

    //SUSCRIPCI�N al EVENTO
    void OnEnable() {
        //Enemy.OnDoDamage += TakeDamage;
        Enemy.OnDamagePlayer += TakeDamage;
    }
    //DESUSCRIPCI�N al EVENTO
    void OnDisable() {
        //Enemy.OnDoDamage -= TakeDamage;
        Enemy.OnDamagePlayer -= TakeDamage;
    }


    // M�todo para restar vida al Player
    private void TakeDamage(float damage)
    {
        life = Mathf.Clamp(life - damage, 0, maxLife);
        // Actualizaci�n de la barra de vida
        lifeImg.fillAmount = life / maxLife;

        CheckSelfDead();
    }

    // M�todo para restar mana al Player
    public void TakeMana(float invocationMana)
    {
        mana = Mathf.Clamp(mana - invocationMana, 0, maxMana);
        // Actualizaci�n de la barra de mana
        manaImg.fillAmount = mana / maxMana;
    }

    IEnumerator ChargeMana() {
        while (true) {
            yield return new WaitForSeconds(timeChargeMana);
            mana = Mathf.Clamp(mana + chargeManaAmount, 0, maxMana);
            // Actualizaci�n de la barra de mana
            manaImg.fillAmount = mana / maxMana;
        }
    }

    private void CheckSelfDead() {
        if (life <= 0 && !isDead) {
            //Animaci�n de muerte
            PlayerRotationAnim();
            //Espera un tiempo
            StartCoroutine(WaitDeadAndGameOver());
        }
    }

    private void PlayerRotationAnim() {
        float elapsedTime = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(180f, 0f, 0f); // Rotaci�n final cuando el jugador est� muerto

        while (elapsedTime < 5f) {
            // Interpola entre la rotaci�n inicial y final para crear una animaci�n de muerte suave
            //transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime);
            //elapsedTime += Time.deltaTime * deathRotationSpeed;

            
            // Rotar el objeto constantemente en el eje Y
            //transform.Rotate(Vector3.up, deathRotationSpeed * Time.deltaTime);
        }
    }

    IEnumerator WaitDeadAndGameOver() {
        // Rotar el objeto constantemente en el eje Y
        //transform.Rotate(Vector3.up, deathRotationSpeed * Time.deltaTime);


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
