using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering.PostProcessing;

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

    //post processing volume
    [SerializeField] private GameObject postProcessVolume;
    [SerializeField] private float effectTransitionSpeed;
    private float requiredWeight = 0f;

    //audio volume variable
    private float audioVolume = 0f;

    //UI elements and colours
    [SerializeField] private GameObject cooldownContainer_OBJ;
    [SerializeField] private Text cooldown_TXT;
    [SerializeField] private Text ready_TXT;
    [SerializeField] private Text active_TXT;
    [SerializeField] private Text seconds_TXT;

    //timer variables
    private int seconds;
    private bool activeTimer;
    private bool cooldownTimer;
    private bool takingAway;

    [SerializeField] private int cooldownTimeLength;
    [SerializeField] private int activeTimeLength;

    //variable holding current dimensional state
    public static bool is3D = false;

    // Update is called once per frame
    void Update()
    {
        //if timer isn't already taking time off && one of the timers is playing
        if(!takingAway && (cooldownTimer || activeTimer))
        {
            //timer is actively taking away time
            takingAway = true;
            Invoke("TakeTime", 1f);
        }


        //if player1 presses dimenional shift button and isn't dead
        if(Input.GetButtonDown("DimensionalShift1") && !player1_2D.gameObject.GetComponent<PlayerHealth>().dead)
        {
            //if player is already in 3D
            if(is3D)
            {
                MoveTo2D();
            }
            else
            {
                //if the cooldown is inactive
                if(!cooldownTimer)
                    MoveTo3D();
            }
        }
        //if player2 presses dimenional shift button and isn't dead
        if (Input.GetButtonDown("DimensionalShift2") && !player2_2D.gameObject.GetComponent<PlayerHealth>().dead)
        {
            //if player is already in 3D
            if (is3D)
            {
                MoveTo2D();
            }
            else
            {
                //if the cooldown is inactive
                if (!cooldownTimer)
                    MoveTo3D();
            }
        }

        //lerp weight of post processing 
        float currentWeight = postProcessVolume.GetComponent<PostProcessVolume>().weight;
        postProcessVolume.GetComponent<PostProcessVolume>().weight = Mathf.Lerp(currentWeight , requiredWeight, Time.deltaTime * effectTransitionSpeed);

        //lerp audio volume as required
        float currentVolume = GetComponent<AudioSource>().volume;
        GetComponent<AudioSource>().volume = Mathf.Lerp(currentVolume, audioVolume, Time.deltaTime * effectTransitionSpeed);
    }

    private void TakeTime()
    {
        //if active timer is playing
        if (activeTimer)
        {
            if (seconds > 0)
            {
                //play ticking sound
                cooldownContainer_OBJ.GetComponent<AudioSource>().Play();
                //remove seconds
                seconds--;
                //set the text to say seconds
                seconds_TXT.text = (":" + seconds);
            }
            else
            {
                //put player back to 2D
                MoveTo2D();
            }
        }

        //if cooldown timer is playing
        if (cooldownTimer)
        {
            if(seconds > 0)
            {
                seconds--;
                //set the text to say seconds
                seconds_TXT.text = (":" + seconds);
            }
            else
            {
                //set cooldown timer to inactive
                cooldownTimer = false;

                //set UI elements accordingly
                seconds_TXT.gameObject.SetActive(false);
                ready_TXT.gameObject.SetActive(true);
                cooldown_TXT.gameObject.SetActive(false);
            }
        }

        //timer is no longer actively taking time away
        takingAway = false;
    }

    private void MoveTo3D()
    {
        //render camera above others
        cam.GetComponent<Camera>().depth = 1;

        //if player one isn't dead
        if(!player1_2D.gameObject.GetComponent<PlayerHealth>().dead)
        {
            //set 2D sprite invisible
            player1_2D.enabled = false;
            //position 3D characters
            player1_3D.transform.position = new Vector3(player1_3D.transform.position.x, player1_3D.transform.position.y, -0.2f);
        }

        //IF player 2 exists AND isn't dead
        if (MultiplayerManager.playerCount == 2 && !player2_2D.gameObject.GetComponent<PlayerHealth>().dead)
        {
            player2_2D.enabled = false;
            player2_3D.transform.position = new Vector3(player2_3D.transform.position.x, player2_3D.transform.position.y, -0.2f);

            //set the Y coordinate offset of camera
            cam.GetComponent<MultipleTargetCamera>().offset.y = -6;
        }
        else
        {
            //set the Y coordinate offset of camera
            cam.GetComponent<MultipleTargetCamera>().offset.y = -3;
        }

        //set the X rotation of the camera
        cam.GetComponent<MultipleTargetCamera>().rotation_X = -6;
        //allow camera to zoom out slightly more
        cam.GetComponent<MultipleTargetCamera>().minZoom = 140;

        //turn off collider of tilemap
        innerTilemap.GetComponent<TilemapCollider2D>().enabled = false;

        //set desired post processing weight to show post processing
        requiredWeight = 1f;

        //set audio volume and play which restarts it without need to stop
        audioVolume = 1f;
        GetComponent<AudioSource>().Play();

        is3D = true;

        //start active timer (countdown for how long player can be in 3D)
        activeTimer = true;
        cooldownTimer = false;
        seconds = activeTimeLength;
        //set seconds on timer
        seconds_TXT.text = (":" + seconds);

        //set UI elements accordingly
        active_TXT.gameObject.SetActive(true);
        seconds_TXT.gameObject.SetActive(true);
        ready_TXT.gameObject.SetActive(false);
    }

    private void MoveTo2D()
    {
        //render camera below others
        cam.GetComponent<Camera>().depth = -1;

        //if player 1 isnt dead
        if (!player1_2D.gameObject.GetComponent<PlayerHealth>().dead)
        {
            //set 2D sprite visible
            player1_2D.enabled = true;
            //position 3D characters out fo view of camera
            player1_3D.transform.position = new Vector3(player1_3D.transform.position.x, player1_3D.transform.position.y, -50.0f);
        }

        //IF player 2 exists AND isnt dead
        if (MultiplayerManager.playerCount == 2 && !player2_2D.gameObject.GetComponent<PlayerHealth>().dead)
        {
            player2_2D.enabled = true;
            player2_3D.transform.position = new Vector3(player2_3D.transform.position.x, player2_3D.transform.position.y, -50.0f);
        }

        //set the Y coordinate offset of camera
        cam.GetComponent<MultipleTargetCamera>().offset.y = 0;
        //set the X rotation of the camera
        cam.GetComponent<MultipleTargetCamera>().rotation_X = 0;
        //reset cameras minimum zoom
        cam.GetComponent<MultipleTargetCamera>().minZoom = 135;

        //turn on collider of tilemap
        innerTilemap.GetComponent<TilemapCollider2D>().enabled = true;

        //set desired post processing weight to turn off post processing
        requiredWeight = 0f;

        //set audio volume
        audioVolume = 0f;

        is3D = false;

        //set cooldown timer to running and set seconds
        cooldownTimer = true;
        activeTimer = false;
        seconds = cooldownTimeLength;

        //set seconds on timer
        seconds_TXT.text = (":" + seconds);

        //set UI accordingly
        active_TXT.gameObject.SetActive(false);
        cooldown_TXT.gameObject.SetActive(true);
    }
}
