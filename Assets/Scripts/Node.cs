using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

/// <summary>
/// A represantation of a node, it keep tracks of it's connected nodes.
/// </summary>
public class Node : MonoBehaviour {

    public List<Node> connections;
    public Material red, blue;
    public GameObject connectionPrefab;
    public List<GameObject> lineChilds;
    public SphereCollider nodeRadius;
    public TextMeshProUGUI uiName;

	void Start () {
    }
    public GameObject instantiateConnection()
    {
        GameObject go = Instantiate(connectionPrefab);
        go.transform.parent = transform;
        lineChilds.Add(go);
        return go;
    }
    public void turnBlue()
    {
        GetComponentInChildren<MeshRenderer>().material = blue;
        GetComponent<Transform>().localScale = new Vector3(8, 8, 8);
    }
    public void turnRed()
    {
        GetComponentInChildren<MeshRenderer>().material = red;
        connections.Clear();
        GetComponent<Transform>().localScale = new Vector3(4, 4, 4);
        //lineChilds.Clear();
        removeLines();
    }
    public void removeLines()
    {
        foreach(GameObject g in lineChilds)
        {
            Destroy(g);
        }
        lineChilds.Clear();
    }

    void OnValidate()
    {
        connections = connections.Distinct().ToList();
    }
    public bool areConnected(Node compare)
    {
        bool alreadyExist = connections.Contains(compare);

        return alreadyExist;
    }
}
