//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GST_DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.Permissions = new HashSet<Permission>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Usertype { get; set; }
        public string Createdate { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Mobilenumber { get; set; }
        public string AdminID { get; set; }
        public string UserId { get; set; }
        public string CompanyID { get; set; }
        public string LastLoginDate { get; set; }
        public string Guid { get; set; }
    
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
