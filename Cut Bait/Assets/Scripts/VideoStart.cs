using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VideoStart : MonoBehaviour, IPointerClickHandler
{
    public GameObject thisPanel;
    public VideoManager videoManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        thisPanel.SetActive(false);
        videoManager.PlayVideo();
    }
}
