using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public void ChangeScene(string sceneToLoad)
    {
        //SceneManager.LoadScene(sceneToLoad);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }
}
