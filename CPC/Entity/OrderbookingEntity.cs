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
    public class OrderbookingEntity
    {
        private SOSTechCPCEntities context;

        public List<CPCOrderBooking> GetAll()
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.CPCOrderBookings.Where(x => x.IsActive).OrderBy(x => x.CreatedOn).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CPCOrderBooking> GetAllApproved()
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.CPCOrderBookings.Where(x => x.IsActive && x.Status == (int)AnnexureStatus.Approved).OrderBy(x => x.CreatedOn).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Vew_Orderbookings> GetAllDetails()
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.Vew_Orderbookings.Where(x => x.IsActive).ToList();

                    //from m in context.Vew_Orderbookings
                    //where m.IsActive == true
                    //group m by m.OrderNo into g
                    //let TotalPoints = g.Sum(m => m.NoOfBundles)
                    //let TotalAmount = g.Sum(m => m.TotalAmount)
                    //orderby TotalPoints descending
                    //select new {  = g.Key, Username = g.Key.Username, TotalPoints };

                    //var query = context.Vew_Orderbookings.Select(u => new Vew_Orderbookings
                    //{
                    //    OrderNo = u.OrderNo,
                    //    TotalAmount = Convert.ToInt32(u.TotalAmount);
                    //}).ToList().OrderByDescending(u => u.MovementCount).Select(u => u.Cat).ToList();

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Vew_Orderbookings> GetAllDetailsById(Guid Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.Vew_Orderbookings.Where(x => x.Id == Id).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //public bool IsDuplicate(int SrNo)
        //{
        //    try
        //    {
        //        using (context = new SOSTechCPCEntities())
        //        {
        //            return context.CPCOrderBookings.Where(x => x.OrderNo == SrNo).FirstOrDefault() != null ? true : false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return true;
        //    }
        //}
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
        public CPCOrderBooking GetById(Guid Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    return context.CPCOrderBookings.Where(x => x.Id == Id).FirstOrDefault();
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
        public Guid? Create(CPCOrderBooking model)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Save
                    context.CPCOrderBookings.Add(model);
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

        public bool UpdateDelivery(CPCOrderBooking model, List<CPCOrderBookingDetail> details)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Save
                    var dbMaster = context.CPCOrderBookings.Where(x => x.Id == model.Id).FirstOrDefault();
                    if (dbMaster != null)
                    {
                        dbMaster.UpdatedBy = model.UpdatedBy;
                        dbMaster.UpdatedOn = model.UpdatedOn;
                        dbMaster.Status = model.Status;
                        context.SaveChanges();
                        //Update Details
                        var dbDetails = context.CPCOrderBookingDetails.Where(x => x.OrderbookingId == model.Id).ToList();
                        //details.ForEach(x =>
                        //{
                        //    x.CashDeliveryPoint = details.Where(y => y.Id == x.Id).FirstOrDefault() != null ? details.Where(y => y.Id == x.Id).FirstOrDefault().CashDeliveryPoint : null;
                        //});
                        foreach (var item in dbDetails)
                        {
                            var obj = details.Where(y => y.Id == item.Id).FirstOrDefault();
                            item.CashDeliveryPoint = obj != null ? obj.CashDeliveryPoint : null;
                        }
                        context.SaveChanges();
                    }
                    
                    #endregion
                    //return model.Id;
                    return true;
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                return false;
            }
        }

        public bool UpdateBookingVehicle(CPCOrderBooking model)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Save
                    var dbMaster = context.CPCOrderBookings.Where(x => x.Id == model.Id).FirstOrDefault();
                    if (dbMaster != null)
                    {
                        dbMaster.UpdatedBy = model.UpdatedBy;
                        dbMaster.UpdatedOn = model.UpdatedOn;
                        dbMaster.Status = model.Status;
                        dbMaster.VehicleId = model.VehicleId;
                        dbMaster.ApprovedById = model.ApprovedById;
                        dbMaster.ApprovedOn = model.ApprovedOn;
                        context.SaveChanges();
                    }
                    #endregion
                    return true;
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                return false;
            }
        }

        public bool Create(List<CPCOrderBookingDetail> modelList)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Save
                    context.CPCOrderBookingDetails.AddRange(modelList);
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
                    var res = context.CPCOrderBookings.Where(x => x.Id == model.Id).FirstOrDefault();
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

        #region Request Approve
        public CPCOrderBooking ApproveRequest(Guid Id, Guid EmployeeId, Guid UserId)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Update Status
                    var res = context.CPCOrderBookings.Where(x => x.Id == Id).FirstOrDefault();
                    if (res != null)
                    {
                        res.ApprovedById = EmployeeId;
                        res.ApprovedOn = DateTime.Now;
                        res.UpdatedBy = UserId;
                        res.UpdatedOn = DateTime.Now;
                        res.Status = (int)AnnexureStatus.Approved;
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
        #endregion

        #region Change Status
        //public void ChangeStatus(Guid bookingId, Guid UserId)
        //{
        //    try
        //    {
        //        using (context = new SOSTechCPCEntities())
        //        {
        //            #region Update Record
        //            var res = context.CPCOrderBookings.Where(x => x.Id == bookingId).FirstOrDefault();
        //            if (res != null)
        //            {
        //                res.UpdatedOn = DateTime.Now;
        //                res.UpdatedBy = UserId;
        //                res.Status = (int)AnnexureStatus.Completed;
        //                context.SaveChanges();
        //            }
        //            #endregion
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        #endregion

        #region Delete
        public bool InActiveRecord(Guid Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Update Status
                    var res = context.CPCOrderBookings.Where(x => x.Id == Id).FirstOrDefault();
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
        public bool DeleteDetails(Guid Id)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Update
                    var res = context.CPCOrderBookingDetails.Where(x => x.Id == Id).ToList();
                    if (res != null)
                    {
                        context.CPCOrderBookingDetails.RemoveRange(res);
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

        #region Request Decline
        public bool DeclineRequest(Guid Id, Guid EmployeeId, Guid UserId)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Update Status
                    var res = context.CPCOrderBookings.Where(x => x.Id == Id).FirstOrDefault();
                    if (res != null)
                    {
                        res.ApprovedById = EmployeeId;
                        res.ApprovedOn = DateTime.Now;
                        res.UpdatedBy = UserId;
                        res.UpdatedOn = DateTime.Now;
                        res.Status = (int)AnnexureStatus.Rejected;
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

        public int GetNextSrNo()
        {
            try
            {
                using (var context = new SOSTechCPCEntities())
                {
                    //int? res = context.CPCOrderBookings.Max(x => x.OrderNo);
                    int? res = context.CPCOrderBookings.Max(u => (int?)u.OrderNo);

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

        public bool avaiableRecord(Guid? bookingId)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region inactive OrderBooking status
                    var res = context.CPCOrderBookings.Where(x => x.Id == bookingId).FirstOrDefault();
                    if (res != null)
                    {
                        res.Status = (int)AnnexureStatus.Approved;
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

        #region Update Status
        public void ChangeStatus(Guid? bookingId, Guid UserId, AnnexureStatus status)
        {
            try
            {
                using (context = new SOSTechCPCEntities())
                {
                    #region Update Child
                    var res = context.CPCOrderBookings.Where(x => x.Id == bookingId).FirstOrDefault();
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
        #endregion
    }
}
