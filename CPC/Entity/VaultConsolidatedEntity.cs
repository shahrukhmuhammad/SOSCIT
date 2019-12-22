using CPC.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPC
{
    public class VaultConsolidatedEntity
    {
        private SOSTechCPCEntities context;
        
        public List<CPCVaultConsolidated> GetAll()
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.CPCVaultConsolidateds.Include(x => x.CPCVaultConsolidatedDetails).Include(x => x.CPCProjectBranch).Where(x => x.IsActive).OrderBy(x => x.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Vew_VaultConsolidate> GetAllDetails()
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.Vew_VaultConsolidate.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Vew_VaultConsolidate> GetAllDetailsById()
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.Vew_VaultConsolidate.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Vew_CPCAnnexureI> GetByDateBranchId(Guid branchId, DateTime dateofCollection)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.Vew_CPCAnnexureI.Where(x => x.ProjectBranchId == branchId && x.DateOfCollection == dateofCollection).ToList();
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                return null;
            }
        }


        public CPCVaultConsolidated GetById(Guid Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.CPCVaultConsolidateds.Include(x => x.CPCCity).Include(x => x.CPCOrderBooking).Include(x => x.CPCProjectBranch).Include(x => x.CPCVaultConsolidatedDetails.Select(y => y.CPCDenomination)).Where(x => x.Id == Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public List<CustomSelectList> GetDepartmentDropdown(Guid? Id = null)
        //{
        //    try
        //    {
        //        using (context = new SOSTechCPCEntities())
        //        {
        //            var ls = new List<Department>();
        //            if (Id.HasValue)
        //            {
        //                ls = context.Departments.Where(x => x.Id == Id).ToList();
        //                return ls.Select(x => new CustomSelectList { Value = x.Id.ToString(), Text = x.Code + " - " + x.Name }).ToList();
        //            }
        //            ls = context.Departments.ToList();
        //            return ls.Select(x => new CustomSelectList { Value = x.Id.ToString(), Text = x.Code + " - " + x.Name }).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        #region Add/Update Employee
        public Guid? Create(CPCVaultConsolidated model)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Save Department
                    context.CPCVaultConsolidateds.Add(model);
                    context.SaveChanges();
                    #endregion
                    return model.Id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Create(List<CPCVaultConsolidatedDetail> modelList)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Save
                    context.CPCVaultConsolidatedDetails.AddRange(modelList);
                    context.SaveChanges();
                    #endregion
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #region Bundle Details
        public List<CPCVaultConsolidatedBundle> GetByIdConsolidatedBundle(Guid VaultConsolicatedId)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    var lsDetailsId = context.CPCVaultConsolidatedDetails.Where(x => x.VaultConsolidatedId == VaultConsolicatedId).Select(x => x.Id).ToList();
                    return context.CPCVaultConsolidatedBundles.Include(x => x.CPCVaultConsolidatedBundlesDetails.Select(y => y.CPCEmployee)).Where(x => lsDetailsId.Contains(x.ConsolidatedDetailsId)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CPCVaultConsolidatedBundle> GetAllConsolidatedBundle()
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.CPCVaultConsolidatedBundles.Include(x => x.CPCVaultConsolidatedBundlesDetails.Select(y => y.CPCEmployee)).OrderBy(x => x.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        public bool CreateVaultConsolidatedBundle(List<CPCVaultConsolidatedBundle> modelList, List<CPCVaultConsolidatedBundlesDetail> childList)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Save
                    context.CPCVaultConsolidatedBundles.AddRange(modelList);
                    //context.CPCVaultConsolidatedBundlesDetails.AddRange(childList);
                    context.SaveChanges();
                    #endregion
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool CreateVaultConsolidatedBundleDetails(List<CPCVaultConsolidatedBundlesDetail> modelList)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Save
                    context.CPCVaultConsolidatedBundlesDetails.AddRange(modelList);
                    context.SaveChanges();
                    #endregion
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(CPCCashInTransit model)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Update Employee
                    var res = context.CPCSortedCashes.Where(x => x.Id == model.Id).FirstOrDefault();
                    if (res != null)
                    {
                        //res.CashHandedOverCPCStaffAId = model.CashHandedOverCPCStaffAId;
                        //res.CashHandedOverCPCStaffBId = model.CashHandedOverCPCStaffBId;
                        //res.CashHandedOverCITStaffAId = model.CashHandedOverCITStaffAId;
                        //res.CashHandedOverCITStaffBId = model.CashHandedOverCITStaffBId;
                        //res.SignatureCPCHandingOverCashAId = model.SignatureCPCHandingOverCashAId;
                        //res.SignatureCPCHandingOverCashBId = model.SignatureCPCHandingOverCashBId;
                        //res.UpdatedOn = model.UpdatedOn;
                        //res.UpdatedBy = model.UpdatedBy;
                        //context.SaveChanges();
                    }
                    #endregion
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Delete
        public bool DeleteDetails(Guid Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Update Employee
                    var res = context.CPCAnnexureIDetails.Where(x => x.AnnexureIId == Id).ToList();
                    if (res != null)
                    {
                        context.CPCAnnexureIDetails.RemoveRange(res);
                        context.SaveChanges();
                    }
                    #endregion
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //public bool Delete(Guid Id)
        //{
        //    try
        //    {
        //        using (context = new SOSTechCPCEntities())
        //        {
        //            #region Update Employee
        //            var res = context.Departments.Where(x => x.Id == Id).FirstOrDefault();
        //            if (res != null)
        //            {
        //                context.Departments.Remove(res);
        //                context.SaveChanges();
        //            }
        //            #endregion
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        //public bool DeleteMultiple(List<Guid> Ids)
        //{
        //    try
        //    {
        //        using (context = new SOSTechCPCEntities())
        //        {
        //            #region Update Employee
        //            var res = context.Departments.Where(x => Ids.Contains(x.Id)).ToList();
        //            if (res != null)
        //            {
        //                context.Departments.RemoveRange(res);
        //                context.SaveChanges();
        //            }
        //            #endregion
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        #endregion

        public int GetNextSerialNumber()
        {
            try
            {
                using (var context = new SOSTechCPCEntities())
                {
                    int? res = context.CPCVaultCustodians.Max(u => (int?)u.SerialNumber);

                    if (res.HasValue)
                    {
                        return Convert.ToInt32(res) + 1;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                //new Logger().LogError(ex);
                return 0;
            }
        }

        public CPCVaultConsolidated InActiveRecord(Guid Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region inactive consolidated Vault
                    var res = context.CPCVaultConsolidateds.Where(x => x.Id == Id).FirstOrDefault();
                    if (res != null)
                    {
                        res.IsActive = false;
                        context.SaveChanges();
                    }
                    #endregion
                    return res;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Vew_VaultConsolidate> GetAllApproved()
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.Vew_VaultConsolidate.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region Update Status
        public void ChangeStatus(Guid? denomId, Guid? bookingId, Guid userId, Guid? ProjBranchId)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Update Record
                    var res = context.CPCVaultConsolidatedDetails.Include(x => x.CPCVaultConsolidated).Where(x => x.CPCVaultConsolidated.OrderBookingId == bookingId && x.CPCVaultConsolidated.ProjectBranchId == ProjBranchId).ToList();
                    if (res != null && res.Count > 0)
                    {
                        //Child Status
                        var denomObj = res.Where(x => x.DenominationId == denomId).FirstOrDefault();
                        if (denomObj != null)
                        {
                            denomObj.Status = (int)AnnexureStatus.Completed;
                            context.SaveChanges();
                        }
                        
                        //For Master
                        if (res.Count == res.Where(x => x.Status == (int)AnnexureStatus.Completed).Count())
                        {
                            res.FirstOrDefault().CPCVaultConsolidated.Status = (int)AnnexureStatus.Completed;
                            res.FirstOrDefault().CPCVaultConsolidated.UpdatedBy = userId;
                            res.FirstOrDefault().CPCVaultConsolidated.UpdatedOn = DateTime.Now;
                            context.SaveChanges();

                            //Update Sorter Cash Entry Status
                            var sortedcashEntity = new SortedCashEntity();
                            sortedcashEntity.ChangeStatus(bookingId,userId, ProjBranchId);

                            //Update Custodian Entry Status
                            var vaultCustodianEntity = new ValutCustodianEntity();
                            vaultCustodianEntity.ChangeStatus(bookingId.Value, userId, ProjBranchId.Value, AnnexureStatus.Completed);

                            //Update Unsorted Cash Entry Status
                            var unsortedCashEntity = new UnsortedCashEntity();
                            unsortedCashEntity.ChangeStatus(bookingId.Value, userId, ProjBranchId.Value, AnnexureStatus.Completed);

                            //Update Annexur Cash Entry Status
                            var annexureIEntity = new AnnexureIEntity();
                            annexureIEntity.ChangeStatus(bookingId.Value, userId, ProjBranchId.Value, AnnexureStatus.Completed);
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
    }
}
