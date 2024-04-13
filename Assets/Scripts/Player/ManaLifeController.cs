using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ManaLifeController : MonoBehaviour
{

    // Variable pública para modificar desde cualquier script
    public float life = 100;
    public float maxLife = 100;
    public float mana = 100;
    public float maxMana = 100;

    [SerializeField] GameObject HPBar;
    [SerializeField] GameObject MPBar;

    private Image lifeImg;
    private Image manaImg;


    // Start is called before the first frame update
    void Start()
    {
        lifeImg = HPBar.GetComponent<Image>();
        manaImg = MPBar.GetComponent<Image>();
        StartCoroutine(ChargeMana());
    }

    // Update is called once per frame
    void Update()
    {

    }


    // Método para sumar vida al Player
    public void restoreHealth(float health)
    {
        life = Mathf.Clamp(life + health, 0, maxLife);
        // Actualización de la barra de vida
        lifeImg.fillAmount = life / maxLife;
    }

    // Método para restar vida al Player
    public void TakeDamage(float damage)
    {
        life = Mathf.Clamp(life - damage, 0, maxLife);
        // Actualización de la barra de vida
        lifeImg.fillAmount = life / maxLife;
    }

    // Método para restar mana al Player
    public void TakeMana(float invocationMana)
    {
        mana = Mathf.Clamp(mana - invocationMana, 0, maxMana);
        // Actualización de la barra de mana
        manaImg.fillAmount = mana / maxMana;
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

    IEnumerator ChargeMana()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            mana = Mathf.Clamp(mana + 5f, 0, maxMana);
        }
    }
}
