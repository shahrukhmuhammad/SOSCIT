﻿using CPC.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPC
{
    public class CPHEntity
    {
        private SOSTechCPCEntities context;

        public List<CPCCashProcessingHouse> GetAll()
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.CPCCashProcessingHouses.OrderBy(x => x.Title).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CPCCashProcessingHouse GetById(Guid Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.CPCCashProcessingHouses.Where(x => x.Id == Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //
        // Summary:
        //     Represents a strongly typed list of Cash Processing House List
        //
        // Type parameters:
        //   The type of element is ProjectId.
        public List<CustomSelectList> GetDropdown(Guid? Id = null)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    var ls = new List<CPCCashProcessingHouse>();
                    if (Id.HasValue)
                    {
                        ls = context.CPCCashProcessingHouses.Where(x => x.IsActive && x.ProjectId == Id).ToList();
                    }
                    else {
                        ls = context.CPCCashProcessingHouses.Where(x => x.IsActive).ToList();
                    }
                    return ls.Select(x => new CustomSelectList { Value = x.Id.ToString(), Text = x.Title }).ToList();
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
