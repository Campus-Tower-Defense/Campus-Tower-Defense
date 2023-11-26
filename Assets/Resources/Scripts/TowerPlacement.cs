using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    public GameObject towerPrefab; // Assign your tower prefab in the Inspector
    public LayerMask gridLayer; // Set the layer for your grid objects
    public float towerOffset = 0.1f; // Offset to avoid z-fighting

    private Dictionary<Vector3, GameObject> occupiedGridCells = new Dictionary<Vector3, GameObject>();

    private void Update()
    {
        if (Input.GetButtonDown("Fire1")) 
        {
            PlaceTower();
        }
    }

    void PlaceTower()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, gridLayer))
        {
            Vector3 towerPosition = hit.point;
            towerPosition.y = towerPrefab.transform.position.y; // Align with the ground

            // Round the position to prevent inaccuracies
            towerPosition = RoundPosition(towerPosition);

            // Check if the grid cell is already occupied
            if (!occupiedGridCells.ContainsKey(towerPosition))
            {
                // No tower in this cell, instantiate the new tower
                GameObject newTower = Instantiate(towerPrefab, towerPosition, Quaternion.identity);

                // Mark this grid cell as occupied
                occupiedGridCells.Add(towerPosition, newTower);
            }
            else
            {
                Debug.Log("Cannot place tower here - cell is already occupied.");
            }
        }
    }

    Vector3 RoundPosition(Vector3 position)
    {
        return new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), Mathf.Round(position.z));
    }
}
