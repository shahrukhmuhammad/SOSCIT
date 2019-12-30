using BaseApp.Entity;
using BaseApp.System;
using CPC;
using CPC.Model;
using ImageResizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using WebApp.Controllers;

namespace WebApp.Areas.CPC.Controllers
{
    [AppAuthorize(AppPermission.All, AppPermission.CPC, BaseApp.Entity.AppPermission.ViewOrderRequests, BaseApp.Entity.AppPermission.OrderRequests)]

    public class OrderApprovalController : AppController
    {
        private OrderbookingEntity orderbookingRepo;
        private BranchEntity branchRepo;
        private Common commonRepo;
        private CPHEntity cashpPocessinHousegRepo;
        private EmployeeEntity employeeRepo;
        private VehicleEntity vehicleRepo;

        public OrderApprovalController()
        {
            orderbookingRepo = new OrderbookingEntity();
            branchRepo = new BranchEntity();
            commonRepo = new Common();
            cashpPocessinHousegRepo = new CPHEntity();
            employeeRepo = new EmployeeEntity();
            vehicleRepo = new VehicleEntity();
        }

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult _AllOrders()
        {
            var model = orderbookingRepo.GetAll();
            ViewBag.DetailsList = orderbookingRepo.GetAllDetails();
            return PartialView(model);
        }


        #region Details
        public ActionResult Details(Guid Id)
        {
            var model = orderbookingRepo.GetById(Id);
            ViewBag.Details = orderbookingRepo.GetAllDetailsById(Id);
            return View(model);
        }
        [AllowAnonymous]
        public ActionResult _ViewOrder(Guid Id)
        {
            var model = orderbookingRepo.GetById(Id);
            ViewBag.Details = orderbookingRepo.GetAllDetailsById(Id);
            if (model.VehicleId.HasValue)
                ViewBag.CrewList = employeeRepo.GetListByVehicleId(model.VehicleId.Value);
            return PartialView(model);
        }

