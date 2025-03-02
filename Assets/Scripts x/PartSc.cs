using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PartSc : MonoBehaviour
{
    public Gradient gradiant;
    public Renderer rend;
    public Rigidbody rb;
    bool destructed = false;
    public Collider coll;
    public GameObject back;

    private void Start()
    {
        //rend.material.color = gradiant.Evaluate(Random.Range(0f, 1f));
        transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,Random.Range(-0.05f, 0.05f));

        StartCoroutine(popup());
    }

    IEnumerator popup()
    {
        Vector3 scale = transform.localScale;
        transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(Random.Range(0f, .5f));
        transform.DOScale(scale, Random.Range(.1f, .4f)).SetEase(Ease.OutBack, 10);
    }

    public void SetColor(Color color)
    {
        rend.material.color = color;
    }


    public void Destruct(float explosion,Vector3 pos,float radius)
    {
        if (destructed) return;
        else destructed = true;

        /*coll.enabled = false;
        transform.DOComplete();
        transform.DOPunchScale(transform.localScale*.5f, .5f, 0).OnComplete(()=> {
            coll.enabled = true;
        });*/

        //back
        
        back.transform.SetParent(null);
        Destroy(back, Random.Range(1f, 1.5f));
        

        rb.useGravity = true;
        rb.isKinematic = false;

        rb.AddExplosionForce(explosion, pos, radius);

        gameObject.layer = LayerMask.NameToLayer("destructed");


        GameManager.instance.pixels.CrushPart(this);
    }
}
