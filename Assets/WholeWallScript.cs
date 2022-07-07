using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WholeWallScript : MonoBehaviour
{
    Collider wallDamage;
    public Action<playerController> OnPlayerHit;

    private void Start()
    {
        wallDamage = GetComponents<Collider>()[1];
    }

    private void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player entered!");
            if (OnPlayerHit != null)
            {
                OnPlayerHit(other.GetComponent<playerController>());
            }
        }
    }

    public void WallOff()
    {
        wallDamage.enabled = false;
    }

}
