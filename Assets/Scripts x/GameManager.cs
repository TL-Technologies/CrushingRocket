using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using GameAnalyticsSDK;
using Facebook.Unity;
using Unity.Advertisement.IosSupport;


public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI coinsTxt;
    int _coins; public int coins
    {
        get => _coins;
        set
        {
            _coins = value;
            coinsTxt.text = coins.ToString("00") + "$";

            coinsTxt.transform.parent.DOComplete();
            coinsTxt.transform.parent.DOPunchScale(Vector3.one, .25f, 0);
            PlayerPrefs.SetInt("Coins", coins);
        }
    }

    public LauncherSc launcher;
    public Animator uiAnim, camAnim;
    public LauncherUpgrade upgrade;


    public static GameManager instance;
    public GameObject transitBg, transitBgLose, transitBgWon;

    public int collectedCoins;
    public TextMeshProUGUI collectedCoinsTxt;

    bool allCrushed = false;

    public BuildPixels pixels;

    public TextMeshProUGUI levelTxt;

    bool toBonusLevel = false;

    public Trajectory trajectory;

    private void Awake()
    {
        instance = this;

        if (PlayerPrefs.GetInt("FirstTime") == 0)
        {
            PlayerPrefs.SetInt("FirstTime", 1);
            PlayerPrefs.SetInt("Level", 1);

            PlayerPrefs.SetInt("Ammo", 1);
            PlayerPrefs.SetInt("Force", 1);
            PlayerPrefs.SetInt("Aim", 1);
            PlayerPrefs.SetInt("Height", 1);

            PlayerPrefs.SetInt("Pixel", 0);
        }

        coins = PlayerPrefs.GetInt("Coins");


        //
        //INIT
        InitializeFacebook();
        GameAnalytics.Initialize();
    }

    private void InitializeFacebook()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(FbInitCallback);
        }
        else
        {
            FB.ActivateApp();
        }
    }

    private void FbInitCallback()
    {
        FB.Mobile.SetAdvertiserTrackingEnabled(true);
        FB.Mobile.SetAutoLogAppEventsEnabled(true);

        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }

        Invoke(nameof(RegisterAppFOrNetworkAttribution), 1f);
    }

    private void RegisterAppFOrNetworkAttribution()
    {
        Debug.Log("SkAdNetworkRegisterAppForNetworkAttribution is called");


        SkAdNetworkBinding.SkAdNetworkRegisterAppForNetworkAttribution();
    }

    private void Start()
    {
        switch (PlayerPrefs.GetInt("TransitBG"))
        {
            case 0:
                transitBg.SetActive(true);
                transitBgLose.SetActive(false);
                transitBgWon.SetActive(false);
                break;
            case 1:
                transitBgLose.SetActive(true);
                break;
            case 2:
                transitBgWon.SetActive(true);
                break;
        }
        PlayerPrefs.SetInt("TransitBG", 0);
        levelTxt.text = "LEVEL " + PlayerPrefs.GetInt("Level").ToString("00");


    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            pixels.NextPixelBuild();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            trajectory.UpdatePoints();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            coins += 200;
        }
    }

    public void Play()
    {
        //launcher.chargeBar.transform.parent.gameObject.SetActive(true);
        uiAnim.SetBool("Play", true);
        camAnim.SetBool("Play", true);

        StartCoroutine(tuto());
        IEnumerator tuto()
        {
            yield return new WaitForEndOfFrame();
            launcher.control = true;

            uiAnim.SetBool("Tuto", true);
            yield return new WaitForSeconds(4.0f);
            uiAnim.SetBool("Tuto", false);
        }

        //
        //Start
        // level => PlayerPrefs.GetInt("Level").ToString()
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "LEVEL" + PlayerPrefs.GetInt("Level").ToString());

        //
    }

    public void Replay()
    {
        if (toBonusLevel)
        {
            ToBonusLevel();
            return;
        }
        SceneManager.LoadScene(0);
    }

    public void Lose()
    {
        transitBg.SetActive(true);
        transitBgLose.SetActive(true);
        transitBgWon.SetActive(false);

        uiAnim.SetBool("Lose", true);

        //
        //LOSE
        // level => PlayerPrefs.GetInt("Level").ToString()
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "LEVEL" + PlayerPrefs.GetInt("Level").ToString());

        //
    }
    public void Won()
    {
        transitBg.SetActive(true);
        transitBgWon.SetActive(true);
        transitBgLose.SetActive(false);

        uiAnim.SetBool("Won", true);

        PlayerPrefs.SetInt("Pixel", PlayerPrefs.GetInt("Pixel") + 1);

        /*PlayerPrefs.SetInt("Ammo", 1);
        PlayerPrefs.SetInt("Force", 1);
        PlayerPrefs.SetInt("Aim", 1);
        PlayerPrefs.SetInt("Height", 1);*/

        

        toBonusLevel = true;

        //
        //WON
        // level => PlayerPrefs.GetInt("Level").ToString()
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "LEVEL" + PlayerPrefs.GetInt("Level").ToString());

        //
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
    }

    public void NoAmmo()
    {
        StartCoroutine(coro());
        IEnumerator coro()
        {
            launcher.OutOfControl();

            yield return new WaitForSeconds(4f);

            //check if complete

            if (!allCrushed)
            {
                PlayerPrefs.SetInt("TransitBG", 1);
                Lose();
                yield return new WaitForSeconds(2.0f);

                SetGainedCoins();
            }
        }
    }
    public void SetGainedCoins()
    {
        collectedCoinsTxt.text = $"+{collectedCoins}$";
        coins += collectedCoins;
        //PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + collectedCoins);
    }

    public void AddToCollectedCoins(int c)
    {
        collectedCoins += c;
    }

    public void AllCrushed()
    {
        allCrushed = true;

        PlayerPrefs.SetInt("TransitBG", 2);
        transitBg.SetActive(true);

        launcher.OutOfControl();


        Won();
        Invoke("SetGainedCoins", 2f);

    }

    public void ToBonusLevel()
    {
        SceneManager.LoadScene(1);
    }
}

