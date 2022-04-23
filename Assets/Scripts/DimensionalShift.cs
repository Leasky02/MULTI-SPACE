using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DimensionalShift : MonoBehaviour
{
    //2D players
    [SerializeField] private SpriteRenderer player1_2D;
    [SerializeField] private SpriteRenderer player2_2D;

    //3D players
    [SerializeField] private GameObject player1_3D;
    [SerializeField] private GameObject player2_3D;

    //tilemaps
    [SerializeField] private GameObject innerTilemap;

    //camera
    [SerializeField] private GameObject cam;

    //variable holding current dimensional state
    private bool is3D = false;

    // Update is called once per frame
    void Update()
    {
        //if player presses dimenional shift button
        if(Input.GetButtonDown("DimensionalShift"))
        {
            //if player is already in 3D
            if(is3D)
            {
                MoveTo2D();
            }
            else
            {
                MoveTo3D();
            }
        }
    }

    private void MoveTo3D()
    {
        //set 2D sprite invisible
        player1_2D.enabled = false;
        player2_2D.enabled = false;
        //position 3D characters
        player1_3D.transform.position = new Vector3(player1_3D.transform.position.x , player1_3D.transform.position.y, -0.2f);
        player2_3D.transform.position = new Vector3(player2_3D.transform.position.x , player2_3D.transform.position.y, -0.2f);

        //set the Y coordinate offset of camera
        cam.GetComponent<MultipleTargetCamera>().offset.y = -6;
        //set the X rotation of the camera
        cam.GetComponent<MultipleTargetCamera>().rotation_X = -6;
        //allow camera to zoom out slightly more
        cam.GetComponent<MultipleTargetCamera>().minZoom = 140;

        //turn off collider of tilemap
        innerTilemap.GetComponent<TilemapCollider2D>().enabled = false;


        is3D = true;
    }

    private void MoveTo2D()
    {
        //set 2D sprite visible
        player1_2D.enabled = true;
        player2_2D.enabled = true;
        //position 3D characters out fo view of camera
        player1_3D.transform.position = new Vector3(player1_3D.transform.position.x, player1_3D.transform.position.y, -50.0f);
        player2_3D.transform.position = new Vector3(player2_3D.transform.position.x, player2_3D.transform.position.y, -50.0f);

        //set the Y coordinate offset of camera
        cam.GetComponent<MultipleTargetCamera>().offset.y = 0;
        //set the X rotation of the camera
        cam.GetComponent<MultipleTargetCamera>().rotation_X = 0;
        //reset cameras minimum zoom
        cam.GetComponent<MultipleTargetCamera>().minZoom = 135;

        //turn on collider of tilemap
        innerTilemap.GetComponent<TilemapCollider2D>().enabled = true;

        is3D = false;
    }
}
