using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class LineDrawer : MonoBehaviour
{

    LineRenderer laser;
    Vector3 startPos, endPos;
    public Transform endPosObj;

    Vector3 direction;
    float distance;


    void Start()
    {

        laser = GetComponent<LineRenderer>();
        startPos = transform.position;
        endPos = endPosObj.position;
        
        laser.SetPosition(0, startPos);
        laser.SetPosition(1, endPos);


    }
}
