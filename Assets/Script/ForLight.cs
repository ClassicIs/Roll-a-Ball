using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForLight : MonoBehaviour {

    private void Update()
    {
        transform.Rotate(new Vector3(0.4f, 0.4f, 0) * Time.deltaTime);

    }
}
