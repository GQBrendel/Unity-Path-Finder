/*
    Code adapted from @masphei
	email : masphei@gmail.com

    Thanks @masphei!
    Original Source: https://github.com/masphei/ConvexHull.git
*/
using System;
using System.Collections.Generic;
using UnityEngine;

public class GrahamScan : MonoBehaviour
{
    public GameObject pointPrefab;
    GameObject go;
    public List<GameObject> pointsList = new List<GameObject>();
    public List<Transform> hullPoints = new List<Transform>();

    public int turn(Vector2 p, Vector2 q, Vector2 r)
    {
        return ((q.x - p.x) * (r.y - p.y) - (r.x - p.x) * (q.y - p.y)).CompareTo(0);
    }

    public void keepLeft(List<Vector2> hull, Vector2 r)
    {
        while (hull.Count > 1 && turn(hull[hull.Count - 2], hull[hull.Count - 1], r) != 1)
        {
             hull.RemoveAt(hull.Count - 1);
        }
        if (hull.Count == 0 || hull[hull.Count - 1] != r)
        {
           hull.Add(r);
        }
    }

    public double getAngle(Vector2 p1, Vector2 p2)
    {
        float xDiff = p2.x - p1.x;
        float yDiff = p2.y - p1.y;
        return Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI;
    }

    public List<Vector2> MergeSort(Vector2 p0, List<Vector2> arrPoint)
    {
        if (arrPoint.Count == 1)
        {
            return arrPoint;
        }
        List<Vector2> arrSortedInt = new List<Vector2>();
        int middle = (int)arrPoint.Count / 2;
        List<Vector2> leftArray = arrPoint.GetRange(0, middle);
        List<Vector2> rightArray = arrPoint.GetRange(middle, arrPoint.Count - middle);
        leftArray = MergeSort(p0, leftArray);
        rightArray = MergeSort(p0, rightArray);
        int leftptr = 0;
        int rightptr = 0;
        for (int i = 0; i < leftArray.Count + rightArray.Count; i++)
        {
            if (leftptr == leftArray.Count)
            {
                arrSortedInt.Add(rightArray[rightptr]);
                rightptr++;
            }
            else if (rightptr == rightArray.Count)
            {
                arrSortedInt.Add(leftArray[leftptr]);
                leftptr++;
            }
            else if (getAngle(p0, leftArray[leftptr]) < getAngle(p0, rightArray[rightptr]))
            {
                arrSortedInt.Add(leftArray[leftptr]);
                leftptr++;
            }
            else
            {
                arrSortedInt.Add(rightArray[rightptr]);
                rightptr++;
            }
        }
        return arrSortedInt;
    }

    public void convexHull(List<Vector2> points)
    {
        Vector2 p0 = Vector2.zero;
        foreach (Vector2 value in points)
        {
            if (p0 == Vector2.zero)
                p0 = value;
            else
            {
                if (p0.y > value.y)
                    p0 = value;
            }
        }
        List<Vector2> order = new List<Vector2>();
        foreach (Vector2 value in points)
        {
            if (p0 != value)
                order.Add(value);
        }

        order = MergeSort(p0, order);
        List<Vector2> result = new List<Vector2>();
        result.Add(p0);
        result.Add(order[0]);
        result.Add(order[1]);
        order.RemoveAt(0);
        order.RemoveAt(0);
        foreach (Vector2 value in order)
        {
            keepLeft(result, value);
        }
        hullPoints.Clear();
        foreach (Vector2 value in result)
        {
             foreach (GameObject go in pointsList)
            {
                if (go.transform.position.x == value.x && go.transform.position.z == value.y)
                {
                    hullPoints.Add(go.transform);
                }
                else
                {
                    LineRenderer lr = go.GetComponent<LineRenderer>();
                    LineRenderer lrC = go.transform.Find("Pin").GetComponent<LineRenderer>();
                    for(int i =0; i < lr.positionCount; i ++)
                    {
                        lr.SetPosition(i, go.transform.position);
                        lrC.SetPosition(i, go.transform.position);
                    }
                }
            }
        }
    }
    public void triggerGrahamScan(List<Vector2> points, List<GameObject> spawnedPins)
    {
        if(spawnedPins != null)
        {
            pointsList = spawnedPins;
        }
        else
        {
            foreach (Vector2 pointPair in points)
            {
                Vector3 pos = new Vector3(pointPair.x, 1, pointPair.y);
                go = Instantiate(pointPrefab, pos, pointPrefab.transform.rotation, this.transform) as GameObject;
                pointsList.Add(go);
            }
        }
        convexHull(points);
        for (int i = 0; i < hullPoints.Count; i++)
        {
            if (i == hullPoints.Count - 1)
            {
                drawLaser(hullPoints[i], hullPoints[0]);
            }
            else
            {
                drawLaser(hullPoints[i], hullPoints[i + 1]);
            }

        }
    }

    private void drawLaser(Transform start, Transform end)
    {
        LineRenderer laser;
        laser = start.GetComponent<LineRenderer>();
       
        Vector3 startPos, endPos;
        startPos = start.position;
        endPos = end.position;

        laser.SetPosition(0, startPos);
        laser.SetPosition(1, endPos);
    }
}

