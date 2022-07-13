using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField]
    float speed;
    
    float high;
    [SerializeField]
    float low;
    [SerializeField]
    float timeToWait;
    [SerializeField]
    float startWaitingTime;
    
    enum TypesOfMovement
    {
        Horizontal, 
        Vertical, 
        Z_Movement
    }
    [SerializeField]
    TypesOfMovement movementOfThisObject;
    [SerializeField]
    LeanTweenType motionType;

    void Start()
    {
        switch (movementOfThisObject)
        {
            case TypesOfMovement.Horizontal:
                high = transform.position.x;
                break;
            case TypesOfMovement.Vertical:
                high = transform.position.y;
                break;
            case TypesOfMovement.Z_Movement:
                high = transform.position.z;
                break;
        }
        this.Invoke(delegate { Move(false); }, startWaitingTime);
    }
    
    void Move(bool up)
    {
        float endPoint;
        Action OnComplete;

        if (up)
        {
            endPoint = high;
        }
        else
        {
            endPoint = low;
        }

        OnComplete = delegate { this.Invoke(delegate { Move(!up); }, timeToWait); };
        
        switch (movementOfThisObject)
        {
            
            case TypesOfMovement.Horizontal:
                LeanTween.moveX(gameObject, endPoint, speed).setOnComplete(OnComplete).setEase(motionType);
                break;
            case TypesOfMovement.Vertical:
                LeanTween.moveY(gameObject, endPoint, speed).setOnComplete(OnComplete).setEase(motionType);
                break;
            case TypesOfMovement.Z_Movement:
                LeanTween.moveZ(gameObject, endPoint, speed).setOnComplete(OnComplete).setEase(motionType);
                break;
        }
    }

}
