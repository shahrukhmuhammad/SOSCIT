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
    public class AnnexureIIEntity
    {
        private SOSTechCPCEntities context;

        public List<CPCAnnexureII> GetAll()
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.CPCAnnexureIIs.Include(x=> x.CPCAnnexureIIDetails).Include(x=> x.CPCProjectBranch).OrderBy(x => x.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CPCAnnexureII GetById(Guid Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.CPCAnnexureIIs.Include(x=>x.CPCAnnexureIIDetails.Select(y=> y.CPCDenomination)).Where(x => x.Id == Id).FirstOrDefault();
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


        #region Add/Update Annexure II
        public Guid? Create(CPCAnnexureII model)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Save Annexure II
                    context.CPCAnnexureIIs.Add(model);
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

        public bool Create(List<CPCAnnexureIIDetail> modelList)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Save CPCAnnexure II
                    context.CPCAnnexureIIDetails.AddRange(modelList);
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
                    #region Update
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
        public bool InActiveRecord(Guid Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Update Annexure
                    var res = context.CPCAnnexureIIs.Where(x => x.Id == Id).FirstOrDefault();
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
        //public bool DeleteDetails(Guid Id)
        //{
        //    try
        //    {
        //        using (context = new SOSTechCPCEntities())
        //        {
        //            #region Delete
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

        public int GetNextSrNo()
        {
            try
            {
                using (var context = new SOSTechCPCEntities())
                {
                    int? res = context.CPCAnnexureIIs.Max(x => x.SrNo);
                    if (res.HasValue)
                    {
                        return Convert.ToInt32(res) + 1;
                    }
                    return 0;
                }
            }
            catch (Exception ex)
            {
                //new Logger().LogError(ex);
                return 0;
            }
        }
    }
}
