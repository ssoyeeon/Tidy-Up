using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public float pickupRange = 3f; // �÷��̾ ��ü�� ���� �� �ִ� �ִ� �Ÿ�
    public LayerMask pickupLayer; // ��ü�� ���� �� �ִ� ���̾�
    public LayerMask placementLayer; // ��ü�� ���� �� �ִ� ���̾�
    public Camera playerCamera; // �÷��̾��� ī�޶�
    public Vector3 heldObjectPosition = new Vector3(-0.5f, -0.3f, 0.5f); // ȭ�鿡 ��ü�� ǥ���� ��ġ (ī�޶� ����)
    public Rigidbody playerRigidbody;

    private GameObject heldObject; // ���� �÷��̾ ��� �ִ� ��ü
    private Rigidbody heldRigidbody; // ��� �ִ� ��ü�� Rigidbody (���� �Ӽ�)
    private Collider heldCollider; // ��� �ִ� ��ü�� Collider (�浹 ó��)
    private Vector3 originalScale; // ��ü�� ���� ũ�� ����
    private Vector3 bottomOffset; // ��ü�� �ϴ����� �������� ��ġ�� �����ϱ� ���� ������

    public List<GameObject> objectList = new List<GameObject>();
    public GameObject box;
    public Vector3 boxPosition = new Vector3(-47, -2, -3);

    public GameObject ESCUI;
    public bool isESC;

    public bool isGrounded;
    private Vector3 velocity;                   // �÷��̾��� ���� �ӵ�

    void Start()
    {
        playerCamera = playerCamera ? playerCamera : Camera.main; // ���� ī�޶� �������� �ʾҴٸ� ���� ī�޶� ���
        Cursor.lockState = CursorLockMode.Locked; // ���콺 Ŀ���� ȭ�� �߾ӿ� ����
        Cursor.visible = false; // ���콺 Ŀ�� ����
        isGrounded = true;
    }

    void Update()
    {
        // ���콺 ���� ��ư�� ������ ��
        if (Input.GetMouseButtonDown(0))
        {
            // ��ü�� ��� ���� ������ ���� �õ�
            if (heldObject == null)
            {
                TryPickup();
            }
            else
            {
                // ��ü�� ��� ������ ���� �õ�
                TryPlace();
            }

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit))
                {
                    if (box == hit.collider.gameObject && objectList.Count > 0)
                    {
                        int random = Random.Range(0, objectList.Count);
                        Debug.Log(random);
                        GameObject temp = Instantiate(objectList[random], boxPosition, Quaternion.identity);
                        //temp.AddComponent<GrabItem>().itemNumber = objectList[random].GetComponent<GrabItem>().itemNumber;

                        if (objectList[random] = null)
                        {
                            Destroy(objectList[random]);
                        }
                        objectList.RemoveAt(random);
                    }
                }
            }
          
        }

        // ��ü�� ��� �ִ� ���� ��� ��ġ ������Ʈ
        if (heldObject != null)
        {
            UpdateHeldObjectPosition();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isESC == true)
            {
                Time.timeScale = 1;
                isESC = false;
                ESCUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                return;
            }
            if (isESC == false)
            {
                Time.timeScale = 0;
                isESC = true;
                ESCUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
        {
            playerRigidbody.AddForce(Vector3.up * 20, ForceMode.Impulse);
        }
        else { Debug.Log("��ٷ�!!!!!!!!"); }
    }

    // ��ü�� ���� �õ��ϴ� �Լ�
    void TryPickup()
    {
        // ī�޶� �߽ɿ��� ���̸� ���� ��ü�� ã��
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // ī�޶� ȭ�� �߾ӿ��� ���� ����
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange, pickupLayer)) // ���̰� �浹�ϸ�
        {
            PickupObject(hit.collider.gameObject); // ��ü ����
        }
    }

    // ��ü�� ������ ���� �Լ�
    void PickupObject(GameObject obj)
    {
        heldObject = obj; // ��� �ִ� ��ü ����
        originalScale = obj.transform.localScale; // ��ü�� ���� ũ�� ����

        // ��ü�� Rigidbody�� ������ ������ ��ȣ�ۿ� ����
        heldRigidbody = obj.GetComponent<Rigidbody>();
        if (heldRigidbody != null)
        {
            heldRigidbody.isKinematic = true; // ��ü�� �ٸ� ��ü�� �浹�ϰų� �߷¿� ������ ���� �ʵ��� ����
            heldRigidbody.useGravity = false; // �߷� ��Ȱ��ȭ
        }

        // ��ü�� Collider�� ������ ��Ȱ��ȭ�Ͽ� �÷��̾�� �浹���� �ʵ��� ����
        heldCollider = obj.GetComponent<Collider>();
        if (heldCollider != null)
        {
            heldCollider.enabled = false; // ��ü�� �浹 ó�� ��Ȱ��ȭ
            bottomOffset = CalculateBottomOffset(heldCollider); // ��ü�� �ϴ� ������ ���
        }

        obj.transform.SetParent(playerCamera.transform); // ��ü�� �÷��̾� ī�޶��� �ڽ����� �����Ͽ� �÷��̾�� �Բ� �̵�
        UpdateHeldObjectPosition(); // ��ü�� ȭ�鿡 ���̵��� ��ġ ������Ʈ
        obj.transform.localScale = originalScale * 0.5f; // ��ü ũ�⸦ ����Ͽ� �÷��̾ ��� �ִٴ� ������ ��
    }

    // ��ü�� �ϴ����� ����Ͽ� ��ü�� ��Ȯ�� ���� ���� �������� ��ȯ�ϴ� �Լ�
    Vector3 CalculateBottomOffset(Collider collider)
    {
        // ��ü�� BoxCollider�� ��� �ϴ��� ���
        if (collider is BoxCollider boxCollider)
        {
            return new Vector3(0, -boxCollider.size.y / 2, 0); // �ڽ��� �ϴ�
        }
        // ��ü�� SphereCollider�� ��� �ϴ��� ���
        else if (collider is SphereCollider sphereCollider)
        {
            return new Vector3(0, -sphereCollider.radius, 0); // ���� �ϴ�
        }
        // ��ü�� CapsuleCollider�� ��� �ϴ��� ���
        else if (collider is CapsuleCollider capsuleCollider)
        {
            return new Vector3(0, -capsuleCollider.height / 2, 0); // ĸ���� �ϴ�
        }
        // �ٸ� ������ �ݶ��̴��� ���ؼ��� �߰��� ó���� �� ����
        return Vector3.zero; // �ݶ��̴� Ÿ���� �ٸ� ��� �⺻�� ��ȯ
    }

    // ��� �ִ� ��ü�� ��ġ�� ȸ���� ������Ʈ�ϴ� �Լ�
    void UpdateHeldObjectPosition()
    {
        heldObject.transform.localPosition = heldObjectPosition; // ��ü�� ������ ȭ�� ��ġ�� �̵�
        heldObject.transform.localRotation = Quaternion.identity; // ��ü�� ȸ���� �ʱ�ȭ�Ͽ� ������ ������ ����
    }

    // ��ü�� ���� �õ��� �ϴ� �Լ�
    void TryPlace()
    {
        // ī�޶� �߾ӿ��� ���̸� ���� ��ü�� ���� �� �ִ� ������ ã��
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // ī�޶� ȭ�� �߾ӿ��� ���� ����
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange, placementLayer)) // ���̰� ��ü ���� ������ ���� ������
        {
            PlaceObject(hit.point, hit.normal); // ��ü�� ����
        }
    }

    // ��ü�� ������ ���� �Լ�
    void PlaceObject(Vector3 position, Vector3 normal)
    {
        heldObject.transform.SetParent(null); // ��ü�� ī�޶��� �ڽĿ��� ����
        heldObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, normal); // ��ü�� �ٴڿ� �°� ȸ��
        heldObject.transform.localScale = originalScale; // ��ü ũ�⸦ ���� ũ��� ����

        // �ϴ����� �������� ��ü�� ��ġ�� �����Ͽ� ��Ȯ�� �ٴڿ� ����
        Vector3 adjustedPosition = position - heldObject.transform.TransformVector(bottomOffset);
        heldObject.transform.position = adjustedPosition; // ��ġ ������ ������ ��ü ��ġ

        // ��ü�� Rigidbody�� ������ ������ ��ȣ�ۿ��� �ٽ� Ȱ��ȭ
        if (heldRigidbody != null)
        {
            heldRigidbody.isKinematic = false; // ������ ��ȣ�ۿ� Ȱ��ȭ
            heldRigidbody.useGravity = true; // �߷� �ٽ� ����
            heldRigidbody.velocity = Vector3.zero; // ��ü�� �̵� �ӵ� �ʱ�ȭ
            heldRigidbody.angularVelocity = Vector3.zero; // ��ü�� ȸ�� �ӵ� �ʱ�ȭ
        }

        // ��ü�� Collider�� �ٽ� Ȱ��ȭ�Ͽ� �浹�� �����ϰ� ��
        if (heldCollider != null)
        {
            heldCollider.enabled = true; // ��ü�� �浹 ó�� �ٽ� Ȱ��ȭ
        }

        // ��� �ִ� ��ü ���� ���� �ʱ�ȭ
        heldObject = null;
        heldRigidbody = null;
        heldCollider = null;
    }
}
