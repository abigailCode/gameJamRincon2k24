using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Enemy;
public class ManaLifeController : MonoBehaviour {
// Variable p�blica para modificar desde cualquier script
public float life = 100;
    public float maxLife = 100;
    public float mana = 100;
    public float maxMana = 100;

    [SerializeField] GameObject HPBar;
    [SerializeField] GameObject MPBar;

    [SerializeField] float timeChargeMana;
    [SerializeField] float chargeManaAmount;

    private Image lifeImg;
    private Image manaImg;


    void Start()
    {
        StartCoroutine(ChargeMana());
        lifeImg = HPBar.GetComponent<Image>();
        manaImg = MPBar.GetComponent<Image>();
    }

    //SUSCRIPCI�N al EVENTO
    void OnEnable() {
    //    Enemy.OnDoDamage += TakeDamage;
    }
    //DESUSCRIPCI�N al EVENTO
    void OnDisable() {
    //    Enemy.OnDoDamage -= TakeDamage;
    }


    // M�todo para restar vida al Player
    private void TakeDamage(float damage)
    {
        life = Mathf.Clamp(life - damage, 0, maxLife);
        // Actualizaci�n de la barra de vida
        lifeImg.fillAmount = life / maxLife;
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
