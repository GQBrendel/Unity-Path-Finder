using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A represantation of a node, it keep tracks of it's connected nodes.
/// </summary>
public class Node : MonoBehaviour {

    public List<Node> connections;
    public Material red, blue;

	void Start () {
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
    }

    void OnValidate()
    {
        connections = connections.Distinct().ToList();
    }
}
