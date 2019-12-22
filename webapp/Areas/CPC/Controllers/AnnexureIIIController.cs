using BaseApp.Entity;
using BaseApp.System;
using CPC;
using CPC.Model;
using ImageResizer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WebApp.Areas.CPC.Controllers
{
    [AppAuthorize(AppPermission.All, AppPermission.ViewCPC, AppPermission.CPC)]
    public class AnnexureIIIController : AppController
    {
        private AnnexureIIIEntity annexureIIIRepo;
        private EmployeeEntity employeeRepo;
        private BranchEntity branchRepo;
        private Common commonRepo;

        public AnnexureIIIController()
        {
            annexureIIIRepo = new AnnexureIIIEntity();
            employeeRepo = new EmployeeEntity();
            branchRepo = new BranchEntity();
            commonRepo = new Common();
        }
        public ActionResult AnnexureIIIs()
        {
            return View();
        }
        public PartialViewResult _AllAnnexureIII()
        {
            var model = annexureIIIRepo.GetAll();
            ViewBag.Employees = employeeRepo.GetAll();
            return PartialView(model);
        }

        #region Details
        public ActionResult Details(Guid Id)
        {
            var model = annexureIIIRepo.GetById(Id);
            //model.CPCAnnexureIIIDetails = annexureIIIRepo.GetDetailsByMasterId(Id);
            ViewBag.Employees = employeeRepo.GetAll();
            var branchInfo = branchRepo.GetById(model.CashCollectedFromId.Value);
            ViewData["BranchName"] = $"{branchInfo.BranchCode} - {branchInfo.BranchName}";
            return View(model);

        }
        #endregion

        #region Record
        public ActionResult Record(Guid? Id)
        {
            //ViewData["IsView"] = Convert.ToString(TempData["IsView"]);
            var model = new CPCAnnexureIII();
            if (Id.HasValue)
            {
                model = annexureIIIRepo.GetById(Id.Value);
            }
            else
            {
                model.SrNo = annexureIIIRepo.GetNextSrNo();
                model.AnnexureIIIDate = DateTime.UtcNow;
                model.DateOfCashReturn = DateTime.Now;
            }
            //ViewBag.EmployeeList = new SelectList(employeeRepo.GetDropdown(), "Value", "Text");
            ViewBag.BrachList = new SelectList(branchRepo.GetDropdown(), "Value", "Text");
            //ViewBag.DenominationList = new SelectList(commonRepo.GetAllDenominationDropdown(), "Value", "Text");
            ViewBag.EmployeeList = new SelectList(employeeRepo.GetDropdown(), "Value", "Text");
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Record(CPCAnnexureIII model, List<CPCAnnexureIIIDetail> CPCAnnexureIIIDetail, string AnnexureIIIDate, string DateOfCashReturn)
        {
            try
            {
                if (model.Id.IsEmpty())
                {
                    model.CreatedBy = CurrentUser.Id;
                    model.CreatedOn = DateTime.Now;
                    model.AnnexureIIIDate = Utils.SetDateFormate(AnnexureIIIDate);
                    model.DateOfCashReturn = Utils.SetDateFormate(DateOfCashReturn);
                    model.Status = (int)AnnexureStatus.Inprocess;
                    model.IsActive = true;
                    model.Id = Guid.NewGuid();
                    var res = annexureIIIRepo.Create(model);
                    if (res.HasValue)
                    {
                        var lsToSave = CPCAnnexureIIIDetail.Where(x => x.UnfitSoiled > 0 && (x.FITReIssuable > 0 || x.UnfitSoiled > 0)).ToList();
                        lsToSave.ForEach(x => { x.Id = Guid.NewGuid(); x.AnnexureIIIId = model.Id; x.CreatedOn = DateTime.Now; x.CreatedBy = CurrentUser.Id; });
                        #region Save Details
                        if (lsToSave.Count > 0)
                        {
                            annexureIIIRepo.Create(lsToSave);
                        }
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

                    TempData["SuccessMsg"] = model.SrNo + " has been created successfully.";
                }
                else
                {
                    //model.UpdatedBy = CurrentUser.Id;
                    //model.UpdatedOn = DateTime.Now;
                    //bool res = annexureIIIRepo.Update(model);


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
            return RedirectToAction("AnnexureIIIs");
        }
        #endregion

        #region Remote Functions

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
                annexureIIIRepo.InActiveRecord(Id);

                TempData["SuccessMsg"] = "AnnexureIII has been deleted successfully.";
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