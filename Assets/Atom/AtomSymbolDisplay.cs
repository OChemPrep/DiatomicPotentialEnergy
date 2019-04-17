using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AtomSymbolDisplay : AtomListener
{
    TMPro.TMP_Text _symbolDisplay;


    public string Symbol
    {
        get
        {
            return _symbolDisplay.text;
        }

        set
        {
            _symbolDisplay.text = value;
        }
    }


    public Color Color
    {
        get
        {
            return _symbolDisplay.color;
        }

        set
        {
            _symbolDisplay.color = value;
        }
    }


    protected override void Awake()
    {
        _symbolDisplay = GetComponent<TMPro.TMP_Text>();

        base.Awake();
    }


    protected override void OnControllerRefreshed()
    {
        if(Controller != null)
        {
            Element element = Controller.Element;

            Color elementColor = ElementHelper.GetColor(element);

            // Set text color to contrast with element color.
            Color textColor = Color.black;

            if(elementColor.grayscale < 0.5f)
                textColor = Color.white;

            Symbol = ElementHelper.GetSymbol(element);
            Color = textColor;
        }
    }

}
