using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Base class for all towers with support for actions, upgrades, and passive effects.
/// </summary>
public abstract class Tower : MonoBehaviour
{
    [SerializeField] private int cost;
    [SerializeField] private int frequency;
    [SerializeField] private int damage;
    [SerializeField] private int range;
    [SerializeField] private string description;
    [SerializeField] private string towerName;

    private float timeSinceLastAction;
    private float actionInterval;

    /// <summary>
    /// Nearby enemies.
    /// </summary>
    protected List<GameObject> enemiesInRange = new List<GameObject>();

    /// <summary>
    /// Upgrades applied to the tower.
    /// </summary>
    private List<Upgrade> upgradesApplied = new List<Upgrade>();


    public int Frequency
    {
        get => frequency;
        set
        {
            frequency = value;
            actionInterval = 1f / (frequency / 60f);
        }
    }
    public int Cost => cost;
    public int Damage { get => damage; set => damage = value; }
    public int Range { get => range; set => range = value; }
    public string Description => description;
    public string TowerName => towerName;


    protected virtual void Start()
    {
        actionInterval = 1f / (frequency / 60f);
        timeSinceLastAction = 0f;
    }

    protected virtual void Update()
    {
        timeSinceLastAction += Time.deltaTime;

        if (timeSinceLastAction >= actionInterval)
        {
            TowerAction();
            timeSinceLastAction -= actionInterval;
            Debug.Log($"{towerName} has performed an action.");
        }
    }

    /// <summary>
    /// Add upgrades to the tower.
    /// </summary>
    /// <param name="upgrade"></param>
    public void ApplyUpgrade(Upgrade upgrade)
    {
        upgradesApplied.Add(upgrade);
        upgrade.Apply(this);
        Debug.Log($"Upgrade {upgrade.upgradeName} applied to {towerName}.");
    }


    /// <summary>
    /// Detect enemies entering range.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Enemy>() != null)
        {
            enemiesInRange.Add(other.gameObject);
            other.gameObject.GetComponent<Enemy>().OnDeath += HandleEnemyDeath;
            Debug.Log($"Enemy entered range of {towerName}.");
        }
    }

    /// <summary>
    /// On trigger exit, remove enemy from list of enemies in range.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            HandleEnemyDeath(enemy.gameObject);
        }
    }

    /// <summary>
    /// Remove enemy from list of enemies in range.
    /// </summary>
    /// <param name="enemy"></param>
    private void HandleEnemyDeath(GameObject enemyGameObject)
    {
        Enemy enemy = enemyGameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemiesInRange.Remove(enemyGameObject);
            enemy.OnDeath -= HandleEnemyDeath;
            Debug.Log($"Enemy left range of {towerName}.");

        }
        else
        {
            Debug.LogError($"Enemy {enemyGameObject.name} does not have an Enemy component.");
        }
    }



    /// <summary>
    /// Tower-specific actions.
    /// </summary>
    protected abstract void TowerAction();

    /// <summary>
    /// Tower-specific passive effects.
    /// </summary>
    protected abstract void PassiveAbility();


}
