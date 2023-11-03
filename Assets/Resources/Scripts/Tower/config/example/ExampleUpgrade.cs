using UnityEngine;
public class ExampleUpgrade : Upgrade
{
    public int additionalDamage;

    public override void Apply(Tower tower)
    {
        tower.Damage += additionalDamage;
        Debug.Log($"Applying {upgradeName} to {tower.name}.");
    }
}