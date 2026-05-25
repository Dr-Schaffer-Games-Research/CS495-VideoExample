using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class FeedEmail : MonoBehaviour, IPointerClickHandler
{
    public List<MatchEmail> emailSegments;
    public bool isLegitimate;
    public GameObject thisPanel;
    public FeedManager feedManager;
    public MatchManager matchManager;

    public string textForTitle;
    public TMP_Text titleText;

    public int storedMatchPoints;
    public List<string> storedFeaturesFound = null;

    public void OnPointerClick(PointerEventData eventData)
    {
        FeedEmail[] fullFeed = Object.FindObjectsByType<FeedEmail>(FindObjectsSortMode.None);

        foreach (FeedEmail email in fullFeed)
        {
            email.thisPanel.SetActive(false);
        }
        foreach (MatchEmail segment in emailSegments)
        {
            segment.thisPanel.SetActive(true);
        }

        feedManager.currentEmail = this;
        matchManager.matchPoints = storedMatchPoints;
        matchManager.emailFeaturesFound = storedFeaturesFound;

        matchManager.activeSegments = emailSegments;
        matchManager.currentIsLegit = isLegitimate;
        feedManager.inFeed = false;
        titleText.text = textForTitle;
    }

    void Start()
    {
        
    }
}
