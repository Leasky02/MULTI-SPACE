using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    //health variables
    [SerializeField] private int startingMaxHealth;
    [SerializeField] private int maxHealth;
    public int currentHealth;
    [SerializeField] private bool dead;

    //should enemy be able to regenerate?
    [SerializeField] private bool regenerate;
    [SerializeField] private int healingRate;

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

        //set health to max
        currentHealth = maxHealth;

        if (regenerate)
            InvokeRepeating("Heal", 1f, 1f);
    }

    private void Update()
    {
        //if player is within reach and can deal damage and enemy isn't dead
        if (doDamage && Vector3.Distance(target.transform.position, gameObject.transform.position) < attackDistance && !dead)
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
        //if enemy is dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal()
    {
        currentHealth += healingRate;
        //if health is already max
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    private void Die()
    {
        dead = true;

        //stop rendering alien GFX
        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        //remove collider
        GetComponent<CapsuleCollider2D>().enabled = false;
        //play death sound
        GetComponent<AudioSource>().Play();
        //play death particles
        gameObject.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        //stop enemy moving
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

        //destroy itself
        Destroy(gameObject , 2f);
    }
}
