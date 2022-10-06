using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{

    GameObject Player;
    float RespawnTimeStamp;
    float DifficultyScaler;
    GameObject BasicEnemy;
    GameObject Charger;
    GameObject Circler;
    GameObject NPCs;
    GameObject Enemy;
    GameObject choice;

    float WaveTimeStamp;
    bool WaveActive;
    int SpawnCounter;
    int MaxSpawns;
    private void Awake()
    {

        Player = GameObject.Find("Player");
        NPCs = GameObject.Find("NPCs");
        BasicEnemy = Resources.Load("BasicEnemy") as GameObject;
        Charger = Resources.Load("Charger") as GameObject;
        Circler = Resources.Load("Circler") as GameObject;
        DifficultyScaler = 1;

        WaveTimeStamp = 0;
        SpawnCounter = 0;
        MaxSpawns = 0;
    }

    private void Spawn()
    {
        if (Time.time <= WaveTimeStamp)
            return;

        if (SpawnCounter >= MaxSpawns)
        {

            MaxSpawns = Random.Range(2, 7) * ((int)DifficultyScaler);
            SpawnCounter = 0;

            WaveTimeStamp = Time.time + 10f + DifficultyScaler;

        }



        if (RespawnTimeStamp <= Time.time)
        {
            SpawnCounter++;
            float random = Random.value;

            if (random >= 0.8f)
                choice = Charger;
            else if (random >= 0.15f)
                choice = BasicEnemy;
            else
                choice = Circler;


            RespawnTimeStamp = Time.time + Random.Range(2, 3);


            Enemy = Instantiate(choice, SpawnPosition(), Quaternion.identity, NPCs.transform);
            Entity enemyScript = Enemy.GetComponent<Entity>();



            SetValues(enemyScript);

            DifficultyScaler = DifficultyScaler + 0.1f;
        }
    }

    private void SetValues(Entity enemyscript)
    {

        float a = Random.Range(20, 80);
        float b = Random.Range(45, 140);

        enemyscript.FireRate = Random.Range(0.5f, 1.5f) / DifficultyScaler;
        enemyscript.Range = (a - 15) * DifficultyScaler;
        enemyscript.Health = b * DifficultyScaler;
        enemyscript.MovementSpeed = ((280 / b) + 5) * DifficultyScaler;
        enemyscript.ProjectileForce = ((a + 15) * DifficultyScaler);



        Enemy.transform.localScale *= (enemyscript.Health / 100);
        enemyscript.ProjectileScale *= (enemyscript.Health / 100);
    }

    private Vector3 SpawnPosition()
    {

        Vector2 screenPosition = Camera.main.WorldToScreenPoint(Player.transform.position);

        bool yes = Random.value < 0.5f;
        bool yes2 = Random.value < 0.5f;
        float y = 0f;
        float x = 0f;
        if (yes)
            y = Screen.height + Random.Range(10, 150);
        else
            y = 0 - Random.Range(10, 150);

        if (yes2)
            x = Screen.width + Random.Range(10, 150);
        else
            x = 0 - Random.Range(10, 150);

        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 0));
        return new Vector3(pos.x, pos.y, 0);
    }



    // Update is called once per frame
    void Update()
    {

        Spawn();

    }
}
