using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float rotationSpeed = 2f;
    private int nextIndex = 0;
    public static bool rotationEnabled;

    private void LateUpdate()
    {
        nextIndex = TutorialMode.nextIndex;
        if (Input.GetMouseButton(1) && rotationEnabled)
        {
            RotateCamera();
        }
    }

    //rotation of the camera if enabled
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