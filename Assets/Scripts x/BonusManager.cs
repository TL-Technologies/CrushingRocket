using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;


public class BonusManager : MonoBehaviour
{
    public HoleController controller;
    public TextMeshProUGUI bonusCoinsTxt;

    int _bonusCoins; public int bonusCoins
    {
        get => _bonusCoins;
        set
        {
            _bonusCoins = value;
            bonusCoinsTxt.text = bonusCoins.ToString("00") +"$";

            bonusCoinsTxt.transform.parent.DOComplete();
            bonusCoinsTxt.transform.parent.DOPunchScale(Vector3.one*.3f, .25f, 0);
        }
    }


    public Animator uiAnim;

    public CoinsManager coinsManager;

    public static BonusManager instance;
    private void Awake()
    {
        instance = this;
    }


    public void Finish()
    {
        uiAnim.SetBool("Finish", true);
    }

    public void ToLevels()
    {
        PlayerPrefs.SetInt("TransitBG", 0);
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + bonusCoins);
        SceneManager.LoadScene(0);
    }
}
