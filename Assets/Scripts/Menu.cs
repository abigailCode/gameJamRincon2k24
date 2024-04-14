using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject CreditsPanel;
    public GameObject SettingsPanel;
    public GameObject InstructionsPanel;


    void Start()
    {
        //AudioManager.instance.PlayMusic("menu");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Test()
    {
        SceneController.instance.LoadScene("Victory");
    }

    public void StartGame()
    {
        AudioManager.instance.PlayMusic("game");
        //GameManager.instance.ResetState();
        SceneController.instance.LoadScene("Level1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    //CREDITS
    public void ShowCredits()
    {
        CreditsPanel.SetActive(true);
    }

    public void HideCredits()
    {
        CreditsPanel.SetActive(false);
    }
      public void ShowSettings()
    {
        SettingsPanel.SetActive(true);
    }

    public void HideSettings()
    {
        SettingsPanel.SetActive(false);
    }

    public void GoToMenu()
    {
        SceneController.instance.LoadScene("Menu");
    }

    public void ShowInstructions()
    {
        InstructionsPanel.SetActive(true);
    }

    public void HideInstructions()
    {
        InstructionsPanel.SetActive(false);
    }
}
