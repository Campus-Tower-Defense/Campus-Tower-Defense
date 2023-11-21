using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : Tower
{



    protected override void TowerAction()
    {
        if (enemiesInRange.Count > 0)
        {
            enemiesInRange[0].GetComponent<Enemy>().Damage(Damage);
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
