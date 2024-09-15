using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;                // 플레이어의 이동 속도
    public float mouseSensitivity = 2f;         // 마우스 감도 (시점 회전 속도)
    public float jumpForce = 1.5f;              // 점프할 때 적용되는 힘
    public float gravity = -12f;              // 중력 가속도 값

    [Header("Interaction Settings")]
    public float pickupRange = 10f;             // 물체를 집을 수 있는 최대 거리
    public LayerMask pickupLayer;               // 집을 수 있는 물체가 속한 레이어
    public LayerMask wallLayer;                 // 벽이 속한 레이어
    public float placeOffset = 0.1f;            // 물체를 놓을 때 벽으로부터의 거리

    [Header("Size Limits")]
    public float minSize = 0.1f;                // 물체의 최소 크기
    public float maxSize = 10f;                 // 물체의 최대 크기

    [Header("Distance Settings")]
    public float minDistance = 1f;              // 플레이어와 물체 사이의 최소 거리

    [Header("Smoothing Settings")]
    public float smoothSpeed = 10f;             // 물체 이동 및 크기 변경의 부드러움 정도
    public float positionPrecision = 0.001f;    // 위치 조정의 정밀도
    public float scalePrecision = 0.0001f;      // 크기 조정의 정밀도

    [Header("UI Settings")]
    public Color pickupCursorColor = Color.green;   // 물체를 집을 수 있을 때의 커서 색상  
    public Texture2D defaultCrosshair;          // 기본 크로스헤어 텍스처
    public Texture2D pickupCrosshair;           // 물체를 집을 수 있을 때의 크로스헤어 텍스처
    public Color crosshairColor = Color.white;  // 크로스헤어 색상
    public float crosshairSize = 32f;           // 크로스헤어 크기

    // Private variables
    private CharacterController controller;     // 플레이어의 CharacterController 컴포넌트
    private Camera playerCamera;                // 플레이어 카메라
    private float verticalRotation = 0f;        // 수직 회전 각도
    private Vector3 velocity;                   // 플레이어의 현재 속도
    private bool isGrounded;                    // 플레이어가 땅에 닿아 있는지 여부

    private GameObject heldObject;              // 현재 들고 있는 물체
    private Vector3 originalScale;              // 들고 있는 물체의 원래 크기
    private float originalDistance;             // 물체를 집었을 때의 원래 거리
    private Rigidbody heldRigidbody;            // 들고 있는 물체의 Rigidbody 컴포넌트
    private Collider heldCollider;              // 들고 있는 물체의 Collider 컴포넌트

    private Vector3 targetPosition;             // 물체의 목표 위치
    private Vector3 targetScale;                // 물체의 목표 크기

    private Quaternion originalRotation;        // 들고 있는 물체의 원래 회전

    [Header("Rotation Settings")]
    public float rotationSpeed = 100f;          // 물체 회전 속도
    public Vector3 rotationAxis = Vector3.up;   // 회전 축 (기본값: Y축)

    private Quaternion objectRotation;          // 물체의 현재 회전 상태
    private bool isRotating = false;            // 회전 중인지 여부

    private bool canPickup = false;             // 물체를 집을 수 있는지 여부

    private Vector3 objectOffset;               // 물체 중심과 피봇 포인트 사이의 오프셋

    [Header("Object Holding Settings")]
    public float verticalOffset = 0f;           // 물체의 수직 위치 조정을 위한 오프셋



    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();

        // 커서 잠금
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
        HandleObjectInteraction();
        CheckPickupRange();
        RotateObject();
    }

    //물체를 들 수 있는 상태인지 확인
    void CheckPickupRange()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickupRange, pickupLayer))
        {
            canPickup = true;
        }
        else
        {
            canPickup = false;
        }
    }

    void OnGUI()
    {
        // 화면 중앙 좌표 계산
        float centerX = Screen.width / 2;
        float centerY = Screen.height / 2;

        // 크로스헤어 위치 및 크기 계산
        Rect crosshairRect = new Rect(centerX - crosshairSize / 2, centerY - crosshairSize / 2, crosshairSize, crosshairSize);

        // 크로스헤어 그리기
        GUI.color = crosshairColor;
        if (canPickup)
        {
            GUI.DrawTexture(crosshairRect, pickupCrosshair);
        }
        else
        {
            GUI.DrawTexture(crosshairRect, defaultCrosshair);
        }
    }


    //마우스 움직임 
    void HandleMovement()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -1f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleObjectInteraction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (heldObject == null)
                PickupObject();
            else
                PlaceObject();
        }
    }

    void PickupObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickupRange, pickupLayer))
        {
            heldObject = hit.transform.gameObject;
            originalScale = heldObject.transform.localScale;
            originalDistance = hit.distance;
            objectRotation = heldObject.transform.rotation;

            // 물체의 중심점 계산
            Bounds bounds = CalculateBounds(heldObject);
            objectOffset = heldObject.transform.position - bounds.center;

            heldRigidbody = heldObject.GetComponent<Rigidbody>();
            heldCollider = heldObject.GetComponent<Collider>();

            if (heldRigidbody != null)
            {
                heldRigidbody.isKinematic = true;
            }

            if (heldCollider != null)
            {
                heldCollider.enabled = false;
            }
        }
    }

    void PlaceObject()
    {
        if (heldObject == null) return;

        Vector3 currentPosition = heldObject.transform.position;
        Vector3 currentScale = heldObject.transform.localScale;

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickupRange, wallLayer))
        {
            currentPosition = hit.point + hit.normal * (currentScale.magnitude * 0.5f);
        }

        heldObject.transform.position = currentPosition;

        if (heldCollider != null)
        {
            heldCollider.enabled = true;
        }

        if (heldRigidbody != null)
        {
            heldRigidbody.isKinematic = false;
            heldRigidbody.velocity = Vector3.zero;
            heldRigidbody.angularVelocity = Vector3.zero;
        }

        heldObject = null;
        heldRigidbody = null;
        heldCollider = null;
    }

    Bounds CalculateBounds(GameObject obj)
    {
        Bounds bounds = new Bounds(obj.transform.position, Vector3.zero);
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }
        return bounds;
    }

    Vector3 AdjustPositionForWalls(Vector3 desiredPosition, Vector3 objectScale)
    {
        float radius = Mathf.Max(objectScale.x, objectScale.y, objectScale.z) / 2;
        RaycastHit hit;
        if (Physics.SphereCast(playerCamera.transform.position, radius, playerCamera.transform.forward, out hit, pickupRange, wallLayer))
        {
            return hit.point + hit.normal * (radius + placeOffset);
        }
        return desiredPosition;
    }

    void RotateObject()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            isRotating = true;
        }
        else if(Input.GetKeyDown(KeyCode.R))
        {
            isRotating = false;
        }

        if(isRotating)
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;
            objectRotation *= Quaternion.AngleAxis(rotationAmount, rotationAxis); 
        }
    }
}
