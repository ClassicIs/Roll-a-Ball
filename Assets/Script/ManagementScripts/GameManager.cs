using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour {

    private bool gameHasEnded = false;

    public float waitTime;

    UIManagerScript UIManagerScript;
    ForCollision playerColission;
    playerController playerController;

    [SerializeField]
    float slowTimeScale;
    [SerializeField]
    int coinValue;

    int currentScrore = 0;
    

    GameManager instance;

    private void Awake()
    {
        
        coinValue = 10;
        slowTimeScale = 0.4f;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        AssignValues();
    }

    void AssignValues()
    {
        Time.timeScale = 1f;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerColission = player.GetComponent<ForCollision>();
        playerColission.OnCoin += IfCoinsChange;
        playerColission.OnDeath += IfDied;
        playerColission.OnWin += IfEndLevel;

        playerController = player.GetComponent<playerController>();
        playerController.OnEscape += IfPressedEscape;
        UIManagerScript = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManagerScript>();
    }


    public void IfDied()
    {
        if (gameHasEnded == false)
        {
            playerController.IsInStun();
            gameHasEnded = true;           
            this.Invoke(Respawn, waitTime);
        }
    }   

    public void Respawn()
    {
        MenuChooserOn(TypesOfMenu.restart);
        /*IUIInterface obj = UIManagerScript.ChooseMenu(TypesOfMenu.restart);
        
        obj.MenuOn(delegate {
            Debug.Log("Respawn");            
            Time.timeScale = slowTimeScale; });*/
    }

    private void IfEndLevel()
    {
        MenuChooserOn(TypesOfMenu.win);

        /*IUIInterface obj = UIManagerScript.ChooseMenu(TypesOfMenu.win);
        playerController.IsInStun();
        obj.MenuOn(true, delegate {
            Debug.Log("End Level");            
            Time.timeScale = slowTimeScale; });*/
    }
    
    private void IfCoinsChange(GameObject coin)
    {
        Destroy(coin);
        
        currentScrore += coinValue;
        UIManagerScript.ChangeScore(currentScrore);
    }

    private void IfPressedEscape()
    {
        MenuChooserOn(TypesOfMenu.pause);
        /*MenuChooserOn(TypesOfMenu.pause, delegate {
            Debug.Log("Paused");
            Time.timeScale = slowTimeScale;
        });*/
    }

    private void MenuChooserOn(TypesOfMenu type)
    {
        UIManagerScript.OpenMenu(type, delegate {
            Debug.Log(type.ToString());
            GameStateManager.Instance.ChangeState(GameStates.Paused);
            //Time.timeScale = slowTimeScale;
        }, delegate {
            GameStateManager.Instance.ChangeState(GameStates.Gameplay);
            //Time.timeScale = 1f;
        });

        /*IUIInterface obj = UIManagerScript.ChooseMenu(type);
        playerController.IsInStun();
        obj.MenuOn(true, OnComplete);*/
    }    
}

public static class Utility
{
    public static void Invoke(this MonoBehaviour mb, Action actionToInvoke, float delay)
    {
        mb.StartCoroutine(InvokeCoroutine(actionToInvoke, delay));
    }

    private static IEnumerator InvokeCoroutine(Action actionToInvoke, float delay)
    {
        yield return new WaitForSeconds(delay);
        actionToInvoke();
    }
}
