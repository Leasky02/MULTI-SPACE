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
    public float currentRotation;

    //directional movement checks
    private bool movingHorizontally = false;
    private bool movingVertically = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentRotation = transform.rotation.z;
    }

    // Update is called once per frame
    void Update()
    {
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
                if(Input.GetAxis("Horizontal" + playerID) < -0.1)
                {
                    currentRotation = 90f;
                }
                //set rotation of object RIGHT
                if (Input.GetAxis("Horizontal" + playerID) > 0.1)
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
                if (Input.GetAxis("Vertical" + playerID) < -0.1)
                {
                    currentRotation = 180f;
                }
                //set rotation of object UP
                if (Input.GetAxis("Vertical" + playerID) > 0.1)
                {
                    currentRotation = 0f;
                }
            }
            else
            {
                movingVertically = false;
            }
        }
        //set rotation of object
        transform.eulerAngles = new Vector3(0,0,currentRotation);
    }
}
