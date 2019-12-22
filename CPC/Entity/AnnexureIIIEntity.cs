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
    public class AnnexureIIIEntity
    {
        private SOSTechCPCEntities context;

        public List<CPCAnnexureIII> GetAll()
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.CPCAnnexureIIIs.Include(x=> x.CPCProjectBranch).Where(x=>x.IsActive).OrderBy(x => x.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CPCAnnexureIIIDetail> GetDetailsByMasterId(Guid Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.CPCAnnexureIIIDetails.Where(x=> x.AnnexureIIIId == Id).OrderBy(x => x.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CPCAnnexureIII GetById(Guid Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.CPCAnnexureIIIs.Include(x=> x.CPCAnnexureIIIDetails).Where(x => x.Id == Id).FirstOrDefault();
                    //(from anxIII in context.CPCAnnexureIIs
                    // join anxID in context.CPCAnnexureIDetails on anxIII.CPCAnnexureIId equals anxID.AnnexureIId
                    // where Id == anxID.AnnexureIId
                    // select new
                    // {
                    //     UID = e.OwnerID,
                    //     TID = e.TID,
                    //     Title = t.Title,
                    //     EID = e.EID
                    // }).Take(10);
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


        #region Add/Update Annexure III
        public Guid? Create(CPCAnnexureIII model)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Save Annexure III
                    context.CPCAnnexureIIIs.Add(model);
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

        public bool Create(List<CPCAnnexureIIIDetail> modelList)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Save CPCAnnexure III
                    context.CPCAnnexureIIIDetails.AddRange(modelList);
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
        public bool DeleteDetails(Guid Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Delete
                    var res = context.CPCAnnexureIIIDetails.Where(x => x.AnnexureIIIId == Id).ToList();
                    if (res != null)
                    {
                        context.CPCAnnexureIIIDetails.RemoveRange(res);
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

        public bool InActiveRecord(Guid Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Update Annexure
                    var res = context.CPCAnnexureIIIs.Where(x => x.Id == Id).FirstOrDefault();
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
                    int? res = context.CPCAnnexureIIIs.Max(x => x.SrNo);
                    if (res.HasValue)
                    {
                        return Convert.ToInt32(res) + 1;
                    }
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
