using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData data;
    private OutlineController outline;

    private void Awake()
    {
        outline = GetComponent<OutlineController>();
    }

    public void SetOutline(OutlineController.HighlightState state)
    {
        if (outline) outline.SetHighlightState(state);
    }
}