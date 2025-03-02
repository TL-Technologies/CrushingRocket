using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CoinHoleSc : MonoBehaviour
{
    public int style=0;
    public TextMeshPro text01, text02;
    public Color[] color;
    public Renderer rend;
    int cost = 1;
    public Rigidbody rb;
    private void Start()
    {
        switch (style)
        {
            case 0:
                cost = 1;
                text01.text = "1"; text02.text = "1";
                rend.material.color = color[0];
                break;
            case 1:
                cost = 2;
                text01.text = "2"; text02.text = "2";
                rend.material.color = color[1];
                break;
            case 2:
                cost = 5;
                text01.text = "5"; text02.text = "5";
                rend.material.color = color[2];
                break;
            case 3:
                cost = 10;
                text01.text = "10"; text02.text = "10";
                rend.material.color = color[3];
                break;
        }
    }

    public void PullDown()
    {
        rb.velocity = new Vector3(rb.velocity.x, -5f, rb.velocity.z);
    }

    public void Collected()
    {
        BonusManager.instance.bonusCoins += cost;
        BonusManager.instance.coinsManager.RemoveCoin(this);
        Destroy(gameObject, 3);
    }

}
