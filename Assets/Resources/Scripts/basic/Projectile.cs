using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float destructTimeIfNoCollision = 5f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destructTimeIfNoCollision);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<EnemyParent>() != null)
        {
            Destroy(gameObject);
        }
    }
}
