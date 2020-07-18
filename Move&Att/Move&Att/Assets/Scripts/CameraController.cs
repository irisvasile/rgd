using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform mainCamera;
    private Transform pivot;
    public Transform target;

    public Vector3 cameraStartRotation;
    public bool enableVerticalRotation;
    private Vector3 localRotation;
    private float currentZoom = 10f;
    public float mouseSensitivity = 4f;
    public float scrollSensitvity = 2f;
    public float rotationSpeed = 10f;
    public float scrollSpeed = 6f;
    public float minScroll = 2f;
    public float maxScroll = 80f;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        pivot.position = target.position;

        //Seteaza pozitia de start a camerei
        localRotation = cameraStartRotation;
    }

    void Start()
    {
        mainCamera = this.transform;
        pivot = this.transform.parent;
        

        
    }

    private void Update()
    {
        //Pivotul va avea mereu aceeasi pozitie ca playerul
        if (target != null)
            pivot.position = target.position;
    }

    void LateUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            //Calculeaza rotatia in functie de miscarea mouse-ului
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                localRotation.x += Input.GetAxis("Mouse X") * mouseSensitivity;

                if (enableVerticalRotation)
                {
                    localRotation.y -= Input.GetAxis("Mouse Y") * mouseSensitivity;
                    localRotation.y = Mathf.Clamp(localRotation.y, 0f, 90f);
                }
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            //Calculeaza facatorul de zoom in/out
            float scrollAmount = Input.GetAxis("Mouse ScrollWheel") * scrollSensitvity;
            scrollAmount *= (currentZoom * 0.3f);

            currentZoom -= scrollAmount;
            currentZoom = Mathf.Clamp(currentZoom, minScroll, maxScroll);
        }

        //Roteste pivotul
        Quaternion rotation = Quaternion.Euler(localRotation.y, localRotation.x, 0);
        pivot.rotation = Quaternion.Lerp(pivot.rotation, rotation, Time.deltaTime * rotationSpeed);

        //Modifica coordonata z a camerei pentru zoom in/out
        if (mainCamera.localPosition.z != currentZoom * -1f)
        {
            mainCamera.localPosition = new Vector3(0f, 0f, Mathf.Lerp(mainCamera.localPosition.z, currentZoom * -1f,
                scrollSpeed * Time.deltaTime));
        }
    }
}
