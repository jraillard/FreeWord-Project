
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
    
public partial class UserProfile
{

    public UserProfile()
    {

        this.webpages_Roles = new HashSet<webpages_Roles>();

    }


    public int UserId { get; set; }

    public string UserName { get; set; }



    public virtual ICollection<webpages_Roles> webpages_Roles { get; set; }

}

}
