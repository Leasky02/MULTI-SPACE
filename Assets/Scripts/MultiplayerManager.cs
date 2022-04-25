using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour
{
    public static int playerCount = 1;
    //set the player count
    public void PlayerCount(int players)
    {
        playerCount = players;
    }
}
