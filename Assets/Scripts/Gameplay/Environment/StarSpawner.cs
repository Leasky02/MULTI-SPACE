using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSpawner : MonoBehaviour
{
    //star prefab
    [SerializeField] private GameObject starPrefab;
    //star count variable
    [SerializeField] private int maxCount;
    [SerializeField] private int minCount;
    private int starCount;
    //boundries for stars to spawn
    [SerializeField] private Transform boundary1;
    [SerializeField] private Transform boundary2;

    // Start is called before the first frame update
    void Start()
    {
        //generate number of stars to spawn
        starCount = Random.Range(minCount, maxCount);

            
        for(int i = 0; i < starCount; i++)
        {
            //create star and set position
            GameObject star = Instantiate(starPrefab);
            star.transform.SetParent(gameObject.transform);
            //calculate random position within boundries
            star.transform.position = new Vector2(Random.Range(boundary1.position.x, boundary2.position.x), Random.Range(boundary1.position.y, boundary2.position.y));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
