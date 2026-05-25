using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SubmitButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public FeedManager feedManager;
    public MatchManager matchManager;

    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public GameObject sliderObj;
    public bool isLegit;

    private Coroutine sendCoroutine;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!feedManager.inFeed && !matchManager.showingFeedback)
            sendCoroutine = StartCoroutine(HoldTimer());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (sendCoroutine != null)
        {
            StopCoroutine(sendCoroutine);
            sliderObj.SetActive(false);
        }
    }

    public IEnumerator HoldTimer()
    {
        sliderObj.SetActive(true);
        slider.value = 0;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        for (int i = 0; i < slider.maxValue; i++)
        {
            yield return new WaitForSeconds(0.015f);
            slider.value++;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }

        matchManager.sendAndScore(isLegit);
    }

    void Start()
     {
        fill.color = gradient.Evaluate(0f);
     }

    // Update is called once per frame
    void Update()
    {
        
    }
}
