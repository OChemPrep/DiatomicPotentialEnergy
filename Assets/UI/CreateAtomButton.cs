using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreateAtomButton : MonoBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Element Element;

    [SerializeField] ScrollRect _scrollRect;
    enum State { Idle, DraggingObject, Scrolling, };
    State _state = State.Idle;

    AtomController _atomBeingDragged;

    public bool Dragging => _atomBeingDragged != null;

    Vector3 _dragOffset;

    int _defaultLayer;
    int _atomsLayer;

    AtomController _potentialTargetAtom;
    AtomController PotentialTargetAtom
    {
        get { return _potentialTargetAtom; }

        set
        {
            if(_potentialTargetAtom != value)
            {

                _potentialTargetAtom = value;
                
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
        _defaultLayer = LayerMask.NameToLayer("Default");

        // Initialize the look of this control to be an atom of the specified element.
        _atomPreview = GetComponentInChildren<AtomController>();
        _atomPreview.Element = Element;
        _atomPreview.Refresh();

        // Size this control based on the size of the preview atom.
        var layoutElement = GetComponent<UnityEngine.UI.LayoutElement>();
        layoutElement.preferredHeight *= _atomPreview.transform.localScale.x;

        _uiCamera = transform.root.GetComponent<Canvas>().worldCamera;

        _atomWorldRadius = (_atomPreview.transform.localScale).x / 2;
        
    }
    


    private void OnDestroy()
    {
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if(_allowMultiMolecule && _state == State.Idle)
        {
            // Create atom in front of camera.

            var createdAtomController = new AtomController();
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
            _dragOffset.z = _atomWorldRadius * 10;
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
                _state = State.Scrolling;

                _scrollRect.OnBeginDrag(eventData);
            }
            else if(absDelta.x > absDelta.y)
            {
                _state = State.DraggingObject;

                _atomBeingDragged = AtomFactory.CreateAtomView(Element);
                _atomBeingDragged.gameObject.layer = _defaultLayer;
                _atomBeingDragged.transform.localPosition = GetWorldDragPos(eventData.position);
                
                //eventData.pointerDrag = _atomBeingDragged.gameObject;
                
                Audio.PlayHighPop();

            }
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        if(_state == State.Scrolling)
        {
            _scrollRect.OnDrag(eventData);
        }
        else if(_state == State.DraggingObject)
        {
            UpdatePotentialTarget(eventData);

            _atomBeingDragged.TargetPosition = GetWorldDragPos(eventData.position);
        }
    }


    private void UpdatePotentialTarget(PointerEventData eventData)
    {
        AtomController potentialTarget = null;

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

        if(PotentialTargetAtom != null)
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
        if(_state == State.Scrolling)
        {
            _scrollRect.OnEndDrag(eventData);
        }
        else if(_state == State.DraggingObject)
        {
            var targetPos = GetWorldDragPos(eventData.position);
            _atomBeingDragged.TargetPosition = targetPos;

            _atomBeingDragged.gameObject.layer = _atomsLayer;
            
            // TODO: Do something here.

            _atomBeingDragged = null;
            PotentialTargetAtom = null;
        }

        _state = State.Idle;
    }
}
