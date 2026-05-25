using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MatchEmail : MonoBehaviour, IPointerClickHandler
{
    public List<string> emailFeatures;
    public MatchManager matchManager;
    public GameObject thisPanel;
    public Image thisPanelImage;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!matchManager.showingFeedback)
        {
            if (matchManager.emailFeaturesSelected == emailFeatures)
            {
                thisPanelImage.color = new Color(1f, 1f, 1f, 1f);
                matchManager.emailFeaturesSelected = null;
                matchManager.updateEmailPos(null);
                matchManager.stopMakeMatch();
            }
            else
            {
                MatchEmail[] allFeats = Object.FindObjectsByType<MatchEmail>(FindObjectsSortMode.None);

                foreach (MatchEmail feat in allFeats)
                {
                    feat.thisPanelImage.color = new Color(1f, 1f, 1f, 1f);
                }
                matchManager.emailFeaturesSelected = emailFeatures;
                thisPanelImage.color = new Color(1f, 0.9f, 0.4f, 1f);

                matchManager.startMakeMatch();
                matchManager.updateEmailPos(thisPanelImage);
            }
        }
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
