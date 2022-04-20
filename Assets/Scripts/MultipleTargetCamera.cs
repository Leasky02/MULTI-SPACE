using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultipleTargetCamera : MonoBehaviour
{
    //targets for camera
    [SerializeField] private List<Transform> targets;
    [SerializeField] private Vector3 offset;
    
    //velocity of camera movement
    private Vector3 velocity;
    private float smoothTime = 0.1f;

    //zoom boundries
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;
    [SerializeField] private float zoomLimiter;

    //camera component
    private Camera cam;

    //Start called on first frame
    private void Start()
    {
        cam = GetComponent<Camera>();
    }
    //called after every update
    private void LateUpdate()
    {
        //if no target, return
        if (targets.Count == 0)
        {
            return;
        }

        //adjust camera position and zoom
        Move();
        Zoom();
    }

    private void Zoom()
    {
        //camera zoom caluclation
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        //smoothly zoom camera
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    private float GetGreatestDistance()
    {
        //calculate width of encapsulated targets
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for(int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        
        return bounds.size.magnitude;
    }

    private void Move()
    {

        //center point calculated in function which takes all players and gets center point
        Vector3 centerPoint = GetCenterPoint();
        //take into account the offset from the center point
        Vector3 newPosition = centerPoint + offset;
        //set camera position to the new position
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    private Vector3 GetCenterPoint()
    {
        //if there is only 1 player, return the player position
        if(targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        //calculate position between all targets
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        //return center point of bounds
        return bounds.center;
    }
}
