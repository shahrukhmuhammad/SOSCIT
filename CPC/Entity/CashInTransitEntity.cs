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
    public class CashInTransitEntity
    {
        private SOSTechCPCEntities context;

        public List<CPCCashInTransit> GetAll()
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    //return context.CPCCashInTransits.OrderBy(x => x.Date).ToList();
                    return context.CPCCashInTransits.Where(x => x.IsActive).OrderBy(x => x.Id).ToList();
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
        public CPCCashInTransit GetById(Guid Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    //return context.CPCCashInTransits.Where(x => x.Id == Id).FirstOrDefault();
                     return context.CPCCashInTransits.Include(x=> x.CPCCashInTransitChilds).Include(x=> x.CPCCashInTransitDenominations.Select(y=> y.CPCDenomination)).Where(x => x.Id == Id).FirstOrDefault();
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
        public Guid? Create(CPCCashInTransit model)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Save Department
                    int shipmentNumber = GetNextShipmentNo();
                    model.ShipmentReceiptNumber = shipmentNumber;
                    model.Status = 1;
                    context.CPCCashInTransits.Add(model);
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

        public bool Create(List<CPCCashInTransitChild> modelList)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Save Department
                    context.CPCCashInTransitChilds.AddRange(modelList);
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

        public bool Create(List<CPCCashInTransitDenomination> modelList)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Save Department
                    context.CPCCashInTransitDenominations.AddRange(modelList);
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
                    var res = context.CPCCashInTransits.Where(x => x.Id == model.Id).FirstOrDefault();
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

        public int GetNextShipmentNo()
        {
            try
            {
                using (var context = new SOSTechCPCEntities())
                {
                    return context.CPCCashInTransits.Max(x => x.ShipmentReceiptNumber) <= 0 ? 1 : (int)context.CPCCashInTransits.Max(x => x.ShipmentReceiptNumber) + 1;
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
                    var res = context.CPCCashInTransits.Where(x => x.Id == Id).FirstOrDefault();
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
    }
}
