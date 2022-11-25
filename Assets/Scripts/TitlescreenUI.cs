using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

//Game UI template

public class TitlescreenUI : MonoBehaviour
{
    public GameObject titleScreen;
    
    public Button PlayButton;
    public Button HighscoresButton;
    public Button QuitButton;

    public bool isGameActive;
    void Start()
    {
        //shows UI
        titleScreen.gameObject.SetActive(true);


    }

    // Update is called once per frame
    void Update()
    {
        
        
        
            //connects buttons
        
        PlayButton.onClick.AddListener(Play);
        HighscoresButton.onClick.AddListener(Highscores);
        QuitButton.onClick.AddListener(Quit);
        
    }


    void Play()
    {
        //connect to game Manager
        Debug.Log("Play Game");
        hide();
    }

    void  Highscores()
    {
        Debug.Log("Show Highscores");
    }

    //hides the UI object
    void hide()
    {
     titleScreen.gameObject.SetActive(false);   
    }

    void Quit()
    {
        Debug.Log("Quit");

        //unity editor quiter
        UnityEditor.EditorApplication.isPlaying = false;

        //game quiter
        Application.Quit();
    }
}
