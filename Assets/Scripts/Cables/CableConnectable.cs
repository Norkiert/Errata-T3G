using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CableConnectable : MonoBehaviour
{
    [SerializeField] [Min(1)] private int numberOfPoints = 3;
    [SerializeField] private float space = 0.3f;
    [SerializeField] private float size = 0.3f;

    [SerializeField] private GameObject start;
    [SerializeField] private GameObject end;
    [SerializeField] private GameObject connector0;
    [SerializeField] private GameObject point0;

    private List<Transform> points;
    private List<Transform> connectors;

    const string cloneText = "Part";

    [Button]
    private void UpdatePoints()
    {
        if (!start || !end || !point0 || !connector0)
        {
            Debug.LogWarning("Can't update because one of objects to set is null!");
            return;
        }

        // delete old
        int length = transform.childCount;
        for (int i = 0; i < length; i++)
            if (transform.GetChild(i).name.StartsWith(cloneText))
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
                length--;
                i--;
            }
                
        // set new
        Vector3 lastPos = start.transform.position;
        Rigidbody lasBody = start.GetComponent<Rigidbody>();
        for (int i = 0; i < numberOfPoints; i++)
        {
            GameObject cConnector = i == 0 ? connector0 : CreateNewCon(i);
            GameObject cPoint = i == 0 ? point0 : CreateNewPoint(i);
            
            Vector3 newPos = CountNewPointPos(lastPos);
            cPoint.transform.position = newPos;
            cPoint.transform.localScale = Vector3.one * size;

            cPoint.GetComponent<SpringJoint>().connectedBody = lasBody;
            lasBody = cPoint.GetComponent<Rigidbody>();

            cConnector.transform.position = CountConPos(lastPos, newPos);
            cConnector.transform.localScale = CountSizeOfCon(lastPos, newPos);
            lastPos = newPos;
        }

        Vector3 endPos = CountNewPointPos(lastPos);
        end.transform.position = endPos;
        end.GetComponent<SpringJoint>().connectedBody = lasBody;

        GameObject endConnector = CreateNewCon(numberOfPoints);
        endConnector.transform.position = CountConPos(lastPos, endPos);

        Vector3 CountNewPointPos(Vector3 lastPos) => lastPos + Vector3.forward * space;

        GameObject CreateNewPoint(int index)
        {
            GameObject temp = Instantiate(point0);
            temp.name = PointName(index);
            temp.transform.parent = transform;
            return temp;
        }
        GameObject CreateNewCon(int index)
        {
            GameObject temp = Instantiate(connector0);
            temp.name = ConnectorName(index);
            temp.transform.parent = transform;
            return temp;
        }
    }

    private void Start()
    {
        points = new List<Transform>();
        connectors = new List<Transform>();

        points.Add(start.transform);
        points.Add(point0.transform);

        connectors.Add(connector0.transform);

        for (int i = 1; i < numberOfPoints; i++)
        {
            Transform conn = transform.Find(ConnectorName(i));
            if (conn == null)
                Debug.LogWarning("Dont found connector number " + i);
            else
                connectors.Add(conn);

            Transform point = transform.Find(PointName(i));
            if (conn == null)
                Debug.LogWarning("Dont found point number " + i);
            else
                points.Add(point);
        }

        Transform endConn = transform.Find(ConnectorName(numberOfPoints));
        if (endConn == null)
            Debug.LogWarning("Dont found connector number " + numberOfPoints);
        else
            connectors.Add(endConn);

        points.Add(end.transform);
    }

    private void Update()
    {
        int length = connectors.Count;
        Transform lastPoint = points[0];
        for (int i = 0; i < length; i++)
        {
            Transform nextPoint = points[i + 1];
            Transform connector = connectors[i].transform;
            connector.position = CountConPos(lastPoint.position, nextPoint.position);
            connector.rotation = Quaternion.LookRotation(nextPoint.position - connector.position);
            connector.localScale = CountSizeOfCon(lastPoint.position, nextPoint.position);
            lastPoint = nextPoint;
        }
    }

    private Vector3 CountConPos(Vector3 start, Vector3 end) => (start + end) / 2f;
    private Vector3 CountSizeOfCon(Vector3 start, Vector3 end) => new Vector3(size, size, (start - end).magnitude / 2f);
    private string ConnectorName(int index) => $"{cloneText}_{index}_Conn";
    private string PointName(int index) => $"{cloneText}_{index}_Point";
}
