using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;

namespace GameManagment
{
    public class HubSelectDimension : MonoBehaviour
    {
        [SerializeField, Required] private Transform cubeParentBR;
        [SerializeField, Required] private Transform cubeParentBL;
        [SerializeField, Required] private Transform cubeParentTR;
        [SerializeField, Required] private Transform cubeParentTL;

        [SerializeField] private List<ConnectCube> cubesConnect = new List<ConnectCube>();

        [Header("Movement")]
        [SerializeField] private float minCubeYPos = -0.3f;
        [SerializeField] private float maxCubeYPos = 1f;
        [SerializeField, Min(0)] private int conCubeImpact = 2;
        [SerializeField, Min(0)] private float conMoveTime = 4;

        private const int numberOfCubesInRow = 16;
        private const int numberOfCubesParentsInRow = 2;
        private const float cubeSzie = 0.6f;
        
        private Cube[,] cubes;

        private void Start()
        {
            SetCubes();

            UpdateAllCubesMovement();

            StartCoroutine(CheckConnections());
        }

        private IEnumerator CheckConnections()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);

                DimensionSO selectedDimension = DimensionManager.DefaultDimension;
                foreach (ConnectCube connectCube in cubesConnect)
                {
                    if (!connectCube.Connector)
                        continue;

                    if (connectCube.Connector.IsConnectedRight)
                    {
                        selectedDimension = connectCube.Dimension;
                        UpdateCubesMovement(connectCube.X, connectCube.Y, false);
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

        private void SetCubes()
        {
            if (!cubeParentBL || !cubeParentBR || !cubeParentTL || !cubeParentTR)
            {
                Debug.LogError($"{name}: one of cube parent is not set!");
                return;
            }

            int width = numberOfCubesInRow * numberOfCubesParentsInRow;
            cubes = new Cube[width, width];

            SetInArry(cubeParentBR, 0, 0);
            SetInArry(cubeParentBL, numberOfCubesInRow, 0);
            SetInArry(cubeParentTR, 0, numberOfCubesInRow);
            SetInArry(cubeParentTL, numberOfCubesInRow, numberOfCubesInRow);

            void SetInArry(Transform parent, int offsetX, int offsetY)
            {
                float tcubeSzie = 100 / cubeSzie;

                for (int i = 0; i < parent.childCount; i++)
                {
                    Transform cube = parent.GetChild(i);
                    int x = Mathf.RoundToInt(cube.localPosition.x * -tcubeSzie) / 100;
                    int y = Mathf.RoundToInt(cube.localPosition.z * tcubeSzie) / 100;
                    cubes[x + offsetX, y + offsetY] = new Cube(cube);

                    for (int c = 0; c < cubesConnect.Count; c++)
                    {
                        if (cube == cubesConnect[c].Transform)
                            cubesConnect[c].Init(x + offsetX, y + offsetY);
                    }
                }
            }
        }

        private void UpdateAllCubesMovement()
        {
            foreach (ConnectCube connectCube in cubesConnect)
            {
                if (!connectCube.Transform)
                    continue;

                UpdateCubesMovement(connectCube.X, connectCube.Y, true);
            }
        }

        private void UpdateCubesMovement(int cx, int cy, bool loop)
        {
            Dictionary<Cube, float> moves = new Dictionary<Cube, float>();

            float maxPos = conCubeImpact * 1.4f;
            for (int y = -conCubeImpact; y < conCubeImpact; y++)
            {
                int fy = cy + y;
                if (fy < 0 || fy >= numberOfCubesInRow * numberOfCubesParentsInRow)
                    continue;

                for (int x = -conCubeImpact; x < conCubeImpact; x++)
                {
                    int fx = cx + x;
                    if (fx < 0 || fx >= numberOfCubesInRow * numberOfCubesParentsInRow)
                        continue;

                    float power = Mathf.Max(0, maxPos - new Vector2(x, y).magnitude);
                    float targetY = Map(power, 0, maxPos, minCubeYPos, maxCubeYPos);

                    if (targetY <= 0)
                        continue;

                    if (moves.TryGetValue(cubes[fx, fy], out float actTargetY))
                    {
                        if (actTargetY < targetY)
                            moves[cubes[fx, fy]] = targetY;
                    }
                    else
                        moves.Add(cubes[fx, fy], targetY);
                }
            }

            foreach (var item in moves)
                item.Key.Move(minCubeYPos, item.Value, conMoveTime, loop);
        }

        private float Map(float x, float in_min, float in_max, float out_min, float out_max) => (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;

        private class Cube
        {
            [field: SerializeField, Required]
            public Transform Transform { get; private set; }


            private Sequence sequence;

            public Cube(Transform transform)
            {
                Transform = transform;
            }

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
                            .SetLoops(-1)
                            .Append(Transform.DOLocalMoveY(maxY, moveTime))
                            .AppendInterval(moveTime / 2f)
                            .Append(Transform.DOLocalMoveY(minY, moveTime))
                            ;
                    }
                    else
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
                    Transform.DOLocalMoveY(maxY, moveTime);
                }
            }
        }

        [System.Serializable]
        public class ConnectCube
        {
            [field: SerializeField, Required]
            public Transform Transform { get; private set; }


            [field: SerializeField, Required]
            public DimensionSO Dimension { get; private set; } = null;


            public int X { get; private set; }
            public int Y { get; private set; }

            public Connector Connector { get; private set; }

            public void Init(int x, int y)
            {
                X = x;
                Y = y;

                Connector = Transform.GetComponent<Connector>();
                if (Connector == null)
                    Debug.LogError($"ConnectCube at {x}, {y} has no {nameof(Connector)} script!");
            }
        }
    }
}