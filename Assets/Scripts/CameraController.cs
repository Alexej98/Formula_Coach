using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public ButtonPresser script;
    [SerializeField] public float moveSpeed = 3f;
    [SerializeField] public float rotationSpeed = 2f;

    private void LateUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            RotateCamera();
        }
        /*if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            Zoom();
        }*/
    }

    private void RotateCamera()
    {
        float yaw = Input.GetAxis("Mouse X");
        float pitch = Input.GetAxis("Mouse Y");
        Vector3 rotateValue = new Vector3(pitch, -yaw, 0) * rotationSpeed;

        if (script.nextIndex == 0)
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

    /*private void Zoom()
    {
        float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;

        #region Limit Zoom Distance
        float nextZoomDistance = Vector3.Distance(target.position, transform.position) - ScrollAmount;

        if (nextZoomDistance >= MaxZoomDistance || nextZoomDistance <= MinZoomDistance)
            return;
        #endregion

        transform.Translate(new Vector3(0, 0, ScrollAmount*0.05f), Space.Self);
    }*/
}