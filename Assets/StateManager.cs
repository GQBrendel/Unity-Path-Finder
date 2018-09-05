using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {

    public GrahamScan grahanScan;
    public GameObject pointPrefab;
    GameObject go;
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawnPosition.y = 1.0f;
            GameObject objectInstance = Instantiate(pointPrefab, spawnPosition, Quaternion.Euler(new Vector3(0, 0, 0)));
        }
    }
}
