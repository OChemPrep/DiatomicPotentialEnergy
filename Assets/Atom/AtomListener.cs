using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AtomListener : MonoBehaviour
{
    [SerializeField]
    private AtomController _controller;

    public AtomController Controller
    {
        get
        {
            return _controller;
        }

        set
        {
            if(_controller != value)
            {
                if(_controller != null)
                {
                    _controller.Refreshed -= OnControllerRefreshed;
                }

                _controller = value;

                if(_controller != null)
                {
                    _controller.Refreshed += OnControllerRefreshed;
                }

                OnControllerRefreshed();

            }
        }
    }


    protected abstract void OnControllerRefreshed();


    protected virtual void Awake()
    {
        if(_controller != null)
        {
            _controller.Refreshed += OnControllerRefreshed;
        }

        OnControllerRefreshed();
    }


    void OnDestroy()
    {
        if(_controller != null)
        {
            _controller.Refreshed -= OnControllerRefreshed;
        }
    }
}
