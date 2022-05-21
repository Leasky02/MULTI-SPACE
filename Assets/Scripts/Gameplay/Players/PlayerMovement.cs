using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //player identity
    [SerializeField] private int playerID;
    //rigidbody
    private Rigidbody2D rb;

    //speed
    [SerializeField] private int speed;

    //rotation
    [HideInInspector] public float currentRotation;

    //directional movement checks
    private bool movingHorizontally = false;
    private bool movingVertically = false;

    //animator of 3D child object
    [SerializeField] private Animator myAnimator;
    //animations to play
    [SerializeField] private string animToPlay;
    //animator check 
    private bool animPlaying = false;

    //holding direction of players movement
    Vector2 movementDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //store current rotation of object
        currentRotation = transform.rotation.z;

        //set movement direction
        movementDirection = new Vector2(Input.GetAxis("Horizontal" + playerID), Input.GetAxis("Vertical" + playerID)).normalized;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if player IS moving
        if (Input.GetAxis("Horizontal" + playerID)  != 0 || Input.GetAxis("Vertical" + playerID) != 0)
        {
            //set isMoving to true
            myAnimator.SetBool("IsMoving", true);

            //if animation is NOT playing
            if (!animPlaying)
            {
                //play WALKING animation (first animation in array)
                myAnimator.Play(animToPlay);
                animPlaying = true;
            }
        }
        //player is NOT moving
        else
        {
            //set isMoving to false
            myAnimator.SetBool("IsMoving", false);

            //if ANIMATOR IS playing
            if (animPlaying)
            {
                animPlaying = false;
            }
        }

        //left joystick input
        
        //create variable holding the input
        float horizontalInput = Input.GetAxis("Horizontal" + playerID);
        float verticalInput = Input.GetAxis("Vertical" + playerID);

        //set the movement direction based on the input
        //IF there is any input
        if (new Vector2(horizontalInput, verticalInput) != Vector2.zero)
        {
            movementDirection = new Vector2(horizontalInput, verticalInput);
            //normalize its magnitude to keep speed of movement consistent
            movementDirection.Normalize();
        }
        // move the character
        rb.AddForce(new Vector2(horizontalInput, verticalInput) * speed * Time.deltaTime, ForceMode2D.Impulse);
    
        //set rotation of object
        Quaternion currentAngle = transform.rotation;

        //rotate
        //if the player is moving
        if(movementDirection != Vector2.zero)
        {
            //rotate to angle of movement
            Quaternion newAngle = Quaternion.LookRotation(Vector3.forward, movementDirection);
            transform.rotation = Quaternion.RotateTowards(currentAngle, newAngle, Time.fixedDeltaTime * 800);
        }
        else
        {
            //round direction to nearest 45
            Vector2 roundedDirection = movementDirection;
            roundedDirection.x = Mathf.Round(roundedDirection.x / 45) * 45;
            roundedDirection.y = Mathf.Round(roundedDirection.y / 45) * 45;
            //rotate to snapped direction
            Quaternion newAngle = Quaternion.LookRotation(Vector3.forward, roundedDirection);
            transform.rotation = Quaternion.RotateTowards(currentAngle, newAngle, Time.fixedDeltaTime * 800);
        }
    }
}
