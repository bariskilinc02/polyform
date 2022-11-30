using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    private Bools Bools;

    #region CameraTPS
    [Header("TPSLook")]
    public Camera Camera;
    public Transform lookAt;

    private const float YMin = 0;
    private const float YMax = 89.0f;


    public float distance = 12.0f;
    public float currentX = 50.0f;
    public float currentY = 50.0f;
    public float sensivity = 800.0f;

    public bool invertMouseLook;

    #endregion

    public bool autoLockCursor;

    public Transform CameraMenuPlacement, CameraGamePlacement;

    #region Unity Functions

    public void Init()
    {
        Cursor.lockState = (autoLockCursor) ? CursorLockMode.Locked : CursorLockMode.None;

        Bools = FindObjectOfType<Bools>();
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (Bools.is_OnTransition == true || Bools.is_InMenu == true) return;

        if(Input.GetMouseButton(1))
            CameraTPSLook();
    }
    #endregion

    #region Camera Look
    public void CameraTPSLook()
    {
        currentX += Input.GetAxis("Mouse X") * sensivity * Time.deltaTime;
        currentY += Input.GetAxis("Mouse Y") * sensivity * (invertMouseLook ? 1 : -1) * Time.deltaTime;

        currentY = Mathf.Clamp(currentY, YMin, YMax);

        Vector3 Direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Camera.transform.position = lookAt.position + rotation * Direction;

        Camera.transform.LookAt(lookAt.position);
    }
    #endregion
   
    #region Set Camera Target Transform
    public void SetCameraTransformTo(Transform transform)
    {
        currentX += Input.GetAxis("Mouse X") * sensivity * Time.deltaTime;
        currentY += Input.GetAxis("Mouse Y") * sensivity * (invertMouseLook ? 1 : -1) * Time.deltaTime;

        currentY = Mathf.Clamp(currentY, YMin, YMax);

        Vector3 Direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = lookAt.position + rotation * Direction;

        transform.LookAt(lookAt.position);
    }

    public void SetCameraTransformToGame()
    {
        SetCameraTransformTo(CameraGamePlacement);
    }

    #endregion

    #region Camera Transitions

    public void CameraTransitionToMenu()
    {
        SetCameraTransformToGame();

        Camera.transform.DOMove(CameraMenuPlacement.position, 1f);
        Camera.transform.DORotateQuaternion(CameraMenuPlacement.rotation, 1f);
    }

    public void CameraTransitionToGame()
    {
        SetCameraTransformToGame();

        Camera.transform.DOMove(CameraGamePlacement.position, 1f);
        Camera.transform.DORotateQuaternion(CameraGamePlacement.rotation, 1f);
    }



    #endregion
}
