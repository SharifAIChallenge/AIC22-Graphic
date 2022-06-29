using System;
using UnityEngine;
using System.Collections;

public class FlyCamera : MonoBehaviour
{
    float mainSpeed = 100.0f; //regular speed
    float camSens = 0.25f; //How sensitive it with mouse

    [Range(0.1f, 10f)]
    public float zoomStepSize = 0.5f;
    
    public float dragSpeed = 3f;
    
    private Vector3
        lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)

    private float totalRun = 1.0f;

    private float xRotation;

    private void Start()
    {
        xRotation = transform.eulerAngles.x;
    }

    void Update()
    {
        Zoom();
        Orbiting();
        ShiftMove();
    }

    private void Zoom()
    {
        var scrollWheelChange = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheelChange != 0){
            transform.position += transform.forward * (scrollWheelChange * zoomStepSize);
        }
    }
    
    private void Orbiting()
    {
        if (!Input.GetMouseButton(1)) return;
        
        var x = Input.GetAxis("Mouse X") * dragSpeed;
        var y = Input.GetAxis("Mouse Y") * dragSpeed;

        if (x != 0)
        {
            transform.Rotate(new Vector3(0, x, 0), Space.World);
        }

        if (y != 0)
        {
            xRotation = Mathf.Clamp(xRotation - y, 0, 85);
            transform.eulerAngles = new Vector3(xRotation, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
    }

    private void ShiftMove()
    {
        float f = 0.0f;
        Vector3 p = GetBaseInput();
        if (p.sqrMagnitude > 0)
        {
            totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
            p = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * p;
            p *= mainSpeed;
            p *= Time.deltaTime;

            transform.position += p;
            /*Vector3 newPosition = transform.position;
            //transform.Translate(p);
            newPosition.x = transform.position.x;
            newPosition.z = transform.position.z;
            transform.position = newPosition;*/
        }
    }

    private Vector3 GetBaseInput()
    {
        //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += Vector3.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += Vector3.back;
        }

        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += Vector3.left;
        }

        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += Vector3.right;
        }

        return p_Velocity;
    }
}