using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : EnemyParent
{

    protected override void InitializeStats()
    {
        this.health = 100;
        this.currencyDrop = 10;
        this.attackRadius = 1f;
        this.attackFrequency = 1f;
        this.damage = 20;
    }

    protected override void PassiveAbility()
    {
        //no passive ability
    }
}
