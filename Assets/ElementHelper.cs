using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element : byte
{
    PseudoAtom = 0,
    Hydrogen = 1,
    Helium = 2,
    Lithium = 3,
    Beryllium = 4,
    Boron = 5,
    Carbon = 6,
    Nitrogen = 7,
    Oxygen = 8,
    Fluorine = 9,
    Neon = 10,
    Sodium = 11,
    Magnesium = 12,
    Aluminum = 13,
    Silicon = 14,
    Phosphorus = 15,
    Sulfur = 16,
    Chlorine = 17,
    Argon = 18,
    Potassium = 19,
    Calcium = 20,
    Scandium = 21,
    Titanium = 22,
    Vanadium = 23,
    Chromium = 24,
    Manganese = 25,
    Iron = 26,
    Cobalt = 27,
    Nickel = 28,
    Copper = 29,
    Zinc = 30,
    Gallium = 31,
    Germanium = 32,
    Arsenic = 33,
    Selenium = 34,
    Bromine = 35,

    Iodine = 53,
    Xenon = 54,

};


public static class ElementHelper
{
    public static Color GetColor(Element element)
    {
        Color color = Color.magenta;

        if(ElementColors.ContainsKey(element))
        {
            color = ElementColors[element];
        }

        return color;
    }


    public static int GetRadius(Element element)
    {
        int radius = 70;

        if(ElementRadii.ContainsKey(element))
        {
            radius = ElementRadii[element];
        }

        return radius;
    }


    // Color source: http://www.biorom.uma.es/contenido/biomodel/Jmol/colors/jmol_colors.en.htm
    static readonly Dictionary<Element, Color> ElementColors = new Dictionary<Element, Color>
    {
        {Element.Hydrogen, new Color32(255, 255, 255, 255)},
        {Element.Helium, new Color32(217, 255, 255, 255)},
        {Element.Lithium, new Color32(204, 128, 255, 255)},
        {Element.Beryllium, new Color32(194, 255, 0, 255)},
        {Element.Boron, new Color32(255, 181, 181, 255)},
        {Element.Carbon, new Color32(144, 144, 144, 255)},
        {Element.Nitrogen, new Color32(48, 80, 248, 255)},
        {Element.Oxygen, new Color32(255, 13, 13, 255)},
        {Element.Fluorine, new Color32(144, 224, 80, 255)},
        {Element.Neon, new Color32(179, 227, 245, 255)},
        {Element.Sodium, new Color32(171, 92, 242, 255)},
        {Element.Magnesium, new Color32(138, 255, 0, 255)},
        {Element.Aluminum, new Color32(191, 166, 166, 255)},
        {Element.Silicon, new Color32(240, 200, 160, 255)},
        {Element.Phosphorus, new Color32(255, 128, 0, 255)},
        {Element.Sulfur, new Color32(255, 255, 48, 255)},
        {Element.Chlorine, new Color32(31, 240, 31, 255)},
        {Element.Argon, new Color32(128, 209, 227, 255)},
        {Element.Potassium, new Color32(143, 64, 212, 255)},
        {Element.Calcium, new Color32(61, 255, 0, 255)},
        {Element.Scandium, new Color32(230, 230, 230, 255)},
        {Element.Titanium, new Color32(191, 194, 199, 255)},
        {Element.Vanadium, new Color32(166, 166, 171, 255)},
        {Element.Chromium, new Color32(138, 153, 199, 255)},
        {Element.Manganese, new Color32(156, 122, 199, 255)},
        {Element.Iron, new Color32(224, 102, 51, 255)},
        {Element.Cobalt, new Color32(240, 144, 160, 255)},
        {Element.Nickel, new Color32(80, 208, 80, 255)},
        {Element.Copper, new Color32(200, 128, 51, 255)},
        {Element.Zinc, new Color32(125, 128, 176, 255)},
        {Element.Gallium, new Color32(194, 143, 143, 255)},
        {Element.Germanium, new Color32(102, 143, 143, 255)},
        {Element.Arsenic, new Color32(189, 128, 227, 255)},
        {Element.Selenium, new Color32(255, 161, 0, 255)},
        {Element.Bromine, new Color32(166, 41, 41, 255)},

        {Element.Iodine, new Color32(148, 0, 148, 255)},
        {Element.Xenon, new Color32(66, 158, 176, 255)},
    };

    // Source: Empirical column of https://en.wikipedia.org/wiki/Atomic_radii_of_the_elements_%28data_page%29
    static readonly Dictionary<Element, int> ElementRadii = new Dictionary<Element, int>
    {
        {Element.Hydrogen, 25},
        {Element.Helium, 120},
        {Element.Lithium, 145},
        {Element.Beryllium, 105},
        {Element.Boron, 85},
        {Element.Carbon, 70},
        {Element.Nitrogen, 65},
        {Element.Oxygen, 60},
        {Element.Fluorine, 50},
        {Element.Neon, 160},
        {Element.Sodium, 180},
        {Element.Magnesium, 150},
        {Element.Aluminum, 125},
        {Element.Silicon, 110},
        {Element.Phosphorus, 100},
        {Element.Sulfur, 100},
        {Element.Chlorine, 100},
        {Element.Argon, 71},
        {Element.Potassium, 220},
        {Element.Calcium, 180},
        {Element.Scandium, 160},
        {Element.Titanium, 140},
        {Element.Vanadium, 135},
        {Element.Chromium, 140},
        {Element.Manganese, 140},
        {Element.Iron, 140},
        {Element.Cobalt, 135},
        {Element.Nickel, 135},
        {Element.Copper, 135},
        {Element.Zinc, 135},
        {Element.Gallium, 130},
        {Element.Germanium, 125},
        {Element.Arsenic, 115},
        {Element.Selenium, 115},
        {Element.Bromine, 115},

        {Element.Iodine, 140},
        {Element.Xenon, 108}, // Calculated
    };

    
    internal static string GetSymbol(Element element)
    {
        string symbol = "?";

        byte atomicNumber = (byte)element;

        if(atomicNumber > 0 && atomicNumber < ElementSymbols.Length)
        {
            symbol = ElementSymbols[atomicNumber];
        }

        return symbol;
    }


    static readonly string[] ElementSymbols = { "?", "H", " He", " Li", "Be", " B", "C", "N", "O", "F", "Ne", "Na", "Mg", "Al", "Si", "P", "S", "Cl", "Ar", "K", "Ca", "Sc", "Ti", "V", "Cr", "Mn", "Fe", "Co", "Ni", "Cu", "Zn", "Ga", "Ge", "As", "Se", "Br", "Kr", "Rb", "Sr", "Y", "Zr", "Nb", "Mo", "Tc", "Ru", "Rh", "Pd", "Ag", "Cd", "In", "Sn", "Sb", "Te", "I", "Xe", "Cs", "Ba", "La", "Ce", "Pr", "Nd", "Pm", "Sm", "Eu", "Gd", "Tb", "Dy", "Ho", "Er", "Tm", "Yb", "Lu", "Hf", "Ta", "W1", "Re", "Os", "Ir", "Pt", "Au", "Hg", "Tl", "Pb", "Bi", "Po", "At", "Rn", "Fr", "Ra", "Ac", "Th", "Pa", "U", "Np", "Pu", "Am", "Cm", "Bk", "Cf", "Es", "Fm", "Md", "No", "Lr", "Rf", "Db", "Sg", "Bh", "Hs", "Mt", "Ds", "Rg", "Cn", "Nh", "Fl", "Mc", "Lv", "Ts", "Og" };
    
}
