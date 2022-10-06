public class ChargerAI : Entity
{

    private void Start()
    {
        Startup(false);
        StartCoroutine(Charge(Player));
    }
    void Update()
    {
        EntityUpdate();
        Follow(Player, true);
        //Strafe(Player);
        //ShootAt(Player);



    }
}


