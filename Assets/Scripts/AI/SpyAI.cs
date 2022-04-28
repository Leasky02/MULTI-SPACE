using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SpyAI : MonoBehaviour
{
    /// <summary>
    /// SPY AI is to locate the player and once within close range, they will find random locations around the player and quickly
    /// dart to it over and over, moving around or through the player. The enemy will stop doing so if the player gets outside of
    /// close range and will then proceed to move towards the closest player and repeat when in range. The enemy cannot lock onto
    /// any other player while it is darting around another, no matter how close another player is. it can only switch target if it
    /// is out of close range from any player
    /// </summary>
    //target to follow
    [SerializeField] private Transform target;
    //potential targets
    private GameObject[] players;
    //speed to move
    [SerializeField] private float speed;
    private float speedMultiplier = 1f;
    //cutting corners
    [SerializeField] private float nextWaypointDistance = 3f;

    //path variables
    private Path path;
    private int currentWayPoint = 0;
    private bool reachedEndOfPath = false;
    private bool followingOffset = false;
    private bool offsetPathReached = false;

    //A* asset script
    [SerializeField] private Seeker seeker;
    [SerializeField] private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        //set attackDamage according to equation and wave***

        //set players
        players = GameObject.FindGameObjectsWithTag("Player");

        //set speed variation randomly
        speed = Random.Range(speed - 100, speed + 100);
        //set nextWayPointDistance variation randomly
        nextWaypointDistance = Random.Range(nextWaypointDistance - 0.5f, nextWaypointDistance + 2f);

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
        for(int i = 0; i < players.Length; i++)
        {
            //get distance between a player and self
            float distance = Vector2.Distance(players[i].transform.position, rb.position);
            //if distance is shortest one so far
            if (distance < shortestDistance)
            {
                //if the enemy is NOT already darting aroiund another player
                if(!followingOffset)
                {
                    //lock onto another player
                    shortestDistance = distance;
                    //set player to target
                    target = players[i].transform;
                }
            }
        }



        //if enemy is within range of the player
        if(Vector2.Distance(target.position, rb.position) < 8f)
        {
            //multiply speed
            speedMultiplier = 2.5f;
            //tell enemy to start following offset
            followingOffset = true;
        }
        else
        {
            followingOffset = false;
            //reset speed multiplier
            speedMultiplier = 1f;
        }

        //if enemy isn't to follow the fofset (not close enough)
        if (!followingOffset)
        {
            //update path
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
        //if enemy is close to player
        else
        {
            //and it has reached its current offset set out then make a new path
            if(reachedEndOfPath)
            {
                //create a new offset target point
                Vector2 positionOffset = new Vector2(Random.Range(-6f, 6f), Random.Range(-6f, 6f));
                //create a new path with target location with the offset
                seeker.StartPath(rb.position, new Vector2 (target.position.x + positionOffset.x , target.position.y + positionOffset.y), OnPathComplete);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //set target of AI's health script to current target
        GetComponent<EnemyHealth>().SetTarget(target);

        if (path == null)
            return;
        //if reached the end of the path and is following the offsestLocation
        if (currentWayPoint >= path.vectorPath.Count && followingOffset)
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
        Vector2 force = direction * speed * speedMultiplier * Time.deltaTime;

        //add force to rigidbody if not already at player
        if(!reachedEndOfPath)
            rb.AddForce(force);

        //distance to next way point
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
        //if reached next waypoint
        if (distance < nextWaypointDistance)
        {
            currentWayPoint++;
        }

        //set rotation
        transform.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);
    }
}
