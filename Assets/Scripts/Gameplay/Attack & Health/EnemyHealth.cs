using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    //health variables
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        //max health = wave equation***


        //set attackDamage according to equation and wave***


        //spawn rate will be handled in the wave manager***


        currentHealth = maxHealth;
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
