using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCameraController : MonoBehaviour
{
    //[SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] CinemachineFreeLook freeLookCamera;
    [SerializeField] CinemachineVirtualCamera virtualCamera;

    [SerializeField] Transform cameraRoot;
    [SerializeField] Transform cameraRootWhenAiming;

    [SerializeField] Vector3 cameraRootWhenAimingFollowTargetOffSet;
    // Start is called before the first frame update
    [SerializeField] float rotateX;
    [SerializeField] float rotateY;

    [SerializeField] float bottomClamp = -30.0f;
    [SerializeField] float topClamp = 70.0f;

    [SerializeField] float mouseMoveSpeed = 60;

    [SerializeField] bool isAiming;
    private void Awake()
    {
        virtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
        freeLookCamera = FindAnyObjectByType<CinemachineFreeLook>();
    }
    // Start is called before the first frame update
    void Start()
    {
        mouseMoveSpeed = 120;

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        if (isAiming)
        {
            RotateCameraWithMouseWhenAiming();
        }
    }

    public void Init(Transform follow, Transform lookAt)
    {
        Debug.Log("MyCameraController Init");
        Cursor.lockState = CursorLockMode.Locked;

        cameraRoot = lookAt;
        cameraRootWhenAiming = lookAt;

        freeLookCamera.Follow = follow;
        freeLookCamera.LookAt = lookAt;

        virtualCamera.Follow = lookAt;
        virtualCamera.LookAt = lookAt;
    }

    public void SwithAimCamera()
    {
        isAiming = true;
        //virtualCamera.gameObject.SetActive(true);
        virtualCamera.Priority = 20;
        //freeLookCamera.gameObject.SetActive(false);
    }

    public void SwithFreeLookCamera()
    {
        isAiming = false;
        virtualCamera.Priority = 5;

        //virtualCamera.gameObject.SetActive(false);
        //freeLookCamera.gameObject.SetActive(true);
    }

    void RotateCameraWithMouse()
    {
        rotateX += Input.GetAxis("Mouse X") * mouseMoveSpeed * Time.deltaTime;
        rotateY += Input.GetAxis("Mouse Y") * mouseMoveSpeed * Time.deltaTime;

        rotateX = ClampAngle(rotateX, float.MinValue, float.MaxValue);
        rotateY = ClampAngle(rotateY, bottomClamp, topClamp);

        cameraRoot.rotation = Quaternion.Euler(-rotateY, rotateX, 0);
    }

    void RotateCameraWithMouseWhenAiming()
    {
        rotateX += Input.GetAxis("Mouse X") * mouseMoveSpeed * Time.deltaTime;
        rotateY += Input.GetAxis("Mouse Y") * mouseMoveSpeed * Time.deltaTime;

        //Gioi han goc quay cua camera va Reset khi goc quay qua 360 do
        rotateY = ClampAngle(rotateY, bottomClamp, topClamp);
        rotateX = ClampAngle(rotateX, float.MinValue, float.MaxValue);

        //cameraRootWhenAiming.position = cameraRoot.position + cameraRootWhenAimingFollowTargetOffSet;

        cameraRootWhenAiming.rotation = Quaternion.Euler(-rotateY, rotateX, 0);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);

    }
}
