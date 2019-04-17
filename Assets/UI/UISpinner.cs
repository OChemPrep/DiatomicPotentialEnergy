using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpinner : MonoBehaviour
{

    void Update()
    {
        transform.Rotate(0, 0, -0.5f * Time.deltaTime * Mathf.PI * Mathf.Rad2Deg);
    }
}
