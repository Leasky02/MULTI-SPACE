using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonActions : MonoBehaviour
{
    public void CloseMenu(GameObject menuToClose , GameObject player)
    {
        //disable menu
        menuToClose.SetActive(false);
    }

    public void SwitchMenu(GameObject menuToClose , GameObject menuToOpen)
    {
        menuToClose.SetActive(false);
        menuToOpen.SetActive(true);
    }

    public void LoadScene(string sceneToLoad)
    {
        //forced to use this method as SceneManager.LoadScene() doesnt exist ???
        SceneManager.LoadScene(sceneToLoad);
    }
}
