using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Rotate : MonoBehaviour {

    Vector3 startPos;
    Vector3 endPos;
    [SerializeField]
    float maxY = 3;
    [SerializeField]
    float speed = 10;
    [SerializeField]
    AnimationCurve curve;

    private void Start()
    {        
        startPos = transform.localPosition;        
        endPos = new Vector3(startPos.x, startPos.y + maxY);        
        LeanTween.moveLocalY(gameObject, endPos.y, speed).setLoopPingPong().setEase(curve);        
    }
    private void Update()
    {
        transform.Rotate(new Vector3 (15,35,28) * Time.deltaTime);       
    }    
}
