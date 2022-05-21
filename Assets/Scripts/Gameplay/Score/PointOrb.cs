using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointOrb : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;
    //prefab of the damage text
    [SerializeField] private GameObject damage_TXT;

    private int scoreToAdd;

    // Start is called before the first frame update
    void Start()
    {
        //destroy itself after 10-15s
        Invoke("RemoveOrb", Random.Range(10f, 15f));

        int classInt = Random.Range(1 , 4);

        switch (classInt)
        {
            case 1:
                transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                scoreToAdd = Random.Range(50, 70);
                break;
            case 2:
                transform.localScale = new Vector3(1, 1, 1);
                scoreToAdd = Random.Range(90, 110);
                break;
            case 3:
                transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                scoreToAdd = Random.Range(130, 150);
                break;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if collided with player
        if(collision.CompareTag("Player"))
        {
            //add score to player randomly
            collision.gameObject.GetComponent<PlayerScore>().AddScore(scoreToAdd);

            //play sound
            GetComponent<AudioSource>().Play();

            RemoveOrb();

            //spawn text object
            GameObject damageDisplay = Instantiate(damage_TXT, transform.position, Quaternion.identity);
            //set text of object (child of instantiated object)
            damageDisplay.gameObject.transform.GetChild(0).GetComponent<Text>().text = ("" + (scoreToAdd ));
            //set colour of the text
            damageDisplay.gameObject.transform.GetChild(0).GetComponent<Text>().color = new Color(0.8675666f, 0.7905393f, 0.8962264f, 1f);
        }
    }

    private void RemoveOrb()
    {
        //hide self
        GetComponent<SpriteRenderer>().enabled = false;
        //stop particles
        ps.loop = false;
        //cue destruction
        Destroy(gameObject, 2.5f);
        //disable collider
        GetComponent<CircleCollider2D>().enabled = false;
    }
}
