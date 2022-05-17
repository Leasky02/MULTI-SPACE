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

    //time scale to go to
    public static float requiredTimeScale = 1f;

    private void Start()
    {
        //reset game over state
        gameOver = false;
        //reset bullet and gun 
        Bullet.damage = 50;
        Gun.fireRate = 1.3f;

        if(MultiplayerManager.playerCount == 2)
        {
            //reset player health states
            PlayerHealth.oneRemaining = false;
        }
    }

    private void Update()
    {
        //lerp to required time scale
        Time.timeScale = Mathf.Lerp(Time.timeScale, requiredTimeScale, Time.deltaTime * 30);
    }

    public void GameOver()
    {
        gameOver = true;

        //stop time
        requiredTimeScale = 0f;

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

        //play sound
        GetComponent<AudioSource>().Play();
    }    
}