        public ActionResult _AssignVehicle(Guid Id)
        {
            var model = orderbookingRepo.GetById(Id);
            ViewBag.Details = orderbookingRepo.GetAllDetailsById(Id);
            ViewBag.VehicleList = new SelectList(vehicleRepo.GetDropdownAvailable(), "Value", "Text");
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult _AssignVehicle(Guid OrderBookingId, Guid VehicleId)
        {
            try
            {
                if (!OrderBookingId.IsEmpty())
                {

                    #region Update Details
                    bool res = false;
                    Guid? empId = employeeRepo.GetByUserId(CurrentUser.Id);
                    if (empId.HasValue)
                    {
                        var model = new CPCOrderBooking();
                        model.UpdatedBy = CurrentUser.Id;
                        model.UpdatedOn = DateTime.Now;
                        model.Status = (int)AnnexureStatus.Approved;
                        model.VehicleId = VehicleId;
                        model.Id = OrderBookingId;
                        model.ApprovedById = empId.Value;
                        model.ApprovedOn = DateTime.Now;

                        res = orderbookingRepo.UpdateBookingVehicle(model);
                        if (res)
                            vehicleRepo.UpdateStatus(VehicleId, true);
                    }
                    else
                        return Json(new { error = "true", messageText = "You are not allowed to approve this Order." });

                    #endregion


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

                    if (res)
                    {
                        return Json(new { error = "false", messageText = "Order has been Approved Successfully." });

                    }
                    else
                        return Json(new { error = "true", messageText = "We have encountered an error while processing your request, Please see log for details." });
                }
                else
                {
                    #region Activity Log
                    //appLog.Create(CurrentUser.OfficeId, model.Id, CurrentUser.Id, AppLogType.Activity, "CRM - Lead", model.FullName + " lead updated", "~/CRM/Contact/LeadRecord > HttpPost", "<table class='table table-hover table-striped table-condensed' style='margin-bottom:15px;'><tr><th class='text-center'>Description</th></tr><tr><td><strong>" + model.FullName + "</strong> lead updated by <strong>" + CurrentUser.FullName + "</strong>.</td></tr></table>");
                    #endregion
                    return Json(new { error = "true", messageText = "We have encountered an error while processing your request, Please see log for details." });
                }
            }
            catch (Exception ex)
            {
                #region Error Log
                //appLog.Create(CurrentUser.OfficeId, null, CurrentUser.Id, AppLogType.Error, "CRM - Lead", ex.GetType().Name.ToSpacedTitleCase(), "~/CMS/FileManager/Index > HttpPost", "<table class='table table-hover table-striped'><tr><th class='text-right'>Source</th><td>" + ex.Source + "</td></tr><tr><th class='text-right'>URL</th><td>" + Request.Url.ToString() + "</td></tr><tr><th class='text-right'>Message</th><td>" + ex.Message + "</td></tr></table><table class='table table-hover table-striped table-condensed'><tr><th class='text-center'>Inner Exception</th></tr><tr><td>" + ex.InnerException + "</td></tr><tr><th class='text-center'>Stack Trace</th></tr><tr><td>" + ex.StackTrace.ToString() + "</td></tr></table>");
                #endregion

                return Json(new { error = "true", messageText = "We have encountered an error while processing your request, Please see log for details." });
            }
            //return Json(new { error = "true", messageText = "We have encountered an error while processing your request, Please see log for details." });
        }
        #endregion

        #region Order Delivery
        public ActionResult OrderDelivery(Guid Id)
        {
            var model = orderbookingRepo.GetById(Id);
            ViewBag.Details = orderbookingRepo.GetAllDetailsById(Id);
            ViewBag.EmployeeList = new SelectList(employeeRepo.GetDropdown(), "Value", "Text");
            //ViewBag.CashPointList = new SelectList(EnumHelper.GetSelectList(typeof(CashPoint)).Select(x => new CustomSelectList { Value = x.Value.ToString(), Text = x.Text.ToString() }).ToList(), "Value", "Text");
            ViewBag.CashPointList = EnumHelper.GetSelectList(typeof(CashPoint)).Select(x => new CustomSelectList { Value = x.Value.ToString(), Text = x.Text.ToString() }).ToList();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult OrderDelivery(CPCOrderBooking model, List<CPCOrderBookingDetail> CPCOrderBookingDetail)
        {
            try
            {
                if (!model.Id.IsEmpty() && CPCOrderBookingDetail != null && CPCOrderBookingDetail.Count > 0)
                {
                    #region Update Details
                    model.UpdatedBy = CurrentUser.Id;
                    model.UpdatedOn = DateTime.Now;
                    model.Status = (int)AnnexureStatus.Delivered;

                    //var lsToSave = CPCOrderBookingDetail.ToList();
                    //lsToSave.ForEach(x => { x. = Guid.NewGuid(); x.OrderbookingId = model.Id; x.CreatedOn = DateTime.Now; x.CreatedBy = CurrentUser.Id; });
                    bool res = orderbookingRepo.UpdateDelivery(model, CPCOrderBookingDetail);
                    #endregion


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

                    if (res)
                    {
                        TempData["SuccessMsg"] = "Order No " + model.OrderNo + " has been Delivered successfully.";
                    }
                    else
                        TempData["ErrorMsg"] = "We have encountered an error while processing your request, Please see log for details.";
                }
                else
                {
                    #region Activity Log
                    //appLog.Create(CurrentUser.OfficeId, model.Id, CurrentUser.Id, AppLogType.Activity, "CRM - Lead", model.FullName + " lead updated", "~/CRM/Contact/LeadRecord > HttpPost", "<table class='table table-hover table-striped table-condensed' style='margin-bottom:15px;'><tr><th class='text-center'>Description</th></tr><tr><td><strong>" + model.FullName + "</strong> lead updated by <strong>" + CurrentUser.FullName + "</strong>.</td></tr></table>");
                    #endregion
                    TempData["ErrorMsg"] = "We have encountered an error while processing your request, Please see log for details.";

                }
            }
            catch (Exception ex)
            {
                #region Error Log
                //appLog.Create(CurrentUser.OfficeId, null, CurrentUser.Id, AppLogType.Error, "CRM - Lead", ex.GetType().Name.ToSpacedTitleCase(), "~/CMS/FileManager/Index > HttpPost", "<table class='table table-hover table-striped'><tr><th class='text-right'>Source</th><td>" + ex.Source + "</td></tr><tr><th class='text-right'>URL</th><td>" + Request.Url.ToString() + "</td></tr><tr><th class='text-right'>Message</th><td>" + ex.Message + "</td></tr></table><table class='table table-hover table-striped table-condensed'><tr><th class='text-center'>Inner Exception</th></tr><tr><td>" + ex.InnerException + "</td></tr><tr><th class='text-center'>Stack Trace</th></tr><tr><td>" + ex.StackTrace.ToString() + "</td></tr></table>");
                #endregion

                TempData["ErrorMsg"] = "We have encountered an error while processing your request, Please see log for details.";
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Approve
        [HttpPost]
        public JsonResult Approve(Guid Id)
        {
            try
            {
                #region Activity Log
                //appLog.Create(CurrentUser.OfficeId, Id, CurrentUser.Id, AppLogType.Activity, "CRM", "Contact Deleted", "~/CRM/Contact/Delete > HttpPost", "<table class='table table-hover table-striped table-condensed' style='margin-bottom:15px;'><tr><th class='text-center'>Description</th></tr><tr><td>Contact deleted by <strong>" + CurrentUser.FullName + "</strong>.</td></tr></table>");
                #endregion
                Guid? empId = employeeRepo.GetByUserId(CurrentUser.Id);
                if (empId.HasValue)
                {
                    var res = orderbookingRepo.ApproveRequest(Id, empId.Value,CurrentUser.Id);
                    if (res != null)
                    {
                        TempData["SuccessMsg"] = "Order No " + res.OrderNo + " has been Approved successfully.";
                        Emailer.Send("muhammad.shahrukh@sosgroup.com.pk", "Order No " + res.OrderNo + " has been Approved successfully. by " + CurrentUser.Code + " - " + CurrentUser.FullName + ".", "Order Approved");
                    }
                    else
                        TempData["ErrorMsg"] = "We have encountered an error while processing your request, Please see log for details.";
                }
                else
                    TempData["ErrorMsg"] = "You don't have permission to approve this request.";
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
        #endregion

        #region Decline
        [HttpPost]
        public JsonResult Decline(Guid Id)
        {
            try
            {
                #region Activity Log
                //appLog.Create(CurrentUser.OfficeId, Id, CurrentUser.Id, AppLogType.Activity, "CRM", "Contact Deleted", "~/CRM/Contact/Delete > HttpPost", "<table class='table table-hover table-striped table-condensed' style='margin-bottom:15px;'><tr><th class='text-center'>Description</th></tr><tr><td>Contact deleted by <strong>" + CurrentUser.FullName + "</strong>.</td></tr></table>");
                #endregion
                Guid? empId = employeeRepo.GetByUserId(CurrentUser.Id);
                if (empId.HasValue)
                {
                    var res = orderbookingRepo.DeclineRequest(Id, empId.Value, CurrentUser.Id);
                    if (res)
                    {
                        TempData["SuccessMsg"] = "Order has been Declined successfully.";
                    }
                    else
                        TempData["ErrorMsg"] = "We have encountered an error while processing your request, Please see log for details.";
                }
                else
                    TempData["ErrorMsg"] = "You don't have permission to approve this request.";
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
        #endregion

        #region Remote Function
        [HttpGet]
        public JsonResult GetVehicleCrew(Guid id)
        {
            try
            {
                var List = employeeRepo.GetListByVehicleId(id);
                return Json(new
                {
                    Details = List.Select(x => new {
                        x.EmployeeId,
                        x.CrewName
                    }).ToList(),

                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}