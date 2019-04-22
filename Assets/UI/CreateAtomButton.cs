using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreateAtomButton : MonoBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] Element _element;
    [SerializeField] ScrollRect _scrollRect;

    AtomController _atomBeingDragged;
    Vector3 _dragOffset;

    int _ignoreRaycastLayer;
    int _atomsLayer;
    int _atomsLayerMask;

    AtomController _potentialTargetAtom;
    AtomController PotentialTargetAtom
    {
        get { return _potentialTargetAtom; }

        set
        {
            if(_potentialTargetAtom != value)
            {
                if(_potentialTargetAtom != null)
                {
                    _potentialTargetAtom.IsSelected = false;
                }
                
                _potentialTargetAtom = value;

                if(_potentialTargetAtom != null)
                {
                    _potentialTargetAtom.IsSelected = true;
                }

            }
        }
    }


    Camera _uiCamera;

    float _atomWorldRadius = 0.5f;
    AtomController _atomPreview;

    bool _allowMultiMolecule = true;


    void Awake()
    {
        _atomsLayer = LayerMask.NameToLayer("Atoms");
        _atomsLayerMask = LayerMask.GetMask("Atoms");
        _ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");

        // Initialize the look of this control to be an atom of the specified element.
        _atomPreview = GetComponentInChildren<AtomController>();
        _atomPreview.Element = _element;

        // Size this control based on the size of the preview atom.
        var layoutElement = GetComponent<UnityEngine.UI.LayoutElement>();
        layoutElement.preferredHeight *= _atomPreview.transform.localScale.x;

        _uiCamera = transform.root.GetComponent<Canvas>().worldCamera;

        _atomWorldRadius = _atomPreview.transform.localScale.x / 2;

    }



    private void OnDestroy()
    {
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        return;

        if(_allowMultiMolecule && _atomBeingDragged == null)
        {
            // Create atom in front of camera.

            var createdAtomController = AtomFactory.CreateAtomView(_element);
            Vector3 screenCenter = new Vector3(0.5f, 0.5f, 6);
            Vector3 worldPos = Camera.main.ViewportToWorldPoint(screenCenter) + Random.insideUnitSphere * 3;
            createdAtomController.transform.position = Camera.main.transform.position + (-2 * Camera.main.transform.up);
            createdAtomController.TargetPosition = worldPos;
        }

    }


    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            _scrollRect.OnInitializePotentialDrag(eventData);

            // Save the vector from the center of the button to the mouse position.

            // The UI Camera renders in "Screen Space - Camera" mode.
            // The button position must be converted to screen coordinates before comparing it to the mouse position.
            _dragOffset = _uiCamera.WorldToScreenPoint(transform.position) - (Vector3)eventData.position;
            _dragOffset.z -= 5;
        }
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            Vector2 absDelta = eventData.position - eventData.pressPosition;
            absDelta.x = Mathf.Abs(absDelta.x);
            absDelta.y = Mathf.Abs(absDelta.y);

            if(absDelta.y > absDelta.x)
            {
                _scrollRect.OnBeginDrag(eventData);
                eventData.pointerDrag = _scrollRect.gameObject;
            }
            else if(absDelta.x > absDelta.y)
            {
                _atomBeingDragged = AtomFactory.CreateAtomView(_element);
                _atomBeingDragged.gameObject.layer = _ignoreRaycastLayer;
                _atomBeingDragged.transform.localPosition = GetWorldDragPos(eventData.position);

                Audio.PlayHighPop();

            }
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        UpdatePotentialTarget(eventData);

        _atomBeingDragged.TargetPosition = GetWorldDragPos(eventData.position);
    }


    private void UpdatePotentialTarget(PointerEventData eventData)
    {
        AtomController potentialTarget = null;

        if(Physics.SphereCast(Camera.main.ScreenPointToRay(eventData.position), 2, out RaycastHit hitInfo, 100, _atomsLayerMask))
        {
            potentialTarget = hitInfo.collider.GetComponent<AtomController>();
        }

        if(potentialTarget != PotentialTargetAtom)
        {
            PotentialTargetAtom = potentialTarget;
            Audio.PlayClick1();
        }
    }


    private Vector3 GetWorldDragPos(Vector2 screenPos)
    {
        Vector3 screenPos3D = (Vector3)screenPos + _dragOffset;

        Vector3 dragAtomPos = Camera.main.ScreenToWorldPoint(screenPos3D);

        if(PotentialTargetAtom != null && Camera.main.orthographic == false)
        {
            // Move the atom being dragged a little closer to the camera than the potential target atom.
            var camPos = Camera.main.transform.position;

            Vector3 camTargetDelta = camPos - PotentialTargetAtom.transform.position;

            float desiredDist = camTargetDelta.magnitude - _atomWorldRadius;

            Vector3 camDragDelta = camPos - dragAtomPos;

            dragAtomPos = camPos - camDragDelta.normalized * desiredDist;
        }

        Vector3 localPos = dragAtomPos;

        return localPos;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if(_potentialTargetAtom != null)
        {
            _potentialTargetAtom.Element = _atomBeingDragged.Element;
        }

        Destroy(_atomBeingDragged.gameObject);

        _atomBeingDragged = null;
        PotentialTargetAtom = null;
    }
}
