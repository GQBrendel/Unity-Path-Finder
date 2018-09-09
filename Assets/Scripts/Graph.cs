using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UnityEngine.UI;

public class Graph : MonoBehaviour
{
    public List<GameObject> pinList;
    public List<Vector2> selectedPoints;
    public GameObject distanceInfoPrefab;
    public List<GameObject> distInfoList;

    public void clearInfo()
    {
        foreach (GameObject go in distInfoList)
        {
            Destroy(go);
        }
        distInfoList.Clear();
    }
    public void buildGraph(List<Vector2> _selectedPoints, List<GameObject> _pinList)
    {
        selectedPoints = _selectedPoints;
        pinList = _pinList.OrderBy(go => Mathf.Atan2(go.transform.position.x, go.transform.position.z)).ToList();
        clearInfo();

        for (int i = 0; i < pinList.Count; i++)
        {
            if (i != pinList.Count - 1 && i != pinList.Count - 2)
            {
                Transform t1 = pinList[i].transform;
                Transform t2 = pinList[i + 1].transform;
                Transform t3 = pinList[i + 2].transform;
                drawLaser(t1, t2);
                drawLaserChild(t1, t3);
            }
        }
    }
    private void instantiateCounter(Transform start, Transform end)
    {
        Vector3 middlePoint1 = ((start.position - end.position) * 0.5f) + end.position;
        middlePoint1 += new Vector3(0, 1, 0);
        GameObject go =
        Instantiate(distanceInfoPrefab, middlePoint1, distanceInfoPrefab.transform.rotation, this.transform);
        distInfoList.Add(go);
        float dist = euclidianDistance(start.position, end.position);
        go.GetComponentInChildren<Text>().text = dist.ToString("f2");
    }
    private void drawLaserChild(Transform start, Transform end)
    {
        LineRenderer laser;
        laser = start.Find("Pin").GetComponent<LineRenderer>();

        laser.SetPosition(0, start.position);
        laser.SetPosition(1, end.position);
        instantiateCounter(start, end);


        start.GetComponent<Node>().connections.Add(end.GetComponent<Node>());
        end.GetComponent<Node>().connections.Add(start.GetComponent<Node>());

    }
    private void drawLaser(Transform start, Transform end)
    {
        LineRenderer laser;
        laser = start.GetComponent<LineRenderer>();
        
        laser.SetPosition(0, start.position);
        laser.SetPosition(1, end.position);

        start.GetComponent<Node>().connections.Add(end.GetComponent<Node>());
        end.GetComponent<Node>().connections.Add(start.GetComponent<Node>());

        instantiateCounter(start, end);
    }
    private float euclidianDistance(Vector3 p1, Vector3 p2)
    {

        float x0 = p1.x;
        float y0 = p1.z;

        float x1 = p2.x;
        float y1 = p2.z;

        float dX = x1 - x0;
        float dY = y1 - y0;
        double distance = Math.Sqrt(dX * dX + dY * dY);

        return (float)distance;
    }
}
