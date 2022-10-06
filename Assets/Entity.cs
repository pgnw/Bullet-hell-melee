using System.Collections;
using UnityEngine;
public class Entity : MonoBehaviour
{



    static public GameObject Player;
    public float FireRate;
    public float TimeStamp;
    static public GameObject Ball;
    public GameObject BarrelEnd;
    static public GameObject ProjectilesContainer;
    public float ProjectileForce;
    public float Health;

    protected float DamageCoolDown;
    protected float DamageTimeStamp;
    private Rigidbody2D EntityRigidBody;
    public float MovementSpeed;
    public float Range;
    public float ProjectileScale;
    static public GameObject HealthPickup;
    static protected GameObject PickupsContainer;

    static public int DamageIntake;

    public float ExpValue;
    protected float StrafeTimeStamp;
    public bool Pause;
    public float ChargeTimeStamp;
    public float ChargingTimeStamp;
    public float CircleDirectionTimeStamp;
    public GameObject[] Parts;
    public SpriteRenderer[] SpriteRenderers;
    private int CircleDirection;
    public void Startup(bool hasBarrel = true)
    {
        CircleDirection = 1;
        CircleDirectionTimeStamp = 1;
        StrafeTimeStamp = 0;
        ChargeTimeStamp = 0;
        ChargingTimeStamp = 0;
        ProjectileScale = 1f;
        Range = 75f;
        MovementSpeed = 8f;
        EntityRigidBody = GetComponent<Rigidbody2D>();
        DamageCoolDown = 0.05f;

        Health = 100;
        ProjectileForce = 45f;
        FireRate = 0.3f;

        ExpValue = 35;

        if (hasBarrel)
        {
            BarrelEnd = transform.Find("Barrel/BarrelEnd").gameObject;
            //Barrel = transform.Find("Barrel").gameObject;
            //BarrelRenderer = Barrel.GetComponent<SpriteRenderer>();
        }

        Parts = new GameObject[transform.childCount];
        SpriteRenderers = new SpriteRenderer[transform.childCount];
        for (int index = 0; index < transform.childCount; index++)
        {
            Parts[index] = transform.GetChild(index).gameObject;
            SpriteRenderers[index] = Parts[index].GetComponent<SpriteRenderer>();
        }












        //Physics2D.IgnoreLayerCollision(8, 9);

    }

    public void SetColor(Color colorInput)
    {
        for (int index = 0; index < Parts.Length; index++)
        {

            SpriteRenderers[index].color = colorInput;

        }
    }


    static public void BlackOutlines(GameObject source)
    {
        float distanceMult = 1f;

        distanceMult = source.transform.localScale.magnitude / 3.5f;

        GameObject body = source.transform.Find("Body").gameObject;
        GameObject barrel = source.transform.Find("Barrel").gameObject;



        GameObject[] bodys = new GameObject[4];
        GameObject[] Barrels = new GameObject[4];
        Vector3 offSet = new Vector3(0, 0, 0);

        for (int counter = 0; counter <= 3; counter++)
        {
            switch (counter)
            {
                case 0:
                    offSet.y = 1 * distanceMult;
                    break;
                case 1:
                    offSet.y = -1 * distanceMult;
                    break;
                case 2:
                    offSet.x = 1 * distanceMult;
                    break;
                case 3:
                    offSet.x = -1 * distanceMult;
                    break;
            }



            bodys[counter] = Instantiate(body, body.transform.position + offSet, Quaternion.identity, source.transform);
            Barrels[counter] = Instantiate(barrel, barrel.transform.position + offSet, Quaternion.identity, source.transform);


            SpriteRenderer bodySprite = bodys[counter].GetComponent<SpriteRenderer>();
            SpriteRenderer barrelSprite = Barrels[counter].GetComponent<SpriteRenderer>();
            bodySprite.color = Color.black;
            barrelSprite.color = Color.black;
            bodySprite.sortingLayerName = "Outlines";
            barrelSprite.sortingLayerName = "Outlines";


            offSet = new Vector3(0, 0, 0);
        }



    }




    public void ShootAt(GameObject target)
    {
        Vector2 heading = target.transform.position - transform.position;
        if (heading.magnitude <= Range)
        {
            if (TimeStamp <= Time.time)
            {
                TimeStamp = Time.time + FireRate;



                Vector2 direction = heading.normalized;


                GameObject projectile = Instantiate(Ball, BarrelEnd.transform.position, Quaternion.identity, ProjectilesContainer.transform);
                projectile.transform.localScale = projectile.transform.localScale * ProjectileScale;


                CircleCollider2D projectileCollider = projectile.GetComponent<CircleCollider2D>();
                Rigidbody2D projectileRigidBody = projectile.GetComponent<Rigidbody2D>();

                projectileRigidBody.AddForce(direction * ProjectileForce, ForceMode2D.Impulse);
                Destroy(projectile, 15);

            }
        }



    }


    public void PointTo(GameObject target)
    {


        Vector2 Facing = target.transform.position - transform.position;
        float angle = Vector2.SignedAngle(Vector2.right, Facing);


        transform.eulerAngles = new Vector3(0, 0, angle);

    }







