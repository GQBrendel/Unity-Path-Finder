using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {

    public GrahamScan grahanScan;
    public Graph graph;
    public GameObject pointPrefab;
    public float offset = 2f;
    public Transform instancePos;
    public List<Vector2> selectedPoints;
    public List<GameObject> pinList;
    GameObject go;
    void Start () {
		
	}
	void Update () {

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            instancePos.position = hit.point + hit.normal * offset;
            if(Input.GetMouseButtonDown(0))
            {
                go = Instantiate(pointPrefab, instancePos.position, Quaternion.identity) as GameObject;
                selectedPoints.Add(new Vector2(instancePos.position.x, instancePos.position.z));
                pinList.Add(go);
            }
        }

    }
    public void startGrahanScan()
    {
        grahanScan.triggerGrahamScan(selectedPoints, pinList);
    }
    public void createFork()
    {
        graph.buildGraph(selectedPoints, pinList);
    }

}
