using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //playerID
    [SerializeField] private int playerID;

    //fire rate
    [SerializeField] private float fireRate;
    private float nextTimeToFire = 0f;
    //damage
    [SerializeField] private int damage;

    //spawn position
    [SerializeField] private Transform bulletSpawnPosition;
    //bullet prefab
    [SerializeField] private GameObject bulletPrefab;

    // Update is called once per frame
    void Update()
    {
        //if shoot button pressed
        if(Input.GetButton("Shoot" + playerID) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            //spawn bullet at the spawn position
            var myBullet = Instantiate(bulletPrefab, bulletSpawnPosition.position, Quaternion.identity);
            //call bullet to shoot after spawning with self as parameter
            myBullet.GetComponent<Bullet>().Shoot(gameObject);
        }
    }
}
