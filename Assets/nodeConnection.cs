using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nodeConnection : MonoBehaviour {


    public Graph graph;
    public List<GameObject> nodesToConnect;

	// Use this for initialization
	void Start () {

        graph = GameObject.Find("PointStarter").GetComponent<Graph>();      
	}
	public void triggerConnections(float radius)
    {
        GetComponent<SphereCollider>().radius = radius;
     
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Node")
        {
            nodesToConnect.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Node")
        {
            nodesToConnect.Remove(other.gameObject);
        }
    }

    void ScanForItems()
    {
       
    }

    // Update is called once per frame
    void Update () {
		
	}
}
