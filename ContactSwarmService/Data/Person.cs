//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ContactSwarmService.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Person
    {
        public Person()
        {
            this.Contacts = new HashSet<Contact>();
        }
    
        public long Id { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<Contact> Contacts { get; set; }
    }
}
