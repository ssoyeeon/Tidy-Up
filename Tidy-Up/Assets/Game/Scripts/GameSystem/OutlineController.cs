using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class OutlineController : MonoBehaviour
{
    [Header("Outline Settings")]
    public Material outlineMaterial; // Inspector에서 설정할 아웃라인 머테리얼

    private MeshRenderer meshRenderer;
    private Material[] originalMaterials;
    private Material[] materialsWithOutline;

    public enum HighlightState
    {
        None,       // 아웃라인 없음
        Correct,    // 초록색 (올바른 위치)
        Incorrect   // 빨간색 (잘못된 위치)
    }

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        // 기존 머테리얼 저장
        originalMaterials = meshRenderer.materials;

        // 아웃라인 머테리얼을 포함한 새로운 머테리얼 배열 생성
        materialsWithOutline = new Material[originalMaterials.Length + 1];
        for (int i = 0; i < originalMaterials.Length; i++)
        {
            materialsWithOutline[i] = originalMaterials[i];
        }

        // 아웃라인 머테리얼 복제하여 추가
        materialsWithOutline[materialsWithOutline.Length - 1] = new Material(outlineMaterial);

        // 초기 상태는 아웃라인 없음
        SetHighlightState(HighlightState.None);
    }

    private void OnDestroy()
    {
        // 생성한 머테리얼 정리
        if (materialsWithOutline != null && materialsWithOutline[materialsWithOutline.Length - 1] != null)
        {
            Destroy(materialsWithOutline[materialsWithOutline.Length - 1]);
        }
    }

    public void SetHighlightState(HighlightState state)
    {
        if (materialsWithOutline == null || materialsWithOutline.Length == 0) return;

        Material outlineMat = materialsWithOutline[materialsWithOutline.Length - 1];

        switch (state)
        {
            case HighlightState.Correct:
                meshRenderer.materials = originalMaterials;
                break;

            case HighlightState.Incorrect:
                outlineMat.SetColor("_OutlineColor", new Color(1, 0, 0, 0.02f)); 
                outlineMat.SetFloat("_OutlineWidth", 10f);
                meshRenderer.materials = materialsWithOutline;
                break;

            case HighlightState.None:
                meshRenderer.materials = originalMaterials;
                break;
        }
    }
}