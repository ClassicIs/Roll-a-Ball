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

    public void OpenMenu(TypesOfMenu type, Action actionOnCompletion = null)
    {
        currentMenu = ChooseMenu(type);
        currentMenu.MenuOn(true, actionOnCompletion);        
    }

    public void CloseMenu()
    {
        CloseMenu(null);
    }

    public void CloseMenu(Action actionOnCompletion = null)
    {
        if (currentMenu != null)
        {
            currentMenu.MenuOn(false, actionOnCompletion);
        } 
    }

    public void ChangeScore(int newScore)
    {
        float speed = 1f;
        if (!isChangingScore)
        {
            Vector3 oldVector = scoreText.GetComponent<RectTransform>().localScale;
            
            Action nextAfterCompletion = delegate
            {
                isChangingScore = false;
            };
            
            Action afterCompletion = delegate
            {
                LeanTween.scale(scoreText.gameObject, oldVector, speed).setOnComplete(nextAfterCompletion);
            };

            LeanTween.scale(scoreText.gameObject, new Vector3(1.2f, 1.2f), speed).setOnComplete(afterCompletion);            
        }
        scoreText.text = newScore.ToString();
    }

}
