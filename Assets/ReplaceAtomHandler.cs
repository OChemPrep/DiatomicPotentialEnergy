using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ReplaceAtomHandler : MonoBehaviour, IDragHandler, IEndDragHandler
{
    AtomController _atomBeingDragged;
    AtomController _potentialTargetAtom;


    public void OnDrag(PointerEventData eventData)
    {
        // Move atom being dragged.
        // Find atoms in range for replacement
        // highlight potential target atom, if any.
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if(_potentialTargetAtom != null)
        {
            _potentialTargetAtom.Element = _atomBeingDragged.Element;
        }

        Destroy(_atomBeingDragged.gameObject);
    }
}
