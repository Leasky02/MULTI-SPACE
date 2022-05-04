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
    //dead players array
    public static List<GameObject> deadPlayers;
    //camera reference to remove player from view
    [SerializeField] private Camera myCam;
    //UI elements
    [SerializeField] private Text nameDisplay;
    [SerializeField] private GameObject healthContainer;
    //colour to set DEAD UI text as
    [SerializeField] private Color deadColour;
    [SerializeField] private Color playerColour;
    //3D object
    [SerializeField] private GameObject object_3D;
    //audio source for death/revive sound
    [SerializeField] private AudioSource deathPlayer;
    //audio clips
    [SerializeField] private AudioClip die_clip;
    [SerializeField] private AudioClip revive_clip;
    //particles for death
    [SerializeField] private ParticleSystem myParticles;

    //health values
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    // % of health to be healed every second
    private float repairRate = 3;
    //variable to prevent player taking damage multiple times in one frame
    private bool receiveDamage = true;

    [HideInInspector] public bool dead = false;
    public static bool oneRemaining = false;

    // Start is called before the first frame update
    void Start()
    {
        deadPlayers = new List<GameObject>();
        InvokeRepeating("Heal", 1f, 2f);
        //set health bar
        healthBar.value = currentHealth;
    }

    //called when attacked
    public void Damage(float damage)
    {
        if(receiveDamage && !dead)
        {
            //prevent player from receiving damage again
            receiveDamage = false;
            Invoke("AllowReceiveDamage", 0.3f);

            //take damage away from current health
            currentHealth -= damage;

            //if player loses all health
            if (currentHealth <= 0 && !dead)
            {
                currentHealth = 0;
                Die();
            }
            //if health is less than 20% of max
            if (currentHealth <= (maxHealth / 100) * 20)
            {
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

                //play damage animation
                gameObject.GetComponent<Animator>().Play("SpriteDamage");
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
        //if one of players isn't already dead
        if (!oneRemaining)
        {
            receiveDamage = false;
            oneRemaining = true;
            dead = true;
            //add this object to list of dead players
            deadPlayers.Add(gameObject);
            //remove this player as a camera target
            myCam.GetComponent<MultipleTargetCamera>().RemoveTarget(playerID - 1);

            //set UI elemnts
            nameDisplay.color = deadColour;

            //Disable components
            gameObject.GetComponent<PlayerMovement>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<Gun>().enabled = false;
            gameObject.GetComponent<AudioSource>().enabled = false;
            object_3D.SetActive(false);
            //Disable UI
            healthContainer.SetActive(false);
            //play death sound audio
            deathPlayer.clip = die_clip;
            deathPlayer.Play();
            //play death particles
            myParticles.Play();
        }
        else
        {
            //game over
            Debug.Log("Game Over");
        }
    }

    public void Revive()
    {
        receiveDamage = true;
        oneRemaining = false;
        dead = false;
        //add this object to list of dead players
        deadPlayers.Remove(gameObject);
        //remove this player as a camera target
        myCam.GetComponent<MultipleTargetCamera>().AddTarget(gameObject);

        //set UI elemnts
        nameDisplay.color = playerColour;

        //Enable components
        gameObject.GetComponent<PlayerMovement>().enabled = true;
        if(!DimensionalShift.is3D)
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<Gun>().enabled = true;
        gameObject.GetComponent<AudioSource>().enabled = true;
        object_3D.SetActive(true);

        //Enable UI
        healthContainer.SetActive(true);
        //play revive sound audio
        deathPlayer.clip = revive_clip;
        deathPlayer.Play();

        //reset health
        currentHealth = maxHealth;
        //turn health bar green
        healthBar_FILL.color = green;

        //turn down heartbeat
        desiredVolume = 0f;

        //update health bar
        healthBar.value = currentHealth;

        //take care of 3D / 2D characters
        if (DimensionalShift.is3D)
        {
            if(playerID == 1)
            {
                DimensionalShift.player1_2D.enabled = false;
                DimensionalShift.player1_3D.transform.position = new Vector3(DimensionalShift.player1_3D.transform.position.x, DimensionalShift.player1_3D.transform.position.y, -0.2f);
            }
            if (playerID == 2)
            {
                DimensionalShift.player2_2D.enabled = false;
                DimensionalShift.player2_3D.transform.position = new Vector3(DimensionalShift.player2_3D.transform.position.x, DimensionalShift.player2_3D.transform.position.y, -0.2f);
            }
        }
        else
        {
            if (playerID == 1)
            {
                DimensionalShift.player1_2D.enabled = true;
                DimensionalShift.player1_3D.transform.position = new Vector3(DimensionalShift.player1_3D.transform.position.x, DimensionalShift.player1_3D.transform.position.y, -50f);
            }
            if (playerID == 2)
            {
                DimensionalShift.player2_2D.enabled = true;
                DimensionalShift.player2_3D.transform.position = new Vector3(DimensionalShift.player2_3D.transform.position.x, DimensionalShift.player2_3D.transform.position.y, -50f);
            }
        }
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
