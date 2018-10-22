using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPoints : MonoBehaviour {

    Transform[] points;

    public void OnDrawGizmos()
    {
        points = GetComponentsInChildren<Transform>();
        for (int i = 1; i < points.Length; i++)
        {
            Gizmos.color = Color.red;
            Vector3 current = points[i].position;
            Gizmos.DrawSphere(points[i].position, 0.3f);
            if (i == 1) continue;
            Vector3 previous = points[i - 1].position;
            Gizmos.DrawLine(previous, current);
        }
    }
}
