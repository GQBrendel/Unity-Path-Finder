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
    //All the nodes in the scenes
    public List<GameObject> pointsList = new List<GameObject>();
    //The list of the points that belongs to the convex hull
    public List<Transform> hullPoints = new List<Transform>();

    public int turn(Vector2 p, Vector2 q, Vector2 r)
    {
        //The points that we take here are
        // P => The current node
        // Q => The proevious node
        // R => The node before the previous node.
        
        //If the result is 1 this means a turn to left, while -1 means turn to right.
        int result = ((q.x - p.x) * (r.y - p.y) - (r.x - p.x) * (q.y - p.y)).CompareTo(0);
        return result;
    }

    /// <summary>
    /// Keep removing points from stack while orientation 
    /// is not counterclockwise.
    /// </summary>
    /// <param name="hull"></param>
    /// <param name="r"></param>
    public void keepLeft(List<Vector2> hull, Vector2 r)
    {
        //Here we cycle throught the points and keep removing points if the do not belong in the convex hull
        //We do that cheking counter clockwise and removing if they are not part of the convex hull
        while (hull.Count > 1 && turn(hull[hull.Count - 2], hull[hull.Count - 1], r) != 1)
        {
             hull.RemoveAt(hull.Count - 1);
        }
        if (hull.Count == 0 || hull[hull.Count - 1] != r)
        {
           hull.Add(r);
        }
    }
    //Returns the angle beetween two given points
    public double getAngle(Vector2 p1, Vector2 p2)
    {
        float xDiff = p2.x - p1.x;
        float yDiff = p2.y - p1.y;
        return Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI;
    }
    /// <summary>
    /// Merge Sort function will divide the array in two smallest array
    /// sort they and then merge into a new array
    /// </summary>
    public List<Vector2> MergeSort(Vector2 p0, List<Vector2> arrPoint)
    {
        if (arrPoint.Count == 1) //Synce the is recursive, we'll return when the size is 1
        {
            return arrPoint;
        }
        List<Vector2> arrSortedInt = new List<Vector2>(); //List to keep track of the sorted array
        int middle = (int)arrPoint.Count / 2; //Simply the middle point

        //First divide the array in left and right to merge the two different sides
        List<Vector2> leftArray = arrPoint.GetRange(0, middle);
        List<Vector2> rightArray = arrPoint.GetRange(middle, arrPoint.Count - middle);

        leftArray = MergeSort(p0, leftArray);
        rightArray = MergeSort(p0, rightArray);

        //Iterators for both sides
        int leftptr = 0;
        int rightptr = 0;

        //Iterate on both arrays and sort based on angle
        for (int i = 0; i < leftArray.Count + rightArray.Count; i++)
        {
            if (leftptr == leftArray.Count) //If it's the last point on the left, just add to the list
            {
                arrSortedInt.Add(rightArray[rightptr]);
                rightptr++;
            } 
            else if (rightptr == rightArray.Count) //Same for right
            {
                arrSortedInt.Add(leftArray[leftptr]);
                leftptr++;
            }
            //If the smalest angle is on the left array, we'll add it to the list
            else if (getAngle(p0, leftArray[leftptr]) < getAngle(p0, rightArray[rightptr]))
            {
                arrSortedInt.Add(leftArray[leftptr]);
                leftptr++;
            }
            //Otherwise we'll add the right node
            else
            {
                arrSortedInt.Add(rightArray[rightptr]);
                rightptr++;
            }
        }
        //At the end we return the sorted array, with right and left together
        return arrSortedInt;
    }
    public void convexHull(List<Vector2> points)
    {
        Vector2 p0 = Vector2.zero;
        //Iterate to get the vector with the smallest value in y.
        foreach (Vector2 value in points)
        {
            if (p0 == Vector2.zero)
                p0 = value;
            else
            {
                if (p0.y > value.y)
                    p0 = value;
            }
        } //So now p0 is the vector with the smallest y

        List<Vector2> order = new List<Vector2>(); //Create a list to be ordered
        foreach (Vector2 value in points)
        {
            if (p0 != value) //The list will have all the points, except the p0
                order.Add(value);
        }

        //So we apply merge sort passing the p0 and the list with all the other vectors

        order = MergeSort(p0, order);
        //Create a new list to save the result, the add the point zero and the two first from the ordered list;
        List<Vector2> result = new List<Vector2>();
        result.Add(p0);
        result.Add(order[0]);
        result.Add(order[1]);
        //Remove the points that are already in the result list
        order.RemoveAt(0);
        order.RemoveAt(0);

        foreach (Vector2 value in order)
        {
            //Remove the points that don't are part of convex hull
            keepLeft(result, value);
        }
        //Clean hullpoints list
        hullPoints.Clear();
        foreach (Vector2 value in result)
        {
             foreach (GameObject go in pointsList)
            {
                if (go.transform.position.x == value.x && go.transform.position.z == value.y)
                {
                    //Add the points that belongs in the convex hull to the list of hullPoints
                    hullPoints.Add(go.transform);
                }
                else
                {
                    //If they are not in the convex hull remove the lines from the draw
                    //This is necessary because youcan change the points dinamically
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
    /// <summary>
    /// Function to start the graham scan algorithm
    /// </summary>
    /// <param name="points">The real points to be calculated</param>
    /// <param name="spawnedPins">the list of objects to visual feedback</param>
    public void triggerGrahamScan(List<Vector2> points, List<GameObject> spawnedPins)
    {
        if(spawnedPins != null)
        {
            //This is used when you trigger the algorithm not on the first time
            //It will feed the list with the points that are already spawned
            pointsList = spawnedPins;
        }
        else
        {
            foreach (Vector2 pointPair in points)
            {
                //Chweck the points and add them to the list
                Vector3 pos = new Vector3(pointPair.x, 1, pointPair.y);
                GameObject go = Instantiate(pointPrefab, pos, pointPrefab.transform.rotation, this.transform) as GameObject;
                pointsList.Add(go);
            }
        }
        //Start the convex hull on the points
        convexHull(points);
        //Draw the lines that connect the points.
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

    //Draw a line from apoint to another 
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

