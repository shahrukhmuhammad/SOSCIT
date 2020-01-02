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
    public class VehicleEntity
    {
        private SOSTechCPCEntities context;

        public List<CITVehicle> GetAllActive()
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.CITVehicles.Where(x=> x.IsActive).OrderBy(x => x.VehicleNumber).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string AuthnticateVehicle(VehicleLoginModel objUser)
        {
            try
            {
                using (var context = new SOSTechCPCEntities())
                {
                    context.Configuration.LazyLoadingEnabled = false;
                    //var scc = context.CITVehicles.FirstOrDefault(u => u.IsActive == true && u.VehicleNumber == objUser.vehicleNumber.Trim());

                    var dbUser = context.CITVehicles.FirstOrDefault(u => u.IsActive == true && u.VehicleNumber.Trim() == objUser.vehicleNumber.Trim());
                    if (dbUser == null)
                        return "Vehicle does not exist";
                    else
                    {
                        var backUser = context.AppUsers.FirstOrDefault(x => x.VehicleId == dbUser.Id && x.Password == objUser.password.Trim());
                        if (backUser != null)
                        {
                            return "Login Successfully!";
                        }
                        else
                            return "Incorrect Password, try again with correct one";
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error : " + ex.Message;
            }
        }
        //public List<CITVehicle> GetById(Guid Id)
        //{
        //    try
        //    {
        //        using (context = new SOSTechCPCEntities())
        //        {
        //            return context.CPCProjectBranches.Include(y => y.CPCCity).Where(x => x.Id == Id).FirstOrDefault();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

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

        public List<CustomSelectList> GetDropdownAvailable(Guid? Id = null)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    var ls = new List<CITVehicle>();
                    if (Id.HasValue)
                        ls = context.CITVehicles.Where(x => x.CityId == Id && x.IsActive == true && x.IsAssigned == false).ToList();
                    else
                        ls = context.CITVehicles.Where(x => x.IsActive && x.IsAssigned == false).ToList();
                    return ls.Select(x => new CustomSelectList { Value = x.Id.ToString(), Text = x.VehicleNumber }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CustomSelectList> GetDropdownAll(Guid? Id = null)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    var ls = new List<CITVehicle>();
                    if (Id.HasValue)
                        ls = context.CITVehicles.Where(x => x.CityId == Id && x.IsActive).ToList();
                    else
                        ls = context.CITVehicles.Where(x => x.IsActive).ToList();
                    return ls.Select(x => new CustomSelectList { Value = x.Id.ToString(), Text = x.VehicleNumber }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateStatus(Guid Id, bool Status)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    var resObject = context.CITVehicles.Where(x => x.Id == Id).FirstOrDefault();
                    if (resObject != null)
                    {
                        resObject.IsAssigned = Status;
                        context.SaveChanges();
                    }
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

    public class VehicleLoginModel
    {
        public string vehicleNumber { get; set; }
        public string password { get; set; }
    }
}
