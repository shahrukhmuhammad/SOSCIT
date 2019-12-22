using BaseApp.Entity;
using BaseApp.System;
using CPC;
using CPC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace WebApp.Controllers
{
    [AppAuthorize(AppPermission.All, AppPermission.Bank, AppPermission.ViewBank, AppPermission.OrderCollection, AppPermission.ViewOrderCollection)] //AppPermission.ViewCPC, AppPermission.CPC
    
    public class OrderbookingController : AppController
    {
        private AnnexureIEntity annexureIRepo;
        private OrderbookingEntity orderbookingRepo;
        private BranchEntity branchRepo;
        private Common commonRepo;
        private CPHEntity cashpPocessinHousegRepo;
        private EmployeeEntity employeeRepo;

        public OrderbookingController()
        {
            orderbookingRepo = new OrderbookingEntity();
            branchRepo = new BranchEntity();
            commonRepo = new Common();
            cashpPocessinHousegRepo = new CPHEntity();
            employeeRepo = new EmployeeEntity();
            annexureIRepo = new AnnexureIEntity();
        }
        public ActionResult Orderbookings()
        {
            return View();
        }
        public PartialViewResult _AllOrderbookings()
        {
            var model = orderbookingRepo.GetAll();
            ViewBag.DetailsList = orderbookingRepo.GetAllDetails();
            ViewBag.EmployeeList = employeeRepo.GetAll();
            return PartialView(model);
        }

        #region Details
        public ActionResult Details(Guid Id)
        {
            var model = orderbookingRepo.GetById(Id);
            ViewBag.Details = orderbookingRepo.GetAllDetailsById(Id);
            ViewBag.AnnexureIDetails = annexureIRepo.GetAllDetailsByOrderNo(model.OrderNo);
            return View(model);
        }
        #endregion
        #region Record
        public ActionResult Record(Guid? Id)
        {
            var model = new CPCOrderBooking();
            if (Id.HasValue)
            {
                model = orderbookingRepo.GetById(Id.Value);
            }
            else
            {
                model.Date = DateTime.Now;
                model.OrderNo = orderbookingRepo.GetNextSrNo();
            }
            ViewBag.BrachList = new SelectList(branchRepo.GetDropdown(), "Value", "Text");
            ViewBag.DenominationList = new SelectList(commonRepo.GetAllDenominationDropdown().Where(x => x.Text != Convert.ToString(1) && x.Text != Convert.ToString(2) && x.Text != Convert.ToString(5)), "Value", "Text");
            //ViewBag.CPHList = new SelectList(cashpPocessinHousegRepo.GetDropdown(), "Value", "Text");
            //ViewBag.CashPointList = new SelectList(GetCashPointDropdown(), "Value", "Text");
            //ViewBag.ProjectList = new SelectList(commonRepo.GetAllProjectsDropdown(), "Value", "Text");
            return View(model);
        }
        public static List<CustomSelectList> GetCashPointDropdown()
        {
            try
            {
                var ls = EnumHelper.GetSelectList(typeof(CashPoint));
                return ls.Select(x => new CustomSelectList { Value = x.Value.ToString(), Text = x.Text.ToString() }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Record(CPCOrderBooking model, List<CPCOrderBookingDetail> CPCOrderBookingDetail, string Date)
        {
            try
            {
                if (model.Id.IsEmpty())
                {
                    model.CreatedBy = CurrentUser.Id;
                    model.CreatedOn = DateTime.Now;
                    model.IsActive = true;
                    model.Status = (int)AnnexureStatus.Inprocess;
                    model.Id = Guid.NewGuid();
                    model.Date = Utils.SetDateFormate(Date);
                    model.OrderNo = orderbookingRepo.GetNextSrNo();

                    var res = orderbookingRepo.Create(model);
                    if (res.HasValue)
                    {
                        var lsToSave = CPCOrderBookingDetail.Where(x => x.DenominationId.HasValue && x.NoOfBundles > 0).ToList();
                        lsToSave.ForEach(x => { x.Id = Guid.NewGuid(); x.OrderbookingId = model.Id; x.CreatedOn = DateTime.Now; x.CreatedBy = CurrentUser.Id; });
                        #region Save Details
                        orderbookingRepo.Create(lsToSave);
                        #endregion
                        model.Id = res.Value;
                    }

                    #region Activity Log
                    //appLog.Create(CurrentUser.OfficeId, model.Id, CurrentUser.Id, AppLogType.Activity, "CRM - Lead", model.FullName + " lead created", "~/CRM/Contact/LeadRecord > HttpPost", "<table class='table table-hover table-striped table-condensed' style='margin-bottom:15px;'><tr><th class='text-center'>Description</th></tr><tr><td><strong>" + model.FullName + "</strong> lead created by <strong>" + CurrentUser.FullName + "</strong>.</td></tr></table>");
                    #endregion

                    #region EmailSending

                    //Emailer.Send(model.Email, EmailTemplateType.ContactSignup, model);

                    //var appMessageModel = new AppMessage();
                    //appMessageModel.Email = model.Email;
                    //appMessageModel.Status = AppMessageStatus.Outbox;
                    //appMessageModel.Subject = EmailTemplateType.ContactSignup;
                    //appMessageModel.Message = EmailTemplateType.ContactSignup;
                    //TempData["SuccessMsg"] = "Message has been sent successfully.";
                    //realtime.UpdateMessages(null);
                    //appMessageModel.CreatorId = appMessageModel.ContactId = CurrentUser.Id;
                    //appMessageModel.Id = appMsg.Create(appMessageModel);

                    ////var cntact = appUser.GetUserById(new Guid(x));
                    //appMsg.Create(null, appMessageModel.Id, appMessageModel.CreatorId, null, AppMessageStatus.Inbox, EmailTemplateType.ContactSignup, model.Id, null);

                    //realtime.UpdateMessages(null);
                    #endregion

                    TempData["SuccessMsg"] = "Order No " + model.OrderNo + " has been booked successfully.";
                }
                else
                {
                    //model.UpdatedBy = CurrentUser.Id;
                    //model.UpdatedOn = DateTime.Now;
                    //bool res = orderbookingRepo.Update(model);


                    //if (res)
                    //{
                    //    TempData["SuccessMsg"] = model.SrNo + " has been updated successfully.";
                    //}
                    //else
                    //{
                    //    TempData["ErrorMsg"] = "We have encountered an error while processing your request, Please see log for details.";
                    //}
                    #region Activity Log
                    //appLog.Create(CurrentUser.OfficeId, model.Id, CurrentUser.Id, AppLogType.Activity, "CRM - Lead", model.FullName + " lead updated", "~/CRM/Contact/LeadRecord > HttpPost", "<table class='table table-hover table-striped table-condensed' style='margin-bottom:15px;'><tr><th class='text-center'>Description</th></tr><tr><td><strong>" + model.FullName + "</strong> lead updated by <strong>" + CurrentUser.FullName + "</strong>.</td></tr></table>");
                    #endregion

                }
            }
            catch (Exception ex)
            {
                #region Error Log
                //appLog.Create(CurrentUser.OfficeId, null, CurrentUser.Id, AppLogType.Error, "CRM - Lead", ex.GetType().Name.ToSpacedTitleCase(), "~/CMS/FileManager/Index > HttpPost", "<table class='table table-hover table-striped'><tr><th class='text-right'>Source</th><td>" + ex.Source + "</td></tr><tr><th class='text-right'>URL</th><td>" + Request.Url.ToString() + "</td></tr><tr><th class='text-right'>Message</th><td>" + ex.Message + "</td></tr></table><table class='table table-hover table-striped table-condensed'><tr><th class='text-center'>Inner Exception</th></tr><tr><td>" + ex.InnerException + "</td></tr><tr><th class='text-center'>Stack Trace</th></tr><tr><td>" + ex.StackTrace.ToString() + "</td></tr></table>");
                #endregion

                TempData["ErrorMsg"] = "We have encountered an error while processing your request, Please see log for details.";
            }
            //if (SaveClick == "SaveAddNew")
            //{
            //    return RedirectToAction("Record");
            //}
            //else
            //{
            return RedirectToAction("Orderbookings");
        }
        #endregion

        #region Remote Functions

        [HttpGet]
        public JsonResult GetAnnexureI(Guid Id, string date)
        {
            //var ls = orderbookingRepo.GetByDateBranchId(Id, Utils.SetDateFormate(date));
            //if (ls != null && ls.Count > 0)
            //{
            //    return Json(new
            //    {
            //        ShipmentReciptNo = ls.FirstOrDefault().ShipmentReciptNo,
            //        SealNo = ls.FirstOrDefault().SealNo,
            //        Id = ls.FirstOrDefault().Id,
            //        details = ls.Select(x => new { x.DenominationId, x.DenominationTitle, }).OrderBy(x => x.DenominationTitle)
            //    }, JsonRequestBehavior.AllowGet);
            //}
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult FetchCashProcessingHouses(Guid Id)
        {
            try
            {
                var List = branchRepo.GetDropdown(Id);
                return Json(cashpPocessinHousegRepo.GetDropdown(Id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
                //throw ex;
            }
        }
        [HttpGet]
        public JsonResult FetchHirerachy(Guid id)
        {
            try
            {
                var List = branchRepo.GetDropdown(id);
                return Json(branchRepo.GetDropdown(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Orderbooking Popup
        public ActionResult _OrdersList()
        {
            var model = orderbookingRepo.GetAllApproved();
            ViewBag.DetailsList = orderbookingRepo.GetAllDetails();
            return PartialView(model);
        }
        #endregion

        [HttpGet]
        public JsonResult GetOrderBookingData(Guid id)
        {
            try
            {
                var List = orderbookingRepo.GetAllDetailsById(id).Where(x => x.Status == (int)AnnexureStatus.Approved);
                return Json(new
                {
                    List.FirstOrDefault().OrderNo,
                    List.FirstOrDefault().Id,
                    Details = List.Select(x => new {
                        x.ProjectId,
                        x.CPCProjectName,
                        x.CashProcessingCellId,
                        x.CashProcessingCellTitle,
                        x.ProjectBranchId,
                        x.BranchCode,
                        x.BranchName,
                        x.DenominationId,
                        x.DenominationTitle,
                        x.NoOfBundles,
                        x.TotalAmount
                    }).ToList(),

                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult RequestCash(Guid Id)
        {
            try
            {
                orderbookingRepo.ChangeStatus(Id,CurrentUser.Id, AnnexureStatus.PendingDelivery);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult ConfirmReceived(Guid Id)
        {
            try
            {
                orderbookingRepo.ChangeStatus(Id, CurrentUser.Id, AnnexureStatus.Completed);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
        #region Delete
        [HttpPost]
        public JsonResult Delete(Guid Id)
        {
            try
            {
                #region Activity Log
               //appLog.Create(CurrentUser.OfficeId, Id, CurrentUser.Id, AppLogType.Activity, "CRM", "Contact Deleted", "~/CRM/Contact/Delete > HttpPost", "<table class='table table-hover table-striped table-condensed' style='margin-bottom:15px;'><tr><th class='text-center'>Description</th></tr><tr><td>Contact deleted by <strong>" + CurrentUser.FullName + "</strong>.</td></tr></table>");
                #endregion
                orderbookingRepo.InActiveRecord(Id);
                TempData["SuccessMsg"] = "Order has been deleted successfully.";
            }
            catch (Exception ex)
            {
                #region Error Log
                //appLog.Create(CurrentUser.OfficeId, null, CurrentUser.Id, AppLogType.Error, "CRM", ex.GetType().Name.ToSpacedTitleCase(), "~/CRM/Contact/Delete > HttpPost", "<table class='table table-hover table-striped'><tr><th class='text-right'>Source</th><td>" + ex.Source + "</td></tr><tr><th class='text-right'>URL</th><td>" + Request.Url.ToString() + "</td></tr><tr><th class='text-right'>Message</th><td>" + ex.Message + "</td></tr></table><table class='table table-hover table-striped table-condensed'><tr><th class='text-center'>Inner Exception</th></tr><tr><td>" + ex.InnerException + "</td></tr><tr><th class='text-center'>Stack Trace</th></tr><tr><td>" + ex.StackTrace.ToString() + "</td></tr></table>");
                #endregion

                TempData["ErrorMsg"] = "We have encountered an error while processing your request, Please see log for details.";
            }
            return Json(true);
        }
        //[HttpPost]
        //public JsonResult Delete(Guid Id)
        //{
        //    try
        //    {
        //        #region Activity Log
        //        //appLog.Create(CurrentUser.OfficeId, Id, CurrentUser.Id, AppLogType.Activity, "CRM", "Contact Deleted", "~/CRM/Contact/Delete > HttpPost", "<table class='table table-hover table-striped table-condensed' style='margin-bottom:15px;'><tr><th class='text-center'>Description</th></tr><tr><td>Contact deleted by <strong>" + CurrentUser.FullName + "</strong>.</td></tr></table>");
        //        #endregion
        //        orderbookingRepo.Delete(Id);

        //        TempData["SuccessMsg"] = "Department has been deleted successfully.";
        //    }
        //    catch (Exception ex)
        //    {
        //        #region Error Log
        //        //appLog.Create(CurrentUser.OfficeId, null, CurrentUser.Id, AppLogType.Error, "CRM", ex.GetType().Name.ToSpacedTitleCase(), "~/CRM/Contact/Delete > HttpPost", "<table class='table table-hover table-striped'><tr><th class='text-right'>Source</th><td>" + ex.Source + "</td></tr><tr><th class='text-right'>URL</th><td>" + Request.Url.ToString() + "</td></tr><tr><th class='text-right'>Message</th><td>" + ex.Message + "</td></tr></table><table class='table table-hover table-striped table-condensed'><tr><th class='text-center'>Inner Exception</th></tr><tr><td>" + ex.InnerException + "</td></tr><tr><th class='text-center'>Stack Trace</th></tr><tr><td>" + ex.StackTrace.ToString() + "</td></tr></table>");
        //        #endregion

        //        TempData["ErrorMsg"] = "We have encountered an error while processing your request, Please see log for details.";
        //    }
        //    return Json(true);
        //}

        //[HttpPost]
        //public JsonResult DeleteMultiple(string Ids)
        //{
        //    try
        //    {
        //        var idsList = Ids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        //        if (!string.IsNullOrEmpty(Ids))
        //        {
        //            var lsIds = new List<Guid>();
        //            foreach (var x in idsList)
        //            {
        //                lsIds.Add(new Guid(x));
        //            }
        //            orderbookingRepo.DeleteMultiple(lsIds);
        //        }

        //        #region Activity Log
        //        //appLog.Create(CurrentUser.OfficeId, null, CurrentUser.Id, AppLogType.Activity, "CRM", "Multiple Contacts Deleted", "~/CRM/Contact/DeleteMultiple > HttpPost", "<table class='table table-hover table-striped table-condensed' style='margin-bottom:15px;'><tr><th class='text-center'>Description</th></tr><tr><td>Multiple contacts deleted by <strong>" + CurrentUser.FullName + "</strong>.</td></tr></table>");
        //        #endregion

        //        TempData["SuccessMsg"] = "Selected department has been deleted successfully.";
        //    }
        //    catch (Exception ex)
        //    {
        //        #region Error Log
        //        //appLog.Create(CurrentUser.OfficeId, null, CurrentUser.Id, AppLogType.Error, "CRM", ex.GetType().Name.ToSpacedTitleCase(), "~/CRM/Contact/DeleteMultiple > HttpPost", "<table class='table table-hover table-striped'><tr><th class='text-right'>Source</th><td>" + ex.Source + "</td></tr><tr><th class='text-right'>URL</th><td>" + Request.Url.ToString() + "</td></tr><tr><th class='text-right'>Message</th><td>" + ex.Message + "</td></tr></table><table class='table table-hover table-striped table-condensed'><tr><th class='text-center'>Inner Exception</th></tr><tr><td>" + ex.InnerException + "</td></tr><tr><th class='text-center'>Stack Trace</th></tr><tr><td>" + ex.StackTrace.ToString() + "</td></tr></table>");
        //        #endregion

        //        TempData["ErrorMsg"] = "We have encountered an error while processing your request, Please see log for details.";
        //    }
        //    return Json(true);
        //}
        #endregion


    }
}