using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class autoPath : MonoBehaviour
{

    public GrahamScan grahanScan;
    public Graph graph;
    public List<Vector2> selectedPoints;
    public List<GameObject> pinList;
    StateManager stateManager;

    public List<Transform> hullPoints;
    public List<Vector2> pointsThatsBelongToConvexHull;

    public List<GameObject> pointsList = new List<GameObject>();

    int currentPointPath = 0;

    public TextMeshProUGUI nameOfCurrentPin;


    private void Start()
    {
        stateManager = GetComponent<StateManager>();
    }

    public void nextPoint()
    {
        if (currentPointPath == hullPoints.Count-1)
        {
            return;
        }
        else
        {
            currentPointPath++;
            nameOfCurrentPin.text = hullPoints[currentPointPath].GetComponent<Node>().uiName.text;
        }
    }
    public void previous()
    {
        if(currentPointPath == 0)
        {
            return;
        }
        else
        {
            currentPointPath--;
            nameOfCurrentPin.text = hullPoints[currentPointPath].GetComponent<Node>().uiName.text;
        }
    }
    public void autoGrahanPath()
    {
        stateManager.startGrahanScan();
        StartCoroutine(graphRoutine());
       
    }
    IEnumerator graphRoutine()
    {
        yield return new WaitForSeconds(1f);
        pointsThatsBelongToConvexHull.Clear();

        selectedPoints = stateManager.selectedPoints;
        pinList = stateManager.pinList;
        hullPoints = grahanScan.hullPoints;

        grahanScan.triggerGrahamScan(selectedPoints, pinList);

        graph.buildGraph(selectedPoints, pinList);
        Debug.Log(hullPoints.Count);
        Debug.Log(pinList.Count);
        graph.buildGraphForAutoPath(hullPoints);

        //        findPathForNodes();

        StartCoroutine(findPathForNodes());
    }

    IEnumerator findPathForNodes()
    {
        yield return new WaitForSeconds(2f);
        //First order the list
        hullPoints = hullPoints.OrderBy(hullPoints => hullPoints.name).ToList();

        //Case of P0

        Node startNode = hullPoints[currentPointPath].GetComponent<Node>();
        Node endNode = findFarthestPoint(hullPoints[currentPointPath]).GetComponent<Node>();

        stateManager.autoPath(startNode, endNode);


    }
    Transform findFarthestPoint(Transform pointToCompare)
    {
        Transform result = null;
        float maxDistance = -1;
        foreach (var point in hullPoints)
        {
            if(point == pointToCompare)
            {
                Debug.Log("Soy Eu");
            }
            else
            {
                float compDist = euclidianDistance(point.position, pointToCompare.position);
                if (compDist >  maxDistance)
                {
                    maxDistance = compDist;
                    result = point;
                }
                Debug.Log("Otro Es");
            }
        }

        Debug.Log("O Ponto mais distante de " + pointToCompare.name + " eh " + result);
        Debug.Log("A distância entre eles eh " + maxDistance);

        return result;
    }


    private float euclidianDistance(Vector3 p1, Vector3 p2)
    {
        float x0 = p1.x;
        float y0 = p1.z;

        float x1 = p2.x;
        float y1 = p2.z;

        float dX = x1 - x0;
        float dY = y1 - y0;
        double distance = System.Math.Sqrt(dX * dX + dY * dY);

        return (float)distance;
    }
}

