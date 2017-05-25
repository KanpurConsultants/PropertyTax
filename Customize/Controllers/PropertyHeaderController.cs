using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using System.Configuration;
using Presentation;
using Components.ExceptionHandlers;
using Services.BasicSetup;
using Models.Customize.ViewModels;
using Models.Customize.Models;
using Services.Customize;
using TimePlanValidator;
using TimePlanValidator.ViewModels;
using TimePlanValidator.Common;
using ProjLib.Constants;
using Models.BasicSetup.ViewModels;
using CookieNotifier;
using Presentation.Helper;
using Models.BasicSetup.Models;
using Models.Customize;
using Models.Company.Models;

namespace Customize.Controllers
{

    [Authorize]
    public class PropertyHeaderController : System.Web.Mvc.Controller
    {
        List<string> UserRoles = new List<string>();

        bool TimePlanValidation = true;
        string ExceptionMsg = "";
        bool Continue = true;

        IPropertyHeaderService _PropertyHeaderService;
        IDocumentTypeService _documentTypeService;
        IExceptionHandler _exception;
        IDocumentValidation _documentValidation;
        IProcessService _ProcessService;

        public PropertyHeaderController(IPropertyHeaderService PropertyHeaderService, IExceptionHandler exec, IDocumentTypeService DocumentTypeServ,
            IDocumentValidation DocValidation,
            IProcessService ProcessService)
        {
            _PropertyHeaderService = PropertyHeaderService;
            _exception = exec;
            _documentTypeService = DocumentTypeServ;
            _documentValidation = DocValidation;
            _ProcessService = ProcessService;

            UserRoles = (List<string>)System.Web.HttpContext.Current.Session["Roles"];
        }

        // GET: /Person/


        public ActionResult DocumentTypeIndex(int id)//DocumentCategoryId
        {
            var p = _documentTypeService.FindByDocumentCategory(id).ToList();

            if (p != null)
            {
                if (p.Count == 1)
                    return RedirectToAction("Index", new { id = p.FirstOrDefault().DocumentTypeId });
            }

            return View("DocumentTypeList", p);
        }

        public ActionResult Index(int id, string IndexType)//DocumentTypeId 
        {
            if (IndexType == "PTS")
            {
                return RedirectToAction("Index_PendingToSubmit", new { id });
            }
            else if (IndexType == "PTR")
            {
                return RedirectToAction("Index_PendingToReview", new { id });
            }
            IQueryable<PropertyHeaderViewModel> p = _PropertyHeaderService.GetPropertyHeaderList(id, User.Identity.Name);
            ViewBag.Name = _documentTypeService.Find(id).DocumentTypeName;
            PrepareViewBag(id);
            ViewBag.PendingToSubmit = PendingToSubmitCount(id);
            ViewBag.PendingToReview = PendingToReviewCount(id);
            ViewBag.IndexStatus = "All";
            ViewBag.id = id;
            return View(p);
        }

        public ActionResult Index_PendingToSubmit(int id)
        {
            IQueryable<PropertyHeaderViewModel> p = _PropertyHeaderService.GetPropertyHeaderListPendingToSubmit(id, User.Identity.Name);

            PrepareViewBag(id);
            ViewBag.PendingToSubmit = PendingToSubmitCount(id);
            ViewBag.PendingToReview = PendingToReviewCount(id);
            ViewBag.IndexStatus = "PTS";
            return View("Index", p);
        }

        public ActionResult Index_PendingToReview(int id)
        {
            IQueryable<PropertyHeaderViewModel> p = _PropertyHeaderService.GetPropertyHeaderListPendingToReview(id, User.Identity.Name);
            PrepareViewBag(id);
            ViewBag.PendingToSubmit = PendingToSubmitCount(id);
            ViewBag.PendingToReview = PendingToReviewCount(id);
            ViewBag.IndexStatus = "PTR";
            return View("Index", p);
        }

        private void PrepareViewBag(int id)
        {
            var DocType = _documentTypeService.Find(id);
            int Cid = DocType.DocumentCategoryId;
            ViewBag.DocTypeList = _documentTypeService.FindByDocumentCategory(Cid).ToList();
            ViewBag.Name = DocType.DocumentTypeName;
            ViewBag.id = id;

            var DivisionId = (int)System.Web.HttpContext.Current.Session["DivisionId"];
            var SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];

        }




        // GET: /Person/Create

