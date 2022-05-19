using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromLasers : MonoBehaviour
{
     private SpriteRenderer plane;
     private GameObject laserOb;
    [SerializeField] private float checkDelay=2f;

    void Start()
    {
        laserOb = GameObject.Find("LaserPlane");
        if (laserOb!=null)
        {
            plane = laserOb.GetComponent<SpriteRenderer>();
            plane.color = new Color(plane.color.r, plane.color.g, plane.color.b, 0.7f);
            if (counter != null)
                StopCoroutine(counter);
            counter = Counter();
            StartCoroutine(counter);
        }
    }

    private IEnumerator counter;

    private IEnumerator Counter()
    {
        while (!SaveManager.isLevelFinished(Dimension.Laser))
            yield return new WaitForSeconds(checkDelay);
        plane.color = new Color(plane.color.r, plane.color.g, plane.color.b, 0f);
    }

    private void OnDisable()
    {
        if (plane != null)
            plane.color = new Color(plane.color.r, plane.color.g, plane.color.b, 0f);
    }
}
