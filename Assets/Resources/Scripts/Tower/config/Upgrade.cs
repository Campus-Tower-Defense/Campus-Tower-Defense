using System;
using UnityEngine;

[Serializable]
public class Upgrade
{
    [SerializeField]
    private readonly string upgradeName;
    [SerializeField]
    private readonly string upgradeDescription;
    [SerializeField]
    private int upgradeCost;
    private Tower tower;

    // Delegates for Apply and Remove actions.
    public Action<Tower> ApplyAction { get; set; }
    public Action<Tower> RemoveAction { get; set; }

    public string UpgradeName => upgradeName;
    public string UpgradeDescription => upgradeDescription;
    public int UpgradeCost { get => upgradeCost; set => upgradeCost = value; }

    public Upgrade(string upgradeName, string upgradeDescription, int upgradeCost, Action<Tower> applyAction, Action<Tower> removeAction, Tower tower)
    {
        this.upgradeName = upgradeName;
        this.upgradeDescription = upgradeDescription;
        this.upgradeCost = upgradeCost;
        this.tower = tower;
        ApplyAction = applyAction ?? throw new ArgumentNullException(nameof(applyAction));
        RemoveAction = removeAction ?? throw new ArgumentNullException(nameof(removeAction));
    }

    // Apply the upgrade to the tower, using the provided delegate.
    public void Apply()
    {
        ApplyAction(tower);
    }

    // Remove the upgrade from the tower, using the provided delegate.
    public void Remove()
    {
        RemoveAction(tower);
    }
}