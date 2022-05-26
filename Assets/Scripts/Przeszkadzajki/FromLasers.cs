using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromLasers : MonoBehaviour
{
     private SpriteRenderer planeSR;
     [SerializeField] private float checkDelay = 2f;

    private void Start()
    {
        GameObject plane = GameObject.Find("PlayerLaserPlane");
        if (plane == null)
        {
            Debug.LogWarning("Cant found LaserPlane");
            return;
        }

        planeSR = plane.GetComponent<SpriteRenderer>();
        planeSR.enabled = true;

        if (counter != null)
            StopCoroutine(counter);
        counter = Counter();
        StartCoroutine(counter);
    }

    private IEnumerator counter;

    private IEnumerator Counter()
    {
        while (!SaveManager.IsLevelFinished(Dimension.Laser))
            yield return new WaitForSeconds(checkDelay);

        planeSR.enabled  = false;
    }

    private void OnDisable()
    {
        if (planeSR != null)
            planeSR.enabled = false;
    }
}
