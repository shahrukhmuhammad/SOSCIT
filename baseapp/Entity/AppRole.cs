using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseApp.Entity
{
    public class AppRole
    {
        public Guid Id { get; set; }
        public bool IsSystem { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Permissions { get; set; }
        public string[] PermissionsList
        {
            get
            {
                if (string.IsNullOrEmpty(Permissions))
                {
                    return null;
                }
                else
                {
                    return Permissions.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                }
            }
        }
        public DateTime CreatedOn { get; set; }


        public virtual List<AppUser> Contacts { get; set; }
    }

    public class AppPermission
    {
        public const string All = "All";

        #region View Permissions
        public const string ViewContact = "ViewContact";
        public const string ViewCMS = "ViewCMS";
        public const string ViewDMS = "ViewDMS";
        public const string ViewEcommerce = "ViewEcommerce";
        public const string ViewHRM = "ViewHRM";
        public const string ViewCRM = "ViewCRM";
        public const string ViewHRMS = "ViewHRMS";
        public const string ViewCPC = "ViewCPC";
        public const string ViewCIT = "ViewCIT";
        public const string ViewBank = "ViewBank";
        public const string ViewOrderRequests = "ViewOrderRequests";
        public const string ViewOrderCollection = "ViewOrderCollection";
        public const string ViewVaultUnsortedHandler = "ViewVaultUnsortedHandler";
        public const string ViewVaultManager = "ViewVaultManager";
        public const string ViewSupervisor = "ViewSupervisor";
        public const string ViewControlRoom = "ViewControlRoom";
        #endregion

        #region Control Permissions
        public const string CMS = "CMS";
        public const string DMS = "DMS";
        public const string Contact = "Contact";
        public const string Report = "Report";
        public const string Ecommerce = "Ecommerce";
        public const string HRM = "HRM";
        public const string CRM = "CRM";
        public const string HRMS = "HRMS";
        public const string CPC = "CPC";
        public const string CIT = "CIT";
        public const string Bank = "Bank";
        public const string CPC_RGM = "CPC_RGM";
        public const string OrderRequests = "OrderRequests";
        public const string OrderCollection = "ViewOrderCollection";
        public const string VaultUnsortedHandler = "VaultUnsortedHandler";
        public const string VaultManager = "VaultManager";
        public const string Supervisor = "Supervisor";
        public const string ControlRoom = "ControlRoom";
        #endregion


        #region  Settings & Configurations
        public const string LogsManagement = "LogManagement";
        public const string TaskManagement = "TaskManagement";
        #endregion

        public static string[] List()
        {
            var ret = typeof(AppPermission).GetFields().Select(x => x.GetValue(null).ToString());
            return ret.ToArray();
        }
    }
}
