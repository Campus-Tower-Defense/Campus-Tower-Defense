using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TowerPlacement : MonoBehaviour
{
    public LayerMask gridLayer; // Set the layer for your grid objects
    public float towerOffset = 0.1f; // Offset to avoid z-fighting

    public InputActionReference triggerPressed = null;

    private GameObject selectedTowerPrefab; // The selected tower prefab
    private Dictionary<Vector3, GameObject> occupiedGridCells = new Dictionary<Vector3, GameObject>();

    // UI variables
    public Canvas towerSelectionCanvas;
    public Button tower1Button;
    public Button tower2Button;
    private Vector3 selectedTowerPosition;
    private GameObject selectedGridObject;

    private void Start()
    {
        // Set up UI button click listeners
        tower1Button.onClick.AddListener(() => SelectTowerPrefab("tower_without_ground Variant"));
        tower2Button.onClick.AddListener(() => SelectTowerPrefab("Tower2"));

        // Initially hide the tower selection UI
        towerSelectionCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        float value = triggerPressed.action.ReadValue<float>();
        
        if (Input.GetButtonDown("Fire1") || value == 1f)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            // Check if the ray hits an object on the grid layer
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, gridLayer))
            {
                selectedGridObject = hit.collider.gameObject; // Store the grid object
                Vector3 hitPoint = hit.point;

                Vector3 gridObjectPosition = selectedGridObject.transform.position;
                // Show the tower selection UI
                if (!occupiedGridCells.ContainsKey(gridObjectPosition))
                {
                    ShowTowerSelectionUI(hitPoint);
                }
                else
                {
                    Debug.Log("Already Placed a Tower");
                    // ShowUpgradeSelectionUI(hitPoint); // Showing a different UI to buy ugprades for the towers
                }
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
        string prefabPath = "Prefabs/Tower/" + towerPrefabName;
        // Find the tower prefab based on the name
        selectedTowerPrefab = Resources.Load<GameObject>(prefabPath);

        if (selectedTowerPrefab != null)
        {
         // Hide the tower selection UI
            towerSelectionCanvas.gameObject.SetActive(false);
        
            // Call the PlaceTower function with the selected position
            PlaceTower(selectedTowerPosition);
        }
        else
        {
            Debug.LogError("Tower prefab not found: " + towerPrefabName);
        }
    }

    void PlaceTower(Vector3 cellPosition)
    {
        if (selectedGridObject != null)
            {
            // Get the position of the selected grid object
            Vector3 gridObjectPosition = selectedGridObject.transform.position;

            // Check if the grid cell is already occupied
            if (!occupiedGridCells.ContainsKey(gridObjectPosition))
            {
                // Calculate the middle of the grid cell, // change the Vector3 Data to fit the tower prefabs
                Vector3 middleOfCell = gridObjectPosition; // + new Vector3(0f, 0.5f, 0f); Add if prefab needs to be higher/lower

                // Instantiate the selected tower at the middle of the cell
                GameObject newTower = Instantiate(selectedTowerPrefab, middleOfCell, Quaternion.identity);

                // Mark this grid cell as occupied
                occupiedGridCells.Add(gridObjectPosition, newTower);
            }
            else
            {
                // There's already a tower in this cell, handle accordingly (e.g., display a message)
                Debug.Log("Cannot place tower here - cell is already occupied.");
            }
        }
        else
        {
           Debug.LogError("No selected grid object to place the tower.");
        }
    }
}
