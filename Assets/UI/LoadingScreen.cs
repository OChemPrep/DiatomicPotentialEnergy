using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    static LoadingScreen _instance;

    static LoadingScreen Instance
    {
        get
        {
            return _instance;
        }

        set
        {
            _instance = value;
        }
    }

    GameObject _clickBlocker;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;

            DontDestroyOnLoad(gameObject);

            _clickBlocker = transform.GetChild(0).gameObject;

            Close();
        }

        if(Instance != null && Instance != this)
        {
            //Debug.LogWarning("Destroying redundant loading screen.");
            Destroy(this);
        }
    }


    public static void Open()
    {
        //Debug.Log("Opening loading menu.");
        Instance?._clickBlocker.SetActive(true);
    }


    public static void Close()
    {
        //Debug.Log("Closing loading menu.");
        Instance?._clickBlocker.SetActive(false);
    }

}
