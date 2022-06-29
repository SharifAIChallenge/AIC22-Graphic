using System;
using UnityEngine;
using System.Collections;

public class FlyCamera : MonoBehaviour
{
    public float moveSpeed = 100.0f;

    public float zoomStepSize = 10f;

    public float orbitingSpeed = 3f;

    public Vector2 moveXLimit = new Vector2(-10, 10);
    public Vector2 moveZLimit = new Vector2(-10, 10);

    public Vector2 zoomLimit = new Vector2(-10, 10);
    public float baseCameraY = 40;

    private float _xRotation;

    [SerializeField] private bool useKeyboard;

    private void Start()
    {
        _xRotation = transform.eulerAngles.x;
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
        if (scrollWheelChange != 0)
        {
            var newPos = transform.position + transform.forward * (scrollWheelChange * zoomStepSize);
            if (newPos.y > baseCameraY + zoomLimit.x && newPos.y < baseCameraY + zoomLimit.y &&
                newPos.z > moveZLimit.x && newPos.z < moveZLimit.y && newPos.x > moveXLimit.x &&
                newPos.x < moveXLimit.y)
                transform.position = newPos;
        }
    }

    private void Orbiting()
    {
        if (!Input.GetMouseButton(1)) return;

        var x = Input.GetAxis("Mouse X") * orbitingSpeed;
        var y = Input.GetAxis("Mouse Y") * orbitingSpeed;

        if (x != 0)
        {
            transform.Rotate(new Vector3(0, x, 0), Space.World);
        }

        if (y != 0)
        {
            _xRotation = Mathf.Clamp(_xRotation - y, 0, 85);
            transform.eulerAngles = new Vector3(_xRotation, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
    }

    private void ShiftMove()
    {
        var p = GetBaseInput();
        if (!(p.sqrMagnitude > 0)) return;


        p = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * p;
        p *= moveSpeed;
        p *= Time.deltaTime;
        p += transform.position;
        transform.position = new Vector3(Mathf.Clamp(p.x, moveXLimit.x, moveXLimit.y),
            p.y, Mathf.Clamp(p.z, moveZLimit.x, moveZLimit.y));
    }

    private Vector3 GetBaseInput()
    {
        if (useKeyboard)
        {
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
        else
        {
            if (!Input.GetMouseButton(0)) return Vector3.zero;

            var x = -Input.GetAxis("Mouse X");
            var y = -Input.GetAxis("Mouse Y");
            print("helo");

            return new Vector3(x, 0, y);
        }

    }
}