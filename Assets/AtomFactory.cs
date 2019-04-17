using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomFactory : MonoBehaviour
{
    [SerializeField] AtomController _atomViewPrefab;

    static AtomFactory _instance;

    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    public static AtomController CreateAtomView(Element element = Element.Carbon)
    {
        var atom = Instantiate(_instance._atomViewPrefab, _instance.transform);
        atom.Element = element;

        return atom;
    }
}
