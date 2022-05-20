using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    //UI screens
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject gameScreen;

    //button to be selected on start
    [SerializeField] private Button resumeButton;

    //whether game can be paused or not
    public bool canPause = true;

    public static bool paused = false;

    // Update is called once per frame
    void Update()
    {
        //if pause input is pressed
        if(Input.GetButtonDown("Pause"))
        {
            //if can pause and game isnt over
            if(canPause && !EndGame.gameOver)
            {
                //pause game
                Pause();
            }
        }
    }

    private void Pause()
    {
        EndGame.requiredTimeScale = 0f;
        //disable pausing again
        canPause = false;

        //pause time
        Time.timeScale = 0f;

        //disable inGameScreen
        gameScreen.SetActive(false);
        //enable pause screen
        pauseScreen.SetActive(true);

        //set active button
        resumeButton.Select();
    }

    public void UnPause()
    {
        //pause time
        Time.timeScale = 1f;
        EndGame.requiredTimeScale = 1f;

        //enable inGameScreen
        gameScreen.SetActive(true);
        //disable pause screen
        pauseScreen.SetActive(false);

        //delay allowing game to pause
        Invoke("AllowPause", 0.5f);
    }

    private void AllowPause()
    {
        canPause = true;
    }
}
