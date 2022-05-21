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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentRotation = transform.rotation.z;
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
        /* used for 2D moveme t ONLY
        //if player isn't moving vertically
        if(!movingVertically)
        {
            //if player is moving in any horizontal direction
            if (Input.GetAxis("Horizontal" + playerID) != 0)
            {
                movingHorizontally = true;
                //apply force * speed
                rb.AddForce(new Vector2(Input.GetAxis("Horizontal" + playerID), 0) * speed * Time.deltaTime, ForceMode2D.Impulse);

                //set rotation of object LEFT
                if(Input.GetAxis("Horizontal" + playerID) < -0.2)
                {
                    currentRotation = 90f;
                }
                //set rotation of object RIGHT
                if (Input.GetAxis("Horizontal" + playerID) > 0.2)
                {
                    currentRotation = 270f;
                }
            }
            else
            {
                movingHorizontally = false;
            }
        }

        if(!movingHorizontally)
        {
            //if player is moving in any vertical direction
            if (Input.GetAxis("Vertical" + playerID) != 0)
            {
                movingVertically = true;
                //apply force * speed
                rb.AddForce(new Vector2(0 , Input.GetAxis("Vertical" + playerID)) * speed * Time.deltaTime , ForceMode2D.Impulse);

                //set rotation of object DOWN
                if (Input.GetAxis("Vertical" + playerID) < -0.2)
                {
                    currentRotation = 180f;
                }
                //set rotation of object UP
                if (Input.GetAxis("Vertical" + playerID) > 0.2)
                {
                    currentRotation = 0f;
                }
            }
            else
            {
                movingVertically = false;
            }
        */
        //used for moving both up/down and left/right at the same time
        //create variable holding the input
        float horizontalInput = Input.GetAxis("Horizontal" + playerID);
        float verticalInput = Input.GetAxis("Vertical" + playerID);

        //set the movement direction based on the input
        Vector2 movementDirection = new Vector2(horizontalInput, verticalInput);
        //normalise the direction
        movementDirection.Normalize();

        // move the character
        rb.AddForce(new Vector2(horizontalInput, verticalInput) * speed * Time.deltaTime, ForceMode2D.Impulse);
    
        //set rotation of object
        Quaternion currentAngle = transform.rotation;
        Quaternion newAngle = Quaternion.LookRotation(Vector3.forward , movementDirection);

        //rotate
        if (movementDirection != Vector2.zero)
            transform.rotation = Quaternion.RotateTowards(currentAngle, newAngle, Time.deltaTime * 1100);
    }
}
