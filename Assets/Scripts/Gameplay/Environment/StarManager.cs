using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarManager : MonoBehaviour
{
    //class ID
    [SerializeField] private int classID;

    //class variables
    private float scale;
    private float speed;

    //used by planets only
    [SerializeField] private bool isPlanet;
    //boundries for planets to spawn
    [SerializeField] private Transform boundary1;
    [SerializeField] private Transform boundary2;
    // Start is called before the first frame update
    void Start()
    {
        if(isPlanet)
        {
            //set position randomly
            transform.position = new Vector2(Random.Range(boundary1.position.x, boundary2.position.x), Random.Range(boundary1.position.y, boundary2.position.y));
            //call planet class
            ClassPlanet();
            return;
        }

        //assign object a classID between 1 and 3
        classID = Random.Range(1, 4);
        //perform class specific function based on classID
        switch (classID)
        {
            case 1:
                ClassOne();
                break;
            case 2:
                ClassTwo();
                break;
            case 3:
                ClassThree();
                break;
        }
    }
    //far away stars
    private void ClassOne()
    {
        //set scale
        transform.localScale = new Vector3(0.15f,0.15f,0.15f);
        //set speed
        speed = 5f;
    }
    //medium distance stars
    private void ClassTwo()
    {
        //set scale
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        //set speed
        speed = 10f;
    }
    //closest stars
    private void ClassThree()
    {
        //set scale
        transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        //set speed
        speed = 15f;
    }

    private void ClassPlanet()
    {
        //set scale
        float scale = Random.Range(0.7f, 1.1f);
        transform.localScale = new Vector3(scale , scale , scale);
        //set speed
        speed = Random.Range(12f , 20f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if the star has reached the boundry
        if (transform.position.x <= -140)
        {
            //teleport to the opposite side of the boundry
            gameObject.transform.position = new Vector2(140, transform.position.y);
        }
        //move star
        transform.Translate(Vector3.left * speed * Time.deltaTime / 10, Space.World);
    }
}
