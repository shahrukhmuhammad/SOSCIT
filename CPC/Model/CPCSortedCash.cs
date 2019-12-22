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
    
    public partial class CPCSortedCash
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CPCSortedCash()
        {
            this.CPCSortedCashDetails = new HashSet<CPCSortedCashDetail>();
        }
    
        public System.Guid Id { get; set; }
        public long ConsignmentNumber { get; set; }
        public System.DateTime Date { get; set; }
        public string Station { get; set; }
        public System.Guid ProjectBranchId { get; set; }
        public Nullable<int> TotalNumberBundles { get; set; }
        public long TotalBalance { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.Guid CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<System.Guid> UpdatedBy { get; set; }
        public Nullable<byte> Status { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> OrderNumber { get; set; }
        public Nullable<System.Guid> OrderBookingId { get; set; }
        public Nullable<System.Guid> DenominationId { get; set; }
        public Nullable<int> DenominationTitle { get; set; }
        public string SealNumber { get; set; }
        public Nullable<System.Guid> CityId { get; set; }
        public string CityName { get; set; }
        public string BranchName { get; set; }
    
        public virtual CPCDenomination CPCDenomination { get; set; }
        public virtual CPCProjectBranch CPCProjectBranch { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CPCSortedCashDetail> CPCSortedCashDetails { get; set; }
    }
}