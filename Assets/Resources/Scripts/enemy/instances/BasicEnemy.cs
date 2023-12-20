using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : EnemyParent
{

    protected override void InitializeStats()
    {
        this.health = 2;
        this.currencyDrop = 1;
        this.attackFrequency = 0;
        this.damage = 1;
    }

    protected override void PassiveAbility()
    {
        //no passive ability
    }
}
