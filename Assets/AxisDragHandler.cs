using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AxisDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] AtomController LeftAtom;
    [SerializeField] AtomController RightAtom;

    AtomController _atomBeingDragged = null;
    AtomController _otherAtom = null;

    float _minPos = 0;
    float _maxPos = 10;


    public event System.Action<float> DistanceChanged;


    public void OnBeginDrag(PointerEventData eventData)
    {
        _atomBeingDragged = eventData.rawPointerPress.GetComponent<AtomController>();

        _minPos = 0;
        _maxPos = 10;

        float minDistance = (RightAtom.transform.localScale.x + LeftAtom.transform.localScale.x) / 5;

        if(_atomBeingDragged == LeftAtom)
        {
            _otherAtom = RightAtom;
            _maxPos = RightAtom.transform.localPosition.x - minDistance;
        }
        else
        {
            _otherAtom = LeftAtom;
            _minPos = LeftAtom.transform.localPosition.x + minDistance;
        }

    }


    public void OnDrag(PointerEventData eventData)
    {
        var worldPointerPos = Camera.main.ScreenToWorldPoint(eventData.position);
        var localPointerPos = transform.InverseTransformPoint(worldPointerPos);

        var atomPos = _atomBeingDragged.transform.localPosition;
        atomPos.x = Mathf.Clamp(localPointerPos.x, _minPos, _maxPos);
        _atomBeingDragged.transform.localPosition = atomPos;

        var distance = CalculateDistance();

        DistanceChanged?.Invoke(distance);
    }


    public float CalculateDistance()
    {
        var worldDistance = Mathf.Abs(LeftAtom.transform.localPosition.x - RightAtom.transform.localPosition.x);

        // Scale the distance by the ratio of picometers to world units. (The diameter of a carbon atom)
        var distance = worldDistance * ElementHelper.GetVanDerWaalRadius(Element.Carbon) * 2;

        return distance;
    }
}
