using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour {

    public GrahamScan grahanScan;
    public Graph graph;
    public GameObject pointPrefab;
    public float offset = 2f;
    public Transform instancePos;
    public List<Vector2> selectedPoints;
    public List<GameObject> pinList;
    public List<Node> selectedNodes;
    public Canvas options;
    public Text pathText;
    Dijkstras djk;
    GameObject go;
    int pinNumber = 0;
    bool calculatingShortestPath = false;
    void Start () {

        djk = GetComponent<Dijkstras>();
        options.GetComponent<GraphicRaycaster>().enabled = false;

	}
	void Update () {

        RaycastHit hit;
        if(calculatingShortestPath)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    GameObject hitted = hit.transform.gameObject;
                    if(hitted.tag == "Node")
                    {
                        Debug.Log("Selected " + hitted.name);
                        selectedNodes.Add(hitted.GetComponent<Node>());
                        hitted.GetComponent<Node>().turnBlue();
                        if (selectedNodes.Count == 2)
                        {
                            startDijkstras();
                        }
                    }
                }
            }
        }
        else
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {                
                instancePos.position = hit.point + hit.normal * offset;
                if (Input.GetMouseButtonDown(0))
                {
                    go = Instantiate(pointPrefab, instancePos.position, Quaternion.identity) as GameObject;
                    selectedPoints.Add(new Vector2(instancePos.position.x, instancePos.position.z));
                    go.name = "pin" + pinNumber.ToString();
                     pinNumber++;
                    pinList.Add(go);
                    djk.nodesList.Add(go.GetComponent<Node>());
                    if (pinNumber == 4)
                        options.GetComponent<GraphicRaycaster>().enabled = true;
                }
                if (Input.GetMouseButtonDown(1))
                {
                    djk.GetShortestPath(djk.nodesList[0], djk.nodesList[djk.nodesList.Count - 1]);
                }
            }
        }
    }
    public void startGrahanScan()
    {
        clearGraph();
        options.transform.Find("Toggle").GetComponent<Toggle>().isOn = false;
        graph.clearInfo();
        grahanScan.triggerGrahamScan(selectedPoints, pinList);
        calculatingShortestPath = false;
    }
    public void clearGraph()
    {
        foreach (GameObject n in pinList)
        {
            n.GetComponent<Node>().turnRed();
        }
    }
    public void createGraph()
    {
        clearGraph();
        graph.buildGraph(selectedPoints, pinList);
    }
    void startDijkstras()
    {
        string output =
        djk.GetShortestPath(selectedNodes[0],selectedNodes[1]).ToString();
        pathText.text = output;
        selectedNodes.Clear();
    }
    public void togglecalculatingShortestPath()
    {
        calculatingShortestPath = !calculatingShortestPath;
    }

}
