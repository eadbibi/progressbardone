using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //Scene manager unity package
using UnityEngine.UI; //UI Elementer package

public class Pause : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu; //The variable is private, but shows up in the editor windoe in Unity


    public void PauseMenu()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; //Time scale sat to 0 to stop time in Unity (meaning the game is paused)

    }


    public void Resume()
    {
        pauseMenu.gameObject.SetActive(false); //When the resume button ("Genoptag spillet") is pressed, the pauseMenu panel should disable and resume the game
        Time.timeScale = 1f; //Time scale sat to 1 to resume time in Unity (meaning the game is unpaused, and time is sat as it was before the game was paused)
    }


    public void MainMenu(string SceneID)
    {
        //When you pressed the main menu button ("Hovedmenu") it takes you back to the main menu
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneID);
    }


}