        public ActionResult Create(int id)//DocumentTypeId
        {
            PropertyHeaderViewModel p = new PropertyHeaderViewModel();

            p.CreatedDate = DateTime.Now;
            p.DivisionIds = System.Web.HttpContext.Current.Session["DivisionId"].ToString();
            p.SiteIds = System.Web.HttpContext.Current.Session["SiteId"].ToString();
            p.DocTypeId = id;
            PrepareViewBag(id);
            List<DocumentTypeAttributeViewModel> tem = _PropertyHeaderService.GetAttributeForDocumentType(id).ToList();
            p.DocumentTypeAttributes = tem;

            //p.Code = _PropertyHeaderService.FGetNewPersonCode(p.DocTypeId);
            ViewBag.Mode = "Add";
            ViewBag.Name = _documentTypeService.Find(id).DocumentTypeName;
            ViewBag.id = id;
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Post(PropertyHeaderViewModel svm)
        {
            bool BeforeSave = true;

            int SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];
            svm.Code = _PropertyHeaderService.FGetNewPersonCode(SiteId, svm.GodownId, svm.BinLocationId);
            svm.Suffix = svm.Code;

            #region DocTypeTimeLineValidation

            try
            {

                if (svm.PersonID <= 0)
                    TimePlanValidation = _documentValidation.ValidateDocument(Mapper.Map<DocumentUniqueId>(svm), DocumentTimePlanTypeConstants.Create, User.Identity.Name, out ExceptionMsg, out Continue);
                else
                    TimePlanValidation = _documentValidation.ValidateDocument(Mapper.Map<DocumentUniqueId>(svm), DocumentTimePlanTypeConstants.Modify, User.Identity.Name, out ExceptionMsg, out Continue);

            }
            catch (Exception ex)
            {
                string message = _exception.HandleException(ex);
                TempData["CSEXC"] += message;
                TimePlanValidation = false;
            }

            if (!TimePlanValidation)
                TempData["CSEXC"] += ExceptionMsg;

            #endregion



            if (ModelState.IsValid && BeforeSave && (TimePlanValidation || Continue))
            {
                //CreateLogic
                #region CreateRecord
                if (svm.PersonID <= 0)
                {
                    try
                    {
                        _PropertyHeaderService.Create(svm, User.Identity.Name);
                    }

                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        TempData["CSEXC"] += message;
                        PrepareViewBag(svm.DocTypeId);
                        ViewBag.Mode = "Add";
                        return View("Create", svm);
                    }

                    return RedirectToAction("Modify", "PropertyHeader", new { Id = svm.PersonID }).Success("Data saved successfully");

                }
                #endregion


                //EditLogic
                #region EditRecord

                else
                {
                    try
                    {
                        _PropertyHeaderService.Update(svm, User.Identity.Name);
                    }

                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);

                        PrepareViewBag(svm.DocTypeId);
                        TempData["CSEXC"] += message;
                        ViewBag.id = svm.DocTypeId;
                        ViewBag.Mode = "Edit";
                        return View("Create", svm);
                    }

