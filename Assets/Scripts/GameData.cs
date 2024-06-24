using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/GameData", fileName = "GameData")]
public class GameData : ScriptableObject
{
    [Min(0)]
    public float templeCurrentHealth;
    [Min(0)]
    public float templeTotalHealth;
    public int scorePoints;
    public int convertedPlayers;
    public int noOfArcherries;
    public int noOfDemolition;
    public int noOfRepairer;
    public int enemiesKilled;

    public bool templePurchased;
    public bool archerryPurchased;
    public bool DemolationPurchased;
    public bool WorkshopPurchased;

    public List<int> upgradesPrice = new List<int>() { 100, 500, 2000, 750, 7500 };
    public List<int> upgradesBuildingPrice = new List<int>() { 5000, 8000, 15000, 25000 };

}

public static class UTILS
{
    public static readonly string ENEMY_SMALLER = "Enemy";
    public static readonly string ENEMY_SMALL = "EnemySmall";
    public static readonly string ENEMY_BIG = "EnemyBig";
    public static readonly string PLAYER = "Player";




    public static readonly string TEMPLECURRENTHEALTH = "templeCurrentHealth";
    public static readonly string TEMPLETOTALHEALTH = "templeTotalHealth";
    public static readonly string SCOREPOINTS = "scorePoints";
    public static readonly string CONVERTEDPLAYERS = "convertedPlayers";
    public static readonly string NOOFARCHERRIES = "noOfArcherries";
    public static readonly string NOOFREPAIRER = "noOfRepairer";
    public static readonly string ENEMIESKILLED = "enemiesKilled";

    public static readonly string TEMPLEPURCHASED = "templePurchased";
    public static readonly string ARCHERRYPURCHASED = "archerryPurchased";
    public static readonly string DEMOLATIONPURCHASED = "DemolationPurchased";
    public static readonly string WORKSHOPPURCHASED = "WorkshopPurchased";
}

[SerializeField]
public class Data
{
    [Min(0)]
    public float templeCurrentHealth;
    [Min(0)]
    public float templeTotalHealth;
    public int scorePoints;
    public int convertedPlayers;
    public int noOfArcherries;
    public int noOfRepairer;
    public int enemiesKilled;

    public bool templePurchased;
    public bool archerryPurchased;
    public bool DemolationPurchased;
    public bool WorkshopPurchased;
}