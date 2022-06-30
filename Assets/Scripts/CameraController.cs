using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 3f;
    [SerializeField] public float rotationSpeed = 2f;
    private int nextIndex;
    private bool rotationEnabled;

    private void Start()
    {
        nextIndex = ButtonPresser.nextIndex;
        rotationEnabled = ButtonPresser.rotationEnabled;
    }

    private void LateUpdate()
    {
        nextIndex = ButtonPresser.nextIndex;
        rotationEnabled = ButtonPresser.rotationEnabled;
        if (Input.GetMouseButton(1) && rotationEnabled)
        {
            RotateCamera();
        }
    }

    private void RotateCamera()
    {
        float yaw = Input.GetAxis("Mouse X");
        float pitch = Input.GetAxis("Mouse Y");
        Vector3 rotateValue = new Vector3(pitch, -yaw, 0) * rotationSpeed;

        if (nextIndex == 0)
        {
            if (((Camera.main.transform.eulerAngles.x - rotateValue.x) > 271 || (Camera.main.transform.eulerAngles.x - rotateValue.x) < 89))
            {
                Camera.main.transform.eulerAngles -= rotateValue;
            }
        }
        else
        {
            if (((Camera.main.transform.eulerAngles.y - rotateValue.y) > 320 || (Camera.main.transform.eulerAngles.y - rotateValue.y) < 40)
            && ((Camera.main.transform.eulerAngles.x - rotateValue.x) > 271 || (Camera.main.transform.eulerAngles.x - rotateValue.x) < 89))
            {
                Camera.main.transform.eulerAngles -= rotateValue;
            }
        }
    }
}