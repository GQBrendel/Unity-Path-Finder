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

    [HideInInspector]
    public List<float> lowestDistance;
    float distance = Mathf.Infinity;

    /// <summary>
    /// Get all the nodes and clear the refference from connected nodes.
    /// </summary>
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
        //Order list counterclockwise
        pinList = _pinList.OrderBy(go => Mathf.Atan2(go.transform.position.x, go.transform.position.z)).ToList();
        clearInfo();


        biggestLowestDistance();
        for (int i = 0; i < pinList.Count; i++)
        {
            pinList[i].GetComponentInChildren<nodeConnection>().triggerConnections(distance);
        }
        StartCoroutine(PerformConnections());

        //
   
        //

    }
    IEnumerator PerformConnections()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < pinList.Count; i++)
        {
            List<GameObject> connectedList = pinList[i].GetComponentInChildren<nodeConnection>().nodesToConnect;

            foreach (GameObject connectable in connectedList)
            {
                connectTwoNodes(pinList[i].transform, connectable.transform);
            }

        }

    }
    private void connectTwoNodes(Transform start, Transform end)
    {
        LineRenderer laser;

        GameObject lineHolder = start.gameObject.GetComponent<Node>().instantiateConnection();

        laser = lineHolder.GetComponent<LineRenderer>();

        laser.SetPosition(0, start.position);
        laser.SetPosition(1, end.position);

        start.GetComponent<Node>().connections.Add(end.GetComponent<Node>());
        //end.GetComponent<Node>().connections.Add(start.GetComponent<Node>());

        instantiateCounter(start, end);
    }
    public float getBiggestLowestDistance()
    {
        if(distance < Mathf.Infinity)
        {
            return distance;
        }
        else
        {
            return biggestLowestDistance();
        }
    }
    float biggestLowestDistance()
    {
        for (int i = 0; i < pinList.Count; i++)
        {
            for (int j = 0; j < pinList.Count; j++)
            {
                float calculatedDistance = euclidianDistance((pinList[i].transform.position), (pinList[j].transform.position));
                if (calculatedDistance < distance && (i != j))
                {
                    distance = calculatedDistance;
                }
            }
            lowestDistance.Add(distance);
            distance = Mathf.Infinity;
        }
        lowestDistance.Sort();
      
        Debug.Log("The biggest Lowest Distance is " + lowestDistance[lowestDistance.Count - 1]);

        distance = lowestDistance[lowestDistance.Count - 1];
        return distance;
    }

    /// <summary>
    /// Gets the middle point beetween two nodes and instantiate a object to show the distance value 
    /// beetween they.
    /// </summary>
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
    //Returns the euclidian distance beetween two points
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
