using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionFxSc : MonoBehaviour
{
    public ParticleSystemRenderer rend;
    public ParticleSystem trailRend;
    ParticleSystem.TrailModule trail;
    private void Awake()
    {
        trail = trailRend.trails;
    }
    public void Setup(Vector3 scale,Color color)
    {
        transform.localScale = scale;
        rend.material.color = color;

        //trailRend.material.color = color;
        Color colorTrans = new Color(color.r,color.g,color.b,.1f);
        trail.colorOverLifetime = colorTrans;
    }
}
