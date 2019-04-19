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
    Krypton = 36,
    Rubidium = 37,
    Strontium = 38,
    Yttrium = 39,
    Zirconium = 40,
    Niobium = 41,
    Molybdenum = 42,
    Technetium = 43,
    Ruthenium = 44,
    Rhodium = 45,
    Palladium = 46,
    Silver = 47,
    Cadmium = 48,
    Indium = 49,
    Tin = 50,
    Antimony = 51,
    Tellurium = 52,
    Iodine = 53,
    Xenon = 54,
    Caesium = 55,
    Barium = 56,
    Lanthanum = 57,
    Cerium = 58,
    Praseodymium = 59,
    Neodymium = 60,
    Promethium = 61,
    Samarium = 62,
    Europium = 63,
    Gadolinium = 64,
    Terbium = 65,
    Dysprosium = 66,
    Holmium = 67,
    Erbium = 68,
    Thulium = 69,
    Ytterbium = 70,
    Lutetium = 71,
    Hafnium = 72,
    Tantalum = 73,
    Tungsten = 74,
    Rhenium = 75,
    Osmium = 76,
    Iridium = 77,
    Platinum = 78,
    Gold = 79,
    Mercury = 80,
    Thallium = 81,
    Lead = 82,
    Bismuth = 83,
    Polonium = 84,
    Astatine = 85,
    Radon = 86,
    Francium = 87,
    Radium = 88,
    Actinium = 89,
    Thorium = 90,
    Protactinium = 91,
    Uranium = 92,
    Neptunium = 93,
    Plutonium = 94,
    Americium = 95,
    Curium = 96,
    Berkelium = 97,
    Californium = 98,
    Einsteinium = 99,
    Fermium = 100,
    Mendelevium = 101,
    Nobelium = 102,
    Lawrencium = 103,
    Rutherfordium = 104,
    Dubnium = 105,
    Seaborgium = 106,
    Bohrium = 107,
    Hassium = 108,
    Meitnerium = 109,
    Darmstadtium = 110,
    Roentgenium = 111,
    Copernicium = 112,
    Nihonium = 113,
    Flerovium = 114,
    Moscovium = 115,
    Livermorium = 116,
    Tennessine = 117,
    Oganesson = 118,
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
        int radius = 70; // Value for carbon.

        int atomicNumber = (int)element;

        if(atomicNumber > 0 && atomicNumber < EmpiricalRadii.Length)
        {
            radius = EmpiricalRadii[atomicNumber];

            if(radius < 0)
                radius = CalculatedRadii[atomicNumber];
        }

        return radius;
    }


    public static int GetVanDerWaalRadius(Element element)
    {
        int radius = 170; // Value for carbon.

        int atomicNumber = (int)element;

        if(atomicNumber > 0 && atomicNumber < VanDerWaalsRadii.Length)
        {
            radius = VanDerWaalsRadii[atomicNumber];
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


    // Source: https://en.wikipedia.org/wiki/Atomic_radii_of_the_elements_%28data_page%29
    static readonly string[] ElementSymbols = { "?", "H", " He", " Li", "Be", "B", "C", "N", "O", "F", "Ne", "Na", "Mg", "Al", "Si", "P", "S", "Cl", "Ar", "K", "Ca", "Sc", "Ti", "V", "Cr", "Mn", "Fe", "Co", "Ni", "Cu", "Zn", "Ga", "Ge", "As", "Se", "Br", "Kr", "Rb", "Sr", "Y", "Zr", "Nb", "Mo", "Tc", "Ru", "Rh", "Pd", "Ag", "Cd", "In", "Sn", "Sb", "Te", "I", "Xe", "Cs", "Ba", "La", "Ce", "Pr", "Nd", "Pm", "Sm", "Eu", "Gd", "Tb", "Dy", "Ho", "Er", "Tm", "Yb", "Lu", "Hf", "Ta", "W1", "Re", "Os", "Ir", "Pt", "Au", "Hg", "Tl", "Pb", "Bi", "Po", "At", "Rn", "Fr", "Ra", "Ac", "Th", "Pa", "U", "Np", "Pu", "Am", "Cm", "Bk", "Cf", "Es", "Fm", "Md", "No", "Lr", "Rf", "Db", "Sg", "Bh", "Hs", "Mt", "Ds", "Rg", "Cn", "Nh", "Fl", "Mc", "Lv", "Ts", "Og" };
    static readonly int[] EmpiricalRadii = { -1, 25, 120, 145, 105, 85, 70, 65, 60, 50, 160, 180, 150, 125, 110, 100, 100, 100, 71, 220, 180, 160, 140, 135, 140, 140, 140, 135, 135, 135, 135, 130, 125, 115, 115, 115, -1, 235, 200, 180, 155, 145, 145, 135, 130, 135, 140, 160, 155, 155, 145, 145, 140, 140, -1, 260, 215, 195, 185, 185, 185, 185, 185, 185, 180, 175, 175, 175, 175, 175, 175, 175, 155, 145, 135, 135, 130, 135, 135, 135, 150, 190, 180, 160, 190, -1, -1, -1, 215, 195, 180, 180, 175, 175, 175, 175, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
    static readonly int[] CalculatedRadii = { -1, 53, 31, 167, 112, 87, 67, 56, 48, 42, 38, 190, 145, 118, 111, 98, 88, 79, 71, 243, 194, 184, 176, 171, 166, 161, 156, 152, 149, 145, 142, 136, 125, 114, 103, 94, 88, 265, 219, 212, 206, 198, 190, 183, 178, 173, 169, 165, 161, 156, 145, 133, 123, 115, 108, 298, 253, 195, 158, 247, 206, 205, 238, 231, 233, 225, 228, 226, 226, 222, 222, 217, 208, 200, 193, 188, 185, 180, 177, 174, 171, 156, 154, 143, 135, 127, 120, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
    static readonly int[] VanDerWaalsRadii = { -1, 120, 140, 182, 153, 192, 170, 155, 152, 147, 154, 227, 173, 184, 210, 180, 180, 175, 188, 275, 231, 211, -1, -1, -1, -1, -1, -1, 163, 140, 139, 187, 211, 185, 190, 185, 202, 303, 249, -1, -1, -1, -1, -1, -1, -1, 163, 172, 158, 193, 217, 206, 206, 198, 216, 343, 268, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 175, 166, 155, 196, 202, 207, 197, 202, 220, 348, 283, -1, -1, -1, 186, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };

}
