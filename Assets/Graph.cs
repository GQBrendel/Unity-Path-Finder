using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
            if (i == pinList.Count - 1 || i == pinList.Count - 2)
            {
            }
            else
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
        GameObject go =
        Instantiate(distanceInfoPrefab, middlePoint1, distanceInfoPrefab.transform.rotation, this.transform);
        distInfoList.Add(go);
    }
    private void drawLaserChild(Transform start, Transform end)
    {
        LineRenderer laser;
        laser = start.Find("Pin").GetComponent<LineRenderer>();

        laser.SetPosition(0, start.position);
        laser.SetPosition(1, end.position);
        instantiateCounter(start, end);

    }
    private void drawLaser(Transform start, Transform end)
    {
        LineRenderer laser;
        laser = start.GetComponent<LineRenderer>();
        
        laser.SetPosition(0, start.position);
        laser.SetPosition(1, end.position);

        instantiateCounter(start, end);
    }
    private float euclidianDistance(Vector2 p1, Vector2 p2)
    {

        float x0 = p1.x;
        float y0 = p1.y;

        float x1 = p2.x;
        float y1 = p2.y;

        float dX = x1 - x0;
        float dY = y1 - y0;
        double distance = Math.Sqrt(dX * dX + dY * dY);

        return (float)distance;
    }
}
