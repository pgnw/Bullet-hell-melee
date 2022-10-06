using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{


    public float MovementSpeed;



    private int FireRateCounter;
    private int DamageCounter;
    private int SpeedCounter;
    private int HealthCounter;


    public float FireRate;
    private GameObject BarrelEnd;
    private Vector3 MousePositionScreen;
    private Vector3 MousePositionWorld;
    public GameObject Player;
    public GameObject Ball;
    private bool Firing;
    public float ProjectileForce;
    private GameObject ProjectilesContainer;
    private CircleCollider2D PlayerCircleCollider;
    private BoxCollider2D PlayerBoxCollider;
    private LineRenderer Line;
    private float TimeStamp;
    private Rigidbody2D PlayerRigidBody2D;
    public float Health;
    public Slider HealthBarSlider;
    private float DamageCoolDown;
    private float DamageTimeStamp;
    private GameObject Body;
    private GameObject Barrel;
    private SpriteRenderer BodyRenderer;
    private SpriteRenderer BarrelRenderer;
    private float Exp;
    private float ProjectileScale;


    private Slider ExpBarSlider;

    private GameObject UpgradeMenu;

    [SerializeField] int UnspentLevels;

    private Vector2 PushDirection;

    [SerializeField] TextMeshProUGUI TimeAliveUI;
    // Start is called before the first frame update

    private void Awake()
    {




        Body = transform.Find("Body").gameObject;
        Barrel = transform.Find("Barrel").gameObject;
        BodyRenderer = Body.GetComponent<SpriteRenderer>();
        BarrelRenderer = Barrel.GetComponent<SpriteRenderer>();
        Health = 100;
        DamageCoolDown = 0.25f;
        Ball = Resources.Load("PlayerProjectile") as GameObject;

        PlayerRigidBody2D = GetComponent<Rigidbody2D>();
        MovementSpeed = 30f;
        FireRate = 2.5f;
        BarrelEnd = transform.Find("Barrel/BarrelEnd").gameObject;
        PlayerCircleCollider = GetComponent<CircleCollider2D>();
        PlayerBoxCollider = GetComponent<BoxCollider2D>();
        ProjectileForce = 50f;

        ProjectilesContainer = GameObject.Find("ProjectilesContainer");

        Physics2D.IgnoreLayerCollision(6, 7, true);
        Line = GetComponent<LineRenderer>();
        //Entity.BlackOutlines(Player);

        UpgradeMenu = GameObject.Find("Main Camera/Canvas/Upgrades/UpgradeButtons");


        ExpBarSlider = GameObject.Find("Main Camera/Canvas/LevelUpBar").GetComponent<Slider>();

        ProjectileScale = 1f;




    }
    void Start()
    {




    }

    // Update is called once per frame



    private void PointToMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        transform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void Controls2()
    {

        float updown = (Input.GetAxis("Vertical") * MovementSpeed) * Time.deltaTime;
        float leftright = (Input.GetAxis("Horizontal") * MovementSpeed) * Time.deltaTime;

        transform.Translate(new Vector3(leftright + PushDirection.x, updown + PushDirection.y, 0), Space.World);



        if (Input.GetMouseButtonDown(0))
        {
            Firing = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Firing = false;
        }

    }



    private void Controls()
    {




        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, 1, 0) * MovementSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, -1, 0) * MovementSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-1, 0, 0) * MovementSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(1, 0, 0) * MovementSpeed;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Firing = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Firing = false;
        }

    }



    private void Fire()
    {

        if (TimeStamp <= Time.time)
        {

            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mouseposworld = new Vector2(worldPoint.x, worldPoint.y);

            Vector2 heading = mouseposworld - transform.position;
            Vector2 direction = heading.normalized;

            GameObject projectile = Instantiate(Ball, BarrelEnd.transform.position, Quaternion.identity, ProjectilesContainer.transform);

            projectile.transform.localScale *= ProjectileScale;


            CircleCollider2D projectileCollider = projectile.GetComponent<CircleCollider2D>();
            Rigidbody2D projectileRigidBody = projectile.GetComponent<Rigidbody2D>();

            projectileRigidBody.AddForce(direction * ProjectileForce, ForceMode2D.Impulse);
            TimeStamp = Time.time + 1 / FireRate;
        }

    }




    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Projectile")
        {
            Destroy(collision.gameObject);

            TakeDamage(15);

        }
        else if (collision.gameObject.tag == "Pickup")
        {

            if (Health < HealthBarSlider.maxValue)
            {
                BodyRenderer.color = Color.green;
                BarrelRenderer.color = Color.green;
                DamageTimeStamp = Time.time + DamageCoolDown;
                Destroy(collision.gameObject);

                Health += HealthBarSlider.maxValue / 5;
                if (Health >= HealthBarSlider.maxValue)
                    Health = HealthBarSlider.maxValue;


            }
            HealthBarSlider.value = Health;

        }
        else if (collision.gameObject.tag == "Charger")
        {
            StartCoroutine(GetPushed(collision.gameObject));


        }


    }

    public void TakeDamage(float damage)
    {
        if (DamageTimeStamp <= Time.time)
        {

            Health -= damage;
            HealthBarSlider.value = Health;
            BodyRenderer.color = Color.red;
            BarrelRenderer.color = Color.red;
            DamageTimeStamp = Time.time + DamageCoolDown;
            if (Health <= 0)
            {
                //Destroy(Player);
                HighScoreScript.OldTimeAlive = HighScoreScript.TimeAlive;
                HighScoreScript.TimeAlive = 0;
                SceneManager.LoadScene("SampleScene");
            }
        }
    }

    private IEnumerator GetPushed(GameObject pusher)
    {
        TakeDamage(pusher.transform.localScale.magnitude * 7);
        Vector2 heading = transform.position - pusher.transform.position;

        PushDirection = heading.normalized * 0.2f;

        yield return new WaitForSeconds(0.2f);
        PushDirection = Vector2.zero;

    }

    public void ExpGain(float amount)
    {

        Exp += amount;
        ExpBarSlider.value = Exp;



        if (Exp >= 100)
        {
            UnspentLevels++;
            UpgradeMenu.SetActive(true);
            Exp -= 100;
            ExpBarSlider.value = Exp;
        }



    }

    public void Upgrade(int choice)
    {
        UnspentLevels--;
        switch (choice)
        {
            case (0):

                FireRate += 3.6f;
                if (FireRate > 18)
                    FireRate = 18;
                FireRateCounter++;
                GameObject.Find($"Main Camera/Canvas/Upgrades/FireRate/Image ({FireRateCounter})").SetActive(true);
                if (FireRateCounter >= 4)
                    Destroy(GameObject.Find("Main Camera/Canvas/Upgrades/UpgradeButtons/FireRateButton"));
                break;


            case (1):

                Entity.DamageIntake += 10;
                ProjectileForce += 10;
                ProjectileScale *= 1.1f;
                DamageCounter++;
                if (DamageCounter >= 4)
                    Destroy(GameObject.Find("Main Camera/Canvas/Upgrades/UpgradeButtons/DamageButton"));
                GameObject.Find($"Main Camera/Canvas/Upgrades/Damage/Image ({DamageCounter})").SetActive(true);
                break;

            case (2):
                MovementSpeed += 8;
                SpeedCounter++;
                if (SpeedCounter >= 4)
                    Destroy(GameObject.Find("Main Camera/Canvas/Upgrades/UpgradeButtons/SpeedButton"));
                GameObject.Find($"Main Camera/Canvas/Upgrades/Speed/Image ({SpeedCounter})").SetActive(true);
                break;
            case (3):

                Health += HealthBarSlider.maxValue * 0.3f;
                HealthBarSlider.maxValue += HealthBarSlider.maxValue * 0.3f;
                HealthBarSlider.value = Health;

                Player.transform.localScale *= 1.2f;
                GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize *= 1.1f;



                ProjectileScale *= 1.1f;
                HealthCounter++;

                GameObject.Find($"Main Camera/Canvas/Upgrades/Health/Image ({HealthCounter})").SetActive(true);
                if (HealthCounter >= 4)
                    Destroy(GameObject.Find("Main Camera/Canvas/Upgrades/UpgradeButtons/HealthButton"));
                break;
        }




        if (UnspentLevels != 0)
            UpgradeMenu.SetActive(true);
        else
            UpgradeMenu.SetActive(false);
    }


    void Update()
    {
        if (DamageTimeStamp <= Time.time)
        {
            BodyRenderer.color = Color.white;
            BarrelRenderer.color = Color.white;
        }


        PointToMouse();
        Controls2();
        HighScoreScript.TimeAlive = HighScoreScript.TimeAlive + Time.deltaTime;

        TimeAliveUI.text = ($"Time alive: {(int)HighScoreScript.TimeAlive}");

        if (Firing)
        {
            Fire();




        }


    }
}
