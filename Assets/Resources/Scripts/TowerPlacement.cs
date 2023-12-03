using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerPlacement : MonoBehaviour
{
    public LayerMask gridLayer; // Set the layer for your grid objects
    public float towerOffset = 0.1f; // Offset to avoid z-fighting

    private GameObject selectedTowerPrefab; // The selected tower prefab
    private Dictionary<Vector3, GameObject> occupiedGridCells = new Dictionary<Vector3, GameObject>();

    // UI variables
    public Canvas towerSelectionCanvas;
    public Button tower1Button;
    public Button tower2Button;

    private void Start()
    {
        // Set up UI button click listeners
        tower1Button.onClick.AddListener(() => SelectTowerPrefab("Tower1"));
        tower2Button.onClick.AddListener(() => SelectTowerPrefab("Tower2"));

        // Initially hide the tower selection UI
        towerSelectionCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            // Check if the ray hits an object on the grid layer
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, gridLayer))
            {
                Vector3 hitPoint = hit.point;

                // Show the tower selection UI
                ShowTowerSelectionUI(hitPoint);
            }
        }
    }

    void ShowTowerSelectionUI(Vector3 position)
    {
        // Set the tower selection UI position to be above the grid position
        towerSelectionCanvas.transform.position = position + new Vector3(0, 1.5f, 0);

        // Activate the tower selection UI
        towerSelectionCanvas.gameObject.SetActive(true);
    }

    void SelectTowerPrefab(string towerPrefabName)
    {
        // Find the tower prefab based on the name
        selectedTowerPrefab = Resources.Load<GameObject>(towerPrefabName);

        if (selectedTowerPrefab != null)
        {
            // Hide the tower selection UI
            Debug.Log("Tower prefab selected: " + towerPrefabName);
            towerSelectionCanvas.gameObject.SetActive(false);
            
        }
        else
        {
            Debug.LogError("Tower prefab not found: " + towerPrefabName);
        }
    }

    void PlaceTower(Vector3 towerPosition)
    {
        // Check if the grid cell is already occupied
        if (!occupiedGridCells.ContainsKey(towerPosition))
        {
            // Calculate the middle of the grid cell
            Vector3 middleOfCell = towerPosition + new Vector3(0.5f, 0f, 0.5f);

            // Instantiate the selected tower at the middle of the cell
            GameObject newTower = Instantiate(selectedTowerPrefab, middleOfCell, Quaternion.identity);

            // Mark this grid cell as occupied
            occupiedGridCells.Add(towerPosition, newTower);
        }
        else
        {
            // There's already a tower in this cell, handle accordingly (e.g., display a message)
            Debug.Log("Cannot place tower here - cell is already occupied.");
        }
    }
}
