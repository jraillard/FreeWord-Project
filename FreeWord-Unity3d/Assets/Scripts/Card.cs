using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour {
    //All Card have a value which define the character which display on it 

    /********************************* Variables *********************************/

    protected char value;

    /********************************* Methods *********************************/
    
    //Get and Set basic methods for value
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