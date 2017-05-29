using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour {

    /********************************* Variables *********************************/

    protected char value;

    /********************************* Methods *********************************/
    
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