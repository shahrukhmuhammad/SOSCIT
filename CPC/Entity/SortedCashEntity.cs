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
    public class SortedCashEntity
    {
        private SOSTechCPCEntities context;

        public List<CPCSortedCash> GetAll()
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.CPCSortedCashes.Include(x => x.CPCSortedCashDetails).Include(x => x.CPCProjectBranch).Where(x => x.IsActive).OrderBy(x => x.Id).ToList();
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
                    return context.Vew_CPCAnnexureI.ToList();
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


        public CPCSortedCash GetById(Guid Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.CPCSortedCashes.Include(x=>x.CPCDenomination).Include(x => x.CPCSortedCashDetails).Where(x => x.Id == Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Vew_SortedCash> GetByIdVew(Guid Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.Vew_SortedCash.Where(x => x.Id == Id).ToList();
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
        public Guid? Create(CPCSortedCash model)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Save Department
                    //int consNumber = GetNextConsignmentNumber();
                    //context.Entry(model).State = EntityState.Detached;
                    context.CPCSortedCashes.Add(model);
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

        public bool Create(List<CPCSortedCashDetail> modelList)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Save Department
                    context.CPCSortedCashDetails.AddRange(modelList);
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

        public int GetNextConsignmentNumber()
        {
            try
            {
                using (var context = new SOSTechCPCEntities())
                {
                    return context.CPCSortedCashes.Max(x => x.ConsignmentNumber) <= 0 ? 1 : (int)context.CPCSortedCashes.Max(x => x.ConsignmentNumber) + 1;
                }
            }
            catch (Exception ex)
            {
                //new Logger().LogError(ex);
                return 0;
            }
        }

        public bool InActiveRecord(Guid Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Update Employee
                    var res = context.CPCSortedCashes.Where(x => x.Id == Id).FirstOrDefault();
                    if (res != null)
                    {
                        res.IsActive = false;
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

        #region Update Status
        public void ChangeStatus(Guid? bookingId, Guid userId, Guid? ProjBranchId)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Update Record
                    var res = context.CPCSortedCashes.Where(x => x.OrderBookingId == bookingId && x.ProjectBranchId == ProjBranchId).ToList();
                    if (res != null && res.Count > 0)
                    {
                        res.ForEach(x => { x.Status = (int)AnnexureStatus.Completed; x.UpdatedOn = DateTime.Now; x.UpdatedBy = userId; });
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
    }
}
