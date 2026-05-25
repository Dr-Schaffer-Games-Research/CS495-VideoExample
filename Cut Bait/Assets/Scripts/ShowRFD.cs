using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowRFD : MonoBehaviour, IPointerClickHandler
{
    public GameObject RFD;
    public TimeManager timeManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        timeManager.showGuidesToday();
    }
}
