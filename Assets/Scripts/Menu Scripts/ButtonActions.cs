using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonActions : MonoBehaviour
{
    //animator for scene transitions
    [SerializeField] private Animator sceneTransition_ANIM;

    //scene to load
    private string nextScene;

    private void Start()
    {
        //lock mouse to center of screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
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
        nextScene = sceneToLoad;
        StartCoroutine("ExitScene");
    }

    IEnumerator ExitScene()
    {
        //play scene exit transition
        sceneTransition_ANIM.Play("ExitTransition");

        //set time to default
        Time.timeScale = 1f;

        //wait before changing scene
        yield return new WaitForSeconds(1f);

        //forced to use this method as SceneManager.LoadScene() doesnt exist ???
        SceneManager.LoadScene(nextScene);
    }
}
