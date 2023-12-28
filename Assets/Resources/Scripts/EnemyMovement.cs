using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
   public float speed = 10f;
   private Transform target;
   private int wavepointIndex = 0;
   private Waypoints waypoints;

   void Awake()
   {
      waypoints = GetComponentInParent<Waypoints>();
      if (waypoints != null)
      {
         target = waypoints.points[0];
      }
      else
      {
         Debug.LogError("Waypoints script not found in parent objects.");
      }
   }

   void Update()
   {
      
      // Zum Wegpunkt bewegen
      Vector3 dir = target.position - transform.position;
      transform.Translate(dir.normalized * (speed * Time.deltaTime), Space.World);

      // Prüfen, ob Enemy am Wegpunkt angekommen ist
      if (Vector3.Distance(transform.position, target.position) <= 0.2f)
      {
         GetNextWayPoint();
      }
   }

   // Nächsten Wegpunkt als Ziel nehmen
   void GetNextWayPoint()
   {
      if (wavepointIndex >= waypoints.points.Length - 1)
      {
         Destroy(gameObject);
         // Man muss hier ein "return" einfügen, weil die "Destroy()" etwas Zeit braucht, aber das Programm..
         // ... schon die nächsten Programmzeile bearbeitet/liest und deshalb der Fehler auftaucht "Array out of Bounce/Index"
         return;
      }
      wavepointIndex++;
      target = waypoints.points[wavepointIndex];
   }
}