    public void OnTriggerEnter2D(Collider2D collision)
    {


        if (DamageTimeStamp <= Time.time & collision.tag == "Projectile")
        {
            Health -= DamageIntake;
            SetColor(Color.red);

            Destroy(collision.gameObject);
            if (Health <= 0)
            {
                Death();
                Destroy(gameObject);
            }
            DamageTimeStamp = Time.time + DamageCoolDown;
        }
        else if (collision.tag == "Projectile")
        {
            Destroy(collision.gameObject);

        }
    }


    public void Follow(GameObject target, bool charger = false)
    {



        Vector2 heading = target.transform.position - transform.position;
        Vector2 direction = heading.normalized;






        if (!Pause)
        {
            if (!charger)
                EntityRigidBody.AddForce(direction * MovementSpeed * SlowWhenClose(target), ForceMode2D.Impulse);
            else
                EntityRigidBody.AddForce(direction * MovementSpeed * SlowWhenCloseCharger(target), ForceMode2D.Impulse);

            PointTo(target);
        }
    }
    private float SlowWhenCloseCharger(GameObject target)
    {
        Vector2 heading = target.transform.position - transform.position;

        float distanceFromTarget = heading.magnitude;
        float speedAdjust = 1f;

        if (distanceFromTarget <= 8)
            speedAdjust = 0.6f;

        else if (distanceFromTarget <= 14)
            speedAdjust = 0.7f;
        else if (distanceFromTarget <= 16)
            speedAdjust = 0.8f;
        else if (distanceFromTarget <= 20)
            speedAdjust = 0.9f;
        return speedAdjust;
    }
    private float SlowWhenClose(GameObject target)
    {
        Vector2 heading = target.transform.position - transform.position;




        float distanceFromTarget = heading.magnitude;
        float speedAdjust = 1f;

        if (distanceFromTarget <= 10)
            speedAdjust = 0;

        else if (distanceFromTarget <= 12)
            speedAdjust = 0.2f;

        else if (distanceFromTarget <= 14)
            speedAdjust = 0.5f;
        else if (distanceFromTarget <= 16)
            speedAdjust = 0.7f;
        else if (distanceFromTarget <= 20)
            speedAdjust = 0.8f;
        else if (distanceFromTarget <= 26)
            speedAdjust = 0.9f;

        return speedAdjust;
    }
    public void Strafe(GameObject target)
    {

        Vector2 distanceVector = transform.position - target.transform.position;

        if (distanceVector.magnitude >= 18 && !Pause)
        {



            if (StrafeTimeStamp <= Time.time)
            {
                bool leftRight = Random.value >= 0.5;
                StrafeTimeStamp = Time.time + Random.Range(1, 6);

                if (leftRight)
                {

                    EntityRigidBody.AddForce(transform.up * MovementSpeed * 10000, ForceMode2D.Force);

                }
                else
                {

                    EntityRigidBody.AddForce(transform.up * -1 * MovementSpeed * 10000, ForceMode2D.Force);

                }
            }




        }




    }


    public void w(GameObject target)
    {

        Vector2 heading = Vector2.zero;
        Vector2 direction = Vector2.zero;

        if (ChargeTimeStamp <= Time.time)
        {
            ChargeTimeStamp = Time.time + 8;

            ChargingTimeStamp = Time.time + 3;


            heading = target.transform.position - transform.position;

            direction = heading.normalized;


        }

        if (ChargingTimeStamp >= Time.time)
        {
            Pause = true;
            SetColor(Color.red);
            EntityRigidBody.AddForce(direction * MovementSpeed * 120, ForceMode2D.Force);
        }
        else
        {
            SetColor(Color.white);
            Pause = false;
        }
    }
    public void EntityUpdate()
    {
        if (DamageTimeStamp <= Time.time && !Pause)
        {
            SetColor(Color.white);

        }
    }
    public IEnumerator Charge(GameObject target)
    {

        while (true)
        {

            yield return new WaitForSeconds(3);
            Vector2 heading = target.transform.position - transform.position;
            Vector2 direction = heading.normalized;

            if (heading.magnitude >= 15)
            {


                PointTo(Player);
                Pause = true;
                SetColor(Color.red);


                yield return new WaitForSeconds(0.4f);



                for (int counter = 0; counter <= 60; counter++)
                {
                    EntityRigidBody.AddForce(direction * MovementSpeed * 1000, ForceMode2D.Force);
                    yield return new WaitForEndOfFrame();
                }

                PointTo(Player);
                yield return new WaitForSeconds(1);
                Pause = false;
                SetColor(Color.white);

                StrafeTimeStamp += 2;
            }
        }
    }

    public void Circle(GameObject target)
    {


        if (CircleDirectionTimeStamp <= Time.time)
        {
            CircleDirectionTimeStamp += 5;

            CircleDirection *= -1;


        }

        EntityRigidBody.AddForce(transform.up * CircleDirection * MovementSpeed, ForceMode2D.Impulse);

    }




    public void Death()
    {


        Player.GetComponent<PlayerController>().ExpGain(ExpValue);

        Instantiate(HealthPickup, transform.position, Quaternion.identity, PickupsContainer.transform);


    }

}

