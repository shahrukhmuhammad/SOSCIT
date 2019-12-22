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
    public class UnsortedCashEntity
    {
        private SOSTechCPCEntities context;

        public List<CPCUnsortedCash> GetAll()
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.CPCUnsortedCashes.Include(x => x.CPCUnsortedCashDetails).Include(x => x.CPCProjectBranch).Where(x => x.IsActive).OrderBy(x => x.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Vew_UnsortedCash> GetAllDetails()
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.Vew_UnsortedCash.ToList();
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


        public CPCUnsortedCash GetById(Guid Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.CPCUnsortedCashes.Include(x => x.CPCCity).Include(x => x.CPCOrderBooking).Include(x => x.CPCProjectBranch).Include(x => x.CPCUnsortedCashDetails.Select(y => y.CPCDenomination)).Where(x => x.Id == Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Vew_UnsortedCash> GetAllDetailsById(Guid Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.Vew_UnsortedCash.Where(x => x.UnsortedCashId == Id).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<CPCUnsortedCash> GetAllApproved()
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.CPCUnsortedCashes.Where(x => x.IsActive && x.Status == (int)AnnexureStatus.Inprocess).OrderBy(x => x.CreatedOn).ToList();
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

        #region Update Status
        public void ChangeStatus(Guid? bookingId, Guid UserId, Guid BranchId, AnnexureStatus status)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Update Status
                    var res = context.CPCUnsortedCashes.Where(x => x.OrderBookingId == bookingId && x.ProjectBranchId == BranchId).FirstOrDefault();
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
        public void ChangeStatusToAvailable(Guid Id, Guid UserId)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Update Status
                    var res = context.CPCUnsortedCashes.Where(x => x.OrderBookingId == Id).FirstOrDefault();
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

        #region Add/Update 
        public Guid? Create(CPCUnsortedCash model)
        {
            try
            {

                using (context = new SOSTechCPCEntities())
                {
                    int ConNumber = GetNextSerialNumber();
                    #region Save
                    context.CPCUnsortedCashes.Add(model);
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

        public bool Create(List<CPCUnsortedCashDetail> modelList)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Save Department
                    context.CPCUnsortedCashDetails.AddRange(modelList);
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
                    var res = context.CPCUnsortedCashes.Where(x => x.Id == model.Id).FirstOrDefault();
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
                    return context.CPCUnsortedCashes.Max(x => x.SerialNumber) <= 0 ? 1 : (int)context.CPCUnsortedCashes.Max(x => x.SerialNumber) + 1;
                }
            }
            catch (Exception ex)
            {
                //new Logger().LogError(ex);
                return 0;
            }
        }

        public CPCUnsortedCash InActiveRecord(Guid Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region inactive Custodian Record
                    var res = context.CPCUnsortedCashes.Where(x => x.Id == Id).FirstOrDefault();
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


        public bool avaiableRecord(Guid? bookingId, Guid projBranchId)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region inactive unsorted status
                    var res = context.CPCUnsortedCashes.Where(x => x.OrderBookingId == bookingId && x.ProjectBranchId == projBranchId).FirstOrDefault();
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
