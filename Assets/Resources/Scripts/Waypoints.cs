using UnityEngine;

public class Waypoints : MonoBehaviour
{
    // "Transform[]": Liste von Game Objects
    public Transform[] points;

    private void Awake()
    {
        // Anzahl der Kinderobjekte des aktuellen Parent-Objekts
        int childCount = transform.childCount;
        points = new Transform[childCount];

        for (int i = 0; i < childCount; i++)
        {
            points[i] = transform.GetChild(i);
        }
    }
}