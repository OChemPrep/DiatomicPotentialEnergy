using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AtomController : MonoBehaviour
{
    public event Action Refreshed;
    public event Action<AtomController> OnEnabled;
    public event Action<AtomController> OnDisabled;

    public Renderer AtomRenderer;

    public static float IsMovingTolerance = 0.005f; // local units
    public static float MoveDuration = 0.2f;
    Vector3 _currentVelocity;


    public bool IsMoving
    {
        get
        {
            return _moveCoroutine != null;
        }
    }

    Coroutine _moveCoroutine;

    private Vector3? _targetPosition;
    public Vector3 TargetPosition
    {
        get { return _targetPosition.GetValueOrDefault(); }
        set
        {
            if(_targetPosition != value)
            {
                //Debug.Log("Setting target position: " + _targetPosition);
                _targetPosition = value;

                StartMoveToPosition();
            }
        }
    }

    [SerializeField] private Element _element;
    public Element Element { get => _element; set { _element = value; Refresh(); } }

    [SerializeField] AtomSymbolDisplay SymbolDisplay;

    public void SetShowSymbol(bool show)
    {
        SymbolDisplay.gameObject.SetActive(show);
    }


    private void OnEnable()
    {
        OnEnabled?.Invoke(this);
        StartMoveToPosition();
    }


    private void OnDisable()
    {
        OnDisabled?.Invoke(this);
    }


    public void Refresh()
    {
        // Set atom size relative to the size of a carbon atom.
        transform.localScale = Vector3.one * ElementHelper.GetRadius(Element) / ElementHelper.GetRadius(Element.Carbon);

        AtomRenderer.material.color = ElementHelper.GetColor(Element);

        Refreshed?.Invoke();
    }


    void StartMoveToPosition()
    {
        if(_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);

        _moveCoroutine = StartCoroutine(MoveToPosition());
    }


    private IEnumerator MoveToPosition()
    {
        while(Vector3.Distance(transform.localPosition, TargetPosition) > AtomController.IsMovingTolerance)
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, TargetPosition, ref _currentVelocity, MoveDuration);

            //float dist = Vector3.Distance(transform.localPosition, _targetPosition);
            //Debug.Log(transform.localPosition + " -> " + _targetPosition + " = " + dist);
            yield return null;
        }

        transform.localPosition = TargetPosition;

        _moveCoroutine = null;

    }
}
