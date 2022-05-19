using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromLasers : MonoBehaviour
{
    [SerializeField] private GameObject plane;
    [SerializeField] private float checkDelay=2f;
    private void Awake()
    {
        plane = GameObject.Find("LaserPlane");

    }
    void Start()
    {
        plane.SetActive(true);
        if (counter != null)
            StopCoroutine(counter);
        counter = Counter();
        StartCoroutine(counter);
    }

    private IEnumerator counter;

    private IEnumerator Counter()
    {
        while (!SaveManager.isLevelFinished(Dimension.Laser))
            yield return new WaitForSeconds(checkDelay);
        plane.SetActive(false);
    }
}
