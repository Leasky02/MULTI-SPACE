using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    //is game over
    public static bool gameOver;
    //score manager
    [SerializeField] private GameObject scoreManager;
    //button action manager
    [SerializeField] private GameObject buttonActionManager;
    [SerializeField] private GameObject pauseScreenManager;
    //UI canvas
    [SerializeField] private GameObject p1EndGameCanvas;
    [SerializeField] private GameObject p2EndGameCanvas;

    [SerializeField] private GameObject inGameCanvas;
    //button to select when game ends
    [SerializeField] private Button p1SelectedButton;
    [SerializeField] private Button p2SelectedButton;

    private void Start()
    {
        gameOver = false;
    }

    public void GameOver()
    {
        gameOver = true;

        //stop Time
        Time.timeScale = 0f;

        //if 1 player game
        if(MultiplayerManager.playerCount == 1)
        {
            //Switch canvas
            buttonActionManager.GetComponent<ButtonActions>().SwitchMenu(inGameCanvas, p1EndGameCanvas);
            //select button as default
            p1SelectedButton.Select();
            //save player 1 score
            scoreManager.GetComponent<ScoreManager>().SaveP1Score();
        }
        //else, its a 2 player game
        else
        {
            //Switch canvas
            buttonActionManager.GetComponent<ButtonActions>().SwitchMenu(inGameCanvas, p2EndGameCanvas);
            //select button as default
            p2SelectedButton.Select();
            //save player 2 score
            scoreManager.GetComponent<ScoreManager>().SaveP2Score();
        }

    }    
}
