using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickToClose : MonoBehaviour, IPointerClickHandler
{
    public GameObject thisPanel;

    public void OnPointerClick(PointerEventData eventData)
    {
        thisPanel.SetActive(false);
    }
}
