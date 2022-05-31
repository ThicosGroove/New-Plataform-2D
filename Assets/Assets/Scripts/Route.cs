using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    PlataformCheckPoint[] childNodes;
    public List<Transform> childNodeList = new List<Transform>();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        FillNodes();

        for (int i = 0; i < childNodeList.Count; i++)
        {
            Vector2 currentPos = childNodeList[i].position;

            if (i > 0)
            {
                Vector2 prvPos = childNodeList[i - 1].position;

                Gizmos.DrawLine(prvPos, currentPos);
            }
        }
    }


    void FillNodes()
    {
        childNodeList.Clear();

        childNodes = GetComponentsInChildren<PlataformCheckPoint>();

        foreach (PlataformCheckPoint child in childNodes)
        {
            if (child != this.transform)
            {
                childNodeList.Add(child.transform);
            }
        }
    }

}
