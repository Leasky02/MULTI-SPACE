using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CommanderAI : MonoBehaviour
{
    /// <summary>
    /// The Commander will look for the closest player and lock onto them. When they get low on health they will proceed to
    /// find a random point in the map that is not within a certain radius to the player and move towards it (run away). Only
    /// once their health is higher will they go back in to fight.
    /// </summary>
    
    //target to follow
    [SerializeField] private Transform target;
    //retreat position to go to
    Vector2 retreatPosition;
    //potential targets
    private GameObject[] players;
    //speed to move
    [SerializeField] private float speed;
    private float retreatSpeedMultiplier = 1f;
    [SerializeField] private float nextWaypointDistance;
    //path variables
    private Path path;
    private int currentWayPoint = 0;
    private bool reachedEndOfPath = false;
    private bool followingRetreatPath = false;
    //if player has low health
    private bool lowHealth = false;
    [SerializeField] private int lowHealthValue;

    //A* asset scripts
    [SerializeField] private Seeker seeker;
    [SerializeField] private EnemyHealth healthScript;
    [SerializeField] private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {

        //set players
        players = GameObject.FindGameObjectsWithTag("Player");

        //set speed variation randomly
        speed = Random.Range(speed - 100 , speed + 100);
        //set speed accodring to equation (subtle speed change)
        speed += WaveSystem.wave * 120;
        //if speed is too high keep within limit
        if (speed > 2210)
            speed = 2210;

        //set nextWayPointDistance variation randomly
        nextWaypointDistance = Random.Range(nextWaypointDistance - 0.2f, nextWaypointDistance + 0.5f);

        //start generating path repeatedly (slight random start point to make enemies not pathfind simultaneously)
        InvokeRepeating("UpdatePath", Random.Range(0f, 1f), 1f);
    }
    //called when path is complete
    void OnPathComplete(Path p)
    {
        //if no error
        if(!p.error)
        {
            //set waypoint to path
            path = p;
            currentWayPoint = 0;
        }
    }
    //upate the path to follow
    void UpdatePath()
    {
        //get health of enemy
        if(healthScript.currentHealth < lowHealthValue)
        {
            lowHealth = true;
            //play low health animation
            GetComponent<Animator>().Play("CommanderLowHealth");
        }
        else if(healthScript.currentHealth > lowHealthValue + 20)
        {
            lowHealth = false;
            //play normal health animation
            GetComponent<Animator>().Play("CommanderMaintain");
        }

        //update target
        //set shortestDistance to high value
        float shortestDistance = 1000f;

        //if enemy doesnt have low health, move to player
        if(!lowHealth)
        {
            followingRetreatPath = false;

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
            //decrease speed of enemy while running away
            retreatSpeedMultiplier = 1f;
            //update path with player position
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
        //if enemy does have low health
        else
        {
            //check if it has reached the retreat position yet before going to the next one
            if(Vector2.Distance(retreatPosition , transform.position) < 10f)
            {
                followingRetreatPath = false;
            }

            //if enemy has reached its retreat point
            if(!followingRetreatPath)
            {
                //create multiple positions and pick furthest frmo player to move to
                Vector2[] retreatPositionOptions = new Vector2[5];
                float playerDistance = 0f;

                for (int i = 0; i < retreatPositionOptions.Length; i++)
                {
                    //set position of position choice
                    retreatPositionOptions[i] = new Vector2(Random.Range(-40f, 40f), Random.Range(-40f, 40f));
                    //check against longest length
                    if (Vector2.Distance(retreatPositionOptions[i], target.position) > playerDistance)
                    {
                        //if current location is longest yet then set it as location to go to
                        retreatPosition = retreatPositionOptions[i];
                    }
                }
                followingRetreatPath = true;
                //increase speed of enemy while running away
                retreatSpeedMultiplier = 1.5f;
                //update path with retreat position
                seeker.StartPath(rb.position, retreatPosition, OnPathComplete);
            }
        }
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
        if(currentWayPoint >= path.vectorPath.Count)
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
        rb.AddForce(force * retreatSpeedMultiplier);

        //distance to next way point
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
        //if reached next waypoint
        if(distance < nextWaypointDistance && !reachedEndOfPath)
        {
            currentWayPoint++;
        }
        //set rotation
        Quaternion currentAngle = transform.rotation;
        Quaternion newAngle = Quaternion.LookRotation(Vector3.forward, rb.velocity);
        transform.rotation = Quaternion.RotateTowards(currentAngle, newAngle, Time.deltaTime * 500);
    }
}
