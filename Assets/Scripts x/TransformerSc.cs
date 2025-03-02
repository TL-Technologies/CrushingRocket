using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TransformerSc : MonoBehaviour
{
    public GameObject coin;
    public int cost = 2;
    int transformed = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Part"))
        {
            ReTransform(other.transform);
        }
    }

    public void ReTransform(Transform t)
    {
        transformed++;
        if (transformed>=cost)
        {
            GameObject c = Instantiate(coin, t.position, t.rotation, transform);
            //c.transform.DOScale(Vector3.one * Random.Range(.6f, 1.5f),.2f).SetEase(Ease.OutBack,3);
            Destroy(c, Random.Range(4f, 13f));

            transformed = 0;
        }
        
        Destroy(t.gameObject);
    }
}
