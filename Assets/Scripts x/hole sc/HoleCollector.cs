using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HoleCollector : MonoBehaviour
{
    public ParticleSystem popFx;
    public AnimationCurve scale;
    public int collected = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CoinStack"))
        {
            popFx.transform.position = other.transform.position;
            popFx.Play();
            other.GetComponent<CoinHoleSc>().Collected();

            NewScale();
        }
    }

    public void NewScale()
    {
        collected++;
        float s = scale.Evaluate(collected);
        transform.parent.localScale = new Vector3(s, s, 3);
        /*
        Vector3 punch = new Vector3 (s*.1f, s*.1f, 0);
        transform.parent.DOComplete();
        transform.parent.DOPunchScale(punch, .2f,0);*/
    }
}
