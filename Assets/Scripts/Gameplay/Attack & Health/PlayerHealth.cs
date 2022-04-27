using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    //healthbar variable
    [SerializeField] private Slider healthBar;
    [SerializeField] private Image healthBar_FILL;
    [SerializeField] private Image healthBar_DAMAGE;

    //audio volume
    private float desiredVolume = 0f;

    //colour variable
    [SerializeField] private Color red;
    [SerializeField] private Color green;

    //playerID
    [SerializeField] private int playerID;

    //health values
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    // % of health to be healed every second
    private float repairRate = 3;
    //variable to prevent player taking damage multiple times in one frame
    private bool receiveDamage = true;

    //a player has low health
    static bool lowHealth;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Heal", 1f, 1f);
        //set health bar
        healthBar.value = currentHealth;
        //set low health to false
        lowHealth = false;
    }

    //called when attacked
    public void Damage(float damage)
    {
        if(receiveDamage)
        {
            //prevent player from receiving damage again
            receiveDamage = false;
            Invoke("AllowReceiveDamage", 0.3f);

            //take damage away from current health
            currentHealth -= damage;

            //if player loses all health
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Die();
            }
            //if health is less than 20% of max
            if (currentHealth <= (maxHealth / 100) * 20)
            {
                lowHealth = true;
                //turn health bar red
                healthBar_FILL.color = red;

                //turn up heartbeat
                desiredVolume = 1f;
            }

            //update health bar
            healthBar.value = currentHealth;

            //play animation of health bar damage
            healthBar_DAMAGE.GetComponent<Animator>().Play("healthbar");

            //play damage audio clip with random pitch
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
                GetComponent<AudioSource>().Play();
            }
        }
    }
    //called to heal player
    private void Heal()
    {
        //if player can be healed
        if(currentHealth <= maxHealth)
        {
            //add repair rate
            currentHealth += repairRate;
            //if health has gone over max
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
        }

        //if health is less than 20% of max
        if (currentHealth > (maxHealth / 100) * 20)
        {
            lowHealth = false;
            //turn health bar green
            healthBar_FILL.color = green;

            //turn down heartbeat
            desiredVolume = 0f;
        }

        //update health bar
        healthBar.value = currentHealth;
    }

    //called when player dies
    private void Die()
    {

    }

    private void AllowReceiveDamage()
    {
        receiveDamage = true;
    }

    //called every frame
    private void Update()
    {
        float currentVolume = healthBar.gameObject.GetComponent<AudioSource>().volume;
        healthBar.gameObject.GetComponent<AudioSource>().volume = Mathf.Lerp(currentVolume, desiredVolume, Time.deltaTime * 5);
    }
}
