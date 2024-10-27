using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;

    public Rigidbody rb;
    public Transform cameraTransform;
    public LayerMask groundMask;

    private float verticalRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb.freezeRotation = true; // ������ٵ��� ȸ���� ����
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
    }
    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        // �̵� ó��
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement = transform.right * moveX + transform.forward * moveZ;
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        if()
        {

        }
    }
}
