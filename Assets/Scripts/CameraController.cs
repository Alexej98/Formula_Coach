using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] public float rotationSpeed = 2f;
    [SerializeField] private Transform target = null;
    [SerializeField] private float scrollSpeed = 3.0f;
    [SerializeField] private float MaxZoomDistance = 20.0f;
    [SerializeField] private float MinZoomDistance = 4.0f;

    private void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            RotateCamera();
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            Zoom();
        }
    }

    private void RotateCamera()
    {
        Debug.Log(Camera.main.transform.eulerAngles);
        float yaw = Input.GetAxis("Mouse X");
        float pitch = Input.GetAxis("Mouse Y");
        Vector3 rotateValue = new Vector3(pitch, -yaw, 0) * rotationSpeed;
        if (((Camera.main.transform.eulerAngles.y - rotateValue.y) > 312 || (Camera.main.transform.eulerAngles.y - rotateValue.y) < 48)
            && ((Camera.main.transform.eulerAngles.x - rotateValue.x) > 271 || (Camera.main.transform.eulerAngles.x - rotateValue.x) < 89))
        {
            Camera.main.transform.eulerAngles -= rotateValue;
        }
           
    }

    private void Zoom()
    {
        float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;

        #region Limit Zoom Distance
        float nextZoomDistance = Vector3.Distance(target.position, transform.position) - ScrollAmount;

        if (nextZoomDistance >= MaxZoomDistance || nextZoomDistance <= MinZoomDistance)
            return;
        #endregion

        transform.Translate(new Vector3(0, 0, ScrollAmount*0.05f), Space.Self);
    }
}
