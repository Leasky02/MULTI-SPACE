using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    //MULTIPLAYER end screen score displays
    [SerializeField] private Text p1Score_TXT;
    [SerializeField] private Text p2Score_TXT;
    [SerializeField] private Text totalScore_TXT;
    //SINGLEPLAYER
    [SerializeField] private Text soloTotalScore_TXT;

    //leaderboard text variables
    [SerializeField] private Text[] p1HighScore_TXT = new Text[3];
    [SerializeField] private Text[] p2HighScore_TXT = new Text[3];
    [SerializeField] private Text[] p1Wave_TXT = new Text[3];
    [SerializeField] private Text[] p2Wave_TXT = new Text[3];
    [SerializeField] private GameObject[] p1Highlight_TXT = new GameObject[3];
    [SerializeField] private GameObject[] p2Highlight_TXT = new GameObject[3];

    //players
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
        
    //score variables
    private int totalScore;

    //saves player 1's score
    public void SaveP1Score()
    {
        //get total score
        totalScore = player1.GetComponent<PlayerScore>().score;
        //set displays of score
        soloTotalScore_TXT.text = ("" + totalScore);

        //if score is greater than 1st score
        if(totalScore > PlayerPrefs.GetInt("P1_Score_1ST"))
        {
            //save each score to the one below
            PlayerPrefs.SetInt("P1_Score_3RD", PlayerPrefs.GetInt("P1_Score_2ND"));
            PlayerPrefs.SetInt("P1_Score_2ND", PlayerPrefs.GetInt("P1_Score_1ST"));
            PlayerPrefs.SetInt("P1_Score_1ST", totalScore);

            //save each wave to the one below
            PlayerPrefs.SetInt("P1_Wave_3RD", PlayerPrefs.GetInt("P1_Wave_2ND"));
            PlayerPrefs.SetInt("P1_Wave_2ND", PlayerPrefs.GetInt("P1_Wave_1ST"));
            PlayerPrefs.SetInt("P1_Wave_1ST", WaveSystem.wave);

            //highlight first place
            p1Highlight_TXT[0].SetActive(true);
        }
        //else if greater than 2nd score
        else if(totalScore > PlayerPrefs.GetInt("P1_Score_2ND"))
        {
            //save each score to the one below
            PlayerPrefs.SetInt("P1_Score_3RD", PlayerPrefs.GetInt("P1_Score_2ND"));
            PlayerPrefs.SetInt("P1_Score_2ND", totalScore);

            //save each wave to the one below
            PlayerPrefs.SetInt("P1_Wave_3RD", PlayerPrefs.GetInt("P1_Wave_2ND"));
            PlayerPrefs.SetInt("P1_Wave_2ND", WaveSystem.wave);

            //highlight second place
            p1Highlight_TXT[1].SetActive(true);
        }
        //else if greater than 3rd score
        else if(totalScore > PlayerPrefs.GetInt("P1_Score_3RD"))
        {
            //save each score to the one below
            PlayerPrefs.SetInt("P1_Score_3RD", totalScore);

            //save each wave to the one below
            PlayerPrefs.SetInt("P1_Wave_3RD", WaveSystem.wave);

            //highlight third place
            p1Highlight_TXT[2].SetActive(true);
        }

        //set leaderboard text
        p1HighScore_TXT[0].text = ("" + PlayerPrefs.GetInt("P1_Score_1ST"));
        p1HighScore_TXT[1].text = ("" + PlayerPrefs.GetInt("P1_Score_2ND"));
        p1HighScore_TXT[2].text = ("" + PlayerPrefs.GetInt("P1_Score_3RD"));

        p1Wave_TXT[0].text = ("" + PlayerPrefs.GetInt("P1_Wave_1ST"));
        p1Wave_TXT[1].text = ("" + PlayerPrefs.GetInt("P1_Wave_2ND"));
        p1Wave_TXT[2].text = ("" + PlayerPrefs.GetInt("P1_Wave_3RD"));
    }

    //saves player 2's score
    public void SaveP2Score()
    {
        //get total score
        totalScore = player1.GetComponent<PlayerScore>().score + player2.GetComponent<PlayerScore>().score;
        //set displays of score
        totalScore_TXT.text = ("" + totalScore);
        p1Score_TXT.text = ("" + player1.GetComponent<PlayerScore>().score);
        p2Score_TXT.text = ("" + player2.GetComponent<PlayerScore>().score);

        //if score is greater than 1st score
        if (totalScore > PlayerPrefs.GetInt("P2_Score_1ST"))
        {
            //save each score to the one below
            PlayerPrefs.SetInt("P2_Score_3RD", PlayerPrefs.GetInt("P2_Score_2ND"));
            PlayerPrefs.SetInt("P2_Score_2ND", PlayerPrefs.GetInt("P2_Score_1ST"));
            PlayerPrefs.SetInt("P2_Score_1ST", totalScore);

            //save each wave to the one below
            PlayerPrefs.SetInt("P2_Wave_3RD", PlayerPrefs.GetInt("P2_Wave_2ND"));
            PlayerPrefs.SetInt("P2_Wave_2ND", PlayerPrefs.GetInt("P2_Wave_1ST"));
            PlayerPrefs.SetInt("P2_Wave_1ST", WaveSystem.wave);

            //highlight first place
            p2Highlight_TXT[0].SetActive(true);
        }
        //else if greater than 2nd score
        else if (totalScore > PlayerPrefs.GetInt("P2_Score_2ND"))
        {
            //save each score to the one below
            PlayerPrefs.SetInt("P2_Score_3RD", PlayerPrefs.GetInt("P2_Score_2ND"));
            PlayerPrefs.SetInt("P2_Score_2ND", totalScore);

            //save each wave to the one below
            PlayerPrefs.SetInt("P2_Wave_3RD", PlayerPrefs.GetInt("P2_Wave_2ND"));
            PlayerPrefs.SetInt("P2_Wave_2ND", WaveSystem.wave);

            //highlight second place
            p2Highlight_TXT[1].SetActive(true);
        }
        //else if greater than 3rd score
        else if (totalScore > PlayerPrefs.GetInt("P2_Score_3RD"))
        {
            //save each score to the one below
            PlayerPrefs.SetInt("P2_Score_3RD", totalScore);

            //save each wave to the one below
            PlayerPrefs.SetInt("P2_Wave_3RD", WaveSystem.wave);

            //highlight third place
            p2Highlight_TXT[2].SetActive(true);
        }

        //set leaderboard text
        p2HighScore_TXT[0].text = ("" + PlayerPrefs.GetInt("P2_Score_1ST"));
        p2HighScore_TXT[1].text = ("" + PlayerPrefs.GetInt("P2_Score_2ND"));
        p2HighScore_TXT[2].text = ("" + PlayerPrefs.GetInt("P2_Score_3RD"));

        p2Wave_TXT[0].text = ("" + PlayerPrefs.GetInt("P2_Wave_1ST"));
        p2Wave_TXT[1].text = ("" + PlayerPrefs.GetInt("P2_Wave_2ND"));
        p2Wave_TXT[2].text = ("" + PlayerPrefs.GetInt("P2_Wave_3RD"));
    }
}
