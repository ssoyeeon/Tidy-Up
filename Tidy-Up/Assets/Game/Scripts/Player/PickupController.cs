using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PickupController : MonoBehaviour
{
    [Header("UI Settings")]
    private Color pickupCursorColor = Color.green;   // ��ü�� ���� �� ���� ���� Ŀ�� ����  
    public Texture2D defaultCrosshair;          // �⺻ ũ�ν���� �ؽ�ó
    private Color crosshairColor = Color.white;  // ũ�ν���� ����
    private float crosshairSize = 25f;           // ũ�ν���� ũ��


    public float rotationSpeed = 100f;          // ��ü ȸ�� �ӵ�
    public Vector3 rotationAxis = Vector3.up;   // ȸ�� �� (�⺻��: Y��)
    private Quaternion objectRotation;          // ��ü�� ���� ȸ�� ����

    public float pickupRange = 3f; // �÷��̾ ��ü�� ���� �� �ִ� �ִ� �Ÿ�
    public LayerMask pickupLayer; // ��ü�� ���� �� �ִ� ���̾�
    public LayerMask placementLayer; // ��ü�� ���� �� �ִ� ���̾�
    public Camera playerCamera; // �÷��̾��� ī�޶�
    public Vector3 heldObjectPosition = new Vector3(-0.5f, -0.3f, 0.5f); // ȭ�鿡 ��ü�� ǥ���� ��ġ (ī�޶� ����)
    public Rigidbody playerRigidbody;
    public float jumpForce = 3;

    private GameObject heldObject; // ���� �÷��̾ ��� �ִ� ��ü
    private Rigidbody heldRigidbody; // ��� �ִ� ��ü�� Rigidbody (���� �Ӽ�)
    private Collider heldCollider; // ��� �ִ� ��ü�� Collider (�浹 ó��)
    private Vector3 originalScale; // ��ü�� ���� ũ�� ����
    private Vector3 bottomOffset; // ��ü�� �ϴ����� �������� ��ġ�� �����ϱ� ���� ������

    [Header("Box Settings")]
    public BoxData boxData;
    public GameObject box;
    private List<ItemData> remainingItems;
    private float boxTimer = 1f;
    //����ؼ� ������ �� ������ Ÿ�̸� ����

    public GameObject ESCUI;
    public bool isESC;

    public bool isGrounded;
    public float jumpTime;
    public Image fadeImage; // ���̵�� �̹���
    public float fadeDuration = 1f; // ���̵� �ð�
    public float holdDuration = 1f;

    private void Awake()
    {
        remainingItems = new List<ItemData>(boxData.spawnableItems);
    }

    void Start()
    {
        Time.timeScale = 1;
        playerCamera = playerCamera ? playerCamera : Camera.main; // ���� ī�޶� �������� �ʾҴٸ� ���� ī�޶� ���
        Cursor.lockState = CursorLockMode.Locked; // ���콺 Ŀ���� ȭ�� �߾ӿ� ����
        Cursor.visible = false; // ���콺 Ŀ�� ����
        isGrounded = true;
        jumpTime = 0f;
        if (fadeImage != null)
        {
            StartCoroutine(FadeIn());
        }
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
                Debug.Log("PickUp");
            }
            else
            {
                // ��ü�� ��� ������ ���� �õ�
                TryPlace();
                Debug.Log("PickDown");
            }
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit))
            {
                if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit))
                {
                    if (box == hit.collider.gameObject && remainingItems.Count > 0 && boxTimer <= 0 && heldObject == null)
                    {
                        int random = Random.Range(0, remainingItems.Count);
                        ItemData selectedItem = remainingItems[random];

                        Vector3 spawnPoint = box.transform.position + Vector3.up * (box.GetComponent<Collider>().bounds.size.y + 0.1f);

                        GameObject temp = Instantiate(selectedItem.prefab, spawnPoint, Quaternion.identity);
                        remainingItems.RemoveAt(random);

                        if (remainingItems.Count == 0)
                        {
                            Destroy(box);
                        }
                        boxTimer = boxData.spawnCooldown;

                    }
                }

            }
        }
        boxTimer -= Time.deltaTime;

        if(boxTimer <= 0)
        {
            boxTimer = 0;
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
                playerCamera.enabled = true;
                return;
            }
            if (isESC == false)
            {
                Time.timeScale = 0;
                isESC = true;
                ESCUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                playerCamera.enabled = false;
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true && jumpTime <= 0)
        {
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpTime = 1f;
            isGrounded = false;
            Debug.Log("Jump");
        }
        else if(isGrounded == false)
        {
            jumpTime -= Time.deltaTime;
            if (jumpTime < 0)
            {
                jumpTime = 0; 
                isGrounded = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit))
            {
                if (hit.collider.CompareTag("Picker"))
                {
                    hit.collider.gameObject.transform.Rotate(0, 90, 0);
                    Debug.Log("��ü ȸ��");
                }
            }
        }
    }

    void OnGUI()
    {
        // ȭ�� �߾� ��ǥ ���
        float centerX = Screen.width / 2;
        float centerY = Screen.height / 2;

        // ũ�ν���� ��ġ �� ũ�� ���
        Rect crosshairRect = new Rect(centerX - crosshairSize / 2, centerY - crosshairSize / 2, crosshairSize, crosshairSize);

        // ũ�ν���� �׸���
        GUI.color = crosshairColor;
        GUI.DrawTexture(crosshairRect, defaultCrosshair);
    }

    // ��ü�� ���� �õ��ϴ� �Լ�
    void TryPickup()
    {
        // ī�޶� �߽ɿ��� ���̸� ���� ��ü�� ã��
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // ī�޶� ȭ�� �߾ӿ��� ���� ����
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange, pickupLayer)) // ���̰� �浹�ϸ�
        {
            if (hit.collider.tag == "Picker" && hit.collider != null)
            {
                try
                {
                    PickupObject(hit.collider.gameObject); // ��ü ����
                }
                catch
                {
                    Debug.Log(hit.collider.name);
                }
               
            }
           
        }
    }

    // ��ü�� ������ ���� �Լ�
    void PickupObject(GameObject obj)
    {
        heldObject = obj; // ��� �ִ� ��ü ����
        originalScale = obj.transform.localScale; // ��ü�� ���� ũ�� ����

        if(obj.tag == "Picker")
        {
            ObjectItem temp = obj.GetComponent<ObjectItem>();
            if (temp.inGroup == true)
            {
                ObjectItemGroup itemgroup = temp.group;
                itemgroup.ObjectOut(temp);
            }
        }        

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
        obj.transform.localScale = originalScale * 1f; // ��ü ũ�⸦ ����Ͽ� �÷��̾ ��� �ִٴ� ������ ��
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
        // ǥ���� ������ Ȯ�� (���� ���)
        float surfaceAngle = Vector3.Dot(normal, Vector3.up);
        bool isWall = Mathf.Abs(surfaceAngle) < 0.5f; // 45�� �̻� ������ ǥ���� ������ ����

        heldObject.transform.SetParent(null);

        // ���� ����� ��ġ ����
        if (isWall)
        {
            // �� ǥ�鿡�� �ణ ������ ������
            float wallOffset = 0.05f; // ������ �󸶳� ����߸��� ����
            position += normal * wallOffset;

            // ��ü�� ���� �����ϰ� ȸ��
            Quaternion wallRotation = Quaternion.LookRotation(-normal);
            heldObject.transform.rotation = wallRotation;
        }
        else
        {
            // ���� �ٴ� ��ġ ���� ����
            heldObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, normal);
        }

        heldObject.transform.localScale = originalScale;

        // ���� ���� �ٴ��� ����� ��ġ ���� ���� �и�
        Vector3 adjustedPosition;
        if (isWall)
        {
            // ���� ���� ���� ��ü�� �߽��� �������� ��ġ ����
            adjustedPosition = position;

            // ��ü�� �ٴڿ� �굵�� Y�� ��ġ ����
            float bottomY = heldCollider != null ?
                position.y + heldCollider.bounds.extents.y :
                position.y;
            adjustedPosition.y = bottomY;
        }
        else
        {
            // ���� �ٴ� ��ġ ���� ����
            adjustedPosition = position - heldObject.transform.TransformVector(bottomOffset);
        }

        heldObject.transform.position = adjustedPosition;

        // Rigidbody ����
        if (heldRigidbody != null)
        {
            heldRigidbody.isKinematic = false;
            heldRigidbody.useGravity = true;
            heldRigidbody.velocity = Vector3.zero;
            heldRigidbody.angularVelocity = Vector3.zero;

            // ���� ���� ���� ��� ���� kinematic���� �����Ͽ� ����ȭ
            if (isWall)
            {
                StartCoroutine(StabilizeOnWall(heldRigidbody));
            }
        }

        if (heldCollider != null)
        {
            heldCollider.enabled = true;
        }

        heldObject = null;
        heldRigidbody = null;
        heldCollider = null;
    }

    private IEnumerator StabilizeOnWall(Rigidbody rb)
    {
        rb.isKinematic = true;
        yield return new WaitForSeconds(0.1f); // 0.1�� ���� ����ȭ
        if (rb != null) // ��ü�� ���� �����ϴ��� Ȯ��
        {
            rb.isKinematic = false;
        }
    }

    public IEnumerator FadeIn()
    {
        // ���� ȭ�� ����
        yield return new WaitForSeconds(holdDuration);

        float elapsedTime = 0f;
        Color color = fadeImage.color;
        color.a = 1f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;
    }
}
