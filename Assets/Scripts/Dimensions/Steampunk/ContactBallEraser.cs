using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactBallEraser : ImpulseTrackHandler
{
    [SerializeField] protected LayerMask layer;
    [SerializeField] protected bool locked;
    [SerializeField] protected float timeToUnlock;

    protected void OnCollisionEnter(Collision collision)
    {
        if((1 << collision.gameObject.layer & layer.value) != 0)
        {
            if (!locked && collision.gameObject.TryGetComponent(out BallBehavior ball))
            {
                ball.InitDestruction();
            }
        }
    }

    public override bool QualifyImpulse(Impulse impulse)
    {
        return true;
    }
    public override void HandleImpulse(Impulse impulse)
    {
        locked = false;
        StartCoroutine(UnlockTimer());
    }
    protected IEnumerator UnlockTimer()
    {
        float timeElapsed = 0f;
        for(; ; )
        {
            timeElapsed += Time.deltaTime;
            if(timeElapsed >= timeToUnlock)
            {
                locked = true;
                yield break;
            }
            yield return null;
        }
    }
}
