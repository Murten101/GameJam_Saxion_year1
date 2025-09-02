using System.Collections.Generic;
using UnityEngine;

public class TimeSwitcher : MonoBehaviour
{
    [SerializeField]
    Vector2 positionA, positionB;
    [SerializeField]
    float camSizeA, camSizeB;
    [SerializeField]
    List<Transform> objectsToMove;

    private Vector2 PosDelta;
    private float sizeDelta;
    private void Start()
    {
        PosDelta = positionB - positionA;
        // oh oh, stinky
        foreach (var go in GameObject.FindGameObjectsWithTag("warp"))
        {
            if (!objectsToMove.Contains(go.transform))
                objectsToMove.Add(go.transform);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Switch();
        }
    }

    private void Switch()
    {
        Camera cam = Camera.main;
        cam.transform.position = cam.transform.position + (Vector3)PosDelta;
        
        foreach(var objTrans in objectsToMove)
        {
            objTrans.position = objTrans.position + (Vector3)PosDelta;
        }

        //invert delta so we move back next time.
        PosDelta = -PosDelta;
        sizeDelta = -sizeDelta;
    }

    public static void DrawCameraRect(Camera cam, Vector2 pos, Color color)
    {
        if (cam == null) return;

        Vector3 bl = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 br = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane));
        Vector3 tr = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));
        Vector3 tl = cam.ViewportToWorldPoint(new Vector3(0, 1, cam.nearClipPlane));

        Vector3 posv3 = (Vector3)pos;

        bl += posv3 - cam.transform.position;
        br += posv3 - cam.transform.position;
        tr += posv3 - cam.transform.position;
        tl += posv3 - cam.transform.position;

        Gizmos.color = color;
        Gizmos.DrawLine(bl, br);
        Gizmos.DrawLine(br, tr);
        Gizmos.DrawLine(tr, tl);
        Gizmos.DrawLine(tl, bl);
    }

    private void OnDrawGizmos()
    {
        DrawCameraRect(Camera.main, positionA, Color.green);
        DrawCameraRect(Camera.main, positionB, Color.red);
    }
}