
//------------------------------------------------------------------------------
// <auto-generated>
//    Ce code a été généré à partir d'un modèle.
//
//    Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//    Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------


namespace EchoMe.Models
{

using System;
    using System.Collections.Generic;
    
public partial class WoGamProfile
{

    public WoGamProfile()
    {

        this.WoGamCategories = new HashSet<WoGamCategory>();

    }


    public int usr_id { get; set; }

    public string usr_name { get; set; }

    public string usr_pwd { get; set; }

    public string usr_gameLangage { get; set; }



    public virtual ICollection<WoGamCategory> WoGamCategories { get; set; }

}

}
