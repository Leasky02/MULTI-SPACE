using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //playerID
    [SerializeField] private int playerID;

    //static fire rate with default 1 to start with
    static float fireRate = 1.5f;
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
        //if shoot button pressed AND player isn't in 3D
        if(Input.GetButton("Shoot" + playerID) && Time.time >= nextTimeToFire && !DimensionalShift.is3D)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            //spawn bullet at the spawn position
            var myBullet = Instantiate(bulletPrefab, bulletSpawnPosition.position, Quaternion.identity);
            //call bullet to shoot after spawning with self as parameter
            myBullet.GetComponent<Bullet>().Shoot(gameObject);
        }
    }
}
