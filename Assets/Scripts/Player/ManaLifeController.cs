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

    //[SerializeField] GameObject HPBar;
    //[SerializeField] GameObject MPBar;

    [SerializeField] int invocationMana1;

    private Image lifeImg;
    private Image manaImg;

    //[SerializeField] GameObject creature1;

    // Start is called before the first frame update
    /*void Start()
    {
        lifeImg = HPBar.GetComponent<Image>();
        manaImg = MPBar.GetComponent<Image>();
    }*/

    // Update is called once per frame
    void Update()
    {

        // Código para probar que la barra de vida funciona
        if (Input.GetKey(KeyCode.Q)) life = life + 0.25f;
        if (Input.GetKey(KeyCode.E)) life = life - 0.25f;

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
        Debug.Log(mana);
        // Actualización de la barra de mana
        //manaImg.fillAmount = mana / maxMana;
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
