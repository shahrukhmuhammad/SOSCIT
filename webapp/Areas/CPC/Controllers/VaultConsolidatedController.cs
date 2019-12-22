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
    [AppAuthorize(AppPermission.All, AppPermission.ViewCPC, AppPermission.CPC)]
    public class VaultConsolidatedController : AppController
    {
        private VaultConsolidatedEntity vaultConsolidatedRepo;
        private ValutCustodianEntity valutCustodianRepo;
        private EmployeeEntity employeeRepo;
        private BranchEntity branchRepo;
        private Common commonRepo;

        public VaultConsolidatedController()
        {
            vaultConsolidatedRepo = new VaultConsolidatedEntity();
            valutCustodianRepo = new ValutCustodianEntity();
            employeeRepo = new EmployeeEntity();
            branchRepo = new BranchEntity();
            commonRepo = new Common();
        }
        public ActionResult VaultConsolidateds()
        {
            return View();
        }
        public PartialViewResult _AllVaultConsolidateds()
        {
            var model = vaultConsolidatedRepo.GetAll();
            return PartialView(model);
        }

        #region VaultConsolidated Popup
        public ActionResult _VaultConsolidatedList()
        {
            var model = vaultConsolidatedRepo.GetAllApproved().Where(x => x.VaultConsolidatedDetailsStatus == (int)AnnexureStatus.Inprocess).ToList();
            ViewBag.DetailsList = vaultConsolidatedRepo.GetAllDetails();
            return PartialView(model);
        }
        #endregion

        #region Details

        public ActionResult Details(Guid Id)
        {
            var model = vaultConsolidatedRepo.GetById(Id);
            ViewBag.ConsolidatedBundleList = vaultConsolidatedRepo.GetByIdConsolidatedBundle(Id);
            //ViewBag.Employees = employeeRepo.GetAll();
            ViewBag.DenominationList = commonRepo.GetAllDenomination();
            var branchInfo = branchRepo.GetById(model.ProjectBranchId);
            ViewData["BranchName"] = $"{branchInfo.BranchCode} - {branchInfo.BranchName}";
            return View(model);
        }
        #endregion

        #region Record
        public ActionResult Record(Guid? Id)
        {
            var model = new CPCVaultConsolidated();
            if (Id.HasValue)
            {
                model = vaultConsolidatedRepo.GetById(Id.Value);
            }
            else
            {
                model.SerialNumber = vaultConsolidatedRepo.GetNextSerialNumber();
                model.Date = DateTime.Now;
            }
            ViewBag.BrachList = new SelectList(branchRepo.GetDropdown(), "Value", "Text");
            ViewBag.EmployeeList = new SelectList(employeeRepo.GetDropdown(), "Value", "Text");
            ViewBag.DenominationList = commonRepo.GetAllDenomination();
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Record(CPCVaultConsolidated model, List<CPCVaultConsolidatedDetail> CPCVaultConsolidatedDetail, List<CPCVaultConsolidatedBundle> CPCVaultConsolidatedBundle, string Date)
        {
            try
            {
                if (model.Id.IsEmpty())
                {
                    //var ss = new CPCVaultConsolidated();
                    //var b = ss.CPCVaultConsolidatedBundles.First();
                    //b.CPCVaultConsolidatedBundlesDetails.First().Amount
                    model.CreatedBy = CurrentUser.Id;
                    model.CreatedOn = DateTime.Now;
                    model.IsActive = true;
                    model.Status = (int)AnnexureStatus.Inprocess;
                    model.Date = Utils.SetDateFormate(Date);
                    model.Id = Guid.NewGuid();
                    var res = vaultConsolidatedRepo.Create(model);
                    if (res.HasValue)
                    {

                        #region Save Details
                        var lsToSave = CPCVaultConsolidatedDetail.Where(x => x.NumberOfBundles > 0 && (x.TotalValue != 0)).ToList();
                        lsToSave.ForEach(x => { x.Id = Guid.NewGuid(); x.VaultConsolidatedId = model.Id; x.Status = (int)AnnexureStatus.Inprocess; x.CreatedOn = DateTime.Now; x.CreatedBy = CurrentUser.Id; });
                        vaultConsolidatedRepo.Create(lsToSave);
                        #endregion

                        #region Save Sub Master
                        var lsMasterToSave = new List<CPCVaultConsolidatedBundle>();
                        var lsChildToSave = new List<CPCVaultConsolidatedBundlesDetail>();
                        var lsVaultConsolidatedBundle = CPCVaultConsolidatedBundle.Where(x => x.DenominationId != Guid.Empty).ToList();
                        foreach (var item in lsVaultConsolidatedBundle)
                        {
                            //var obj = new CPCVaultConsolidatedBundle();
                            //var masterId = Guid.NewGuid();
                            var objConsolidatedDetail = lsToSave.Where(x => x.DenominationId == item.DenominationId).FirstOrDefault();

                            if (objConsolidatedDetail != null && item.CPCVaultConsolidatedBundlesDetails.Count > 0)
                            {
                                #region Bundles Master
                                item.Id = Guid.NewGuid();
                                item.ConsolidatedDetailsId = objConsolidatedDetail.Id;
                                item.OrderBookingId = model.OrderBookingId.Value;
                                item.CreatedOn = DateTime.Now;
                                item.CreatedBy = CurrentUser.Id;
                                lsMasterToSave.Add(item);
                                #endregion

                                #region Bundles Details
                                var detailsList = item.CPCVaultConsolidatedBundlesDetails.Where(x => x.SorterId != Guid.Empty && x.NumberOfBundles > 0).ToList();
                                detailsList.ForEach(x =>
                                {
                                    x.Id = Guid.NewGuid();
                                    x.ConsolidatedBundlesId = item.Id;
                                });
                                lsChildToSave.AddRange(detailsList);
                                #endregion
                            }
                        }
                        if (lsMasterToSave.Count > 0)
                            vaultConsolidatedRepo.CreateVaultConsolidatedBundle(lsMasterToSave, lsChildToSave);
                        #endregion 

                        valutCustodianRepo.ChangeStatus(model.OrderBookingId, CurrentUser.Id);
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
            return RedirectToAction("VaultConsolidateds");
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
                CPCVaultConsolidated res = vaultConsolidatedRepo.InActiveRecord(Id);
                if (res != null)
                {
                    valutCustodianRepo.avaiableRecord(res.OrderBookingId, res.ProjectBranchId);
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

        #region Remote function
        [HttpGet]
        public JsonResult GetVaultConsolidatedData(Guid id, Guid denomId)
        {
            try
            {
                var List = vaultConsolidatedRepo.GetAllDetailsById().Where(x => x.VaultConsolidatedDetailsStatus == (int)AnnexureStatus.Inprocess && (x.OrderBookingId == id && x.DenominationId == denomId)).ToList();
                //for (int i = 0; i < List.Count(); i++)
                //{
                //    List.FirstOrDefault().TotalNumberBundles += List[i].NumberOfBundles;
                //}
                return Json(new
                {
                    List.FirstOrDefault().OrderNumber,
                    List.FirstOrDefault().OrderBookingId,
                    List.FirstOrDefault().SerialNumber,
                    List.FirstOrDefault().Date,
                    List.FirstOrDefault().DenominationTitle,
                    List.FirstOrDefault().DenominationId,
                    List.FirstOrDefault().ProjectBranchId,
                    List.FirstOrDefault().BranchName,
                    List.FirstOrDefault().SealNo,
                    List.FirstOrDefault().CityId,
                    List.FirstOrDefault().CityName,
                    List.FirstOrDefault().TotalValue,
                    List.FirstOrDefault().TotalNumberBundles,
                    Details = List.Select(x => new
                    {
                        x.NumberOfBundles,
                        x.SorterId,
                        x.Name,
                    }).ToList(),

                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}