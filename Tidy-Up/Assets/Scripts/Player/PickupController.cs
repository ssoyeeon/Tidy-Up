using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    [Header("UI Settings")]
    private Color pickupCursorColor = Color.green;   // 물체를 집을 수 있을 때의 커서 색상  
    public Texture2D defaultCrosshair;          // 기본 크로스헤어 텍스처
    private Color crosshairColor = Color.white;  // 크로스헤어 색상
    private float crosshairSize = 25f;           // 크로스헤어 크기


    public float rotationSpeed = 100f;          // 물체 회전 속도
    public Vector3 rotationAxis = Vector3.up;   // 회전 축 (기본값: Y축)
    private Quaternion objectRotation;          // 물체의 현재 회전 상태

    public float pickupRange = 3f; // 플레이어가 물체를 집을 수 있는 최대 거리
    public LayerMask pickupLayer; // 물체를 집을 수 있는 레이어
    public LayerMask placementLayer; // 물체를 놓을 수 있는 레이어
    public Camera playerCamera; // 플레이어의 카메라
    public Vector3 heldObjectPosition = new Vector3(-0.5f, -0.3f, 0.5f); // 화면에 물체를 표시할 위치 (카메라 기준)
    public Rigidbody playerRigidbody;

    private GameObject heldObject; // 현재 플레이어가 들고 있는 물체
    private Rigidbody heldRigidbody; // 들고 있는 물체의 Rigidbody (물리 속성)
    private Collider heldCollider; // 들고 있는 물체의 Collider (충돌 처리)
    private Vector3 originalScale; // 물체의 원래 크기 저장
    private Vector3 bottomOffset; // 물체의 하단점을 기준으로 위치를 조정하기 위한 오프셋

    public List<GameObject> objectList = new List<GameObject>();        //박스 안에 생성할 오브젝트를 넣을 리스트
    public GameObject box;                                              //오브젝트를 생성할 박스
    private Vector3 boxPosition = new Vector3(-55.5f, -1f, -1.7f);              //생성할 오브젝트 위치 설정 -> 박스 포지션
    private float boxTimer = 1;                                              //계속해서 눌러도 안 나오게 타이머 설정

    public GameObject ESCUI;
    public bool isESC;

    public bool isGrounded;
    public float jumpTime;

    void Start()
    {
        Time.timeScale = 1;
        playerCamera = playerCamera ? playerCamera : Camera.main; // 만약 카메라가 설정되지 않았다면 메인 카메라를 사용
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서를 화면 중앙에 고정
        Cursor.visible = false; // 마우스 커서 숨김
        isGrounded = true;
        jumpTime = 0f;
    }

    void Update()
    {
        // 마우스 왼쪽 버튼을 눌렀을 때
        if (Input.GetMouseButtonDown(0))
        {
            // 물체를 들고 있지 않으면 집기 시도
            if (heldObject == null)
            {
                TryPickup();
                Debug.Log("PickUp");
            }
            else
            {
                // 물체를 들고 있으면 놓기 시도
                TryPlace();
                Debug.Log("PickDown");
            }
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit))
            {
                if (box == hit.collider.gameObject && objectList.Count > 0 && boxTimer <= 0 && heldObject == null)
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

                    if(objectList.Count == 0)
                    {
                        Destroy(box);
                    }
                    boxTimer = 2f;
                    Debug.Log("BoxHit and Instaniate Object");
                }
            }
        }
        boxTimer -= Time.deltaTime;

        if(boxTimer <= 0)
        {
            boxTimer = 0;
        }

        // 물체를 들고 있는 동안 계속 위치 업데이트
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
            playerRigidbody.AddForce(Vector3.up * 5, ForceMode.Impulse);
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
                    Debug.Log("물체 회전");
                }
            }
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
        GUI.DrawTexture(crosshairRect, defaultCrosshair);
    }

    // 물체를 집기 시도하는 함수
    void TryPickup()
    {
        // 카메라 중심에서 레이를 쏴서 물체를 찾음
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // 카메라 화면 중앙에서 레이 시작
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange, pickupLayer)) // 레이가 충돌하면
        {
            if (hit.collider.tag == "Picker" && hit.collider != null)
            {
                try
                {
                    PickupObject(hit.collider.gameObject); // 물체 집기
                }
                catch
                {
                    Debug.Log(hit.collider.name);
                }
               
            }
           
        }
    }

    // 물체를 실제로 집는 함수
    void PickupObject(GameObject obj)
    {
        heldObject = obj; // 들고 있는 물체 설정
        originalScale = obj.transform.localScale; // 물체의 원래 크기 저장

        if(obj.tag == "Picker")
        {
            ObjectItem temp = obj.GetComponent<ObjectItem>();
            if (temp.inGroup == true)
            {
                ObjectItemGroup itemgroup = temp.group;
                itemgroup.ObjectOut(temp);
            }
        }        

        // 물체에 Rigidbody가 있으면 물리적 상호작용 중지
        heldRigidbody = obj.GetComponent<Rigidbody>();
        if (heldRigidbody != null)
        {
            heldRigidbody.isKinematic = true; // 물체가 다른 물체와 충돌하거나 중력에 영향을 받지 않도록 설정
            heldRigidbody.useGravity = false; // 중력 비활성화
        }

        // 물체에 Collider가 있으면 비활성화하여 플레이어와 충돌하지 않도록 설정
        heldCollider = obj.GetComponent<Collider>();
        if (heldCollider != null)
        {
            heldCollider.enabled = false; // 물체의 충돌 처리 비활성화
            bottomOffset = CalculateBottomOffset(heldCollider); // 물체의 하단 오프셋 계산
        }

        obj.transform.SetParent(playerCamera.transform); // 물체를 플레이어 카메라의 자식으로 설정하여 플레이어와 함께 이동
        UpdateHeldObjectPosition(); // 물체를 화면에 보이도록 위치 업데이트
        obj.transform.localScale = originalScale * 1f; // 물체 크기를 축소하여 플레이어가 들고 있다는 느낌을 줌
    }

    // 물체의 하단점을 계산하여 물체를 정확히 놓기 위한 오프셋을 반환하는 함수
    Vector3 CalculateBottomOffset(Collider collider)
    {
        // 물체가 BoxCollider일 경우 하단점 계산
        if (collider is BoxCollider boxCollider)
        {
            return new Vector3(0, -boxCollider.size.y / 2, 0); // 박스의 하단
        }
        // 물체가 SphereCollider일 경우 하단점 계산
        else if (collider is SphereCollider sphereCollider)
        {
            return new Vector3(0, -sphereCollider.radius, 0); // 구의 하단
        }
        // 물체가 CapsuleCollider일 경우 하단점 계산
        else if (collider is CapsuleCollider capsuleCollider)
        {
            return new Vector3(0, -capsuleCollider.height / 2, 0); // 캡슐의 하단
        }
        // 다른 종류의 콜라이더에 대해서도 추가로 처리할 수 있음
        return Vector3.zero; // 콜라이더 타입이 다른 경우 기본값 반환
    }

    // 들고 있는 물체의 위치와 회전을 업데이트하는 함수
    void UpdateHeldObjectPosition()
    {
        heldObject.transform.localPosition = heldObjectPosition; // 물체를 설정된 화면 위치로 이동
        heldObject.transform.localRotation = Quaternion.identity; // 물체의 회전을 초기화하여 일정한 방향을 유지
    }

    // 물체를 놓는 시도를 하는 함수
    void TryPlace()
    {
        // 카메라 중앙에서 레이를 쏴서 물체를 놓을 수 있는 지점을 찾음
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // 카메라 화면 중앙에서 레이 시작
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange, placementLayer)) // 레이가 물체 놓기 가능한 곳에 맞으면
        {
            PlaceObject(hit.point, hit.normal); // 물체를 놓음
        }
    }

    // 물체를 실제로 놓는 함수
    void PlaceObject(Vector3 position, Vector3 normal)
    {
        heldObject.transform.SetParent(null); // 물체를 카메라의 자식에서 해제
        heldObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, normal); // 물체를 바닥에 맞게 회전
        heldObject.transform.localScale = originalScale; // 물체 크기를 원래 크기로 복구

        // 하단점을 기준으로 물체의 위치를 조정하여 정확히 바닥에 놓음
        Vector3 adjustedPosition = position - heldObject.transform.TransformVector(bottomOffset);
        heldObject.transform.position = adjustedPosition; // 위치 조정된 값으로 물체 배치

        // 물체에 Rigidbody가 있으면 물리적 상호작용을 다시 활성화
        if (heldRigidbody != null)
        {
            heldRigidbody.isKinematic = false; // 물리적 상호작용 활성화
            heldRigidbody.useGravity = true; // 중력 다시 적용
            heldRigidbody.velocity = Vector3.zero; // 물체의 이동 속도 초기화
            heldRigidbody.angularVelocity = Vector3.zero; // 물체의 회전 속도 초기화
        }

        // 물체의 Collider도 다시 활성화하여 충돌이 가능하게 함
        if (heldCollider != null)
        {
            heldCollider.enabled = true; // 물체의 충돌 처리 다시 활성화
        }

        // 들고 있던 물체 관련 변수 초기화
        heldObject = null;
        heldRigidbody = null;
        heldCollider = null;
    }
}
