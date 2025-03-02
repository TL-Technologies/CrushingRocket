using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    public GameObject dot;
    public LauncherSc launcher;
    public float deltaTime;
    int instantiatedPoints = 150;
    public int numberOfPoints;

    public GameObject[] points;

    //public Transform followPos;
    private void Start()
    {
        points = new GameObject[150];
        for (int i = 0; i < instantiatedPoints; i++)
        {
            points[i] = Instantiate(dot, transform.position, Quaternion.identity,transform);
        }
        UpdatePoints();
    }

    private void Update()
    {
        //transform.position = followPos.position;
        //transform.rotation = followPos.rotation;
        for (int i = 0; i < points.Length; i++)
        {
            points[i].transform.position = PointPosition(launcher.transform.forward,i * deltaTime);
        }
    }
    Vector3 PointPosition(Vector3 direction,float t)
    {
        return (transform.position + (direction.normalized * launcher.force * t) + .5f * Physics.gravity * (t * t));
    }

    public void UpdatePoints()
    {
        for (int i = 0; i < numberOfPoints; i++)
        {
            points[i].SetActive(true);
        }
        for (int i = numberOfPoints; i < instantiatedPoints; i++)
        {
            points[i].SetActive(false);
        }
    }
}
