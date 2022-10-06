public class CirclerAI : Entity
{
    public void Start()
    {
        Startup();
    }

    public void Update()
    {

        EntityUpdate();
        PointTo(Player);
        Circle(Player);
        Follow(Player);
        ShootAt(Player);
    }




}