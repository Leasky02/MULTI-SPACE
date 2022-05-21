using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //destroy parent in 1 second
        Destroy(transform.parent.gameObject, 1f);
        //apply force in random direction
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 10 , ForceMode2D.Impulse);
    }
}
