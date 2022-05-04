using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToRemove;
    public static int playerCount = 1;
    //set the player count
    public void PlayerCount(int players)
    {
        playerCount = players;
    }

    private void Start()
    {
        //if singleplayer
        if(playerCount == 1)
        {
            //destroy required objects
            for(int i = 0; i< objectsToRemove.Length; i++)
            {
                Destroy(objectsToRemove[i]);
            }
            PlayerHealth.oneRemaining = true;
        }
    }
}
