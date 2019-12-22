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
    public class BranchEntity
    {
        private SOSTechCPCEntities context;

        public List<CPCProjectBranch> GetAll()
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.CPCProjectBranches.OrderBy(x => x.BranchCode).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CPCProjectBranch GetById(Guid Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.CPCProjectBranches.Include(y=> y.CPCCity).Where(x => x.Id == Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CustomSelectList> GetDropdown(Guid? Id = null)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    var ls = new List<CPCProjectBranch>();
                    if (Id.HasValue)
                    {
                        ls = context.CPCProjectBranches.Where(x => x.CPHId == Id).ToList();
                        return ls.Select(x => new CustomSelectList { Value = x.Id.ToString(), Text = x.BranchCode + " - " + x.BranchName }).ToList();
                    }
                    ls = context.CPCProjectBranches.ToList();
                    return ls.Select(x => new CustomSelectList { Value = x.Id.ToString(), Text = x.BranchCode + " - " + x.BranchName }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region Add/Update Branch
        //public Guid? Create(CPCAnnexureI model)
        //{
        //    try
        //    {
        //        using (context = new SOSTechCPCEntities())
        //        {
        //            #region Save Department
        //            context.CPCAnnexureIs.Add(model);
        //            context.SaveChanges();
        //            #endregion
        //            return model.Id;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public bool Create(List<CPCAnnexureIDetail> modelList)
        //{
        //    try
        //    {
        //        using (context = new SOSTechCPCEntities())
        //        {
        //            #region Save Department
        //            context.CPCAnnexureIDetails.AddRange(modelList);
        //            context.SaveChanges();
        //            #endregion
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //            return false;
        //    }
        //}

        //public bool Update(CPCAnnexureI model)
        //{
        //    try
        //    {
        //        using (context = new SOSTechCPCEntities())
        //        {
        //            #region Update Employee
        //            var res = context.CPCAnnexureIs.Where(x => x.Id == model.Id).FirstOrDefault();
        //            if (res != null)
        //            {
        //                res.CashHandedOverCPCStaffAId = model.CashHandedOverCPCStaffAId;
        //                res.CashHandedOverCPCStaffBId = model.CashHandedOverCPCStaffBId;
        //                res.CashHandedOverCITStaffAId = model.CashHandedOverCITStaffAId;
        //                res.CashHandedOverCITStaffBId = model.CashHandedOverCITStaffBId;
        //                res.SignatureCPCHandingOverCashAId = model.SignatureCPCHandingOverCashAId;
        //                res.SignatureCPCHandingOverCashBId = model.SignatureCPCHandingOverCashBId;
        //                res.UpdatedOn = model.UpdatedOn;
        //                res.UpdatedBy = model.UpdatedBy;
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

        #region Delete
        //public bool DeleteDetails(Guid Id)
        //{
        //    try
        //    {
        //        using (context = new SOSTechCPCEntities())
        //        {
        //            #region Update Employee
        //            var res = context.CPCAnnexureIDetails.Where(x => x.AnnexureIId == Id).ToList();
        //            if (res != null)
        //            {
        //                context.CPCAnnexureIDetails.RemoveRange(res);
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

        //public int GetNextSrNo()
        //{
        //    try
        //    {
        //        using (var context = new SOSTechCPCEntities())
        //        {
        //            return context.CPCAnnexureIs.Max(x => x.SrNo) <= 0 ? 1 : (int)context.CPCAnnexureIs.Max(x => x.SrNo) + 1;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //new Logger().LogError(ex);
        //        return 0;
        //    }
        //}
    }
}
