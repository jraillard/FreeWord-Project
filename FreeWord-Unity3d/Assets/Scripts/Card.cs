using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour {

    protected char value;
    protected bool visibility; /*for a played card : you can't reach them if false 
                                * for a placed card : you can't put another card on it
                               */
    
    public bool IsVisible()
    {
        return visibility;
    }

    public void SetVisibility() //change the visibility on an event
    {
        if(IsVisible())
        {
            visibility = false;
        }else
        {
            visibility = true;
        }
    }

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


                                 

}
