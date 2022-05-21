using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using GameManagment;
using DG.Tweening;

public class HubFloorCylinderBahaviour : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Transform cylindersParent;
    [SerializeField] private GameObject basicCylinder;
    [SerializeField] private GameObject connectorCylinder;


    [Header("Size")]
    [SerializeField, Min(0)] private float side = 0.5f;
    [SerializeField, Min(1)] private int range = 5;
    [SerializeField, Min(0)] private int centerMargin = 2;

    [Header("Movement")]
    [SerializeField] private float minCubeYPos = -0.3f;
    [SerializeField] private float maxCubeYPos = 1f;
    [SerializeField, Min(0)] private int conCubeImpact = 2;
    [SerializeField, Min(0)] private float conMoveTime = 4;


    [Header("Connectors")]
    [SerializeField] private List<ConnectCylinder> connectors = new List<ConnectCylinder>();


    [Button]
    private void GenerateButton() => Generate();


    private Cylinder[,] cylinders;


    private void Start()
    {
        Generate();

        UpdateAllCubesMovement();

        StartCoroutine(CheckConnections());
    }

    private void OnDestroy()
    {
        foreach (Cylinder cylinder in cylinders)
            if (cylinder != null)
                cylinder.CancelMove();
    }

    private void Generate()
    {
        int l = cylindersParent.childCount;
        for (int i = 0; i < l; i++)
           DestroyImmediate(cylindersParent.GetChild(0).gameObject);


        float height = side * Mathf.Sqrt(3) * 0.5f;
        float oddXOffset = height;

        int p = range * 2 - 1;

        cylinders = new Cylinder[p, p];

        for (int y = 0; y < range; y++)
        {
            int n = p - y;
            int center = n / 2;

            for (int x = - center; x < n - center; x++)
            {
                if (y < centerMargin && Mathf.Abs(x) + (y + (x > 0 ? 1 : 0)) / 2 < centerMargin)
                    continue;

                Spawn(x , y);
                if (y != 0)
                    Spawn(x, -y);
            }
        }

        void Spawn(int x, int y)
        {
            bool isOdd = Mathf.Abs(y) % 2 == 1;
            Vector3 offset = new Vector3(x * height * 2 + (isOdd ? oddXOffset : 0), 0, y * side * 1.5f);

            ConnectCylinder connectCylinder = connectors.Find(c => c.X == x && c.Y == y);
            bool isConnector = connectCylinder != null;

            GameObject obj = Instantiate(isConnector ? connectorCylinder : basicCylinder, cylindersParent); ;
            obj.name = isConnector ? $"ConnectorCylinder({x}, {y})" : $"Cylinder({x}, {y})";
            obj.transform.position = cylindersParent.position + offset;

            int tabX = range + x - 1;
            int tabY = range + y - 1;
            cylinders[tabX, tabY] = isConnector ? connectCylinder : new Cylinder();
            cylinders[tabX, tabY].Init(obj.transform, tabX, tabY);

        }
    }

    private IEnumerator CheckConnections()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            DimensionSO selectedDimension = DimensionManager.DefaultDimension;
            foreach (ConnectCylinder connectCube in connectors)
            {
                if (!connectCube.Connector)
                    continue;

                if (connectCube.Connector.IsConnectedRight)
                {
                    selectedDimension = connectCube.Dimension;
                    UpdateCubesMovement(connectCube.TabX, connectCube.TabY, false);
                    break;
                }
            }

            if (DimensionManager.LoadedDimension != selectedDimension)
            {
                if (selectedDimension == DimensionManager.DefaultDimension)
                    UpdateAllCubesMovement();

                DimensionManager.LoadDimension(selectedDimension);
                while (DimensionManager.LoadedDimension != selectedDimension)
                    yield return null;
            }
        }
    }

    private void UpdateAllCubesMovement()
    {
        foreach (ConnectCylinder connectCube in connectors)
        {
            if (!connectCube.Transform)
                continue;

            UpdateCubesMovement(connectCube.TabX, connectCube.TabY, true);
        }
    }

    private void UpdateCubesMovement(int cx, int cy, bool loop)
    {
        Dictionary<Cylinder, float> moves = new Dictionary<Cylinder, float>();

        float maxPos = conCubeImpact * 1.4f;
        for (int y = -conCubeImpact; y < conCubeImpact; y++)
        {
            int fy = cy + y;
            if (fy < 0 || fy >= cylinders.GetLength(0))
                continue;

            for (int x = -conCubeImpact; x < conCubeImpact; x++)
            {
                int fx = cx + x;
                if (fx < 0 || fx >= cylinders.GetLength(0))
                    continue;

                Cylinder cylinder = cylinders[fx, fy];
                if (cylinder == null)
                    continue;

                float power = Mathf.Max(0, maxPos - new Vector2(x, y).magnitude);
                float targetY = Map(power, 0, maxPos, minCubeYPos, maxCubeYPos);

                if (targetY <= 0)
                    continue;

                if (moves.TryGetValue(cylinder, out float actTargetY))
                {
                    if (actTargetY < targetY)
                        moves[cylinder] = targetY;
                }
                else
                    moves.Add(cylinder, targetY);
            }
        }

        foreach (var item in moves)
            item.Key.Move(minCubeYPos, item.Value, conMoveTime, loop);
    }

    private float Map(float x, float in_min, float in_max, float out_min, float out_max) => (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;

    public class Cylinder
    {
        public Transform Transform { get; private set; }

        public int TabX { get; private set; } = 0;
        public int TabY { get; private set; } = 0;

        public virtual void Init(Transform transform, int tabX, int tabY)
        {
            Transform = transform;
            TabX = tabX;
            TabY = tabY;
        }

        private Sequence sequence;

        public void CancelMove()
        {
            sequence.Kill();
            Transform.DOKill();
        }
        public void Move(float minY, float maxY, float moveTime, bool loop)
        {
            CancelMove();

            moveTime *= Random.Range(0.5f, 1.5f);

            if (loop)
            {
                if (Mathf.Abs(Transform.localPosition.y - minY) < Mathf.Abs(Transform.localPosition.y - maxY))
                {
                    sequence = DOTween.Sequence()
                        .Append(Transform.DOLocalMoveY(maxY, moveTime))
                        .OnComplete(Loop);

                    void Loop()
                    {
                        sequence = DOTween.Sequence()
                        .SetLoops(-1)
                        .Append(Transform.DOLocalMoveY(minY, moveTime))
                        .Append(Transform.DOLocalMoveY(maxY, moveTime))
                        .AppendInterval(moveTime / 2f)
                        ;
                    } 
                }
                else
                {
                    sequence = DOTween.Sequence()
                        .Append(Transform.DOLocalMoveY(minY, moveTime))
                        .OnComplete(Loop);

                    void Loop()
                    {
                        sequence = DOTween.Sequence()
                        .SetLoops(-1)
                        .Append(Transform.DOLocalMoveY(maxY, moveTime))
                        .AppendInterval(moveTime / 2f)
                        .Append(Transform.DOLocalMoveY(minY, moveTime))
                        ;
                    }
                }
            }
            else
            {
                Transform.DOLocalMoveY(maxY, moveTime);
            }
        }
    }


    [System.Serializable]
    public class ConnectCylinder : Cylinder
    {
        [field: SerializeField] public DimensionSO Dimension { get; private set; } = null;
        [field: SerializeField] public int X { get; private set; } = 0;
        [field: SerializeField] public int Y { get; private set; } = 0;

        public Connector Connector { get; private set; }


        public override void Init(Transform transform, int tabX, int tabY)
        {
            base.Init(transform, tabX, tabY);

            Connector = transform.GetComponent<Connector>();
            if (Connector == null)
                Debug.LogError($"ConnectCube at {X}, {Y} has no {nameof(Connector)} script!");
        }
    }
}
