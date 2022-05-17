using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RopeQ4 : MonoBehaviour
{
    [SerializeField] private GameObject ob;

    [SerializeField] private float time=0.1f;

    private void Start()
    {
        ob.transform.localEulerAngles = new Vector3(0, 0, 10);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 11)
        {
            if (rotateSwtich != null)
                StopCoroutine(rotateSwtich);
            rotateSwtich = RotateSwitch();
            StartCoroutine(rotateSwtich);
        }
    }

    private IEnumerator rotateSwtich;
    private IEnumerator RotateSwitch()
    {
        ob.transform.DORotate(new Vector3(0, 0, -10),time);
        yield return new WaitForSeconds(time);
        ob.transform.DORotate(new Vector3(0, 0, 10), time);
    }
}
