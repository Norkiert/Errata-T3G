using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneZone : MonoBehaviour
{
    [SerializeField, Required] private Transform objectsParent;
    [SerializeField, Required] private Transform pointsParent;
    [SerializeField] private List<GameObject> models = new List<GameObject>();

    [Header("Shape")]
    [SerializeField, Min(0.2f)] private float range = 0.5f;
    [SerializeField, Min(0.05f)] private float moveStep = 0.2f;
    [SerializeField, Min(0.05f)] private float gap = 0.2f;
    [SerializeField, Min(0)] private float zDefoformation = 0.1f;
    [SerializeField, Min(1)] private int triesPerStep = 3;

    [Header("Movement")]
    [SerializeField, Min(0.1f)] private float moveSpeed = 1f;
    [SerializeField, Min(0.1f)] private float disapireDistance = 1f;
    [SerializeField, Range(0f, 1f)] private float disapireDistanceColliderMultiplier = 0.5f;


    private readonly List<Transform> points = new List<Transform>();
    private readonly Dictionary<int, List<Part>> movingParts = new Dictionary<int, List<Part>>();


    [Button("Genrate")]
    private void GenerateButton()
    {
        if (IsValid())
            Generate();
    }


    private void Start()
    {
        if (!IsValid())
        {
            enabled = false;
            return;
        }

        Generate();
    }
    private void Update()
    {
        MoveObjects();
    }

    private bool IsValid()
    {
        GetPoints();

        if (points.Count < 2)
        {
            Debug.LogWarning($"{name}: Number of points is less then 2!");
            return false;
        }

        if (models.Count < 1)
        {
            Debug.LogWarning($"{name}: Number of models is less then 1!");
            return false;
        }

        return true;
    }


    private void MoveObjects()
    {
        for (int pi = 1; pi < points.Count; pi++)
        {
            bool isOnEnd = pi + 1 == points.Count;
            bool isOnStart = pi == 1;

            float distanceInFrame = moveSpeed * Time.deltaTime;
            Vector3 lineStart = points[pi - 1].position;
            Vector3 lineEnd = points[pi].position;
            Vector3 moveVector = (lineEnd - lineStart).normalized * distanceInFrame;

            Quaternion targetRotation = !isOnEnd ? RotationTo(lineEnd, points[pi + 1].position) : Quaternion.identity;

            List<Part> parts = movingParts[pi - 1];
            for (int i = 0; i < parts.Count; i++)
            {
                Part part = parts[i];

                float distanceToPoint = Vector3.Distance(lineEnd, part.transform.position);

                if (distanceToPoint > distanceInFrame)
                {
                    // move
                    part.transform.position += moveVector;

                    // show
                    if (isOnStart)
                    {
                        float d = Vector3.Distance(lineStart, part.transform.position);
                        if (d < disapireDistance)
                            part.transform.localScale = Vector3.one * d / disapireDistance;
                    }

                    // hide
                    if (isOnEnd && distanceToPoint < disapireDistance)
                    {
                        part.transform.localScale = Vector3.one * distanceToPoint / disapireDistance;
                    }

                    // ratate to target postion
                    if (!isOnEnd && distanceToPoint < 1)
                    {
                        if (part.rotationPercent == 0)
                            part.savedRotation = part.transform.rotation;
                        part.rotationPercent += Time.deltaTime * moveSpeed;
                        if (part.rotationPercent < 1f)
                            part.transform.rotation = Quaternion.Slerp(part.savedRotation, targetRotation, part.rotationPercent);

                    }
                }
                else
                {
                    // change line
                    parts.Remove(part);
                    i--;

                    part.rotationPercent = 0;

                    if (isOnEnd)
                    {
                        part.transform.position = points[0].position;
                        part.transform.rotation = RotationTo(points[0].position, points[1].position);

                        movingParts[0].Add(part);
                    }
                    else
                    {
                        part.transform.position = lineEnd;
                        part.transform.rotation = targetRotation;

                        movingParts[pi].Add(part);
                    }
                }
            }
        }

        Quaternion RotationTo(Vector3 start, Vector3 end) => Quaternion.LookRotation(end - start, Vector3.right);
    }

    private void GetPoints()
    {
        points.Clear();
        for (int i = 0; i < pointsParent.childCount; i++)
            points.Add(pointsParent.GetChild(i));
    }
    private void Generate()
    {
        int l = objectsParent.childCount;
        for (int i = 0; i < l; i++)
            DestroyImmediate(objectsParent.GetChild(0).gameObject);

        GetPoints();
        UpdateColliders();

        movingParts.Clear();
        List<Transform> lastObjects = new List<Transform>();

        for (int pi = 1; pi < points.Count; pi++)
        {
            Transform start = points[pi - 1];
            Transform end = points[pi];
            Vector3 pos = start.position;

            float dist = Vector3.Distance(start.position, end.position);
            int steps = Mathf.FloorToInt(dist / moveStep);
            Vector3 moveVector = (end.position - start.position) / steps;

            List<Part> parts = new List<Part>();
            movingParts.Add(pi - 1, parts);


            // create new parts
            for (int si = 0; si < steps; si++)
            {
                Transform partT = new GameObject($"Part{movingParts.Count}:{parts.Count}").transform;
                partT.position = pos;
                partT.parent = objectsParent;
                Part part = new Part(partT);
                parts.Add(part);

                int maxLastObject = triesPerStep * 3;
                Quaternion moveRotation = Quaternion.LookRotation(end.position - pos, Vector3.right);


                // create objects
                for (int i = 0; i < triesPerStep; i++)
                {
                    float min = gap / 2;
                    Vector3 offset = new Vector3(Random.Range(min, range) * RandomNegative(), Random.Range(min, range) * RandomNegative(), Random.Range(-zDefoformation, zDefoformation));
                    Vector3 spawnPos = pos + moveRotation * offset;

                    if (IsDistanceKeept(spawnPos) == false)
                        continue;

                    GameObject obj = Instantiate(models[Random.Range(0, models.Count)], spawnPos, Random.rotation, partT);

                    if (lastObjects != null)
                    {
                        if (lastObjects.Count > maxLastObject)
                            lastObjects.RemoveAt(0);
                        lastObjects.Add(obj.transform);
                    }
                }

                pos += moveVector;
            }
        }

        bool IsDistanceKeept(Vector3 p)
        {
            if (lastObjects == null)
                return true;

            foreach (var item in lastObjects)
                if (Vector3.Distance(p, item.position) < gap)
                    return false;
            return true;
        }
        int RandomNegative() => Random.Range(0, 2) == 0 ? 1 : -1;
    }
    private void UpdateColliders()
    {
        for (int i = 0; i < points.Count - 1; i++)
        {
            Transform startPoint = points[i];
            Transform endPoint = points[i + 1];

            if (!startPoint.TryGetComponent(out BoxCollider coll))
                coll = startPoint.gameObject.AddComponent<BoxCollider>();

            startPoint.rotation = Quaternion.LookRotation(endPoint.position - startPoint.position, Vector3.right);

            float dist = Vector3.Distance(endPoint.position, startPoint.position);

            float centerOffset = dist / 2f;
            if (i == 0)
                centerOffset += disapireDistance * disapireDistanceColliderMultiplier * 0.5f;
            else if (i == points.Count - 2)
                centerOffset -= disapireDistance * disapireDistanceColliderMultiplier * 0.5f;
            coll.center = centerOffset * Vector3.forward;

            float size = dist;
            if (i == 0 || i == points.Count - 2)
                size -= disapireDistance * disapireDistanceColliderMultiplier;

            coll.size = new Vector3(range * 2, range * 2, size);
        }

        if (points[points.Count - 1].TryGetComponent(out BoxCollider endColl))
            DestroyImmediate(endColl);
    }

    private class Part
    {
        public Transform transform;
        public float rotationPercent;
        public Quaternion savedRotation;

        public Part(Transform transform)
        {
            this.transform = transform;
            rotationPercent = 0;
        }
    }


    private void OnDrawGizmos()
    {
        if (points.Count < 2)
            return;

        Gizmos.color = Color.black;
        Vector3 previousPos = points[0].position;
        for (int i = 1; i < points.Count; i++)
        {
            Gizmos.DrawLine(previousPos, points[i].position);
            previousPos = points[i].position;
        }
    }
}
