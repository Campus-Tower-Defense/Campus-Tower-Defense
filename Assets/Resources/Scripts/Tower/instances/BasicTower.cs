using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : Tower
{

    private bool isTowerActive = true;

    public GameObject projectileSpawnPoint;

    public GameObject projectilePrefab;

    [SerializeField]
    private GameObject top;

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space pressed");
            Shoot();
        }
    }

    private void Shoot(float distance = 10f)
    {
        // Already rotated towards enemy
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.transform.position, Quaternion.identity);

        // Add speed towards looking direction
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = top.transform.forward * 10;

        // Destroy projectile after it traveled a certain distance
        float timeToLive = distance / rb.velocity.magnitude;
        Destroy(projectile, timeToLive);
    }

    protected override void Start()
    {
        Debug.Log("BasicTower Start");
        if (top == null)
        {
            Debug.LogError("Tower-Top not found");
        }
        //call super start
        this.damage = 1;
        base.Start();
    }
    protected override void TowerAction()
    {
        isTowerActive = true;
        Debug.Log("Action?: " + enemiesInRange.Count);
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
            Debug.Log("Shoot");
            Shoot();

            //damage enemy
            Debug.Log("Damage");
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
        Debug.Log("Rotate towards enemy");
        // Berechne die Richtung zum Gegner auf der 2D-Ebene
        Vector3 direction = enemyPosition - top.transform.position;
        direction.y = 0f; // Setze die y-Komponente auf 0, um nur auf der horizontalen Ebene zu rotieren

        // Drehe das Objekt top in die berechnete Richtung
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            top.transform.rotation = Quaternion.RotateTowards(top.transform.rotation, targetRotation, Time.deltaTime * this.frequency);
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
        this.Range = 20;
        this.towerName = "Basic Tower";
        this.description = "A basic tower that shoots at enemies.\n" +
        "Deals 10 damage per shot.\n" +
        "Costs 100 gold.\n" +
        "Range: 10\n" +
        "Attack Speed: 1\n" +
        "Frequency: 60\n" +
        "Fires at the first enemy in range.";
    }
}
