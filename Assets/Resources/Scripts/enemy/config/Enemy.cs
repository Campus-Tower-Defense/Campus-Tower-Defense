using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private int currencyDrop = 10;
    [SerializeField] private float towerAttackRadius = 10f;
    [SerializeField] private float towerAttackFrequency = 1f;

    #pragma warning disable 0414
    [SerializeField] private int damage = 20;
    private float timeSinceLastAttack = 0f;

    private GameObject currencyPrefab;
    public event Action<GameObject> OnDeath;

    private List<EnemyBuff> appliedBuffs = new List<EnemyBuff>();
    private List<GameObject> towersInRange = new List<GameObject>();

    public int Health { get { return health; } }
    public int CurrencyDrop { get { return currencyDrop; } }
    public List<EnemyBuff> AppliedBuffs => appliedBuffs;

    /// <summary>
    /// Initializes enemy stats. Must be implemented in derived classes.
    /// </summary>
    protected abstract void InitializeStats();

    /// <summary>
    /// Defines the passive ability of the enemy. Must be implemented in derived classes.
    /// </summary>
    protected abstract void PassiveAbility();

    /// <summary>
    /// Defines the attack action of the enemy. Must be implemented in derived classes.
    /// </summary>
    protected abstract void AttackAction();

    private void Start()
    {
        currencyPrefab = Resources.Load<GameObject>("Prefabs/General/Currency");
        InitializeStats();
        SetupAttackRadius();
    }

    private void Update()
    {
        PassiveAbility();
        timeSinceLastAttack += Time.deltaTime;
        if (timeSinceLastAttack >= 1f / towerAttackFrequency)
        {
            AttackTowers();
            timeSinceLastAttack = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tower"))
        {
            towersInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Tower"))
        {
            towersInRange.Remove(other.gameObject);
        }
    }

    /// <summary>
    /// Attacks towers within range.
    /// </summary>
    private void AttackTowers()
    {
        foreach (var tower in towersInRange)
        {
            // Implement the logic to attack the tower.
            AttackAction();
        }
    }

    /// <summary>
    /// Applies damage to the enemy and checks for death.
    /// </summary>
    /// <param name="damageAmount">The amount of damage to apply.</param>
    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Handles the death of the enemy, including currency spawning.
    /// </summary>
    private void Die()
    {
        OnDeath?.Invoke(this.gameObject);
        if (currencyPrefab != null)
        {
            Instantiate(currencyPrefab, transform.position, Quaternion.identity);
        }
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Adds a buff to the enemy.
    /// </summary>
    /// <param name="buff">The buff to add.</param>
    protected void AddBuff(EnemyBuff buff)
    {
        appliedBuffs.Add(buff);
    }

    /// <summary>
    /// Removes a buff from the enemy.
    /// </summary>
    /// <param name="buff">The buff to remove.</param>
    protected void RemoveBuff(EnemyBuff buff)
    {
        appliedBuffs.Remove(buff);
    }

    /// <summary>
    /// Sets up the attack radius of the enemy.
    /// </summary>
    private void SetupAttackRadius()
    {
        SphereCollider collider = gameObject.GetComponent<SphereCollider>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<SphereCollider>();
        }
        collider.isTrigger = true;
        collider.radius = towerAttackRadius;
    }

    [System.Serializable]
    public abstract class EnemyBuff
    {
        [SerializeField] private string buffName;
        [SerializeField] private string buffDescription;

        private readonly Action<Enemy> onApply;
        private readonly Action<Enemy> onRemove;

        public EnemyBuff(string name, string description, Action<Enemy> apply, Action<Enemy> remove)
        {
            buffName = name;
            buffDescription = description;
            onApply = apply;
            onRemove = remove;
        }

        public string BuffName => buffName;
        public string BuffDescription => buffDescription;

        /// <summary>
        /// Applies the buff to the enemy.
        /// </summary>
        /// <param name="enemy">The enemy to which the buff will be applied.</param>
        public void Apply(Enemy enemy)
        {
            if (!enemy.AppliedBuffs.Contains(this))
            {
                enemy.AddBuff(this);
                onApply?.Invoke(enemy);
            }
        }

        /// <summary>
        /// Removes the buff from the enemy.
        /// </summary>
        /// <param name="enemy">The enemy from which the buff will be removed.</param>
        public void Remove(Enemy enemy)
        {
            if (enemy.AppliedBuffs.Contains(this))
            {
                enemy.RemoveBuff(this);
                onRemove?.Invoke(enemy);
            }
        }
    }
}
