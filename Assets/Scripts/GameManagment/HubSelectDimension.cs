using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace GameManagment
{
    public class HubSelectDimension : MonoBehaviour
    {
        [SerializeField, Required] private Transform cubeParentBR;
        [SerializeField, Required] private Transform cubeParentBL;
        [SerializeField, Required] private Transform cubeParentTR;
        [SerializeField, Required] private Transform cubeParentTL;

        [SerializeField] private List<ConnectCube> cubesConnect = new List<ConnectCube>();

        private const int numberOfCubesInRow = 16;
        private const int numberOfCubesParentsInRow = 2;
        private const float cubeSzie = 0.6f;
        private Transform[,] cubes;

        private void Start()
        {
            SetCunes();

            StartCoroutine(CheckConnections());
        }

        private IEnumerator CheckConnections()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);

                DimensionManager.Dimension selectedDimension = DimensionManager.Dimension.Main;
                foreach (ConnectCube connectCube in cubesConnect)
                {
                    if (!connectCube.Connector)
                        continue;

                    if (connectCube.Connector.IsConnected)
                    {
                        selectedDimension = connectCube.Dimension;
                        break;
                    }
                }

                if (DimensionManager.LoadedDimension != selectedDimension)
                {
                    DimensionManager.LoadDimension(selectedDimension);
                    while (DimensionManager.LoadedDimension != selectedDimension)
                        yield return null;
                }   
            }
        }

        private void SetCunes()
        {
            if (!cubeParentBL || !cubeParentBR || !cubeParentTL || !cubeParentTR)
            {
                Debug.LogError($"{name}: one of cube parent is not set!");
                return;
            }

            int width = numberOfCubesInRow * numberOfCubesParentsInRow;
            cubes = new Transform[width, width];

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
                    cubes[x + offsetX, y + offsetY] = cube;

                    for (int c = 0; c < cubesConnect.Count; c++)
                    {
                        if (cube == cubesConnect[c].Transform)
                            cubesConnect[c].Init(x + offsetX, y + offsetY);
                    }
                }
            }
        }

        [System.Serializable]
        public class ConnectCube
        {
            [field: SerializeField, Required]
            public Transform Transform { get; private set; }


            [field: SerializeField, ValidateInput(nameof(IsValidThisDimension), "Dimenion can't be None or Main")]
            public DimensionManager.Dimension Dimension { get; private set; } = DimensionManager.Dimension.None;


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

            private bool IsValidThisDimension() => Dimension != DimensionManager.Dimension.None && Dimension != DimensionManager.Dimension.Main;
        }
    }
}