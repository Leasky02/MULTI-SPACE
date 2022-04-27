using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuNavigation : MonoBehaviour
{
    //parent object to rotate
    [SerializeField] private GameObject menuContainer;
    [SerializeField] private float rotationSpeed;
    //desired rotation
    private float rotation_Y;
    
    //rotates main menu container left (Shows level selection)
    public void LevelSelection()
    {
        rotation_Y = -90;
    }

    //rotates main menu container right (Shows player number selection)
    public void PlayerSelection()
    {
        rotation_Y = 0;
    }

    //called every frame
    private void Update()
    {
        //lerp the roation to the desired rotation
        Quaternion currentRotation = menuContainer.transform.rotation;
        Quaternion wantedRotation = Quaternion.Euler(0, rotation_Y , 0);

        menuContainer.transform.rotation = Quaternion.RotateTowards(currentRotation, wantedRotation, Time.deltaTime * rotationSpeed);
    }
}

