//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CPC.Model
{
    using System;
    
    public partial class AppNotifications_GetByOfficeId_Result
    {
        public System.Guid Id { get; set; }
        public Nullable<System.Guid> OfficeId { get; set; }
        public System.Guid ContactId { get; set; }
        public Nullable<System.Guid> ReferenceId { get; set; }
        public byte Type { get; set; }
        public bool IsRead { get; set; }
        public string Title { get; set; }
        public string ActionUrl { get; set; }
        public string Description { get; set; }
        public System.DateTime CreatedOn { get; set; }
    }
}
