public class BasicEnemy : Entity
{








    // Start is called before the first frame update
    void Awake()
    {

        Startup();

        //BlackOutlines(gameObject);
        //StartCoroutine(Charge(Player));

    }



    // Update is called once per frame
    void Update()
    {
        EntityUpdate();
        Follow(Player);
        //Strafe(Player);
        ShootAt(Player);



    }
}
