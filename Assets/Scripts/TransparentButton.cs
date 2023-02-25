using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 
public class TransparentButton : MonoBehaviour, IPointerDownHandler 
{
    public void OnPointerDown(PointerEventData data) => Debug.LogFormat("Click detected at {0}.", data.position);
}
