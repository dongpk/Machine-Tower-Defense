using UnityEngine;

public class WayPontManager : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;

    public Transform[] GetWaypoints()=> waypoints;
   
}
