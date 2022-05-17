using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour
{
    //parent object to rotate
    [SerializeField] private GameObject menuContainer;
    [SerializeField] private float rotationSpeed;
    //desired rotation
    private float rotation_Y;

    //button objects
    [SerializeField] private GameObject[] playerButtons;
    [SerializeField] private GameObject[] levelButtons;

    //can player go back
    private bool canGoBack = false;

    //rotates main menu container left (Shows level selection)
    //coroutine to allow button disable to be delay
    public IEnumerator LevelSelection()
    {
        //play audio source
        menuContainer.GetComponent<AudioSource>().Play();

        rotation_Y = -90;

        yield return new WaitForSeconds(0.3f);

        //disable player buttons
        playerButtons[0].SetActive(false);
        playerButtons[1].SetActive(false);
        //enable level buttons
        levelButtons[0].SetActive(true);
        levelButtons[1].SetActive(true);
        levelButtons[2].SetActive(true);

        //set button to selectdd
        levelButtons[0].GetComponent<Button>().Select();

        canGoBack = true;
    }

    //rotates main menu container right (Shows player number selection)
    //coroutine to allow button disable to be delay
    public IEnumerator PlayerSelection()
    {
        //play audio source
        menuContainer.GetComponent<AudioSource>().Play();

        rotation_Y = 0;

        canGoBack = false;

        yield return new WaitForSeconds(0.3f);

        //enable player buttons
        playerButtons[0].SetActive(true);
        playerButtons[1].SetActive(true);
        //disable level buttons
        levelButtons[0].SetActive(false);
        levelButtons[1].SetActive(false);
        levelButtons[2].SetActive(false);

        //set button to selectd
        playerButtons[0].GetComponent<Button>().Select();
    }

    public void MenuChange(string coroutine)
    {
        StartCoroutine(coroutine);
    }

    //called every frame
    private void Update()
    {

        if(Input.GetButtonDown("Back") && canGoBack)
        {
            StartCoroutine("PlayerSelection");
        }

        //lerp the roation to the desired rotation
        Quaternion currentRotation = menuContainer.transform.rotation;
        Quaternion wantedRotation = Quaternion.Euler(0, rotation_Y , 0);

        menuContainer.transform.rotation = Quaternion.RotateTowards(currentRotation, wantedRotation, Time.deltaTime * rotationSpeed);
    }
}

