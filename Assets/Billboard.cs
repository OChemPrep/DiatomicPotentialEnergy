using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {
    
    private void LateUpdate()
    {
        var camXform = Camera.main.transform;

        var toCamera = (transform.position - camXform.position);

        var rotation = Quaternion.LookRotation(toCamera, camXform.up);
        transform.rotation = rotation;
    }



}
