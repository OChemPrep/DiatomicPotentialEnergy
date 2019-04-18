using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour {

    public float SpinSpeed = 60.0f;

    private void Update()
    {
        transform.Rotate(-Vector3.forward, SpinSpeed * Time.deltaTime);

    }



}
