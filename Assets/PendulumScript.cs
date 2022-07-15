using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PendulumScript : MonoBehaviour 
{
    [SerializeField]
    [Range(-90, 90)]
    float maxAngle;
    float startAngle;
    [SerializeField]
    [Range(0.1f, 100)]
    float speedOfRotation;

    [SerializeField]
    LeanTweenType typeOfMotion;
    [SerializeField]
    AnimationCurve curveOfMotion;

    [SerializeField]
    [Range(0, 100)]
    float timeOnPause;

    enum Directions
    {
        Horizontal,
        Vertical,
        Diagonaly
    }
    [SerializeField]
    Directions directionOfPendulum;

    GameObject pendulumBody;
    delegate void RotationDelegate(GameObject obj, float angle, float speedOfRotation, Action afterEvent);

    private event RotationDelegate eventToRotate;

    void Start()
    {
        
        pendulumBody = gameObject;
        switch(directionOfPendulum)
        {
            case Directions.Horizontal:
                startAngle = transform.position.z;
                eventToRotate = LeanZ;
                break;
            case Directions.Vertical:
                startAngle = transform.position.x;
                eventToRotate = LeanX;
                break;
            case Directions.Diagonaly:
                startAngle = transform.position.y;
                eventToRotate = LeanY;
                break;
        }
        LeanTowards();
    }   

    private void LeanZ(GameObject obj, float angle, float speedOfRotation, Action afterEvent)
    {
        if (typeOfMotion != LeanTweenType.animationCurve)
        {
            LeanTween.rotateZ(obj, angle, speedOfRotation).setOnComplete(afterEvent).setEase(typeOfMotion);
        }
        else
        {
            if (curveOfMotion == null)
            {
                return;
            }
            LeanTween.rotateZ(obj, angle, speedOfRotation).setOnComplete(afterEvent).setEase(curveOfMotion);
        }
    }

    private void LeanX(GameObject obj, float angle, float speedOfRotation, Action afterEvent)
    {
        if (typeOfMotion != LeanTweenType.animationCurve)
        {
            LeanTween.rotateX(obj, angle, speedOfRotation).setOnComplete(afterEvent).setEase(typeOfMotion);
        }
        else
        {
            if (curveOfMotion == null)
            {
                return;
            }
            LeanTween.rotateX(obj, angle, speedOfRotation).setOnComplete(afterEvent).setEase(curveOfMotion);
        }
    }

    private void LeanY(GameObject obj, float angle, float speedOfRotation, Action afterEvent)
    {
        if (typeOfMotion != LeanTweenType.animationCurve)
        {
            LeanTween.rotateY(obj, angle, speedOfRotation).setOnComplete(afterEvent).setEase(typeOfMotion);
        }
        else
        {
            if(curveOfMotion == null)
            {
                return;
            }
            LeanTween.rotateY(obj, angle, speedOfRotation).setOnComplete(afterEvent).setEase(curveOfMotion);
        }
    }

    private void LeanBackwards()
    {
        if(eventToRotate != null)
        {
            eventToRotate(pendulumBody, -maxAngle, speedOfRotation, delegate { 
                this.Invoke(delegate {
                    //Debug.Log("Leaning towards");
                    LeanTowards(); 
                }, timeOnPause); 
            });
        }
    }

    private void LeanTowards()
    {
        if (eventToRotate != null)
        {
            eventToRotate(pendulumBody, maxAngle, speedOfRotation, delegate
            {
                this.Invoke(delegate
                {
                    //eventToRotate(pendulumBody, startAngle, speedOfRotation / 2, )
                    //Debug.Log("Leaning backwards");
                    LeanBackwards();
                }, timeOnPause);
            });
        }
    }
}
