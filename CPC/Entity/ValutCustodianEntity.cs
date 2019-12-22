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

    public class ValutCustodianEntity
    {
        private SOSTechCPCEntities context;
        private UnsortedCashEntity unsortedCashRepo;

        public ValutCustodianEntity()
        {
            unsortedCashRepo = new UnsortedCashEntity();
        }

        public List<CPCVaultCustodian> GetAll()
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.CPCVaultCustodians.Include(x => x.CPCVaultCustodianDetails).Include(x => x.CPCProjectBranch).Where(x => x.IsActive).OrderBy(x => x.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CPCVaultCustodian GetById(Guid Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.CPCVaultCustodians.Include(x => x.CPCVaultCustodianDetails.Select(y => y.CPCDenomination)).Where(x => x.Id == Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Vew_VaultCustodian GetDetailsById(Guid Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.Vew_VaultCustodian.Where(x => x.Id == Id).FirstOrDefault();
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



        #region Add/Update
        public Guid? Create(CPCVaultCustodian model)
        {
            try
            {

                using (context = new SOSTechCPCEntities())
                {
                    #region Save
                    context.CPCVaultCustodians.Add(model);
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

        public bool Create(List<CPCVaultCustodianDetail> modelList)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Save Department
                    context.CPCVaultCustodianDetails.AddRange(modelList);
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
                    #region Update
                    var res = context.CPCVaultCustodians.Where(x => x.Id == model.Id).FirstOrDefault();
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
                    #region Delete
                    var res = context.CPCVaultCustodianDetails.Where(x => x.VaultCustodianId == Id).ToList();
                    if (res != null)
                    {
                        context.CPCVaultCustodianDetails.RemoveRange(res);
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

        public void ChangeStatus(Guid BookingId, Guid UserId, Guid BranchId, AnnexureStatus status)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Update Status
                    var res = context.CPCVaultCustodians.Where(x => x.OrderBookingId == BookingId && x.ProjectBranchId == BranchId).FirstOrDefault();
                    if (res != null)
                    {
                        res.Status = (byte)status;
                        res.UpdatedOn = DateTime.Now;
                        res.UpdatedBy = UserId;
                        context.SaveChanges();
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
            }
        }
        public int GetNextSrNo()
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

        public CPCVaultCustodian InActiveRecord(Guid Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region inactive Custodian Record
                    var res = context.CPCVaultCustodians.Where(x => x.Id == Id).FirstOrDefault();
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

        public List<CPCVaultCustodian> GetAllApproved()
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.CPCVaultCustodians.Where(x => x.IsActive && x.Status == (int)AnnexureStatus.Inprocess).OrderBy(x => x.CreatedOn).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Vew_VaultCustodian> GetAllDetails()
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.Vew_VaultCustodian.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Vew_VaultCustodian> GetAllDetailsById(Guid Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.Vew_VaultCustodian.Where(x => x.VaultCustodianId == Id).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region Update Status
        public void ChangeStatus(Guid? bookingId, Guid UserId)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Update Status
                    var res = context.CPCVaultCustodians.Where(x => x.OrderBookingId == bookingId).FirstOrDefault();
                    if (res != null)
                    {
                        res.Status = (int)AnnexureStatus.Proceeded;
                        res.UpdatedOn = DateTime.Now;
                        res.UpdatedBy = UserId;
                        context.SaveChanges();
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        public bool avaiableRecord(Guid? bookingId, Guid projBranchId)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region inactive custodian status
                    var res = context.CPCVaultCustodians.Where(x => x.OrderBookingId == bookingId && x.ProjectBranchId == projBranchId).FirstOrDefault();
                    if (res != null)
                    {
                        res.Status = (int)AnnexureStatus.Inprocess;
                        context.SaveChanges();
                        return true;
                    }
                    #endregion
                    else
                    { return false; }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
