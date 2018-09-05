/*
	@ masphei
	email : masphei@gmail.com
*/
// --------------------------------------------------------------------------
// 2016-05-11 <oss.devel@searchathing.com> : created csprj and splitted Main into a separate file
using System;
using System.Collections.Generic;
using UnityEngine;

public class GrahamScan : MonoBehaviour
{

    const int TURN_LEFT = 1;
    const int TURN_RIGHT = -1;
    const int TURN_NONE = 0;
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
        while (hull.Count > 1 && turn(hull[hull.Count - 2], hull[hull.Count - 1], r) != TURN_LEFT)
        {
            Debug.Log("Removing Point ({0}, {1}) because turning right " + hull[hull.Count - 1].x + hull[hull.Count - 1].y);
            hull.RemoveAt(hull.Count - 1);
        }
        if (hull.Count == 0 || hull[hull.Count - 1] != r)
        {
            Debug.Log("Adding Point ({0}, {1})" + r.x + r.y);
            hull.Add(r);
        }
        Debug.Log("# Current Convex Hull #");
        foreach (Vector2 value in hull)
        {
            Debug.Log("(" + value.x + "," + value.y + ") ");
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
        Debug.Log("# List of Point #");
        foreach (Vector2 value in points)
        {
            Debug.Log("(" + value.x + "," + value.y + ") ");
        }

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
        Debug.Log("# Sorted points based on angle with point p0 ({0},{1})#" + p0.x + p0.y);
        foreach (Vector2 value in order)
        {
            Debug.Log("(" + value.x + "," + value.y + ") : {0}" + getAngle(p0, value));
        }
        List<Vector2> result = new List<Vector2>();
        result.Add(p0);
        result.Add(order[0]);
        result.Add(order[1]);
        order.RemoveAt(0);
        order.RemoveAt(0);
        //Debug.Log("# Current Convex Hull #");
        foreach (Vector2 value in result)
        {
            Debug.Log("(" + value.x + "," + value.y + ") ");
        }
        foreach (Vector2 value in order)
        {
            keepLeft(result, value);
        }
        Debug.Log("# Convex Hull #");
        foreach (Vector2 value in result)
        {
            Debug.Log("(" + value.x + "," + value.y + ") ");
            foreach (GameObject go in pointsList)
            {
                if (go.transform.position.x == value.x && go.transform.position.z == value.y)
                {
                    hullPoints.Add(go.transform);
                }
            }
        }
    }

    private void Start()
    {
        List<Vector2> hullValues = new List<Vector2>
        {
            new Vector2(9, 1),
            new Vector2(4, 3),
            new Vector2(4, 5),
            new Vector2(3, 2),
            new Vector2(14, 2),
            new Vector2(4, 12),
            new Vector2(4, 10),
            new Vector2(5, 6),
            new Vector2(10, 2),
            new Vector2(1, 2),
            new Vector2(1, 10),
            new Vector2(5, 2),
            new Vector2(11, 2),
            new Vector2(4, 11),
            new Vector2(12, 4),
            new Vector2(3, 1),
            new Vector2(2, 6),
            new Vector2(2, 4),
            new Vector2(7, 8),
            new Vector2(5, 5)
        };

        List<Vector2> hullValuesFixed = new List<Vector2>
        {
            new Vector2(-13,0.5f),
            new Vector2(-10,-11.5f),
            new Vector2(-10,9),
            new Vector2(-4.5f,-2),
            new Vector2(-1,8.5f),
            new Vector2(0.5f,6),
            new Vector2(0.5f,-12),
            new Vector2(2,12.5f),
            new Vector2(3.5f,11),
            new Vector2(6.5f,3.2f),
            new Vector2(7,-10),
            new Vector2(9,-5),
            new Vector2(11.5f,-4)
        };

        triggerGrahamScan(hullValuesFixed);
      
    }
    public void triggerGrahamScan(List<Vector2> points)
    {
        foreach (Vector2 pointPair in points)
        {
            Vector3 pos = new Vector3(pointPair.x, 1, pointPair.y);
            go = Instantiate(pointPrefab, pos, pointPrefab.transform.rotation, this.transform) as GameObject;
            pointsList.Add(go);
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

            Debug.Log("(" + hullPoints[i].position.x + ", " + hullPoints[i].position.z + ") ");
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

