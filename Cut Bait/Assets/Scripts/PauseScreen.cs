using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour, IPointerClickHandler
{
    public GameObject thisPanel;
    public TimeManager timeManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        timeManager.unPauseGame();
        thisPanel.SetActive(false);
    }
}
