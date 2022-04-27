using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class HubFloorCylinderBahaviour : MonoBehaviour
{
    [SerializeField] private Transform cylindersParent;

    [SerializeField] private GameObject basicCylinder;
    [SerializeField, Min(0)] private float side = 0.5f;
    [SerializeField, Min(1)] private int range = 5;

    [Button]
    private void GenerateButton() => Generate();

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void Generate()
    {
        int l = cylindersParent.childCount;
        for (int i = 0; i < l; i++)
           DestroyImmediate(cylindersParent.GetChild(0).gameObject);


        float height = side * Mathf.Sqrt(3) * 0.5f;
        float oddXOffset = height;

        int p = range * 2 - 1;
        for (int y = 0; y < range; y++)
        {
            int n = p - y;
            int center = n / 2;

            for (int x = 0; x < n; x++)
            {
                Spawn(x - center, y);
                if (y != 0)
                    Spawn(x - center, -y);
            }
        }

        void Spawn(int x, int y)
        {
            bool isOdd = Mathf.Abs(y) % 2 == 1;

            GameObject obj = Instantiate(basicCylinder, cylindersParent);
            obj.name = $"Cylinder({x}, {y})";
            obj.transform.position = cylindersParent.position + new Vector3(x * height * 2 + (isOdd ? oddXOffset : 0), 0, y * side * 1.5f);
        }
    }
}
