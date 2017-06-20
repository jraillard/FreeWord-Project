using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EchoMe.Models
{
    public class UserList
    {
        public string Role { get; set; }
        public List<UserField> UserFields { get; set; }
    }

    public class UserField
    {
        public string Username { get; set; }
        [Display(Name = "Created On")]
        public DateTime CreateDateTime { get; set; }
    }
}