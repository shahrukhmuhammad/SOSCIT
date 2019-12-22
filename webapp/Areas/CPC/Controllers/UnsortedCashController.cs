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

namespace WebApp.Areas.CPC.Controllers
{
    [AppAuthorize(AppPermission.All, AppPermission.ViewCPC, AppPermission.CPC, AppPermission.ViewVaultManager, AppPermission.VaultManager)]
    public class UnsortedCashController : AppController
    {
        private UnsortedCashEntity unsortedCashRepo;
        private AnnexureIEntity annexureIrepo;
        private EmployeeEntity employeeRepo;
        private BranchEntity branchRepo;
        private Common commonRepo;

        public UnsortedCashController()
        {
            unsortedCashRepo = new UnsortedCashEntity();
            annexureIrepo = new AnnexureIEntity();
            employeeRepo = new EmployeeEntity();
            branchRepo = new BranchEntity();
            commonRepo = new Common();
        }
        public ActionResult UnsortedCashs()
        {
            return View();
        }
        public PartialViewResult _AllUnsortedCashs()
        {
            var model = unsortedCashRepo.GetAll();
            return PartialView(model);
        }

        #region Details

        public ActionResult Details(Guid Id)
        {
            var model = unsortedCashRepo.GetById(Id);
            ViewBag.Employees = employeeRepo.GetAll();
            ViewBag.DenominationList = commonRepo.GetAllDenomination();
            var branchInfo = branchRepo.GetById(model.ProjectBranchId);
            ViewData["BranchName"] = $"{branchInfo.BranchCode} - {branchInfo.BranchName}";
            return View(model);
        }
        #endregion

        #region Record
        public ActionResult Record(Guid? Id)
        {
            var model = new CPCUnsortedCash();
            if (Id.HasValue)
            {
                model = unsortedCashRepo.GetById(Id.Value);
            }
            else
            {
                model.SerialNumber = unsortedCashRepo.GetNextSerialNumber();
                model.Date = DateTime.Now;
                //model.IsActive = true;
            }
            //ViewBag.EmployeeList = new SelectList(employeeRepo.GetDropdown(), "Value", "Text");
            //ViewBag.DenominationList = new SelectList(commonRepo.GetAllDenominationDropdown(), "Value", "Text");
            ViewBag.EmployeeList = new SelectList(employeeRepo.GetDropdown(), "Value", "Text");
            ViewBag.DenominationList = commonRepo.GetAllDenomination();
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Record(CPCUnsortedCash model, List<CPCUnsortedCashDetail> CPCUnsortedCashDetail, string Date)
        {
            try
            {
                if (model.Id.IsEmpty())
                {
                    model.CreatedBy = CurrentUser.Id;
                    model.CreatedOn = DateTime.Now;
                    model.IsActive = true;
                    model.Status =(int)AnnexureStatus.Inprocess;
                    model.Date = Utils.SetDateFormate(Date);
                    model.Id = Guid.NewGuid();
                    var res = unsortedCashRepo.Create(model);
                    if (res.HasValue)
                    {
                        var lsToSave = CPCUnsortedCashDetail.Where(x => x.NumberOfBundles > 0 && (x.TotalValue != 0)).ToList();
                        lsToSave.ForEach(x => { x.Id = Guid.NewGuid(); x.UnsortedCashId = model.Id; x.CreatedOn = DateTime.Now; x.CreatedBy = CurrentUser.Id; });
                        #region Save Details
                        unsortedCashRepo.Create(lsToSave);
                        #endregion

                        annexureIrepo.ChangeStatus(model.OrderBookingId, CurrentUser.Id, model.ProjectBranchId, AnnexureStatus.Proceeded);
                        //annexureIrepo.ChangeMasterStatus(model.OrderBookingId, CurrentUser.Id);
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

                    TempData["SuccessMsg"] = model.SerialNumber + " has been created successfully.";
                }
                else
                {
                    //model.UpdatedBy = CurrentUser.Id;
                    //model.UpdatedOn = DateTime.Now;
                    //bool res = annexureIIRepo.Update(model);


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
            return RedirectToAction("UnsortedCashs");
        }
        #endregion

        #region Annexure Popup
        public ActionResult _UnsortedCashList()
        {
            var model = unsortedCashRepo.GetAllApproved();
            ViewBag.DetailsList = unsortedCashRepo.GetAllDetails();
            return PartialView(model);
        }
        #endregion

        #region Remote Functions
        [HttpGet]
        public JsonResult GetUnsortedCash(Guid id)
        {
            try
            {
                var List = unsortedCashRepo.GetAllDetailsById(id);
                return Json(new
                {
                    List.FirstOrDefault().ProjectBranchId,
                    List.FirstOrDefault().BranchName,
                    List.FirstOrDefault().OrderNumber,
                    List.FirstOrDefault().SerialNumber,
                    List.FirstOrDefault().CityId,
                    List.FirstOrDefault().CityName,
                    List.FirstOrDefault().OrderBookingId,

                    Details = List.Select(x => new {
                        x.DenominationId,
                        x.DenominationTitle,
                        x.NumberOfBundles,
                        x.TotalValue,
                        x.SealNo
                    }).ToList(),

                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region
        //[HttpPost]
        //public JsonResult GetCentersByRegionId(Guid Id)
        //{
        //    return Json("");//centerRepo.GetCentersDropdown(Id));
        //}
        //[HttpPost]
        //public JsonResult GetDesignationsByDepartId(Guid Id)
        //{
        //    return Json("");//designationRepo.GetDesignationDropdown(Id));
        //}
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
                CPCUnsortedCash res = unsortedCashRepo.InActiveRecord(Id);
                if (res != null)
                {
                    annexureIrepo.avaiableRecord(res.OrderBookingId, res.ProjectBranchId);
                    TempData["SuccessMsg"] = "Department has been deleted successfully.";
                }
                
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
        //            annexureIRepo.DeleteMultiple(lsIds);
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