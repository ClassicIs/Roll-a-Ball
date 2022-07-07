using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class MenuControllerScript : MonoBehaviour, IUIInterface
{
    [SerializeField]
    List<GameObject> buttons;
    [SerializeField]
    AnimationCurve curve;
    Image bgImage;
    Color bgColor;
    [SerializeField]
    RectTransform text;

    [SerializeField]
    float speedToAppear;
    [SerializeField]
    LeanTweenType typeOfEasing;

    void Awake()
    {
        bgImage = GetComponent<Image>();
        bgColor = bgImage.color;
        bgColor.a = 0f;
        bgImage.color = bgColor;        
        ButtonsOn(false);
        foreach (GameObject btn in buttons)
        {
            btn.GetComponent<RectTransform>().localScale = Vector3.zero;
        }
        if (text)
        {
            Color tmpColor = text.GetComponent<Text>().color;
            text.GetComponent<Text>().color = tmpColor;
        }
    }


    private void ButtonsOn(bool on)
    {
        foreach (GameObject btn in buttons)
        {
            btn.transform.gameObject.SetActive(on);
        }
        text.gameObject.SetActive(on);
        bgImage.gameObject.SetActive(on);
    }

    private void ButtonsOff()
    {
        foreach (GameObject btn in buttons)
        {
            btn.transform.gameObject.SetActive(false);
            //btn.GetComponent<RectTransform>().localScale = Vector3.zero;
        }
    }

    public void MenuOn(bool on, Action onCompletion = null)
    {
        
        Debug.Log("Menu on");
        Vector3 startVector;
        Vector3 endVector;

        Color endColor = bgColor;

        if (on)
        {
            if (text)
            {
                LeanTween.textAlpha(text, 1f, speedToAppear);
            }
            ButtonsOn(true);
            startVector = Vector3.zero;
            endVector = Vector3.one;
            endColor.a = 0.35f;

        }
        else
        {
            if (text)
            {
                LeanTween.textAlpha(text, 0f, speedToAppear);
            }

            startVector = Vector3.one;
            endVector = Vector3.zero;

            endColor.a = 0f;
        }


        LeanTween.value(bgImage.gameObject, SetColor, bgImage.color, endColor, speedToAppear);
        StartCoroutine(WaitForButtons(endVector, onCompletion, 0.4f, on));
    }

    void SetColor(Color c)
    {
        bgImage.color = c;
    }


    IEnumerator WaitForButtons(Vector3 endVector, Action onCompletion, float timeToWait, bool onOrOff)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (i == buttons.Count - 1)
            {
                Debug.LogFormat("Last button is {0}. It's scaled up.", i);
                var LeanTweanEx = LeanTween.scale(buttons[i], endVector, speedToAppear).setEase(curve).setOnComplete(onCompletion);
                if (!onOrOff) LeanTweanEx.setOnComplete(ButtonsOff);
                continue;
            }
            LeanTween.scale(buttons[i], endVector, speedToAppear).setEase(curve);
            yield return new WaitForSeconds(timeToWait);
        }
    }
}
