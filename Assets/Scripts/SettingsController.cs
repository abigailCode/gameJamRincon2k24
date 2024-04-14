using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    // Referencia al dropdown para elegir las canciones
    //[SerializeField] TMP_Dropdown dropdown;
    private int dropdownValue;
    // Referencia al slider de musica
    [SerializeField] Slider sliderMusic;
    // Referencia al slider de efectos
    [SerializeField] Slider sliderSFX;
    // Referencia a los AudioSource
    AudioSource sourceMusic;
    AudioSource sourceSFX;

    int savedSong;
    float savedMusicVolume;
    float savedSFXVolume;
    // Start is called before the first frame update
    void Start()
    {
        //dropdown = dropdown.GetComponent<TMP_Dropdown>();
        sourceMusic = AudioManager.instance.musicSource;
        sourceSFX = AudioManager.instance.sfxSource;
        // Recupera la configuración del volumen (si existe) o utiliza un valor predeterminado
        savedMusicVolume = PlayerPrefs.GetFloat("BackgroundMusicVolume", 0.5f);
        savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        // Recupera la configuración de la canción
        //savedSong = PlayerPrefs.GetInt("BackgroundMusicSong");
        LoadMusicVolume(savedMusicVolume); // Configuramos el volumen cargado
        LoadSFXVolume(savedSFXVolume); // Configuramos el estado de la última canción escogida

    }

    void LoadMusicVolume(float volume)
    { // Método para configurar un volumen específico en el Slider
        sliderMusic.value = volume; // Configuramos el Slider como corresponda
        sourceMusic.volume = volume; // Configuramos el AudioSource como corresponda
    }

    void LoadSFXVolume(float volume)
    {   // Método para configurar un volumen específico en el Slider
        sliderSFX.value = volume; // Configuramos el Slider como corresponda
        sourceSFX.volume = volume; // Configuramos el AudioSource como corresponda
    }

  

    //void LoadSFXVolume(int song)
    //{ // Método para configurar un volumen específico en el Slider
    //    dropdown.value = song; // Configuramos el dropdown como corresponda
    //    MusicSelect(); // Cargamos la canción que corresponda
    //}

    // Update is called once per frame
    void Update()
    {

    }

    // Método para subir y bajar el volumen en base al slider. Se
    // llama desde OnValueChanged() del GameObject con el Slider
    public void ChangeMusicVolume()
    {
        // Configuramos el volumen con el valor del slider
        sourceMusic.volume = sliderMusic.value;
        PlayerPrefs.SetFloat("BackgroundMusicVolume", sourceMusic.volume);
    }

    public void ChangeSFXVolume()
    {
        // Configuramos el volumen con el valor del slider
        sourceSFX.volume = sliderSFX.value;
        PlayerPrefs.SetFloat("SFXVolume", sourceSFX.volume);
    }

    
     
    //public void MusicSelect()
    //{
    //    if (PlayerPrefs.GetInt("BackgroundMusicSong") != dropdown.value)
    //    {
    //        switch (dropdown.value)
    //        {
    //            case 0:
    //                PlayerPrefs.SetInt("BackgroundMusicSong", dropdown.value);
    //                AudioManager.instance.PlayMusic("BackgroundMusic1");
    //                break;
    //            case 1:
    //                PlayerPrefs.SetInt("BackgroundMusicSong", dropdown.value);
    //                AudioManager.instance.PlayMusic("BackgroundMusic2");
    //                break;
    //            case 2:
    //                PlayerPrefs.SetInt("BackgroundMusicSong", dropdown.value);
    //                AudioManager.instance.PlayMusic("BackgroundMusic3");
    //                break;
    //            case 3:
    //                PlayerPrefs.SetInt("BackgroundMusicSong", dropdown.value);
    //                AudioManager.instance.PlayMusic("BackgroundMusic4");
    //                break;
    //            case 4:
    //                PlayerPrefs.SetInt("BackgroundMusicSong", dropdown.value);
    //                AudioManager.instance.PlayMusic("BackgroundMusic5");
    //                break;
    //        }

    //    }

    //}
    

    public void SaveSettings()
    { // Metodo para guardar los PlayerPrefs
        PlayerPrefs.Save(); // Guarda los PlayerPrefs
        //SCManager.instance.LoadSceneByName("Menu");
    }
}
