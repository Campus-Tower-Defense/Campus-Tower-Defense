using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{

    public float pickupRadius = 1f;

    void Update()
    {
        //check if player is in radius
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);

            if (distance <= pickupRadius)
            {
                AddMoneyToPlayer();
                Destroy(gameObject);
            }
        }
    }

    void AddMoneyToPlayer()
    {
        //TODO: add money to player
    }
}
