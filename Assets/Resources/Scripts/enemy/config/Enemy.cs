using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public event Action<GameObject> OnDeath;

    public int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke(this.gameObject); // Trigger the death event.
        Destroy(this.gameObject);
    }
}
