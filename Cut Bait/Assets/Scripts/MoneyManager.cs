using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public int money, correct, incorrect;
    public TMP_Text moneyText, correctText, incorrectText, updateText;

    public IEnumerator updateMoney(int amount)
    {
        money += amount;
        moneyText.text = money.ToString();

        if (amount > 0)
        {
            updateText.text = "+" + amount.ToString();
            updateText.color = new Color(0.2627451f, 0.627451f, 0.2784314f, 1f);
        } else
        {
            updateText.text = amount.ToString();
            updateText.color = new Color(1f, 0f, 0f, 1f);
        }

        for (int i = 0; i < 50; i++)
        {
            updateText.GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, -1f);
            yield return new WaitForSeconds(0.0125f);
        }
        updateText.color = new Color(0f, 0f, 0f, 0f);
        updateText.GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, 50f);
    }

    public void updateScore(bool correctDecision)
    {
        if (correctDecision)
        {
            correct++;
            correctText.text = correct.ToString();
            StartCoroutine(updateMoney(20));
        } else
        {
            incorrect++;
            incorrectText.text = incorrect.ToString();
            StartCoroutine(updateMoney(-15));
        }
    }

    public void resetDayMoney()
    {
        money = 0;
        correct = 0;
        incorrect = 0;
    }

    void Start()
    {
        resetDayMoney();

        moneyText.text = "0";
        incorrectText.text = "0";
        correctText.text = "0";
    }
}
