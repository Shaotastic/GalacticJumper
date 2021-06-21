using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Options_SfxSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Options.m_IsSelected = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Options.m_IsSelected = false;
    }
}
