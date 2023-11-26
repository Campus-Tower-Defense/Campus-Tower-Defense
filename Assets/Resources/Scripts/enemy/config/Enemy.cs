using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int health = 100;
    [SerializeField] protected int currencyDrop = 10;
    [SerializeField] protected float attackRadius = 1f;
    [SerializeField] protected float attackFrequency = 1f;

    [SerializeField] protected int damage = 20;

    protected float timeSinceLastAttack = 0f;

    private GameObject goal;

    private bool inAttackMode = false;

    private GameObject currencyPrefab;

    public event Action<GameObject> OnDeath;

    private List<EnemyBuff> appliedBuffs = new List<EnemyBuff>();

    private NavMeshAgent navMeshAgent;

    public int Health => health;
    public int CurrencyDrop => currencyDrop;
    public List<EnemyBuff> AppliedBuffs => appliedBuffs;

    protected abstract void InitializeStats();
    protected abstract void PassiveAbility();
    protected abstract void AttackAction();

    public void SetGoal(GameObject newGoal)
    {
        goal = newGoal;
    }

    private void Start()
    {
        InitializeStats();
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent not found");
        }
        currencyPrefab = Resources.Load<GameObject>("Prefabs/General/Currency");
        if (currencyPrefab == null)
        {
            Debug.LogError("Currency prefab not found");
        }
    }

    private void Update()
    {
        PassiveAbility();

        if (goal != null)
        {
            float distanceToGoal = Vector3.Distance(transform.position, goal.transform.position);

            if (distanceToGoal <= attackRadius)
            {
                // Der Enemy ist im Angriffsmodus
                inAttackMode = true;

                // Deaktiviere den NavMeshAgent, wenn in den Angriffsmodus gewechselt wird
                if (navMeshAgent != null)
                {
                    navMeshAgent.enabled = false;
                }
            }

            if (inAttackMode)
            {
                timeSinceLastAttack += Time.deltaTime;
                if (timeSinceLastAttack >= 1f / attackFrequency)
                {
                    AttackGoal();
                    timeSinceLastAttack = 0f;
                }
            }
        }
    }

    private void AttackGoal()
    {
        // Führe die Basic-Attacke auf das Ziel aus
        // goal.GetComponent<Goal>().Damage(damage);
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke(this.gameObject);

        for (int i = 0; i < currencyDrop; i++)
        {
            Vector3 spawnPosition = transform.position + UnityEngine.Random.insideUnitSphere * 2f;
            spawnPosition.y = 0f;

            Instantiate(currencyPrefab, spawnPosition, Quaternion.identity);
        }

        Destroy(this.gameObject);
    }

    public void AddBuff(EnemyBuff buff)
    {
        appliedBuffs.Add(buff);
        buff.Apply(this);
    }

    public void RemoveBuff(EnemyBuff buff)
    {
        appliedBuffs.Remove(buff);
        buff.Remove(this);
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

        public void Apply(Enemy enemy)
        {
            if (!enemy.AppliedBuffs.Contains(this))
            {
                enemy.AddBuff(this);
                onApply?.Invoke(enemy);
            }
        }

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
