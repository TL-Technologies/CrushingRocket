using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinSc : MonoBehaviour
{
    public TextMeshPro text01, text02;
    public Color[] color;
    public Renderer rend;

    private void Start()
    {
        Randomize();
    }

    public void Randomize()
    {
        int r = Random.Range(0,6);
        //01  234  5

        switch (r)
        {
            case 0:
            case 1:
                text01.text = "1"; text02.text = "1";
                rend.material.color=color[0];
                transform.localScale = Vector3.one * .8f;

                GameManager.instance.AddToCollectedCoins(1);
                break;
            case 2:
            case 3:
            case 4:
                text01.text = "2"; text02.text = "2";
                rend.material.color = color[1];
                transform.localScale = Vector3.one * 1.4f;

                GameManager.instance.AddToCollectedCoins(2);
                break;
            case 5:
                text01.text = "5"; text02.text = "5";
                rend.material.color = color[2];
                transform.localScale = Vector3.one * 2.0f;

                GameManager.instance.AddToCollectedCoins(5);
                break;
        }
    }
}
