using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    [Header("Fields necessery in pathfinding")]
    public float fCost; // summary cost
    public float gCost; // distance from start
    public float hCost; // distance from end


    public Point lastPoint;
    public List<Point> neighbours = new List<Point>();

    private Pathfinding pathfinding;


    void Start()
    {
        pathfinding = FindObjectOfType<Pathfinding>();
        FindNeighbours();
    }

    private void FindNeighbours()
    {
        Point[] allPoints;
        allPoints = FindObjectsOfType<Point>();
        foreach (Point point in allPoints)
        {
            if(point.transform.position == transform.position)
            {
                continue;
            }
            if(Vector3.Distance(transform.position, point.transform.position)<=pathfinding.neighboursDistance)
            {
                RaycastHit hit;
                Ray ray = new Ray(transform.position, (point.transform.position - transform.position));
                Physics.Raycast(ray, out hit, pathfinding.neighboursDistance);
                if (hit.collider == null || Vector2.Distance(new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.z),new Vector2(transform.position.x,transform.position.z))> Vector3.Distance(transform.position, point.transform.position))
                {
                    neighbours.Add(point);
                    Debug.DrawLine(transform.position, point.transform.position, Color.red, 300f);
                }
            }
        }
    }
}
