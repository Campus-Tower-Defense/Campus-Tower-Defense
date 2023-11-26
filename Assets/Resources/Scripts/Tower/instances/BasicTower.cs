using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : Tower
{

    private bool isTowerActive = true;

    [SerializeField]
    private GameObject top;

    protected override void Start()
    {
        top = gameObject.transform.Find("Top").gameObject;
        if (top == null)
        {
            Debug.LogError("Tower-Top not found");
        }
        //call super start
        base.Start();
    }
    protected override void TowerAction()
    {
        isTowerActive = true;
        if (enemiesInRange.Count > 0)
        {
            RotateTowardsEnemy(enemiesInRange[0].transform.position);

            //shoot enemy
            enemiesInRange[0].GetComponent<Enemy>().Damage(Damage);
        }

        timeSinceLastAction = Time.deltaTime;
        isTowerActive = false;
    }

    private void RotateTowardsEnemy(Vector3 enemyPosition)
    {
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
        this.damage = 10;
        this.Frequency = 60;
        this.Range = 10;
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
