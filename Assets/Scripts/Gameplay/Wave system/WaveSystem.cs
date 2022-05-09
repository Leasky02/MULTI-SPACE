using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSystem : MonoBehaviour
{
    //Progression equation variables (Y = MX + C) , Y = 6x + 1
    [SerializeField] private int startingEnemies; //C
    [SerializeField] private int enemyMultiplier; //M
    public static int wave; //X
    [SerializeField] private int totalEnemiesToSpawn; //Y

    //alien prefabs
    [SerializeField] private GameObject[] aliens;
    //spawn rates
    private int[] probabilities = new int[10];
    //spawan locations
    [SerializeField] private Vector2[] currentSpawnLocations;
    [SerializeField] private Vector2[] spawnLocations;

    //should it check for aliens
    private bool checkAliens;
    private GameObject[] activeAliens;

    //UI element
    [SerializeField] private Text waveDisplay;

    // Start is called before the first frame update
    void Start()
    {
        //if single player then make game slightly easier
        if (MultiplayerManager.playerCount == 1)
            enemyMultiplier -= 2;
        //start first wave
        Invoke("NextWave", 1f);

        //check for aliens
        InvokeRepeating("CheckForAliens", 1f, 1f);

        //set probabilities (1 = spy, 2 = soldier, 3 = commander)
        probabilities[0] = 0;
        probabilities[1] = 0;
        probabilities[2] = 1;
        probabilities[3] = 1;
        probabilities[4] = 1;
        probabilities[5] = 1;
        probabilities[6] = 1;
        probabilities[7] = 1;
        probabilities[8] = 1;
        probabilities[9] = 2;
    }

    //check for aliens in the scene
    private void CheckForAliens()
    {
        //if the game needrs to be detecting aliens
        if(checkAliens)
        {
            //get all aliens
            activeAliens = GameObject.FindGameObjectsWithTag("Enemy");
            //if there are NO aliens
            if(activeAliens.Length == 0)
            {
                NextWave();
            }
        }
    }

    //start next wave
    private void NextWave()
    {
        wave++;

        checkAliens = false;

        //calculate enemies to spawn Y = MX + C
        totalEnemiesToSpawn = (enemyMultiplier * wave) + startingEnemies;

        //pick random spawn locations
        currentSpawnLocations = new Vector2[Random.Range(4 , 8)];
        
        for(int i = 0; i < currentSpawnLocations.Length; i++)
        {
            currentSpawnLocations[i] = spawnLocations[Random.Range(0, spawnLocations.Length)];
        }

        //spawn first alien
        Invoke("SpawnAlien", 0.5f);

        //change UI tet in half a second (in sync with animation)
        waveDisplay.gameObject.GetComponent<Animator>().Play("WaveTransition");
        //cue text change
        Invoke("ChangeText", 0.2f);

        if (PlayerHealth.deadPlayers.Count > 0)
        {
            //revive dead player
            PlayerHealth.deadPlayers[0].GetComponent<PlayerHealth>().Revive();
        }
    }

    private void ChangeText()
    {
        waveDisplay.text = ("Wave " + wave);
        //play sound
        waveDisplay.gameObject.GetComponent<AudioSource>().Play();
    }

    //spawn alien
    private void SpawnAlien()
    {
        checkAliens = true;

        //subtract one from total enemies to spawn
        totalEnemiesToSpawn--;
        //create random alien variation at random location
        Instantiate(aliens[probabilities[Random.Range(0, probabilities.Length)]], currentSpawnLocations[Random.Range(0, currentSpawnLocations.Length)] , Quaternion.identity);

        //if there are still enemies to spawn
        if(totalEnemiesToSpawn > 0)
        {
            //call function again
            Invoke("SpawnAlien", Random.Range(0.2f, 0.5f));
        }
    }
}
