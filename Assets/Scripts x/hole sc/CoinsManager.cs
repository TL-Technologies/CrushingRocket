using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    public List<CoinHoleSc> coins= new List<CoinHoleSc>();

    private void Start()
    {
        coins = new List<CoinHoleSc>();
        coins.AddRange(GetComponentsInChildren<CoinHoleSc>());
    }


    public void RemoveCoin(CoinHoleSc coin)
    {
        coins.Remove(coin);
        if (coins.Count==0)
        {
            BonusManager.instance.Finish();
        }
    }
}
