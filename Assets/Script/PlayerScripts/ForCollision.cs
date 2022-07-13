using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ForCollision : MonoBehaviour {

    public Action OnDeath;
    public Action OnWin;
    public Action<GameObject> OnCoin;

    public float timeToWin;

    public GameObject winScreen;
    public GameObject deathMenu;
    public playerController movement;

    public int reward;
    private bool hasWon;
    private bool hasDied;

    [SerializeField]
    Vector3 theBottomPosition;
    [SerializeField]
    Transform groundCheckPosition;
    [SerializeField]
    float groundCheckRadius;
    [SerializeField]
    LayerMask whatIsGround;

    void FixedUpdate()
    {
        Collider[] groundResponse = Physics.OverlapSphere(groundCheckPosition.position, groundCheckRadius, whatIsGround);
        //Debug.LogFormat("Collider count is {0}", groundResponse.Length);
        if(groundResponse.Length == 0)
        {
            isOnGround = false;
            //Debug.Log("Off ground");
            movement.OffGround();
        }
        else
        {
            isOnGround = true;
            //Debug.Log("On ground");
            movement.OnGround();
        }

        if (transform.position.y <= theBottomPosition.y)
        {
            if (OnDeath != null)
            {
                OnDeath();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*if (collision.collider.CompareTag("Bad"))
        {
            if(OnDeath != null)
            {
                OnDeath();
            }
        }

        if (!hasDied)
        {
            if (collision.collider.CompareTag("Ground"))
            {
                movement.OnGround();
            }
        }*/
    }

    private void OnCollisionExit(Collision collision)
    {
        /*if (collision.collider.CompareTag("Ground"))
        {
            movement.OffGround();            
        }*/
    }    
    


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            if (OnWin != null)
            {
                OnWin();
            }
            /*hasWon = true;
            movement.enabled = false;
            Invoke("Win", timeToWin);*/

        }

        if (other.CompareTag("Bad"))
        {
            Debug.Log("Collided with obsticle");
            if (OnDeath != null)
            {
                OnDeath();
            }
        }

        if (other.CompareTag("Coin"))
        {
            if (OnCoin != null)
            {
                OnCoin(other.gameObject);
            }

            /*Destroy(other.gameObject);
            score += reward;
            setCount();*/
        }
    }
    /*
    public void setCount()
    {
        scoreText.text = score.ToString();
    }


    public void Win()
    {
        scoreText.text = " ";
        Time.timeScale = 0.4f;
        winScreen.SetActive(true);

    }


    public void Death()
    {
        deathMenu.SetActive(true);
        Time.timeScale = 0.5f;      
    }*/
    bool isOnGround;
    private void OnDrawGizmos()
    {
        if(isOnGround)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireSphere(groundCheckPosition.position, groundCheckRadius);
    }
}
