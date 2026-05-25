using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class FeedManager : MonoBehaviour
{
    public MatchManager matchManager;
    public TimeManager timeManager;
    public DisplayBar displayBar;

    public List<GameObject> currentFeed;
    public List<GameObject> day2Feed;
    public List<GameObject> day3Feed;
    public List<GameObject> day4Feed;
    public List<GameObject> tutorials;

    public TMP_Text titleText;
    public bool inFeed;
    public bool dayStarted;

    public GameObject clickMeIndicator;
    public Image clickMeImage;

    public FeedEmail currentEmail;

    public void dropCurrentFromList()
    {
        currentFeed.Remove(currentEmail.thisPanel);
    }

    public void returnToFeed()
    {
        if (!inFeed)
        {
            if (!dayStarted)
            {
                timeManager.startTime();
            }

            clickMeIndicator.SetActive(false);
            clickMeImage.color = new Color(0.8392158f, 0.8392158f, 0.8392158f, 1f);

            MatchEmail[] displayedSegments = Object.FindObjectsByType<MatchEmail>(FindObjectsSortMode.None);
            MatchGuide[] allGuides = Object.FindObjectsByType<MatchGuide>(FindObjectsSortMode.None);

            foreach (MatchEmail segment in displayedSegments)
            {
                segment.thisPanel.SetActive(false);
                segment.thisPanelImage.color = new Color(1f, 1f, 1f, 1f);
            }
            foreach (MatchGuide guide in allGuides)
            {
                guide.thisPanelImage.color = new Color(1f, 0.9568627f, 1f, 1f);
                guide.emailImageForLine = null;
            }
            foreach (GameObject email in currentFeed)
            {
                email.SetActive(true);
            }

            inFeed = true;
            titleText.text = "Arnie's Inbox";

            Debug.Log(currentFeed.Count <= 0);
            if (currentFeed.Count <= 0)
                timeManager.endDayExternal();

            currentEmail.storedMatchPoints = matchManager.matchPoints;
            currentEmail.storedFeaturesFound = matchManager.emailFeaturesFound;

            matchManager.activeSegments = null;
            matchManager.showingFeedback = false;

            matchManager.CFBPanel.SetActive(false);
            matchManager.SFBPanel.SetActive(false);
            matchManager.emailFeaturesSelected = null;
            matchManager.updateEmailPos(null);
            matchManager.guideSelected = null;
            matchManager.updateGuidePos(null);
            displayBar.stopMatchLoad();
        }
    }

    public void day2Start() {
        returnToFeed();
        foreach (GameObject email in currentFeed)
        {
            email.SetActive(false);
        }
        foreach (GameObject email in day2Feed)
        {
            currentFeed.Add(email);
        }
        tutorials[0].SetActive(true);

    }
    public void day3Start()
    {
        returnToFeed();
        foreach (GameObject email in day3Feed)
        {
            currentFeed.Add(email);
        }
        foreach (GameObject email in currentFeed)
        {
            email.SetActive(false);
        }
        tutorials[1].SetActive(true);

    }
    public void day4Start()
    {
        returnToFeed();
        foreach (GameObject email in day4Feed)
        {
            currentFeed.Add(email);
        }
        foreach (GameObject email in currentFeed)
        {
            email.SetActive(false);
        }
        tutorials[2].SetActive(true);

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
