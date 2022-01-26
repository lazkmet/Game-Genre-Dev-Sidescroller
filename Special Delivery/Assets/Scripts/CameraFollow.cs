using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform targetObject;
    public Vector3 startPosition { get; private set; }
    public float smoothingTime;
    private Vector3 velocity = Vector3.zero;
    private void Start()
    {
        startPosition = gameObject.transform.position;
        Reset();
    }
    void Update()
    {
        Vector3 targetPos = targetObject.position;
        
        Vector3 newPos = Vector3.SmoothDamp(gameObject.transform.position, targetPos, ref velocity, smoothingTime);
        newPos.z = gameObject.transform.position.z;
        gameObject.transform.position = newPos;
    }
    public void Reset()
    {
        gameObject.transform.position = startPosition;
    }
}
