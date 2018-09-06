using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public List<GameObject> pinList;
    public List<Vector2> selectedPoints;

    public void buildGraph(List<Vector2> _selectedPoints, List<GameObject> _pinList)
    {
        selectedPoints = _selectedPoints;
        pinList = _pinList.OrderBy(go => Mathf.Atan2(go.transform.position.x, go.transform.position.z)).ToList();

        for (int i = 0; i < pinList.Count; i++)
        {
            if (i == pinList.Count - 1)
            {
                //drawLaser(_pinList[i].transform, _pinList[0].transform);
            }
            else if (i == pinList.Count - 2)
            {
                //drawLaser(_pinList[i].transform, _pinList[0].transform);
            }
            else
            {
                drawLaser(pinList[i].transform, pinList[i + 1].transform, pinList[i + 2].transform);
            }

        }
    }
    private void drawLaser(Transform start, Transform middle, Transform end)
    {
        LineRenderer laser;
        laser = start.GetComponent<LineRenderer>();

        laser.positionCount = 3;
        laser.SetPosition(0, start.position);
        laser.SetPosition(1, end.position);
        laser.SetPosition(2, middle.position);
    }
}
