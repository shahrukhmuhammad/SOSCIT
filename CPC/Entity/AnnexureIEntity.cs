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
    public class AnnexureIEntity
    {
        private SOSTechCPCEntities context;

        public List<CPCAnnexureI> GetAll()
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.CPCAnnexureIs.Where(x => x.IsActive).OrderBy(x => x.CreatedOn).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Vew_CPCAnnexureI> GetAllDetails()
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.Vew_CPCAnnexureI.Where(x => x.IsActive).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<Vew_CPCAnnexureI> GetAllDetailsById(Guid Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.Vew_CPCAnnexureI.Where(x => x.Id == Id).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<Vew_CPCAnnexureI> GetAllDetailsById(Guid Id, Guid PriojId)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.Vew_CPCAnnexureI.Where(x => x.OrderBookingId == Id && x.ProjectBranchId == PriojId).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<Vew_CPCAnnexureI> GetAllDetailsByOrderNo(int Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.Vew_CPCAnnexureI.Where(x => x.OrderNumber == Id).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool IsDuplicate(int SrNo)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.CPCAnnexureIs.Where(x => x.SrNo == SrNo).FirstOrDefault() != null ? true : false;
                }
            }
            catch (Exception ex)
            {
                return true;
            }
        }
        public List<Vew_CPCAnnexureI> GetByDateBranchId(Guid branchId, DateTime dateofCollection)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.Vew_CPCAnnexureI.Where(x => x.ProjectBranchId == branchId && x.DateOfCollection == dateofCollection && x.IsActive).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public CPCAnnexureI GetById(Guid Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.CPCAnnexureIs.Find(Id);
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
        public Guid? Create(CPCAnnexureI model)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Save Department
                    context.CPCAnnexureIs.Add(model);
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

        public bool Create(List<CPCAnnexureIDetail> modelList)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Save Department
                    context.CPCAnnexureIDetails.AddRange(modelList);
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

        public bool Update(CPCAnnexureI model)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Update Employee
                    var res = context.CPCAnnexureIs.Where(x => x.Id == model.Id).FirstOrDefault();
                    if (res != null)
                    {
                        res.CashHandedOverCPCStaffAId = model.CashHandedOverCPCStaffAId;
                        res.CashHandedOverCPCStaffBId = model.CashHandedOverCPCStaffBId;
                        res.CashHandedOverCITStaffAId = model.CashHandedOverCITStaffAId;
                        res.CashHandedOverCITStaffBId = model.CashHandedOverCITStaffBId;
                        res.SignatureCPCHandingOverCashAId = model.SignatureCPCHandingOverCashAId;
                        res.SignatureCPCHandingOverCashBId = model.SignatureCPCHandingOverCashBId;
                        res.UpdatedOn = model.UpdatedOn;
                        res.UpdatedBy = model.UpdatedBy;
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
        #endregion

        #region Delete
        public CPCAnnexureI InActiveRecord(Guid Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region inactive Custodian Record
                    var res = context.CPCAnnexureIs.Where(x => x.Id == Id).FirstOrDefault();
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

        public int GetNextSrNo()
        {
            try
            {
                using (var context = new SOSTechCPCEntities())
                {
                    int? res = context.CPCAnnexureIs.Max(u => (int?)u.SrNo);

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

        public List<CPCAnnexureI> GetAllApproved()
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.CPCAnnexureIs.Where(x => x.IsActive && x.Status == (int)AnnexureStatus.Inprocess).OrderBy(x => x.CreatedOn).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public void ChangeStatus(Guid? bookingId, Guid userId, Guid? ProjBranchId)
        //{
        //    try
        //    {
        //        using (context = new SOSTechCPCEntities())
        //        {
        //            #region Update Record
        //            var res = context.CPCAnnexureIs.Include(x => x.CPCAnnexureIDetails).Where(x => x.OrderBookingId == bookingId).FirstOrDefault();
        //            if (res != null)
        //            {
        //                var resDetail = res.CPCAnnexureIDetails.Where(x => x.ProjectBranchId == ProjBranchId).ToList();
        //                resDetail.ForEach(x => { x.DetailStatus = (int)AnnexureStatus.Proceeded; });
        //                context.SaveChanges();
        //            }
        //            #endregion
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        public void ChangeStatus(Guid? bookingId, Guid userId, Guid? ProjBranchId, AnnexureStatus status)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Update Child
                    var res = context.CPCAnnexureIs.Include(x => x.CPCAnnexureIDetails).Where(x => x.OrderBookingId == bookingId).FirstOrDefault();
                    if (res != null)
                    {
                        var resDetail = res.CPCAnnexureIDetails.Where(x => x.ProjectBranchId == ProjBranchId).ToList();
                        resDetail.ForEach(x => { x.DetailStatus = (byte)status; });
                        context.SaveChanges();

                        #region Update Record
                        res.CPCAnnexureIDetails.Select(x => x.ProjectBranchId).Distinct().ToList();
                        if (resDetail.Count == res.CPCAnnexureIDetails.Where(x => x.DetailStatus == (int)AnnexureStatus.Completed).Count())
                        {
                            res.Status = (int)AnnexureStatus.Completed;
                            res.UpdatedBy = userId;
                            res.UpdatedOn = DateTime.Now;

                            #region Update Bank Status
                            var orderbookingEntity = new OrderbookingEntity();
                            orderbookingEntity.ChangeStatus(bookingId, userId, AnnexureStatus.Sorted);
                            #endregion
                        }
                        context.SaveChanges();
                        #endregion
                    }
                    #endregion

                    
                }
            }
            catch (Exception ex)
            {
            }
        }

        public bool avaiableRecord(Guid? bookingId, Guid projBranchId)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region inactive custodian status
                    var res = context.CPCAnnexureIs.Include(x => x.CPCAnnexureIDetails).Where(x => x.OrderBookingId == bookingId).FirstOrDefault();
                    var resDetails = res.CPCAnnexureIDetails.Where(x => x.AnnexureIId == res.Id && x.ProjectBranchId == projBranchId);
                    if (res != null)
                    {
                        if (res.Status == (int)AnnexureStatus.Proceeded)
                        {
                            res.Status = (int)AnnexureStatus.Inprocess;
                            resDetails.FirstOrDefault().DetailStatus = (int)AnnexureStatus.Inprocess;
                        }
                        else
                        {
                            resDetails.FirstOrDefault().DetailStatus = (int)AnnexureStatus.Inprocess;
                        }
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
