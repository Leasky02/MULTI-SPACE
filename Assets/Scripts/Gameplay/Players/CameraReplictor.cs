using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraReplictor : MonoBehaviour
{
    /// <summary>
    /// used to replicate the main camera exactly. This camera renders the players to allow for seperate post processing
    /// </summary>
    /// 
    [SerializeField] private Camera myCam;
    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<Camera>().fieldOfView = myCam.fieldOfView;
    }
}
