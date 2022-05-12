using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitApplication : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //if exit button is pressed
        if(Input.GetButtonDown("Exit"))
        {
            Application.Quit();
        }
    }
}
