using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemiesManager : MonoBehaviour{
	public static List<int> EnemiesAttacking;
    public static bool IgnorePlayer;

    public static void ResetListAttacking () {
        EnemiesAttacking = new List<int>();
    }
}
