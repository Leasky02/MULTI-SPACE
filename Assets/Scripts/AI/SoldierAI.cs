using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SoldierAI : MonoBehaviour
{
    /// <summary>
    /// The soldier will instantly look for the closest player and pathfind towards it. This is the simplest of all the AI's
    /// </summary>

    //target to follow
    [SerializeField] private Transform target;
    //potential targets
    private GameObject[] players;
    //speed to move
    [SerializeField] private float speed;
    [SerializeField] private float nextWaypointDistance = 3f;
    //path variables
    private Path path;
    private int currentWayPoint = 0;
    private bool reachedEndOfPath = false;

    //A* asset scripts
    [SerializeField] private Seeker seeker;
    [SerializeField] private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {

        //set players
        players = GameObject.FindGameObjectsWithTag("Player");

        //set speed variation randomly
        speed = Random.Range(speed - 100, speed + 100);
        //set speed accodring to equation (subtle speed change)
        speed += WaveSystem.wave * 10;

        //set nextWayPointDistance variation randomly
        nextWaypointDistance = Random.Range(nextWaypointDistance - 0.5f, nextWaypointDistance + 1f);

        //start generating path repeatedly (slight random start point to make enemies not pathfind simultaneously)
        InvokeRepeating("UpdatePath", Random.Range(0f, 1f), 1f);
    }
    //called when path is complete
    void OnPathComplete(Path p)
    {
        //if no error
        if (!p.error)
        {
            //set waypoint to path
            path = p;
            currentWayPoint = 0;
        }
    }
    //upate the path to follow
    void UpdatePath()
    {
        //update target
        //set shortestDistance to high value
        float shortestDistance = 1000f;
        //get shortest distance
        for (int i = 0; i < players.Length; i++)
        {
            if(!players[i].GetComponent<PlayerHealth>().dead)
            {
                //get distance between a player and self
                float distance = Vector2.Distance(players[i].transform.position, transform.position);
                //if distance is shortest one so far
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    //set player to target
                    target = players[i].transform;
                }
            }
        }
        
        //update path
        seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if there is a target
        if (target != null)
        {
            //set target of AI's health script to current target
            GetComponent<EnemyHealth>().SetTarget(target);
        }

        if (path == null)
            return;
        
        //if reached the end of the path
        if (currentWayPoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }
        
        //gives vector direction to next waypoint

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        //move object along path in direction calculated above
        Vector2 force = direction * speed * Time.deltaTime;

        //add force to rigidbody
        rb.AddForce(force);

        //distance to next way point
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

        //if reached next waypoint
        if (distance < nextWaypointDistance && !reachedEndOfPath)
        {
            currentWayPoint++;
        }

        //set rotation
        Quaternion currentAngle = transform.rotation;
        Quaternion newAngle = Quaternion.LookRotation(Vector3.forward, rb.velocity);
        transform.rotation = Quaternion.RotateTowards(currentAngle, newAngle, Time.deltaTime * 500);
    }
}