                    return RedirectToAction("Index", new { id = svm.DocTypeId }).Success("Data saved successfully");

                }
                #endregion

            }
            PrepareViewBag(svm.DocTypeId);
            ViewBag.Mode = "Add";
            return View("Create", svm);
        }


        [HttpGet]
        public ActionResult Modify(int id, string IndexType)
        {
            Person header = _PropertyHeaderService.Find(id);
            if (header.Status == (int)StatusConstants.Drafted || header.Status == (int)StatusConstants.Import)
                return Edit(id, IndexType);
            else
                return HttpNotFound();
        }

        [HttpGet]
        public ActionResult ModifyAfter_Submit(int id, string IndexType)
        {
            Person header = _PropertyHeaderService.Find(id);
            if (header.Status == (int)StatusConstants.Submitted || header.Status == (int)StatusConstants.Modified)
                return Edit(id, IndexType);
            else
                return HttpNotFound();
        }

        [HttpGet]
        public ActionResult ModifyAfter_Approve(int id, string IndexType)
        {
            Person header = _PropertyHeaderService.Find(id);
            if (header.Status == (int)StatusConstants.Approved)
                return Edit(id, IndexType);
            else
                return HttpNotFound();
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Person header = _PropertyHeaderService.Find(id);
            if (header.Status == (int)StatusConstants.Drafted || header.Status == (int)StatusConstants.Import)
                return Remove(id);
            else
                return HttpNotFound();
        }

        [HttpGet]
        public ActionResult DeleteAfter_Submit(int id)
        {
            Person header = _PropertyHeaderService.Find(id);
            if (header.Status == (int)StatusConstants.Submitted || header.Status == (int)StatusConstants.Modified)
                return Remove(id);
            else
                return HttpNotFound();
        }

        [HttpGet]
        public ActionResult DeleteAfter_Approve(int id)
        {
            Person header = _PropertyHeaderService.Find(id);
            if (header.Status == (int)StatusConstants.Approved)
                return Remove(id);
            else
                return HttpNotFound();
        }

        [HttpGet]
        public ActionResult DetailInformation(int id, int? DocLineId, string IndexType)
        {
            return RedirectToAction("Detail", new { id = id, transactionType = "detail", DocLineId = DocLineId, IndexType = IndexType });
        }



        // GET: /Person/Edit/5
        private ActionResult Edit(int id, string IndexType)
        {
            ViewBag.IndexStatus = IndexType;
            PropertyHeaderViewModel s = _PropertyHeaderService.GetPropertyHeader(id);

            #region DocTypeTimeLineValidation
            try
            {

                TimePlanValidation = _documentValidation.ValidateDocument(Mapper.Map<DocumentUniqueId>(s), DocumentTimePlanTypeConstants.Modify, User.Identity.Name, out ExceptionMsg, out Continue);

            }
            catch (Exception ex)
            {
                string message = _exception.HandleException(ex);
                TempData["CSEXC"] += message;
                TimePlanValidation = false;
            }

            if (!TimePlanValidation)
                TempData["CSEXC"] += ExceptionMsg;
            #endregion

            if ((!TimePlanValidation && !Continue))
            {
                return RedirectToAction("DetailInformation", new { id = id, IndexType = IndexType });
            }


            List<DocumentTypeAttributeViewModel> tem = _PropertyHeaderService.GetAttributeForPerson(id).ToList();
            s.DocumentTypeAttributes = tem;
            
            PrepareViewBag(s.DocTypeId);
            if (s == null)
            {
                return HttpNotFound();
            }

            ViewBag.Mode = "Edit";
            ViewBag.transactionType = "";

            ViewBag.Name = _documentTypeService.Find(s.DocTypeId).DocumentTypeName;
            ViewBag.id = s.DocTypeId;


            ViewBag.CollectionDocTypeId = Constants.DocumentTypeIdConstants.Collection;



            if (!(System.Web.HttpContext.Current.Request.UrlReferrer.PathAndQuery).Contains("Create"))
                _PropertyHeaderService.LogDetailInfo(s);

            return View("Create", s);
        }

        private ActionResult Remove(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person Person = _PropertyHeaderService.Find(id);

            if (Person == null)
            {
                return HttpNotFound();
            }

            #region DocTypeTimeLineValidation

            try
            {
                TimePlanValidation = _documentValidation.ValidateDocument(Mapper.Map<DocumentUniqueId>(Person), DocumentTimePlanTypeConstants.Delete, User.Identity.Name, out ExceptionMsg, out Continue);
                TempData["CSEXC"] += ExceptionMsg;
            }
            catch (Exception ex)
            {
                string message = _exception.HandleException(ex);
                TempData["CSEXC"] += message;
                TimePlanValidation = false;
            }

            if (!TimePlanValidation && !Continue)
            {
                return PartialView("AjaxError");
            }
            #endregion

            ReasonViewModel rvm = new ReasonViewModel()
            {
                id = id,
            };
            return PartialView("_Reason", rvm);
        }



        [Authorize]
        public ActionResult Detail(int id, string IndexType, string transactionType, int? DocLineId)
        {
            if (DocLineId.HasValue)
                ViewBag.DocLineId = DocLineId;
            //Saving ViewBag Data::

            ViewBag.transactionType = transactionType;
            ViewBag.IndexStatus = IndexType;

            PropertyHeaderViewModel s = _PropertyHeaderService.GetPropertyHeader(id);


            PrepareViewBag(s.DocTypeId);
            if (s == null)
            {
                return HttpNotFound();
            }

            if (String.IsNullOrEmpty(transactionType) || transactionType == "detail")
                _PropertyHeaderService.LogDetailInfo(s);

            return View("Create", s);
        }

        // POST: /PurchaseOrderHeader/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(ReasonViewModel vm)
        {
            if (ModelState.IsValid)
            {
                //Commit the DB
                try
                {
                    _PropertyHeaderService.Delete(vm, User.Identity.Name);
                }

                catch (Exception ex)
                {
                    string message = _exception.HandleException(ex);
                    TempData["CSEXC"] += message;
                    return PartialView("_Reason", vm);
                }

                return Json(new { success = true });
            }
            return PartialView("_Reason", vm);
        }


        public ActionResult Submit(int id, string IndexType, string TransactionType)
        {

            #region DocTypeTimeLineValidation
            Person s = _PropertyHeaderService.Find(id);

            try
            {
                TimePlanValidation = _documentValidation.ValidateDocument(Mapper.Map<DocumentUniqueId>(s), DocumentTimePlanTypeConstants.Submit, User.Identity.Name, out ExceptionMsg, out Continue);
                TempData["CSEXC"] += ExceptionMsg;
            }
            catch (Exception ex)
            {
                string message = _exception.HandleException(ex);
                TempData["CSEXC"] += message;
                TimePlanValidation = false;
            }

            if (!TimePlanValidation && !Continue)
            {
                return RedirectToAction("Index", new { id = s.DocTypeId, IndexType = IndexType });
            }
            #endregion

            return RedirectToAction("Detail", new { id = id, IndexType = IndexType, transactionType = string.IsNullOrEmpty(TransactionType) ? "submit" : TransactionType });
        }


        [HttpPost, ActionName("Detail")]
        [MultipleButton(Name = "Command", Argument = "Submit")]
        public ActionResult Submitted(int Id, string IndexType, string UserRemark, string IsContinue, string GenGatePass)
        {

            Person pd = _PropertyHeaderService.Find(Id);


            if (ModelState.IsValid)
            {

                if (User.Identity.Name == pd.ModifiedBy || UserRoles.Contains("Admin"))
                {
                    try
                    {
                        _PropertyHeaderService.Submit(Id, User.Identity.Name, GenGatePass, UserRemark);
                    }

                    catch (Exception ex)
                    {
                        string message = _exception.HandleException(ex);
                        TempData["CSEXC"] += message;
                        return RedirectToAction("Index", new { id = pd.DocTypeId });
                    }
                }
                else
                    return RedirectToAction("Index", new { id = pd.DocTypeId, IndexType = IndexType }).Warning("Record can be submitted by user " + pd.ModifiedBy + " only.");
            }

            return RedirectToAction("Index", new { id = pd.DocTypeId, IndexType = IndexType });
        }





        public ActionResult Review(int id, string IndexType, string TransactionType)
        {
            return RedirectToAction("Detail", new { id = id, IndexType = IndexType, transactionType = string.IsNullOrEmpty(TransactionType) ? "review" : TransactionType });
        }


        [HttpPost, ActionName("Detail")]
        [MultipleButton(Name = "Command", Argument = "Review")]
        public ActionResult Reviewed(int Id, string IndexType, string UserRemark, string IsContinue)
        {
            Person pd = _PropertyHeaderService.Find(Id);

            if (ModelState.IsValid)
            {

                _PropertyHeaderService.Review(Id, User.Identity.Name, UserRemark);

                string ReturnUrl = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"] + "/" + "PropertyHeader" + "/" + "Index" + "/" + pd.DocTypeId + "?IndexType=" + IndexType;

                return Redirect(ReturnUrl);
            }

            return RedirectToAction("Index", new { id = pd.DocTypeId, IndexType = IndexType }).Warning("Error in Reviewing.");
        }

        [HttpGet]
        public ActionResult Report(int id)
        {

            var Dt = _documentTypeService.Find(id);

            Dictionary<int, string> DefaultValue = new Dictionary<int, string>();

            if (!Dt.ReportMenuId.HasValue)
                throw new Exception("Report Menu not configured in document types");

            var menu = _PropertyHeaderService.GetMenu(Dt.ReportMenuId ?? 0);

            if (menu != null)
            {
                var header = _PropertyHeaderService.GetReportHeader(menu.MenuName);

                var Line = _PropertyHeaderService.GetReportLine("DocumentType", header.ReportHeaderId);
                if (Line != null)
                    DefaultValue.Add(Line.ReportLineId, id.ToString());
                var Site = _PropertyHeaderService.GetReportLine("Site", header.ReportHeaderId);
                if (Site != null)
                    DefaultValue.Add(Site.ReportLineId, ((int)System.Web.HttpContext.Current.Session["SiteId"]).ToString());
                var Division = _PropertyHeaderService.GetReportLine("Division", header.ReportHeaderId);
                if (Division != null)
                    DefaultValue.Add(Division.ReportLineId, ((int)System.Web.HttpContext.Current.Session["DivisionId"]).ToString());
            }

            TempData["ReportLayoutDefaultValues"] = DefaultValue;

            return Redirect((string)System.Configuration.ConfigurationManager.AppSettings["CustomizeDomain"] + "/Report_ReportPrint/ReportPrint/?MenuId=" + Dt.ReportMenuId);

        }


        public int PendingToSubmitCount(int id)
        {
            return (_PropertyHeaderService.GetPropertyHeaderListPendingToSubmit(id, User.Identity.Name)).Count();
        }

        public int PendingToReviewCount(int id)
        {
            return (_PropertyHeaderService.GetPropertyHeaderListPendingToReview(id, User.Identity.Name)).Count();
        }

        [HttpGet]
        public ActionResult NextPage(int DocId, int DocTypeId)//CurrentHeaderId
        {
            var nextId = _PropertyHeaderService.NextPrevId(DocId, DocTypeId, User.Identity.Name, PrevNextConstants.Next);
            return Edit(nextId, "");
        }
        [HttpGet]
        public ActionResult PrevPage(int DocId, int DocTypeId)//CurrentHeaderId
        {
            var PrevId = _PropertyHeaderService.NextPrevId(DocId, DocTypeId, User.Identity.Name, PrevNextConstants.Prev);
            return Edit(PrevId, "");
        }

        protected override void Dispose(bool disposing)
        {
            if (!string.IsNullOrEmpty((string)TempData["CSEXC"]))
            {
                GenerateCookie.CreateNotificationCookie(NotificationTypeConstants.Danger, (string)TempData["CSEXC"]);
                TempData.Remove("CSEXC");
            }
            if (disposing)
            {
                _PropertyHeaderService.Dispose();
            }
            base.Dispose(disposing);
        }


     
        public ActionResult GeneratePrints(string Ids, int DocTypeId)
        {

            if (!string.IsNullOrEmpty(Ids))
            {
                try
                {
                    var MergedPdf = _PropertyHeaderService.GetReport(Ids, DocTypeId, User.Identity.Name, "Web.spRep_PropertyPrint");
                    return File(MergedPdf, "application/pdf");
                }
                catch (Exception ex)
                {
                    string message = _exception.HandleException(ex);
                    return Json(new { success = "Error", data = message }, JsonRequestBehavior.AllowGet);
                }

            }
            return Json(new { success = "Error", data = "No Records Selected." }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GenerateInvoicePrints(string Ids, int DocTypeId)
        {

            if (!string.IsNullOrEmpty(Ids))
            {
                try
                {
                    var MergedPdf = _PropertyHeaderService.GetReport(Ids, DocTypeId, User.Identity.Name, "Web.spRep_InvoicePrint");
                    return File(MergedPdf, "application/pdf");
                }
                catch (Exception ex)
                {
                    string message = _exception.HandleException(ex);
                    return Json(new { success = "Error", data = message }, JsonRequestBehavior.AllowGet);
                }

            }
            return Json(new { success = "Error", data = "No Records Selected." }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DuplicateCheckForCreate(string docno, int doctypeId)
        {
            var Exists = (_PropertyHeaderService.CheckForDocNoExists(docno,doctypeId));
            return Json(new { returnvalue = Exists });
        }

        public JsonResult DuplicateCheckForEdit(string docno, int doctypeId, int headerid)
        {
            var Exists = (_PropertyHeaderService.CheckForDocNoExists(docno, headerid,doctypeId));
            return Json(new { returnvalue = Exists });
        }

        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        public JsonResult GetHouseNoJson(int GodownId, int BinLocationId)
        {
            int SiteId = (int)System.Web.HttpContext.Current.Session["SiteId"];

            var temp = _PropertyHeaderService.FGetNewHouseNo(SiteId,GodownId, BinLocationId);

            if (temp != null)
            {
                return Json(temp);
            }
            else
            {
                return null;
            }
        }

        public ActionResult GetPersonWithDocType(string searchTerm, int pageSize, int pageNum, int filter)
        {
            return new JsonpResult
            {
                Data = _PropertyHeaderService.GetPersonWithDocType(searchTerm, pageSize, pageNum, filter),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }




        public ActionResult GetProperty(string searchTerm, int pageSize, int pageNum)
        {
            return new JsonpResult
            {
                Data = _PropertyHeaderService.GetProperty(searchTerm, pageSize, pageNum),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        //[HttpGet]
        //public ActionResult Report()
        //{
        //    DocumentType Dt = _documentTypeService.Find(Constants.DocumentTypeIdConstants.Godown);
        //    return Redirect((string)System.Configuration.ConfigurationManager.AppSettings["CustomizeDomain"] + "/Report_ReportPrint/ReportPrint/?MenuId=" + Dt.ReportMenuId);
        //}


    }
}
