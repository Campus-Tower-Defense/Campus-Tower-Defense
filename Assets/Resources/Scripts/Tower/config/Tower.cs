using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Base class for all towers with support for actions, upgrades, and passive effects.
/// </summary>
public abstract class Tower : MonoBehaviour
{
    [SerializeField] protected int cost;
    [SerializeField] protected float frequency;
    [SerializeField] protected int damage;
    [SerializeField] protected int range;
    [SerializeField] protected string description;
    [SerializeField] protected string towerName;
    protected float timeSinceLastAction;
    private float actionInterval;

    [SerializeField]
    /// <summary>
    /// Nearby enemies.
    /// </summary>
    protected List<GameObject> enemiesInRange = new List<GameObject>();

    [SerializeField]
    /// <summary>
    /// Upgrades applied to the tower.
    /// </summary>
    private List<Upgrade> appliedUpgrades = new List<Upgrade>();

    [SerializeField]
    /// <summary>
    /// Buffs applied to the tower. E.g. by upgrades.
    /// </summary>
    private List<TowerBuff> appliedBuffs = new List<TowerBuff>();

    [SerializeField]

    protected Dictionary<int, Upgrade> availableUpgrades;

    public Dictionary<int, Upgrade> AvailableUpgrades => availableUpgrades;

    protected abstract void InitializeUpgrades();

    protected abstract void InitializeStats();



    public float Frequency
    {
        get => frequency;
        set
        {
            frequency = value;
            actionInterval = 1f / (frequency / 60f);
            Debug.Log($"Action interval changed to {actionInterval}.");
            Debug.Log($"Frequency changed to {frequency}.");
        }
    }
    public int Cost => cost;
    public int Damage { get => damage; set => damage = value; }
    public int Range
    {
        get => range;
        set
        {
            range = value;
            if (GetComponent<SphereCollider>() == null)
            {
                Debug.LogError("Adding sphere collider, because there was none during range change.");
                gameObject.AddComponent<SphereCollider>();
            }
            GetComponent<SphereCollider>().radius = range;
            Debug.Log($"Range changed to {range}.");
        }
    }

    public string Description => description;
    public string TowerName => towerName;


    public List<Upgrade> AppliedUpgrades => appliedUpgrades;

    public List<TowerBuff> AppliedBuffs => appliedBuffs;

    public void AddUpgrade(int upgradeId)
    {
        Upgrade upgrade = availableUpgrades[upgradeId];
        if (upgrade == null)
        {
            Debug.LogError($"Upgrade with id {upgradeId} does not exist.");
            return;
        }

        if (appliedUpgrades.Contains(upgrade))
        {
            Debug.LogError($"Upgrade with id {upgradeId} is already applied.");
            return;
        }

        appliedUpgrades.Add(upgrade);
        upgrade.Apply();
    }

    /// <summary>
    /// Add a buff to the applied buffs list.
    /// </summary>
    /// <param name="buff"></param>
    private void AddBuff(TowerBuff buff)
    {
        appliedBuffs.Add(buff);

    }

    /// <summary>
    /// Remove a buff from the applied buffs list.
    /// </summary>
    /// <param name="buff"></param>
    private void RemoveBuff(TowerBuff buff)
    {
        appliedBuffs.Remove(buff);
    }

    protected virtual void Start()
    {
        actionInterval = 1f / (frequency / 60f);
        timeSinceLastAction = 0f;
        InitializeUpgrades();
        InitializeStats();
    }

    protected virtual void Update()
    {
        timeSinceLastAction += Time.deltaTime;
        if (timeSinceLastAction >= actionInterval)
        {
            TowerAction();
            Debug.Log($"{towerName} has performed an action. Time since last action: {timeSinceLastAction}. Action interval: {actionInterval} seconds.");
            timeSinceLastAction = 0;
        }
    }



    /// <summary>
    /// Detect enemies entering range.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<EnemyParent>() != null)
        {
            enemiesInRange.Add(other.gameObject);
            other.gameObject.GetComponent<EnemyParent>().OnDeath += HandleEnemyDeath;
            Debug.Log($"Enemy entered range of {towerName}.");
        }
    }

    /// <summary>
    /// On trigger exit, remove enemy from list of enemies in range.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        EnemyParent enemy = other.gameObject.GetComponent<EnemyParent>();
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
        EnemyParent enemy = enemyGameObject.GetComponent<EnemyParent>();
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


    /// <summary>
    /// Class for tower buffs. Applied e.g. by upgrades.
    /// </summary>
    [System.Serializable]
    public abstract class TowerBuff
    {

        [SerializeField]
        private string buffName;

        [SerializeField]
        private string buffDescription;

        private readonly Action<Tower> onApply;
        private readonly Action<Tower> onRemove;

        public TowerBuff(string buffName, string buffDescription, Action<Tower> onApply, Action<Tower> onRemove)
        {
            this.buffName = buffName;
            this.buffDescription = buffDescription;
            this.onApply = onApply;
            this.onRemove = onRemove;
        }

        public string BuffName => buffName;
        public string BuffDescription => buffDescription;

        /// <summary>
        /// Apply the buff to the tower. If not already applied.
        /// </summary>
        /// <param name="tower"></param>
        /// <returns>
        /// True if the buff was applied, false if it was already applied.
        /// </returns>
        public bool Apply(Tower tower)
        {
            //check if already applied
            if (tower.AppliedBuffs.Contains(this))
            {
                return false;
            }
            tower.AddBuff(this);
            onApply(tower);
            return true;
        }


        /// <summary>
        /// Remove the buff from the tower if it is applied.
        /// </summary>
        /// <param name="tower"></param>
        /// <returns>
        /// True if the buff was removed, false if it was not applied in the first place.
        /// </returns>
        public bool Remove(Tower tower)
        {
            //check if already removed
            if (!tower.AppliedBuffs.Contains(this))
            {
                return false;
            }
            tower.RemoveBuff(this);
            onRemove(tower);
            return true;
        }

    }



}
