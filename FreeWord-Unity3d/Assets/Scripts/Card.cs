using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour {
    //All Card have a value which define the character which display on it 

    /********************************* Variables *********************************/

    protected char value;

    private bool visibility; //true mean for PlayedCard : we see the frontCard , false the backCard
                             //          for PlacedCard : letter shown or not 
    /********************************* Methods *********************************/

    //Get and Set basic methods for value and visibility
    public char GetValue()
    {
        return value;

    }

    public void SetValue(char c)
    {
        if(char.IsLetter(c)) // https://msdn.microsoft.com/fr-fr/library/yyxz6h5w(v=vs.110).aspx => Notes 
        {
            value = c;
        }
    }

    public bool IsVisible()
    {
        return visibility;
    }

    public void SetVisibility(bool b)
    {
        visibility = b;
    }

}