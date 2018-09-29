using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    private void Start()
    {
        stateManager = GetComponent<StateManager>();
    }
    public void autoGrahanPath()
    {
        stateManager.startGrahanScan();
        StartCoroutine(graphRoutine());
       
    }
    IEnumerator graphRoutine()
    {
        yield return new WaitForSeconds(2f);
        pointsThatsBelongToConvexHull.Clear();

        selectedPoints = stateManager.selectedPoints;
        pinList = stateManager.pinList;
        hullPoints = grahanScan.hullPoints;


        grahanScan.triggerGrahamScan(selectedPoints, pinList);

        graph.buildGraph(selectedPoints, pinList);
        Debug.Log(hullPoints.Count);
        Debug.Log(pinList.Count);
        graph.buildGraphForAutoPath(hullPoints);
    }
}

