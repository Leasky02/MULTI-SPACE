using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //speed of bullet
    [SerializeField] private int speed;
    //static variable for damage player can do (50 as default)
    public static int damage = 50;
    //bullet been used already?
    private bool used = false;
    //audio clips
    [SerializeField] private AudioClip bulletShoot;
    [SerializeField] private AudioClip bulletHit;

    private void Start()
    {
        //set pitch of audio source
        GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.5f);
    }

    public void Shoot(GameObject player)
    {
        //set damage of bullet according to upgrade level

        //set direction to fire to player rotation
        float directionToFire = player.GetComponent<PlayerMovement>().currentRotation ;

        //fire up
        if (directionToFire == 0)
        {
            //set bullet rotation
            transform.eulerAngles = new Vector3(0, 0, directionToFire);
            //add force
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * speed * Time.deltaTime, ForceMode2D.Impulse);
        }
        //fire down
        if (directionToFire == 180)
        {
            //set bullet rotation
            transform.eulerAngles = new Vector3(0, 0, directionToFire);
            //add force
            GetComponent<Rigidbody2D>().AddForce(Vector2.down * speed * Time.deltaTime, ForceMode2D.Impulse);
        }
        //fire left
        if (directionToFire == 90)
        {
            //set bullet rotation
            transform.eulerAngles = new Vector3(0, 0, directionToFire);
            //add force
            GetComponent<Rigidbody2D>().AddForce(Vector2.left * speed * Time.deltaTime, ForceMode2D.Impulse);
        }
        //fire right
        if (directionToFire == 270)
        {
            //set bullet rotation
            transform.eulerAngles = new Vector3(0, 0, directionToFire);
            //add force
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * speed * Time.deltaTime, ForceMode2D.Impulse);
        }

        //Play shooting sound
        GetComponent<AudioSource>().clip = bulletShoot;
        GetComponent<AudioSource>().Play();
    }

    //when the bullet collides with something
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if collision is with enemy and bullet hasn't already hit anything else
        if (collision.CompareTag("Enemy") && !used)
        {
            collision.gameObject.GetComponent<EnemyHealth>().Damage(damage);

            //Play target HIT sound
            GetComponent<AudioSource>().clip = bulletHit;
            GetComponent<AudioSource>().Play();
        }
        //remove bullet
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        Destroy(gameObject, 1f);

        //play death particles
        gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Play();

        //bullet cannot deal damage anymore
        //prevent it being used on multiple enemies
        used = true;
    }
}
