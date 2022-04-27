using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //speed of bullet
    [SerializeField] private int speed;
    //static variable for damage player can do (50 as default)
    static int damage = 50;

    //audio clips
    [SerializeField] private AudioClip bulletShoot;
    [SerializeField] private AudioClip bulletHit;

    public void Shoot(GameObject player)
    {
        //set damage of bullet according to equation and wave***


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
        //if collision is with enemy
        if(collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyHealth>().Damage(damage);

            //Play target HIT sound
            GetComponent<AudioSource>().clip = bulletHit;
            GetComponent<AudioSource>().Play();
        }
        //remove bullet
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        Destroy(gameObject, 1f);

    }
}
