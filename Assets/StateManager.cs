using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {

    public GrahamScan grahanScan;
    public GameObject pointPrefab;
    public float offset = 2f;
    public Transform instancePos;
    GameObject go;
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            instancePos.position = hit.point + hit.normal * offset;
            if(Input.GetMouseButtonDown(0))
            {
                Instantiate(pointPrefab, instancePos.position, Quaternion.identity);
            }
        }

    }

}
