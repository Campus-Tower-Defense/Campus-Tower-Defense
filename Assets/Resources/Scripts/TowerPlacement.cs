using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TowerPlacement : MonoBehaviour
{
    public LayerMask gridLayer;
    public float towerOffset = 0.1f;
    public InputActionReference triggerPressed = null;
    public Canvas towerSelectionCanvas;
    public Button tower1Button;
    public Button tower2Button;
    private Vector3 selectedTowerPosition;
    private GameObject selectedGridObject;
    private GameObject selectedTowerPrefab;
    private Dictionary<Vector3, GameObject> occupiedGridCells = new Dictionary<Vector3, GameObject>();

    private void Start()
    {
        tower1Button.onClick.AddListener(() => SelectTowerPrefab("tower_without_ground Variant"));
        tower2Button.onClick.AddListener(() => SelectTowerPrefab("Tower2"));
        towerSelectionCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        float value = triggerPressed.action.ReadValue<float>();

        if (Input.GetButtonDown("Fire1") || value == 1f)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, gridLayer))
            {
                selectedGridObject = hit.collider.gameObject;
                Vector3 hitPoint = hit.point;
                Vector3 gridObjectPosition = selectedGridObject.transform.position;

                if (!occupiedGridCells.ContainsKey(gridObjectPosition))
                {
                    ShowTowerSelectionUI(hitPoint);
                }
                else
                {
                    Debug.Log("Already Placed a Tower");
                }
            }
        }
    }

    void ShowTowerSelectionUI(Vector3 position)
    {
        towerSelectionCanvas.transform.position = GetMiddleOfGrid(selectedGridObject) + new Vector3(0, 1.5f, 0);
        towerSelectionCanvas.gameObject.SetActive(true);
    }
    Vector3 GetMiddleOfGrid(GameObject gridObject)
    {
        if (gridObject != null)
        {
            // Calculate the middle of the grid cell
            return gridObject.transform.position;
        }

        // Default to the origin if no grid object is selected
        return Vector3.zero;
    }

    void SelectTowerPrefab(string towerPrefabName)
    {
        string prefabPath = "Prefabs/Tower/" + towerPrefabName;
        selectedTowerPrefab = Resources.Load<GameObject>(prefabPath);

        if (selectedTowerPrefab != null)
        {
            // Check if the player has enough currency to place the tower
            int playerCurrency = GetPlayerCurrency(); 
            Debug.Log(playerCurrency);
            int towerCost = selectedTowerPrefab.GetComponent<BasicTower>().GetTowerCost();

            if (playerCurrency >= towerCost)
            {
                towerSelectionCanvas.gameObject.SetActive(false);
                PlaceTower(selectedTowerPosition);
                // Deduct currency after placing the tower
                DeductCurrency(towerCost);
            }
            else
            {
                Debug.Log("Insufficient funds to place this tower.");
            }
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
            Vector3 gridObjectPosition = selectedGridObject.transform.position;

            if (!occupiedGridCells.ContainsKey(gridObjectPosition))
            {
                Vector3 middleOfCell = gridObjectPosition;
                GameObject newTower = Instantiate(selectedTowerPrefab, middleOfCell, Quaternion.identity);
                occupiedGridCells.Add(gridObjectPosition, newTower);
            }
            else
            {
                Debug.Log("Cannot place tower here - cell is already occupied.");
            }
        }
        else
        {
            Debug.LogError("No selected grid object to place the tower.");
        }
    }

    int GetPlayerCurrency()
    {
        return CurrencyManager.GetCurrentCurrency();
    }

    void DeductCurrency(int amount)
    {
        CurrencyManager.DeductCurrency(amount);
    }
}
