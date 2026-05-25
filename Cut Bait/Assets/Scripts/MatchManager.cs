/* Benjamin Lockard ~ Owen Schaffer ~ Honors Independent Research: Game Development ~ 3/23/2025 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MatchManager : MonoBehaviour
{
    public bool constructiveFeedback, showingFeedback;
    
    public MoneyManager moneyManager;
    public DisplayBar progressBar;
    
    private float confirmationScore;
    public string guideSelected;
    public List<string> emailFeaturesSelected;
    public List<string> emailFeaturesFound;
    public bool currentIsLegit;
    
    private LineRenderer lineRenderer;
    private Image emailPos, guidePos;

    public Coroutine makeMatchRoutine;
    public DisplayBar displayBar;

    public GameObject CFBPanel, SFBPanel;
    public TMP_Text CFBText, SFBText;

    private string CFB, SFB;
    public Image CFBPend, CFBConf;
    public int matchPoints;

    public List<MatchEmail> activeSegments;

    public FeedManager feedManager;

    public int correct;
    public int incorrect;

    public void resetDayScans()
    {
        correct = 0;
        incorrect = 0;
    }

    // Match Processing -----------------------------------------------------------

    public void startMakeMatch()
    {
        if (makeMatchRoutine != null)
        {
            StopCoroutine(makeMatchRoutine);
        }

        makeMatchRoutine = StartCoroutine(makeMatch());
    }

    public void stopMakeMatch()
    {
        progressBar.stopMatchLoad();

        if (makeMatchRoutine != null)
            StopCoroutine(makeMatchRoutine);
    }

    public IEnumerator makeMatch()
    {
        bool foundMatch = false;
        bool repeatMatch = false;
        if (guideSelected != null && emailFeaturesSelected != null)
        {
            progressBar.startMatchLoad();
            yield return new WaitForSeconds(1.8f);

            foreach (string feature in emailFeaturesFound)
            {
                if (feature == guideSelected)
                {
                    repeatMatch = true;
                }
            }

            if (!repeatMatch)
            {
                foreach (string feature in emailFeaturesSelected)
                {
                    if (feature == guideSelected)
                    {
                        foundMatch = true;
                        emailFeaturesFound.Add(feature);
                        if (guideSelected == "2C" || guideSelected == "2D" || guideSelected == "2E"
                            || guideSelected == "3C" || guideSelected == "3D" || guideSelected == "4C")
                        {
                            matchPoints++;
                        } else
                        {
                            matchPoints += 2;
                        }
                    }
                }
            }

            if (repeatMatch)
            {
                CFB += "risk already found\n >>> - <<< \n";
                CFBText.color = new Color(1f, 1f, 0.25f, 1f);
            }
            else if (foundMatch)
            {
                CFB += "risk identified\n >>> O <<< \n";
                CFBText.color = new Color(0.5f, 1f, 0.25f, 1f);
                StartCoroutine(moneyManager.updateMoney(5));
                correct++;
            }
            else
            {
                CFB += "no correlation\n >>> X <<< \n";
                CFBText.color = new Color(1f, 0.25f, 0.25f, 1f);
                StartCoroutine(moneyManager.updateMoney(-1));
                incorrect++;
            }

            CFBPend.enabled = false;
            CFBConf.enabled = false;

            if (matchPoints >= 2)
            {
                CFB += "scam confirmed";
                CFBConf.enabled = true;
            }
            else if (matchPoints >= 1)
            {
                CFB += "scam possible";
                CFBPend.enabled = true;
            }
            else
            {
                CFB += "no suspicion";
                CFBPend.enabled = true;
            }

            CFBPanel.SetActive(true);
            StartCoroutine(Type(CFBPanel, CFBText, CFB, 0.0075f));


            CFB = "";
            makeMatchRoutine = null;

        }
    }
    
    public void updateEmailPos (Image ePos)
    {
        emailPos = ePos;
    }
    public void updateGuidePos(Image gPos)
    {
        guidePos = gPos;
    }

    // Visual Prep --------------------------------------------------------

    IEnumerator Type(GameObject panel, TMP_Text dest, string text, float typeSpeed)
    {
        dest.text = "";
        foreach (char letter in text)
        {
            dest.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    // Submission & Feed Prep --------------------------------------------------------

    public void sendAndScore(bool sentLegit)
    {
        moneyManager.updateScore(sentLegit == currentIsLegit);
        matchPoints = 0;
        emailFeaturesFound.Clear();
        feedManager.clickMeIndicator.SetActive(true);
        feedManager.clickMeImage.color = new Color(1f, 0.9f, 0.4f, 1f);
        CFBPanel.SetActive(false);

        showingFeedback = true;

        emailFeaturesSelected = null;
        updateEmailPos(null);
        guideSelected = null;
        updateGuidePos(null);

        feedManager.dropCurrentFromList();

        if (constructiveFeedback)
        {

            MatchGuide[] allGuides = Object.FindObjectsByType<MatchGuide>(FindObjectsSortMode.None);

            foreach (MatchEmail segment in activeSegments)
            {
                if (segment.thisPanelImage.color != new Color(1f, 0.8f, 0.8f, 1f))
                    segment.thisPanelImage.color = new Color(1f, 1f, 1f, 1f);

                foreach (MatchGuide guide in allGuides)
                {
                    if (guide.thisPanelImage.color != new Color(1f, 0.8f, 0.8f, 1f))
                        guide.thisPanelImage.color = new Color(1f, 0.9568627f, 1f, 1f);

                    foreach (string feature in segment.emailFeatures)
                    {
                        if (feature == guide.guideFeature)
                        {
                            guide.emailImageForLine = segment.thisPanelImage;
                            segment.thisPanelImage.color = new Color(1f, 0.8f, 0.8f, 1f);
                            guide.thisPanelImage.color = new Color(1f, 0.8f, 0.8f, 1f);
                        }
                    }
                }
            }

            SFB = "";
            if (currentIsLegit)
            {
                SFB += "email was legitimate\n >>> ";
            }
            else
            {
                SFB += "email was a scam\n >>>  ";
            } 

            if (currentIsLegit == sentLegit)
            {
                SFB += "O  <<< \nrisk factors shown";
                SFBText.color = new Color(0.5f, 1f, 0.25f, 1f);
            }
            else
            {
                SFB += "X  <<< \nrisk factors shown";
                SFBText.color = new Color(1f, 0.25f, 0.25f, 1f);
            }

            SFBPanel.SetActive(true);
            StartCoroutine(Type(SFBPanel, SFBText, SFB, 0.0075f));

        } else
        {
            SFB = "";
            if (currentIsLegit)
            {
                SFB += "email was legitimate\n >>> ";
            }
            else
            {
                SFB += "email was a scam\n >>>  ";
            }

            if (currentIsLegit == sentLegit)
            {
                SFB += "O  <<< \nreturn to inbox";
                SFBText.color = new Color(0.5f, 1f, 0.25f, 1f);
            }
            else
            {
                SFB += "X  <<< \nreturn to inbox";
                SFBText.color = new Color(1f, 0.25f, 0.25f, 1f);
            }

            SFBPanel.SetActive(true);
            StartCoroutine(Type(SFBPanel, SFBText, SFB, 0.0075f));

            MatchEmail[] displayedSegments = Object.FindObjectsByType<MatchEmail>(FindObjectsSortMode.None);
            MatchGuide[] allGuides = Object.FindObjectsByType<MatchGuide>(FindObjectsSortMode.None);


            foreach (MatchEmail segment in displayedSegments)
                {
                    segment.thisPanelImage.color = new Color(1f, 1f, 1f, 1f);
                }
                foreach (MatchGuide guide in allGuides)
                {
                    guide.thisPanelImage.color = new Color(1f, 0.9568627f, 1f, 1f);
                    guide.emailImageForLine = null;
                }
        }
    }

    // ----------------------------------------------------------------------------------
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;

        guideSelected = null;
        emailFeaturesSelected = null;

        showingFeedback = false;

        CFB = "";
        SFB = "";
        matchPoints = 0;

        resetDayScans();
    }

    // Update is called once per frame
    void Update()
    {
        if (guidePos == null && emailPos == null)
        {
            lineRenderer.enabled = false;
        } else 
        {
            lineRenderer.enabled = true;
            if (guidePos != null && emailPos != null)
            {
                lineRenderer.SetPosition(0, emailPos.transform.position);
                lineRenderer.SetPosition(1, guidePos.transform.position);
            }
            else
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = 5.0f;

                if (guidePos != null)
                {
                    lineRenderer.SetPosition(0, mouseWorldPos);
                    lineRenderer.SetPosition(1, guidePos.transform.position);
                    //Debug.Log(guidePos.transform.position);
                }
                else if (emailPos != null)
                {
                    lineRenderer.SetPosition(0, emailPos.transform.position);
                    lineRenderer.SetPosition(1, mouseWorldPos);
                }
            }
        }





    }
}
