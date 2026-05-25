using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TMP_Text textbox;
    private Coroutine matchLoadRoutine;

    public void startMatchLoad()
    {
        if (matchLoadRoutine != null)
        {
            StopCoroutine(matchLoadRoutine);
        }

        matchLoadRoutine = StartCoroutine(matchLoad());
    }

    public void stopMatchLoad()
    {
        StopCoroutine(matchLoadRoutine);
        slider.value = 0;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        textbox.text = "Awaiting Comparison...";
    }

    public IEnumerator matchLoad()
    {
        slider.value = 0;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        textbox.text = "Comparing...";
        for (int i = 0; i < slider.maxValue; i++) {
            yield return new WaitForSeconds(0.02f);
            slider.value++;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }

        textbox.text = "Comparison Complete!";
        yield return new WaitForSeconds(1.5f);
        fill.color = gradient.Evaluate(slider.normalizedValue);
        textbox.text = "Awaiting Comparison...";

        for (int i = 0; i < (slider.maxValue); i++)
        {
            yield return new WaitForSeconds(0.005f);
            slider.value--;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }

        matchLoadRoutine = null;
    }

    void Start()
    {
        fill.color = gradient.Evaluate(0f);
        textbox.text = "Awaiting Comparison...";
    }
}
