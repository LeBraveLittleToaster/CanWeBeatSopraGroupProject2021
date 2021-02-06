using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float moveSpeed = 1f;

    public float zoomSpeed = 5.0f;
    public float zoomFactor = 1.0f;

    public float minZoomF = 0.1f;
    public float maxZoomF = 5f;

    private float origionalSize = 0f;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        this.cam = GetComponent<Camera>();
        origionalSize = this.cam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        doZoom();

    }

    private void doMove()
    {
        if (Input.GetKey(KeyCode.A))
        {
            this.cam.transform.Translate(new Vector3(-1, 0, 0) * moveSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey(KeyCode.D))
        {
            this.cam.transform.Translate(new Vector3(1, 0, 0) * moveSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey(KeyCode.W))
        {
            this.cam.transform.Translate(new Vector3(0, 1, 0) * moveSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey(KeyCode.S))
        {
            this.cam.transform.Translate(new Vector3(0, -1, 0) * moveSpeed * Time.deltaTime, Space.World);
        }

    }

    private void doZoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            SetZoom(this.zoomFactor - Input.GetAxis("Mouse ScrollWheel"));
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            SetZoom(this.zoomFactor - Input.GetAxis("Mouse ScrollWheel"));
        }


        float targetSize = this.origionalSize * zoomFactor;
        if (targetSize != this.cam.orthographicSize)
        {
            this.cam.orthographicSize = Mathf.Lerp(this.cam.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
        }
    }

    void SetZoom(float zoomFactor)
    {
        this.zoomFactor = Mathf.Clamp(zoomFactor, minZoomF, maxZoomF);
    }
}
