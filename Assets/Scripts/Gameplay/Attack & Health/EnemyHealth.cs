using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    //health variables
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    //target that enemy is chasing
    [SerializeField] private Transform target;

    //damage variables
    private bool doDamage = true;
    //length of time between attacks
    [SerializeField] private float attackCooldown;
    //distance from player to attack
    [SerializeField] private float attackDistance;
    //damage (10 by default)
    [SerializeField] private int startingAttackDamage = 10;
    [SerializeField] private int attackDamage;

    // Start is called before the first frame update
    void Start()
    {
        //max health = wave equation***


        //set attackDamage according to equation and wave***
        attackDamage = startingAttackDamage;

        //spawn rate will be handled in the wave manager***


        currentHealth = maxHealth;
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
    //set target to AI's target
    public void SetTarget(Transform targetAI)
    {
        target = targetAI;
    }

    private void AllowDamage()
    {
        doDamage = true;
    }

    public void Damage(int damage)
    {
        currentHealth -= damage;
        //if player is dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //destroy itself
        Destroy(gameObject);
    }
}
