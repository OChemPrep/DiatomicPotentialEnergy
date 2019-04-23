using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AxisDragHandler : MonoBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IDragHandler
{
    [SerializeField] AtomController LeftAtom;
    [SerializeField] AtomController RightAtom;

    AtomController _atomBeingDragged = null;
    AtomController _otherAtom = null;

    float _minPos = 0;
    float _maxPos = 10;


    public event System.Action<float> DistanceChanged;


    void OnAtomRefreshed()
    {

    }


    public void OnInitializePotentialDrag(PointerEventData eventData)
    {

    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        _atomBeingDragged = eventData.rawPointerPress.GetComponent<AtomController>();

        _minPos = 0;
        _maxPos = 10;

        if(_atomBeingDragged == LeftAtom)
        {
            _otherAtom = RightAtom;
            _maxPos = RightAtom.transform.localPosition.x - RightAtom.transform.localScale.x / 10f;
        }
        else
        {
            _otherAtom = LeftAtom;
            _minPos = LeftAtom.transform.localPosition.x + LeftAtom.transform.localScale.x / 10f;
        }

    }


    public void OnDrag(PointerEventData eventData)
    {
        var worldPointerPos = transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(eventData.position));

        var atomPos = _atomBeingDragged.transform.localPosition;
        atomPos.x = Mathf.Clamp(worldPointerPos.x, _minPos, _maxPos);
        _atomBeingDragged.transform.localPosition = atomPos;

        var worldDistance = Mathf.Abs(atomPos.x - _otherAtom.transform.localPosition.x);

        var distance = worldDistance * ElementHelper.GetRadius(Element.Carbon) * 2;

        DistanceChanged?.Invoke(distance);
    }
}
