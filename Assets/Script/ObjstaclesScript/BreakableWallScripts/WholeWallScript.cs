using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WholeWallScript : MonoBehaviour
{
    BoxCollider wallDamage;
    BoxCollider wallTrigger;
    public Action<playerController> OnPlayerHit;


    private void Awake()
    {
        wallDamage = GetComponent<BoxCollider>();
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

    public void SetWallSize(Vector3 wallSize, Vector3 centerOfWall)
    {
        transform.position = centerOfWall;
        //wallDamage.bounds.Expand(wallSize - wallDamage.bounds.size);
        transform.localScale = wallSize;
        wallTrigger = gameObject.AddComponent<BoxCollider>();

        float offset = 0.3f;
        wallTrigger.center = new Vector3(wallTrigger.center.x, wallTrigger.center.y, wallTrigger.center.z - offset / 2);
        wallTrigger.size = new Vector3(wallTrigger.size.x + offset / 3f, wallTrigger.size.y, wallTrigger.size.z + offset);
        wallTrigger.isTrigger = true;
    }

    public void WallOff()
    {
        wallDamage.enabled = false;
    }

}
