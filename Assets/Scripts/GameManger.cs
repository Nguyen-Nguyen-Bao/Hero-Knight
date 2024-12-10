using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class GameManager : ScriptableObject
{
    public bool inVines;
    public bool gradVines;
    public bool isBloking;
    public Vector3 playerposition;
    public float playerhealth;
    public int coin;
    public void Begin()
    {
        playerhealth = 100;
        coin = 0;
    }
}
