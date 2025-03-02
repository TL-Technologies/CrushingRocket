using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleController : MonoBehaviour
{
    public Joystick joystick;
    public float speed;

    Vector3 dirSmooth = Vector3.zero;


    void Update()
    {

        Vector3 direction = Camera.main.transform.forward * joystick.Vertical + Camera.main.transform.right * joystick.Horizontal;

        //direction.Normalize();

        direction = new Vector3(direction.x, 0, direction.z);
        dirSmooth = Vector3.Lerp(dirSmooth, direction, Time.fixedDeltaTime * 7);

        //transform.Translate(direction * speed * Time.deltaTime);
        //rb.velocity = direction * speed * 100 * Time.deltaTime;
        Vector3 pos = (transform.position + direction * speed * Time.deltaTime);
        pos = new Vector3(Mathf.Clamp(pos.x, -8.8f, 8.8f), pos.y, Mathf.Clamp(pos.z, -8.8f, 8.8f));
        transform.position = pos;
    }
}
