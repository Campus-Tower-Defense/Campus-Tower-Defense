using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTower : Tower
{

    private bool isTowerActive = true;

    [SerializeField]
    private GameObject top;

    [SerializeField]
    private float projectileSpeed = 100f;

    [SerializeField]    
    private ParticleSystem shootParticleSystem;

    private AudioSource audioSource;

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space pressed");
            Shoot();
        }
    }

    private void Shoot(float distance = 100f, Vector3? target = null)
    {
        // Already rotated towards enemy

        // Add speed towards looking direction
        shootParticleSystem.Play();
        if (audioSource != null)
        {
            audioSource.Play();
        }

    }

    protected override void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        //Debug.Log("WallTower Start");
        if (top == null)
        {
            Debug.LogError("Tower-Top not found");
        }
        if (shootParticleSystem == null)
        {
            Debug.LogError("ParticleSystem not found");
        }

        //call super start
        this.damage = 1;
        base.Start();
        
    }
    protected override void TowerAction()
    {
        isTowerActive = true;
       // Debug.Log("Action?: " + enemiesInRange.Count);
        for (int i = 0; i < enemiesInRange.Count; i++)
        {

            if (!IsInSight(enemiesInRange[i]))
            {
                Debug.Log("Enemy not in sight");
                continue;
            }

            //rotate towards enemy
            RotateTowardsEnemy(enemiesInRange[i].transform.position);

            //shoot enemy
           // Debug.Log("Shoot");
            Shoot(target: enemiesInRange[i].transform.position);

            //damage enemy
          //  Debug.Log("Damage");
            enemiesInRange[i].GetComponent<EnemyParent>().Damage(Damage);
            break;
        }

        timeSinceLastAction = Time.deltaTime;
        isTowerActive = false;
    }

    private bool IsInSight(GameObject enemy)
    {
        //TODO fix this
        return true;
        /*
        RaycastHit hit;
        if (Physics.Raycast(top.transform.position, enemy.transform.position - top.transform.position, out hit))
        {
            if (hit.collider.gameObject == enemy)
            {
                return true;
            }
        }
        return false;*/
    }

    private void RotateTowardsEnemy(Vector3 enemyPosition)
    {
       // Debug.Log("Rotate towards enemy");
        // Berechne die Richtung zum Gegner auf der 2D-Ebene
        Vector3 direction = enemyPosition - top.transform.position;

     //   Debug.Log("Direction: " + direction);
        direction.y = 0f; // Setze die y-Komponente auf 0, um nur auf der horizontalen Ebene zu rotieren
        direction = Quaternion.Euler(0f, 360f - 90f, 0f) * direction;
        // Drehe das Objekt top in die berechnete Richtung
        if (direction != Vector3.zero)
        {
            float maxRotation = Time.deltaTime * this.frequency * 1000f;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            top.transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
        }
    }

    protected override void PassiveAbility()
    {
        //no passive ability
    }

    protected override void InitializeUpgrades()
    {
        availableUpgrades = new Dictionary<int, Upgrade>();
        Upgrade upgrade1 = new Upgrade("Damage Upgrade", "Increases damage by 5.", 100, (tower) => tower.Damage += 5, (tower) => tower.Damage -= 5, this);
    }

    protected override void InitializeStats()
    {
        this.cost = 100;
        this.damage = 1;
        this.Frequency = 100f;
        this.Range = 10;
        this.towerName = "Wall Tower";
        this.description = "A Wall tower that shoots at enemies.\n" +
        "Deals " + damage + " damage per shot.\n" +
        "Costs " + cost + " gold.\n" +
        "Range: " + range + "\n" +
        "Frequency: " + frequency + "\n" +
        "Fires at the first enemy in range.";
    }
}
