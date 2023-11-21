using UnityEngine;

public class Waypoints : MonoBehaviour
{
   // "Transform[]": Liste von Game Objects
   public static Transform[] points;

   private void Awake()
   {
      points = new Transform[transform.childCount];
      for (int i = 0; i < points.Length; i++)
      {
         points[i] = transform.GetChild(i);
      }
   }
}
