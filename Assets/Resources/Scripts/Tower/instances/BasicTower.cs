using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : Tower
{

    private void Start()
    {
        //init damage, frequency, range, cost, description, towerName
        Damage = 10;
        Frequency = 60;
        Range = 10;
    }

    protected override void TowerAction()
    {
        if (enemiesInRange.Count > 0)
        {
            enemiesInRange[0].GetComponent<Enemy>().TakeDamage(Damage);
        }
    }

    protected override void PassiveAbility()
    {
        //no passive ability
    }
}
