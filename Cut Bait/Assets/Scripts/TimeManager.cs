using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class TimeManager : MonoBehaviour
{
    public int timePerShift;
    public float idleLimit;
    private float idleTimer;
    private Vector3 lastMousePosition;
    public GameObject pauseScreen;

    public FeedManager feedManager;
    public DataMiner dataMiner;


    private float currentTime;
    public TMP_Text timerText;
    public bool ticking, dayEnded;

    public int day;
    public GameObject notifPanel, pureBlackPanel, dayEndPanel1, dayEndPanel2,
        nButton1, nButton2, mButton, pButton;
    public Image pureBlackImage;

    public MoneyManager moneyManager;
    public MatchManager matchManager;

    public int savings, pEarn, hEarn, dEarn, 
        rExp, fExp, uExp, hExp, total,
        hWelfare, dWelfare, mWelfare;
    public bool fPaid, uPaid, hPaid;
    public TMP_Text notifType, conText, dayText1, dayText2, momText;
    public List<TMP_Text> dollarValues;
    public string upcomingOutput, momLetter;

    IEnumerator Type(GameObject toShowAfter, TMP_Text dest, string text, float typeSpeed)
    {
        dest.text = "";
        foreach (char letter in text)
        {
            dest.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }

        toShowAfter.SetActive(true);
    }
    IEnumerator Type(TMP_Text dest, string text, float typeSpeed)
    {
        dest.text = "";
        foreach (char letter in text)
        {
            dest.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }
    }


    // DAY TRANSITIONS -------------------------------------------------------------------------
    public void endDayExternal()
    {
        resetTime();
        StartCoroutine(endDay(false));
    } 


    public IEnumerator endDay(bool timedOut)
    {
            notifPanel.SetActive(true);
            if (timedOut)
            {
                notifType.text = "time's up";
            } else
            {
                notifType.text = "all emails processed";
            }

            

            pureBlackPanel.SetActive(true);

            float duration = 2f;
            float elapsed = 0f;
            yield return new WaitForSeconds(0.5f);

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;

                float alpha = Mathf.Clamp01(elapsed / duration);

                Color c = pureBlackImage.color;
                c.a = alpha;
                pureBlackImage.color = c;

                yield return null;
            }

            notifPanel.SetActive(false);
            dayEndPanel1.SetActive(true);
            dayText1.text = day.ToString();
            pEarn = moneyManager.money;
            determineContext();
    }

    public void determineContext()
    {
        upcomingOutput = "";

        switch (day)
        { 
            case 1:
            upcomingOutput += "You've finished your shift for the day.\nYou made $" + pEarn.ToString() + ".\n";
            upcomingOutput += "\nYour spouse works in IT for a large corporation.\nThey earned $" + hEarn.ToString() + " today.\n";
            upcomingOutput += "\nYour daughter serves as a laborer in a silicon mine.\nShe earned $" + dEarn.ToString() + " today.\n";
            upcomingOutput += "\nYour mother is in hospice. She cannot afford her medical bills alone.\n";
            upcomingOutput += "\nNow it's time to manage expenses. \nUse your family's earnings to pay bills on the following page. \nClick on the checkbox next to an expense to toggle its payment. \nUnpaid bills often lead to hardship.";

                dataMiner.day1CorrectDecisions = moneyManager.correct;
                dataMiner.day1IncorrectDecisions = moneyManager.incorrect;
                dataMiner.day1CorrectScans = matchManager.correct;
                dataMiner.day1IncorrectScans = matchManager.incorrect;

                StartCoroutine(Type(nButton1, conText, upcomingOutput, 0.01f));

            break;
        case 2:
            upcomingOutput += "You've finished your shift for the day.\nYou made $" + pEarn.ToString() + ".\n";
            if (hWelfare >= 6) {
                hEarn += 20;
                upcomingOutput += "\nYour spouse had an especially productive day!\nThey earned $" + hEarn.ToString() + " today.\n";
            } else if (hWelfare >= 5)
            {
                upcomingOutput += "\nYour spouse had a standard day at work.\nThey earned $" + hEarn.ToString() + " today.\n";
            } else
            {
                hEarn -= 40;
                upcomingOutput += "\nYour spouse is running tired, they fell behind schedule today.\nThey earned $" + hEarn.ToString() + " today.\n";
            }

            if (dWelfare >= 5)
            {
                upcomingOutput += "\nYour daughter put in work in the mines today.\nShe earned $" + dEarn.ToString() + " today.\n";
            } else {
                dEarn -= 35;
                upcomingOutput += "\nYour daughter has damaged some mining equipment. It will be replaced using a portion of her pay. \nShe earned $" + dEarn.ToString() + " today.\n";
            }

            if (mWelfare > 1)
            {
                upcomingOutput += "\nYour mother is recovering slowly but steadily.\n";
            } else
            {
                upcomingOutput += "\nYour mother is unresponsive. It's unsure whether she'll live.\n";
            }

            uExp += 10;
            upcomingOutput += "\nDue to increased use of electricity, your utilities bill has risen.\n";
            rExp += 20;
            upcomingOutput += "\nHousing is in high demand. Rent has gone up.";

            Debug.Log(hWelfare);
            Debug.Log(dWelfare);

                dataMiner.day2CorrectDecisions = moneyManager.correct;
                dataMiner.day2IncorrectDecisions = moneyManager.incorrect;
                dataMiner.day2CorrectScans = matchManager.correct;
                dataMiner.day2IncorrectScans = matchManager.incorrect;

                StartCoroutine(Type(nButton1, conText, upcomingOutput, 0.01f));

            break;
        case 3:
            break;
        case 4:
            break;
        } 
    }


    public void next()
    {
        dayEndPanel2.SetActive(true);

        if (dayText2.text != day.ToString())
        {
            dayText2.text = day.ToString();

            dollarValues[0].text = savings.ToString();
            dollarValues[1].text = pEarn.ToString();
            dollarValues[2].text = hEarn.ToString();
            dollarValues[3].text = dEarn.ToString();
            dollarValues[4].text = rExp.ToString();
            dollarValues[5].text = fExp.ToString();
            dollarValues[6].text = uExp.ToString();
            dollarValues[7].text = hExp.ToString();


            total = (savings + pEarn + hEarn + dEarn) - rExp;
            if (fPaid)
                total -= fExp;
            if (uPaid)
                total -= uExp;
            if (hPaid)
                total -= hExp;
            dollarValues[8].text = "$ " + total.ToString();

            mButton.SetActive(true);
            momText.text = "";
            switch (day)
            {
                case 1:
                    momLetter = "Sunshine,\n\n" +
                        "Congratulations on your new job as a phishing detector!\n\n" +
                        "I know it's not glamorous, but you're a huge help in keeping people safe as they work. " +
                        "Hell, I wish I'd have hired one before retirement. " +
                        "\n\nI can't tell you how many times I got in hot water for falling for a scam at work while stressed and overtired." +
                        "\n\nThanks again for helping take care of me on my way out, sweetie." +
                        "\n\nLove,\nMom";
                    break;

                case 2:
                    if(mWelfare > 1)
                    {
                        momLetter = "Sunshine,\n\n" +
                            "I hope you're coming to visit soon, I want to hear all the stories which come with your new job.\n\n" +
                            "I'm sure you've gotten the hang of phishing detection by now, you've always been a fast learner. \n\n" +
                            "If going gets tough, just slow down and take a breath. Think of it like boxing!\n\n" +
                            "Scammers don't want you to think things though, just like opponents in the ring want you out of breath and swinging wildly.\n\n" +
                            "Take care of yourself, sweetie!\n\n" +
                            "Love,\n" +
                            "Mom";
                    } else
                    {
                        momLetter = "\n\n\t[No mail today. You hope she's okay.]";
                    }
                        break;
                case 3:
                    break;
                case 4:
                    break;
            }

        } 
    }

    public void prev()
    {
        dayEndPanel2.SetActive(false);
        StartCoroutine(Type(nButton1, conText, upcomingOutput, 0.005f));
    }

    public void toggleChangedF(bool isChecked)
    {
        fPaid = isChecked;
        if (fPaid)
        {
            dollarValues[5].color = new Color(1f, 1f, 1f, 1f);
            total -= fExp;
            dollarValues[8].text = "$ " + total.ToString();

        } else
        {
            dollarValues[5].color = new Color(0.5f, 0.5f, 0.5f, 1f);
            total += fExp;
            dollarValues[8].text = "$ " + total.ToString();
        }
    }

    public void toggleChangedU(bool isChecked)
    {
        uPaid = isChecked;
        if (uPaid)
        {
            dollarValues[6].color = new Color(1f, 1f, 1f, 1f);
            total -= uExp;
            dollarValues[8].text = "$ " + total.ToString();

        }
        else
        {
            dollarValues[6].color = new Color(0.5f, 0.5f, 0.5f, 1f);
            total += uExp;
            dollarValues[8].text = "$ " + total.ToString();
        }
    }
    public void toggleChangedH(bool isChecked)
    {
        hPaid = isChecked;
        if (hPaid)
        {
            dollarValues[7].color = new Color(1f, 1f, 1f, 1f);
            total -= hExp;
            dollarValues[8].text = "$ " + total.ToString();

        }
        else
        {
            dollarValues[7].color = new Color(0.5f, 0.5f, 0.5f, 1f);
            total += hExp;
            dollarValues[8].text = "$ " + total.ToString();
        }
    }

    public void showMomLetter()
    {
        StartCoroutine(Type(momText, momLetter, 0.0125f));
        mButton.SetActive(false);
    }


    public void startNextDay()
    {
        savings += total;
        moneyManager.resetDayMoney();
        matchManager.resetDayScans();

        resetTime();
        if (!fPaid)
        {
            hWelfare--;
            dWelfare -= 2;
        }
        if (!uPaid)
        {
            dWelfare--;
            hWelfare -= 2;
        }
        if (!hPaid)
        {
            mWelfare--;
        }

        day++;

        dayEndPanel2.SetActive(false);
        dayEndPanel1.SetActive(false);

        StartCoroutine(fadeIn());

        //showGuidesToday();
        switch (day)
        {
            case 2:
                feedManager.day2Start();
                break;
            case 3:
                feedManager.day3Start();
                break;
            case 4:
                feedManager.day4Start();
                break;
        }
    }

    public IEnumerator fadeIn() {
        yield return new WaitForSeconds(0.35f);

        float duration = 3f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float alpha = 1f - Mathf.Clamp01(elapsed / duration);

            Color c = pureBlackImage.color;
            c.a = alpha;
            pureBlackImage.color = c;

            yield return null;
        }

        pureBlackPanel.SetActive(false);
    }

    // REGULAR -------------------------------------------------------------------------

    public void showGuidesToday()
    {
        MatchGuide[] allGuides = Object.FindObjectsByType<MatchGuide>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (MatchGuide guide in allGuides)
        {
            if (guide.dayShown == day)
            {
                guide.thisPanel.SetActive(true);
            }
        }
    }


    public void resetTime()
    {
        ticking = false;
        currentTime = timePerShift;
        DisplayTime(currentTime);
    }

    public void startTime()
    {
        ticking = true;
    }

    void Start()
    {
        resetTime();
        lastMousePosition = Input.mousePosition;
        day = 1;

        hEarn = 200;
        dEarn = 100;
        
        savings = 0;
        rExp = 150;
        fExp = 40;
        uExp = 30;
        hExp = 100;
        total = 0;

        hWelfare = 10;
        dWelfare = 10; 
        mWelfare = 2;
    }

    void Update()
    {
        CheckForIdle();
        dataMiner.playTime += Time.deltaTime;

        if (currentTime <= 0)
        {
            resetTime();
            StartCoroutine(endDay(true));

        } else if (ticking == true) {
            DisplayTime(currentTime);
            currentTime -= Time.deltaTime;
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void CheckForIdle()
    {
        bool inputDetected = false;
        if (Input.anyKey)
            inputDetected = true;

        if (Input.mousePosition != lastMousePosition)
        {
            inputDetected = true;
            lastMousePosition = Input.mousePosition;
        }
        if (Input.mouseScrollDelta.y != 0)
        {
            inputDetected = true;
        }

        if (inputDetected)
        {
            idleTimer = 0f;
        }
        else
        {
            idleTimer += Time.deltaTime;
        }


        if (idleTimer >= idleLimit)
        {
            pauseGame();
        }
    }

    void pauseGame()
    {
        Time.timeScale = 0f;
        pauseScreen.SetActive(true);
    }

    public void unPauseGame()
    {
        Time.timeScale = 1f;
        idleTimer = 0f;
    }
}
