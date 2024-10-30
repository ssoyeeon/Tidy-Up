using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class OutlineController : MonoBehaviour
{
    [Header("Outline Settings")]
    public Material outlineMaterial; // Inspector���� ������ �ƿ����� ���׸���

    private MeshRenderer meshRenderer;
    private Material[] originalMaterials;
    private Material[] materialsWithOutline;

    public enum HighlightState
    {
        None,       // �ƿ����� ����
        Correct,    // �ʷϻ� (�ùٸ� ��ġ)
        Incorrect   // ������ (�߸��� ��ġ)
    }

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        // ���� ���׸��� ����
        originalMaterials = meshRenderer.materials;

        // �ƿ����� ���׸����� ������ ���ο� ���׸��� �迭 ����
        materialsWithOutline = new Material[originalMaterials.Length + 1];
        for (int i = 0; i < originalMaterials.Length; i++)
        {
            materialsWithOutline[i] = originalMaterials[i];
        }

        // �ƿ����� ���׸��� �����Ͽ� �߰�
        materialsWithOutline[materialsWithOutline.Length - 1] = new Material(outlineMaterial);

        // �ʱ� ���´� �ƿ����� ����
        SetHighlightState(HighlightState.None);
    }

    private void OnDestroy()
    {
        // ������ ���׸��� ����
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