using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LauncherUpgrade : MonoBehaviour
{
    public LauncherSc launcher;
    public AnimationCurve cost;

    [Space(15)]
    public AnimationCurve ammo;
    public CardSc ammoCard;
    public Transform bulletsUI;
    public Color disableColor;

    [Space(15)]
    public AnimationCurve force;
    public CardSc forceCard;

    /*[Space(5)]
    public AnimationCurve recharge;
    public CardSc rechargeCard;*/
    [Space(5)]
    public AnimationCurve aim;
    public CardSc aimCard;

    [Space(5)]
    public AnimationCurve height;
    public CardSc heightCard;


    private void Start()
    {

        UpdateAll();
    }
    public void UpgradeForce()
    {
        GameManager.instance.coins -= forceCard.cost;
        forceCard.level++; PlayerPrefs.SetInt("Force", forceCard.level);
        launcher.radius = force.Evaluate(forceCard.level);

        CheckButtons();

        launcher.UpdateCannonModel();
    }
    public void UpgradeAmmo()
    {
        GameManager.instance.coins -= ammoCard.cost;
        ammoCard.level++; PlayerPrefs.SetInt("Ammo", ammoCard.level);
        launcher.ammo = (int)ammo.Evaluate(ammoCard.level);

        for (int i = 0; i < bulletsUI.childCount; i++)
        {
            if (i<launcher.ammo)
            {
                bulletsUI.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                bulletsUI.GetChild(i).gameObject.SetActive(false);
            }
        }

        CheckButtons();
    }
    public void DisableLastBulletUI(int bullet)
    {
        bulletsUI.GetChild(bullet).GetComponent<Image>().color = disableColor;
        bulletsUI.GetChild(bullet).transform.DOPunchScale(Vector3.one * .75f, .3f, 0);
    }
    /*public void UpgradeRecharge()
    {
        GameManager.instance.coins -= rechargeCard.cost;
        rechargeCard.level++; PlayerPrefs.SetInt("Time", rechargeCard.level);
        launcher.chargeTime = recharge.Evaluate(rechargeCard.level);

        CheckButtons();
    }*/
    public void UpgradeHeight()
    {
        GameManager.instance.coins -= heightCard.cost;
        heightCard.level++; PlayerPrefs.SetInt("Height", heightCard.level);
        launcher.force = height.Evaluate(heightCard.level);

        CheckButtons();

        launcher.UpdateCannonModel();
    }

    public void UpgradeAim()
    {
        GameManager.instance.coins -= aimCard.cost;
        aimCard.level++; PlayerPrefs.SetInt("Aim", aimCard.level);
        GameManager.instance.trajectory.numberOfPoints = (int)aim.Evaluate(aimCard.level);
        GameManager.instance.trajectory.UpdatePoints();

        CheckButtons();
    }


    public void CheckButtons()
    {
        forceCard.UpdateButton();
        ammoCard.UpdateButton();
        heightCard.UpdateButton();
        aimCard.UpdateButton();
    }
    public void UpdateAll()
    {
        ammoCard.level = PlayerPrefs.GetInt("Ammo");
        ammoCard.maxLevel = (int)ammo.keys[ammo.keys.Length - 1].time;
        launcher.ammo = (int)ammo.Evaluate(ammoCard.level);
        for (int i = 0; i < bulletsUI.childCount; i++)
        {
            if (i < launcher.ammo)
            {
                bulletsUI.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                bulletsUI.GetChild(i).gameObject.SetActive(false);
            }
        }

        forceCard.level = PlayerPrefs.GetInt("Force");
        forceCard.maxLevel = (int)force.keys[force.keys.Length - 1].time;
        launcher.radius = force.Evaluate(forceCard.level);

        /*rechargeCard.level = PlayerPrefs.GetInt("Time");
        rechargeCard.maxLevel = (int)recharge.keys[recharge.keys.Length - 1].time;
        launcher.chargeTime = recharge.Evaluate(rechargeCard.level);*/
        aimCard.level = PlayerPrefs.GetInt("Aim");
        aimCard.maxLevel = (int)aim.keys[aim.keys.Length - 1].time;
        GameManager.instance.trajectory.numberOfPoints = (int)aim.Evaluate(aimCard.level);
        GameManager.instance.trajectory.UpdatePoints();

        heightCard.level = PlayerPrefs.GetInt("Height");
        heightCard.maxLevel = (int)height.keys[height.keys.Length - 1].time;
        launcher.force = height.Evaluate(heightCard.level);

        CheckButtons();
    }
}
