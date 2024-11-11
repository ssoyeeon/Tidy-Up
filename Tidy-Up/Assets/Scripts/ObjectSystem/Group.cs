using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{
    public GroupData data;
    public bool isComplete;
    private HashSet<int> currentItems = new HashSet<int>();

    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<Item>();
        if (!item || !item.data) return;

        if (IsValidItem(item.data))
        {
            currentItems.Add(item.data.id);
            item.SetOutline(OutlineController.HighlightState.Correct);
        }
        else
        {
            item.SetOutline(OutlineController.HighlightState.Incorrect);
        }

        CheckComplete();
    }

    private void OnTriggerExit(Collider other)
    {
        var item = other.GetComponent<Item>();
        if (!item || !item.data) return;

        currentItems.Remove(item.data.id);
        item.SetOutline(OutlineController.HighlightState.None);
        CheckComplete();
    }

    private bool IsValidItem(ItemData itemData)
    {
        return System.Array.Exists(data.items, x => x.id == itemData.id);
    }

    private void CheckComplete()
    {
        isComplete = currentItems.Count == data.items.Length;
    }
}