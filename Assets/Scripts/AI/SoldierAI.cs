using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SoldierAI : MonoBehaviour
{
    //target to follow
    [SerializeField] private Transform target;
    //speed to move
    [SerializeField] private float speed = 200f;
    [SerializeField] private float nextWaypointDistance = 3f;
    //path variables
    private Path path;
    private int currentWayPoint = 0;
    private bool reachedEndOfPath = false;

    //A* asset scripts
    private Seeker seeker;
    private Rigidbody2D rb;

    //damage variables
    private bool doDamage = true;
    //length of time between attacks
    [SerializeField] private float attackCooldown;
    //distance from player to attack
    [SerializeField] private float attackDistance;
    //damage (10 by default)
    [SerializeField] private int attackDamage = 12;

    // Start is called before the first frame update
    void Start()
    {
        //set attackDamage according to equation and wave***

        //set component variables
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        //start generating path repeatedly
        InvokeRepeating("UpdatePath", 0f, 1f);
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
        seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
        if (distance < nextWaypointDistance)
        {
            currentWayPoint++;
        }

        //set rotation
        transform.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);
    }

    private void Update()
    {
        //if player is within reach and can deal damage
        if (doDamage && Vector3.Distance(target.transform.position, gameObject.transform.position) < attackDistance)
        {
            //do damage to player
            target.gameObject.GetComponent<PlayerHealth>().Damage(attackDamage);
            //prevent damage
            doDamage = false;
            //cue damage reactivate
            Invoke("AllowDamage", attackCooldown);
        }
    }
    private void AllowDamage()
    {
        doDamage = true;
    }
}
