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
}