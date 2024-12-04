using Fur;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public Rigidbody rb;
    public Transform cameraTransform;
    public LayerMask groundMask;
    private float verticalRotation = 0f;
    private Vector3 moveDirection;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb.freezeRotation = true; // 리지드바디의 회전을 고정
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // 충돌 감지 모드 변경
    }

    void Update()
    {
        // 마우스 회전 처리
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // 이동 입력 저장
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        moveDirection = transform.right * moveX + transform.forward * moveZ;
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f); // 대각선 이동 속도 정규화
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        // Y축 속도는 유지하면서 XZ평면의 이동만 조정
        Vector3 targetVelocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);

        // 현재 속도와 목표 속도를 부드럽게 보간
        Vector3 velocityChange = (targetVelocity - rb.velocity);
        velocityChange.y = 0; // Y축 변화는 무시

        // 속도 변경을 제한하여 적용
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            ContactPoint contact = collision.GetContact(0);
            Vector3 normal = contact.normal;
            moveDirection = Vector3.ProjectOnPlane(moveDirection, normal);
        }
    }
}
