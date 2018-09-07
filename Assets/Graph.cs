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

    public void buildGraph(List<Vector2> _selectedPoints, List<GameObject> _pinList)
    {
        selectedPoints = _selectedPoints;
        pinList = _pinList.OrderBy(go => Mathf.Atan2(go.transform.position.x, go.transform.position.z)).ToList();

        for (int i = 0; i < pinList.Count; i++)
        {

            Transform t1 = pinList[i].transform;
            if (i == pinList.Count - 1 || i == pinList.Count - 2)
            {
                Vector3 middlePoint1 = ((t1.position - pinList[0].transform.position) * 0.5f) + pinList[0].transform.position;

                Instantiate(distanceInfoPrefab, middlePoint1, distanceInfoPrefab.transform.rotation, this.transform);
                drawLaser(t1, pinList[0].transform, pinList[0].transform);
            }
            else if (i == pinList.Count - 2)
            {
                //drawLaser(_pinList[i].transform, _pinList[0].transform);
            }
            else
            {

                Transform t2 = pinList[i + 1].transform;
                Transform t3 = pinList[i + 2].transform;

                drawLaser(t1, t2, t2);
                Vector3 middlePoint1 = ((t1.position - t2.position) * 0.5f) + t2.position;
                Vector3 middlePoint2 = ((t3.position - t2.position) * 0.5f) + t2.position;

                Instantiate(distanceInfoPrefab,middlePoint1,distanceInfoPrefab.transform.rotation,this.transform);
               // Instantiate(distanceInfoPrefab, middlePoint2, distanceInfoPrefab.transform.rotation, this.transform);

            }

        }
    }
    private void drawLaser(Transform start, Transform middle, Transform end)
    {
        LineRenderer laser;
        laser = start.GetComponent<LineRenderer>();

//        laser.positionCount = 3;
        laser.positionCount = 2;
        laser.SetPosition(0, start.position);
        laser.SetPosition(1, end.position);
  //      laser.SetPosition(2, middle.position);
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
