//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EchoMe.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class WoGamCategory
    {
        public WoGamCategory()
        {
            this.WoGamWords = new HashSet<WoGamWord>();
        }
    
        public int cat_id { get; set; }
        public string cat_name { get; set; }
        public bool cat_reached { get; set; }
        public int cat_lng { get; set; }
    
        public virtual WoGamLangage WoGamLangage { get; set; }
        public virtual ICollection<WoGamWord> WoGamWords { get; set; }
    }
}
