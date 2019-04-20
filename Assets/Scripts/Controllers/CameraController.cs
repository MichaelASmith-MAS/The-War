using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public int edgeOfScreenOffset = 10;
    public float zoomMin = 30f;
    public float zoomMax = 5f;
    public float zoomSpeed = 2f;
    public Vector3 cameraOffset;
    public LayerMask groundMask;

    MapGenerator mapGenerator;
    float zoom;
    float mouseX;
    Camera cameraObject;

    // Start is called before the first frame update
    void Start()
    {
        cameraObject = Camera.main;
        cameraObject.transform.position = transform.position + cameraOffset;
        cameraObject.transform.LookAt(transform);
        zoom = cameraOffset.y;

        mapGenerator = FindObjectOfType<MapGenerator>();

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
        Zoom();
    }

    void LateUpdate()
    {
        cameraObject.transform.LookAt(transform);
    }

    void Move()
    {
        float moveX = 0, moveY = 0;
        Vector3 targetDirection = transform.position;

        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y > Screen.height - edgeOfScreenOffset)
        {
            moveY = 1;
        }
        else if(Input.GetKey(KeyCode.S) || Input.mousePosition.y < edgeOfScreenOffset)
        {
            moveY = -1;
        }

        if(Input.GetKey(KeyCode.A) || Input.mousePosition.x < edgeOfScreenOffset)
        {
            moveX = -1;
        }
        else if(Input.GetKey(KeyCode.D) || Input.mousePosition.x > Screen.width - edgeOfScreenOffset)
        {
            moveX = 1;
        }

        targetDirection = (transform.forward * moveY + transform.right * moveX) * moveSpeed;

        float mapEdge = (mapGenerator.meshSettings.NumVertsPerLine * mapGenerator.meshSettings.meshScale) / 2 - 10;

        if (transform.position.x + targetDirection.x < -mapEdge)
        {
            Debug.Log("Reached Map Edge: " + -mapEdge);
            targetDirection.x += .2f;
        }
        else if(transform.position.x + targetDirection.x > mapEdge)
        {
            Debug.Log("Reached Map Edge: " + mapEdge);
            targetDirection.x -= .2f;
        }

        if(transform.position.z + targetDirection.z < -mapEdge)
        {
            Debug.Log("Reached Map Edge: " + -mapEdge);
            targetDirection.z += .2f;
        }
        else if(transform.position.z + targetDirection.z > mapEdge)
        {
            Debug.Log("Reached Map Edge: " + mapEdge);
            targetDirection.z -= .2f;
        }

        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y + 10, transform.position.z), Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 200f, groundMask))
        {
            transform.position = new Vector3(targetDirection.x + transform.position.x, hit.point.y, targetDirection.z + transform.position.z);
        }
    }

    void Rotate()
    {
        if(Input.GetMouseButton(1))
        {
            mouseX += Input.GetAxis("Mouse X") * mouseSensitivity;

            transform.localRotation = Quaternion.Euler(0, mouseX, 0);
        }
    }

    void Zoom()
    {
        zoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        if (zoom < zoomMax)
        {
            zoom = zoomMax;
        }
        else if(zoom > zoomMin)
        {
            zoom = zoomMin;
        }

        cameraOffset.y = zoom;

        if(cameraOffset.y != cameraObject.transform.localPosition.y)
        {
            cameraObject.transform.localPosition = cameraOffset;
        }

    }
}
