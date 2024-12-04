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
        rb.freezeRotation = true; // ������ٵ��� ȸ���� ����
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // �浹 ���� ��� ����
    }

    void Update()
    {
        // ���콺 ȸ�� ó��
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // �̵� �Է� ����
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        moveDirection = transform.right * moveX + transform.forward * moveZ;
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f); // �밢�� �̵� �ӵ� ����ȭ
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        // Y�� �ӵ��� �����ϸ鼭 XZ����� �̵��� ����
        Vector3 targetVelocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);

        // ���� �ӵ��� ��ǥ �ӵ��� �ε巴�� ����
        Vector3 velocityChange = (targetVelocity - rb.velocity);
        velocityChange.y = 0; // Y�� ��ȭ�� ����

        // �ӵ� ������ �����Ͽ� ����
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
