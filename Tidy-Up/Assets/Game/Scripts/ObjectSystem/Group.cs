using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{
    public GroupData data;
    public bool isComplete;
    private HashSet<int> currentItems = new HashSet<int>();
    private bool firstItemPlaced = false;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            isComplete = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<Item>();
        if (!item || !item.data) return;

        if (IsValidItem(item.data))
        {
            currentItems.Add(item.data.id);
            item.SetOutline(OutlineController.HighlightState.Correct);

            if(!firstItemPlaced)
            {
                firstItemPlaced = true;
                AchievementManager.Instance.OnItemCorrectlyPlaced();
            }
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
        bool wasComplete = isComplete;
        isComplete = currentItems.Count == data.items.Length;

        if(!wasComplete && isComplete)      //퀘스트 반복 금지용 
        {
            AchievementManager.Instance.OnGroupCompleted();
        }
    }
}