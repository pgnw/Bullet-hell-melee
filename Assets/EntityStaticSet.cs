using UnityEngine;

public class EntityStaticSet : Entity
{



    void Awake()
    {
        DamageIntake = 15;

        HealthPickup = Resources.Load("Health Pickup") as GameObject;
        PickupsContainer = GameObject.Find("PickupsContainer");
        Ball = Resources.Load("Projectile") as GameObject;
        Player = GameObject.Find("Player");
        ProjectilesContainer = GameObject.Find("ProjectilesContainer");


    }



}
