using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    //handle the current score
    public int score = 0;
    private int displayScore = 0;
    public static int combinedScore = 0;

    //weapon upgrade level
    private int upgradeLevel = 1;
    //upgrade point requirement
    private float scoreRequired = 200;
    private float startingScore = 0;
    //slider
    [SerializeField] private Slider upgradeSlider;

    //UI elements
    [SerializeField] private Text scoreDisplay;
    [SerializeField] private Text WeaponUpgrade_TXT;
    //audio source
    [SerializeField] private AudioSource myAudioSource;

    private void Start()
    {
        //set min and max values of slider
        upgradeSlider.minValue = startingScore;
        upgradeSlider.maxValue = scoreRequired;
    }

    public void AddScore(int scoreToAdd)
    {
        //randomise the score slightly
        scoreToAdd = Random.Range(scoreToAdd - 5, scoreToAdd + 5);
        //add score on
        score += scoreToAdd;

        //update the score display
        scoreDisplay.text = ("" + score);
    }

    public void RemoveScore(int scoreToRemove)
    {
        //randomise the score slightly
        scoreToRemove = Random.Range(scoreToRemove - 5, scoreToRemove + 5);
        //remove score
        score -= scoreToRemove;
    }

    private void FixedUpdate()
    {
        //update score
        if(displayScore < score)
        {
            displayScore+=3;

            if (displayScore > score)
                displayScore = score;

            //update the score display
            scoreDisplay.text = ("" + displayScore);
        }
        if(displayScore > score)
        {
            displayScore-=3;

            if (displayScore < score)
                displayScore = score;
            //update the score display
            scoreDisplay.text = ("" + displayScore);

        }

        //if score has reached goal
        if(score >= scoreRequired)
        {
            UpgradeWeapon();
        }

        //update upgrade slider
        upgradeSlider.value = score;
    }

    private void UpgradeWeapon()
    {
        upgradeLevel++;
        //equation of upgrade level
        startingScore = scoreRequired;
        scoreRequired = score + 200 + upgradeLevel * 50;

        //set min and max values of slider
        upgradeSlider.minValue = startingScore;
        upgradeSlider.maxValue = scoreRequired;

        //play upgrade sound
        myAudioSource.Play();

        //update text
        WeaponUpgrade_TXT.text = ("Weapon Upgrade: " + upgradeLevel);

        //upgrade bullet damage
        Bullet.damage += 8;
        //upgrade gun fire rate
        if(Gun.fireRate < 2.0f)
        {
            Gun.fireRate += 0.2f;
            if (Gun.fireRate > 2.0f)
                Gun.fireRate = 2.0f;
        }
    }
}
