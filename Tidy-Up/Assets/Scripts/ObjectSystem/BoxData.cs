using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Box", menuName = "Box")]
public class BoxData : ScriptableObject
{
    public GameObject boxPrefab;
    public List<ItemData> spawnableItems = new List<ItemData>();
    public float spawnCooldown = 2f;
}