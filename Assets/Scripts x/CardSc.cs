using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class CardSc : MonoBehaviour
{
    public int level=1,cost,maxLevel; 
    public TextMeshProUGUI costTxt, levelTxt;
    public Button button;

    public void UpdateButton()
    {
        if (level>=maxLevel)
        {
            levelTxt.text = level.ToString();
            costTxt.text = "MAX";
            button.interactable = false;
            return;
        }
        cost = (int)GameManager.instance.upgrade.cost.Evaluate(level);
        
        if (GameManager.instance.coins < cost) button.interactable = false;
        else button.interactable = true;

        levelTxt.text = level.ToString();
        costTxt.text = cost.ToString() + "$";
    }

    public void Popup()
    {
        transform.DOComplete();
        transform.DOPunchScale(Vector3.one * .3f, .2f, 0);
    }

}
