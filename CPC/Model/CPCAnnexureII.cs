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
    using System.Collections.Generic;
    
    public partial class CPCAnnexureII
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CPCAnnexureII()
        {
            this.CPCAnnexureIIDetails = new HashSet<CPCAnnexureIIDetail>();
        }
    
        public System.Guid Id { get; set; }
        public System.DateTime AnnexureIIDate { get; set; }
        public System.Guid ProjectBranchId { get; set; }
        public int SrNo { get; set; }
        public string SealNo { get; set; }
        public string ShipmentReceiptNo { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.Guid CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<System.Guid> UpdatedBy { get; set; }
        public Nullable<System.Guid> CashReceivedByCPCStaffAId { get; set; }
        public Nullable<System.Guid> CashReceivedByCPCStaffBId { get; set; }
        public Nullable<System.Guid> CashHandedOverByCITStaffAId { get; set; }
        public Nullable<System.Guid> CashHandedOverByCITStaffStaffBId { get; set; }
        public byte Status { get; set; }
        public bool IsActive { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CPCAnnexureIIDetail> CPCAnnexureIIDetails { get; set; }
        public virtual CPCProjectBranch CPCProjectBranch { get; set; }
    }
}