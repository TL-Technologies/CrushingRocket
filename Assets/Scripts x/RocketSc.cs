using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RocketSc : MonoBehaviour
{
    public Rigidbody rb;
    Vector3 prevPos;

    float radius = 10;
    float explosion = 5;
    public GameObject explfx;
    public GameObject distortionFx;
    //public TrailRenderer trail;

    float time;

    //public GameObject dot;
    public Renderer rend;
    Color color;
    bool exploid = false;
    private void FixedUpdate()
    {
        Vector3 dir = transform.position - prevPos;
        if (dir!=Vector3.zero) transform.rotation = Quaternion.LookRotation(dir);
        prevPos = transform.position;
    }


    public void Setup(Vector3 forward, float force, float explosion, float radius, Color color)
    {
        prevPos = transform.position;
        transform.forward = forward;
        rb.AddForce(transform.forward * force, ForceMode.VelocityChange);

        this.explosion = explosion;
        this.radius = radius;

        time = Time.time;

        this.color = color;
        rend.materials[0].color = color;
        //trail.startColor = color;
        //trail.endColor = color;

        transform.localScale = Vector3.one * 200 * radius;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Exploid();
    }

    public void Exploid()
    {
        if (exploid) return;
        else exploid = true;


        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider item in colliders)
        {
            PartSc part = item.GetComponent<PartSc>();
            if (part!=null)
            {
                part.Destruct(explosion,transform.position,radius);
            }
        }

        //fx
        //Instantiate(explfx, transform.position, Quaternion.identity).GetComponent<ExplosionFxSc>().Setup(radius * Vector3.one,color);

        //distortion
        
        GameObject distortion = Instantiate(distortionFx, transform.position, Quaternion.identity);
        distortion.transform.localScale = transform.localScale*0.001f;
        distortion.transform.DOScale(Vector3.zero, .4f).SetEase(Ease.InBack, 5).OnComplete(() =>
        {
            Destroy(distortion);
        });

        //dot.SetActive(true);
        //trail.transform.SetParent(null);
        //trail.time = Time.time-time + 1.0f;
        Destroy(gameObject);

        
    }

    
}
