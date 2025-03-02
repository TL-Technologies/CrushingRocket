using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LauncherSc : MonoBehaviour
{
    public Transform initPos;
    public GameObject rocket;
    public float force;
    public float explosion=5;
    public float radius=10;

    public Renderer rend;
    public Gradient gradient;
    public bool control = false;

    //
    public float speed = 5;

    float screenWidth;
    float startTouch;
    float deltaSwipe;
    float multiplierSwipe = -200f;
    float startRotation;

    float xRot= -165;


    public Image chargeBar;
    public float chargeTime = 2.0f;

    public int ammo = 3;

    public AnimationCurve longKey, wideKey;
    public SkinnedMeshRenderer cannon;

    private void Awake()
    {
        screenWidth = (Screen.width / multiplierSwipe);
    }

    private void Start()
    {
        UpdateCannonModel();
    }

    public void Update()
    {
        if (!control) return;

        if (/*Input.GetMouseButtonDown(0) || */Input.GetKeyDown(KeyCode.Space))
        {
            Shot();
        }


        if (Input.GetMouseButtonDown(0))
        {
            startTouch = (Input.mousePosition.x / screenWidth);
            startRotation = xRot;
            //Charge();
        }

        if (Input.GetMouseButton(0))
        {
            deltaSwipe = (Input.mousePosition.x / screenWidth) - startTouch;
            xRot = startRotation + deltaSwipe;
            Vector3 r = new Vector3(xRot, -90, 180);
            transform.eulerAngles = r;
            //Debug.Log("xRot: " + xRot + " / euler: " + transform.eulerAngles.x);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Shot();
            //Decharge();
        }
        //FollowMouse();
    }

    public void OutOfControl()
    {
        control = false;
        //Decharge();
    }

    [ContextMenu("fgji")]
    public void dd()
    {
        PlayerPrefs.DeleteAll();
    }


    public void Charge()
    {
        if (ammo <= 0) return;

        chargeBar.DOKill();
        chargeBar.DOFillAmount(1, chargeTime).SetEase(Ease.Linear).OnComplete(() =>
        {
            Shot(); ammo--; GameManager.instance.upgrade.DisableLastBulletUI(ammo);
            Charge();

            if (ammo==0)
            {
                GameManager.instance.NoAmmo();
            }
        });
    }
    public void Decharge()
    {
        chargeBar.DOKill();
        chargeBar.DOFillAmount(0, chargeTime/2);
    }

    void FollowMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = transform.position.z - Camera.main.transform.position.z;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.LookAt(mousePos);
    }

    private void Shot()
    {
        if (ammo <= 0) return;

        Color c = gradient.Evaluate(Random.Range(0f, 1f));
        rend.materials[1].color = c;
        Instantiate(rocket, initPos.position,Quaternion.identity).GetComponent<RocketSc>().Setup(initPos.forward,force,explosion,radius,c);

        cannon.transform.DOComplete();
        cannon.transform.DOPunchScale(cannon.transform.localScale*.35f, .4f, 0);
        cannon.transform.DOPunchPosition(transform.right * .65f, .2f, 0);

        //chargeBar.fillAmount = 0;

        ammo--; GameManager.instance.upgrade.DisableLastBulletUI(ammo);

        if (ammo == 0)
        {
            GameManager.instance.NoAmmo();
        }
    }

    public void UpdateCannonModel()
    {
        if (cannon.sharedMesh != null && cannon.sharedMesh.blendShapeCount > 0)
        {
            cannon.SetBlendShapeWeight(0, longKey.Evaluate(GameManager.instance.upgrade.heightCard.level));
        }
        if (cannon.sharedMesh && cannon.sharedMesh.blendShapeCount > 1)
        {
            cannon.SetBlendShapeWeight(1, wideKey.Evaluate(GameManager.instance.upgrade.forceCard.level));
        }
    }
}
