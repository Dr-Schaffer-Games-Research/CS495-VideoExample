using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MatchGuide : MonoBehaviour, IPointerClickHandler
{
    public string guideFeature;
    public MatchManager matchManager;
    public Image thisPanelImage;
    public GameObject thisPanel;

    public LineRenderer lineRenderer;
    public Image emailImageForLine;

    public int dayShown;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!matchManager.showingFeedback)
        {
            if (matchManager.guideSelected == guideFeature)
            {
                thisPanelImage.color = new Color(1f, 0.9568627f, 1f, 1f);
                matchManager.guideSelected = null;
                matchManager.updateGuidePos(null);
                matchManager.stopMakeMatch();
             }
            else
            {

                MatchGuide[] allGuides = Object.FindObjectsByType<MatchGuide>(FindObjectsSortMode.None);

                foreach (MatchGuide guide in allGuides)
                {
                    guide.thisPanelImage.color = new Color(1f, 0.9568627f, 1f, 1f);
                }

                 matchManager.guideSelected = guideFeature;
                thisPanelImage.color = new Color(1f, 0.9f, 0.4f, 1f);

                matchManager.startMakeMatch();
                matchManager.updateGuidePos(thisPanelImage);
            }
        }

        //normal: 255, 244, 255, 255


        //if (eventData.button == PointerEventData.InputButton.Right)
        //{
        //    Debug.Log("Right click on UI element!");
        //}
        //else if (eventData.button == PointerEventData.InputButton.Left)
        //{
        //    Debug.Log("Left click on UI element!");
        //}
    }



    // Start is called before the first frame update
    void Start()
    {
        emailImageForLine = null;
        thisPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (matchManager.showingFeedback && emailImageForLine != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, emailImageForLine.transform.position);
            lineRenderer.SetPosition(1, thisPanelImage.transform.position);
        } else
        {
            lineRenderer.enabled = false;
        }
    }
}
