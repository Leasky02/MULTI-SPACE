using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOrb : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        //destroy itself after 10-15s
        Invoke("RemoveOrb", Random.Range(10f, 15f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if collided with player
        if(collision.CompareTag("Player"))
        {
            //add score to player randomly
            collision.gameObject.GetComponent<PlayerScore>().AddScore(Random.Range(50 , 100));

            //play sound
            GetComponent<AudioSource>().Play();

            RemoveOrb();

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
