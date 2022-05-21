using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    //damage display prefab
    [SerializeField] private GameObject damage_TXT;

    private void Start()
    {
        //set pitch of audio source
        GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.5f);
    }

    public void Shoot(float directionToFire , float horizontalInput , float verticalInput)
    {
        Vector2 movementDirection = new Vector2(horizontalInput, verticalInput);
        movementDirection.Normalize();

        transform.eulerAngles = new Vector3(0, 0, directionToFire);
        if(movementDirection != Vector2.zero)
        {
            //add force in direction
            GetComponent<Rigidbody2D>().AddForce(movementDirection * speed * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
        else
        {
            //add force in direction
            GetComponent<Rigidbody2D>().AddForce(transform.up.normalized * speed * Time.fixedDeltaTime, ForceMode2D.Impulse);
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

            //create damage display object
            //spawn text object
            GameObject damageDisplay = Instantiate(damage_TXT, transform.position, Quaternion.identity);
            //set text of object (child of instantiated object)
            damageDisplay.gameObject.transform.GetChild(0).GetComponent<Text>().text = ("" + (damage + Random.Range(-2 , 2)));
            //set colour of the text
            damageDisplay.gameObject.transform.GetChild(0).GetComponent<Text>().color = new Color(0.589082f, 0.9622642f, 0.6436454f, 1f);
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
