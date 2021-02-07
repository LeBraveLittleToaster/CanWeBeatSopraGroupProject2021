using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float moveSpeed = 5f;

    public float zoomSpeed = 5.0f;
    public float zoomFactor = 3.0f;

    public float minZoomF = .5f;
    public float maxZoomF = 10f;

    public float centeringSpeed = 50f;

    private float origionalSize = 0f;

    private Camera cam;

    private Vector3 oldMousePos;
    private bool isInTransition = false;
    private Vector3 transitionGoalPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        this.cam = GetComponent<Camera>();
        origionalSize = this.cam.orthographicSize;
        oldMousePos = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        doZoom();
        if (!isInTransition)
        {
            doMoveWASD();
            doMouseDrag();
        }
        else
        {
            isInTransition = doTransition();
        }

        

    }

    private bool doTransition()
    {
        float step = centeringSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, transitionGoalPos, Time.deltaTime * centeringSpeed);

        return Vector3.Distance(transform.position, transitionGoalPos) > 0.1f;
    }

    public void SetCameraToMapMiddle(int x, int y)
    {
        isInTransition = true;
        transitionGoalPos = new Vector3(x / 2, y / 2, transform.position.z);
        zoomFactor = Mathf.Clamp((Mathf.Max(x, y) / 10), minZoomF, maxZoomF);
        Debug.Log("Transitioning");
    }

    private void doMouseDrag()
    {

        if (Input.GetMouseButtonDown(1))
        {
            oldMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButton(1))
        {
            Vector3 curDelta = oldMousePos - Input.mousePosition;
            Debug.Log(curDelta);
            this.transform.Translate(new Vector3(curDelta.x, curDelta.y, 0) * (moveSpeed * (2 * zoomFactor) )* Time.deltaTime, Space.World);
            oldMousePos = Input.mousePosition;
        }
        
        
    }

    private void doMoveWASD()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-1, 0, 0) * moveSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(1, 0, 0) * moveSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0, 1, 0) * moveSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0, -1, 0) * moveSpeed * Time.deltaTime, Space.World);
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
