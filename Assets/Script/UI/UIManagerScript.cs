using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public enum TypesOfMenu
{
    pause,
    restart,
    win
}

public class UIManagerScript : MonoBehaviour
{
    [SerializeField]
    MenuControllerScript pauseMenu;
    [SerializeField]
    MenuControllerScript restartMenu;
    [SerializeField]
    MenuControllerScript winMenu;
    [SerializeField]
    Text scoreText;
    bool isChangingScore = false;
    [SerializeField]
    float speed;

    public IUIInterface ChooseMenu(TypesOfMenu type)
    {
        IUIInterface menuToReturn;
        switch(type)
        {
            case TypesOfMenu.pause:
                menuToReturn = pauseMenu;
                break;
            case TypesOfMenu.restart:
                menuToReturn = restartMenu;
                break;
            case TypesOfMenu.win:
                menuToReturn = winMenu;
                break;
            default:
                menuToReturn = pauseMenu;
                break;
        }
        return menuToReturn;
    }

    IUIInterface currentMenu = null;

    public void OpenMenu(TypesOfMenu type, Action beforeCompletion = null, Action afterCompletion = null)
    {
        currentMenu = ChooseMenu(type);
        currentMenu.MenuOn(beforeCompletion, afterCompletion);        
    }

    public void CloseMenu()
    {
        if (currentMenu == null)
        {
            Debug.Log("There's no menu.");
            return;
        }
        
        currentMenu.MenuOff();
    }

    public void ChangeScore(int newScore)
    {
        scoreText.text = newScore.ToString();

        if (isChangingScore)
        {
            return;
        }

        Vector3 oldVector = scoreText.GetComponent<RectTransform>().localScale;            
        Action nextAfterCompletion = delegate
        {
            isChangingScore = false;
        };
            
        Action afterCompletion = delegate
        {
            LeanTween.scale(scoreText.gameObject, oldVector, speed).setOnComplete(nextAfterCompletion);
            LeanTween.textColor(scoreText.rectTransform, Color.white, speed);
        };
        LeanTween.textColor(scoreText.rectTransform, Color.yellow, speed);
        LeanTween.scale(scoreText.gameObject, new Vector3(1.2f, 1.2f), speed).setOnComplete(afterCompletion);
    }

}
