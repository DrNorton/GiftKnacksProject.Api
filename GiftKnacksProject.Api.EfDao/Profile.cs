//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GiftKnacksProject.Api.EfDao
{
    using System;
    using System.Collections.Generic;
    
    public partial class Profile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Profile()
        {
            this.Contacts = new HashSet<Contact>();
        }
    
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string AvatarUrl { get; set; }
        public string AboutMe { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public bool HideBirthday { get; set; }
        public bool IsFilled { get; set; }
        public Nullable<System.DateTime> LastLoginTime { get; set; }
        public bool Gender { get; set; }
        public System.DateTime CreatedTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual Country Country1 { get; set; }
        public virtual User User { get; set; }
    }
}
