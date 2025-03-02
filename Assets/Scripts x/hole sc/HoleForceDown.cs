using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleForceDown : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CoinStack"))
        {
            other.GetComponent<CoinHoleSc>().PullDown();
        }
    }
}
