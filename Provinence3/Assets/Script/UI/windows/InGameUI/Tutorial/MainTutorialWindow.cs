using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MainTutorialWindow : MonoBehaviour
{
    private TutorialLevel tutorialLevel;
    public Text MainMessage;

    public void Init(TutorialLevel tutorialLevel)
    {
        this.tutorialLevel = tutorialLevel;
        tutorialLevel.OnManecenKilled += OnManecenKilled;
        tutorialLevel.OnTutorialChange += OnTutorialChange;
    }

    private void OnTutorialChange(TutorialPart obj)
    {
        string ss = "";
        switch (obj)
        {
            case TutorialPart.start:
                break;
            case TutorialPart.move:
                ss = "Now go to the adventures! \n To Control use left-side Controller.";
                break;
            case TutorialPart.shoot:
                ss = "You must show how you can shot! \n To shoot just swipe on the right part of the screen. \n Shot both monster to go further.";
                break;
            case TutorialPart.aftershot:
            case TutorialPart.aftercast:
                ss = "Move to next obstacle";
                break;
            case TutorialPart.cast:
                ss = "Let's learn to cast some spells. \n Come closer to enemies and tap the button on the right part of the screen.";
                break;
            case TutorialPart.take:
                ss = "Good news for you. Chest with gold and some magic bonus!";
                break;
            case TutorialPart.boss:
                ss = "Show your skills and kill the boss!";
                break;
        }
        MainMessage.text = ss + tutorialLevel.StatusMessage();
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    private void OnManecenKilled()
    {
        
    }
}

