using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;


//System IO for loading and Saving wallet Data
using System.IO;

//Game UI template

public class TitlescreenUI : MonoBehaviour
{
    public GameObject titleScreen;
    
    public Button PlayButton;
    public Button HighscoresButton;
    public Button QuitButton;

    public bool isGameActive;
    
    //for saving and loading Local highscores data
    private string saveFile; 
    public GameManager GameManagerX;
    
    void loadGameManager()
    {
        //get the game manager
        
        GameManagerX = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        GameManagerX.DoSomething();

    }


    void Start()
    {
        
        loadGameManager();

        //shows UI
        titleScreen.gameObject.SetActive(true);

        //loads local highscore .json 
        saveFile = UnityEngine.Application.persistentDataPath + "/highscore.json"; 


        if (File.Exists(saveFile) == false)
        {
            //Saves Score Info
            SaveHighScore(string.Format("Highscore: "+ 0));

        }

        if (File.Exists(saveFile) == true) {LoadHighscore();}



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

    //Logs to Highscore variable to console
    void  Highscores()
    {
        if (loaded_highscore != "")
        {
            Debug.Log("Highscore: "+ loaded_highscore );
        }
        if (loaded_highscore == "")
        {
            Debug.Log("Highscore: 0");
        }
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

    public void SaveHighScore(string Highscores )
    {
        File.WriteAllText(saveFile, Highscores.ToString());

    }

    
    //loads Highscore from local storage
    public string loaded_highscore ;
    public void LoadHighscore()
    {   //check if files exist
        if (File.Exists(saveFile))
        {
            // Read the Entire file and save its contents
            string fileContents = File.ReadAllText(saveFile);

            //Print Account Details
            Debug.Log(fileContents.ToString());

            //loads account info to a variable
            loaded_highscore = fileContents;

        }
    }

}
