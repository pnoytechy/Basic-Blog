using System.Web.Mvc;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Core.DomainModel;
using System.Collections.Generic;
using SharpArch.Web.NHibernate;
using NHibernate.Validator.Engine;
using System.Text;
using SharpArch.Web.CommonValidator;
using SharpArch.Core;
using BasicBlog.Core;
using BasicBlog.ApplicationServices;
using BasicBlog.ApplicationServices.ViewModels;
using BasicBlog.Core.QueryDtos;
using BasicBlog.Core.RepositoryInterfaces;
 

namespace BasicBlog.Web.Controllers
{
    [HandleError]
    public class EntriesController : Controller
    {
        public EntriesController(IEntryManagementService entryManagementService) {
            Check.Require(entryManagementService != null, "entryManagementService may not be null");

            this.entryManagementService = entryManagementService;
        }

        [Transaction]
        public ActionResult Index() {
            IList<EntryDto> entries = 
                entryManagementService.GetEntrySummaries();
            return View(entries);
        }

        [Transaction]
        public ActionResult Show(int id) {
            Entry entry = entryManagementService.Get(id);
            return View(entry);
        }

        [Transaction]
        public ActionResult Create() {
            EntryFormViewModel viewModel = 
                entryManagementService.CreateFormViewModel();
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Entry entry) {
            if (ViewData.ModelState.IsValid) {
                ActionConfirmation saveOrUpdateConfirmation = 
                    entryManagementService.SaveOrUpdate(entry);

                if (saveOrUpdateConfirmation.WasSuccessful) {
                    TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = 
                        saveOrUpdateConfirmation.Message;
                    return RedirectToAction("Index");
                }
            } else {
                entry = null;
            }

            EntryFormViewModel viewModel = 
                entryManagementService.CreateFormViewModelFor(entry);
            return View(viewModel);
        }

        [Transaction]
        [HttpPost]
        public JsonResult CreateEntry(Entry entry)
        {
            if (ViewData.ModelState.IsValid)
            {
                //entry.PostingDateTime = System.DateTime.Now;
                ActionConfirmation saveOrUpdateConfirmation =
                    entryManagementService.SaveOrUpdate(entry);

                if (saveOrUpdateConfirmation.WasSuccessful)
                {
                    TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] =
                        saveOrUpdateConfirmation.Message;
                    return Json(entry);
                }
            }
            else
            {
                entry = null;
            }

            return Json("Failed to create new entry");
        }

        [Transaction]
        public ActionResult Edit(int id) {
            EntryFormViewModel viewModel = 
                entryManagementService.CreateFormViewModelFor(id);
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Entry entry) {
            if (ViewData.ModelState.IsValid) {
                ActionConfirmation updateConfirmation = 
                    entryManagementService.UpdateWith(entry, entry.Id);

                if (updateConfirmation.WasSuccessful) {
                    TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = 
                        updateConfirmation.Message;
                    return RedirectToAction("Index");
                }
            }

            EntryFormViewModel viewModel = 
                entryManagementService.CreateFormViewModelFor(entry);
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id) {
            ActionConfirmation deleteConfirmation = entryManagementService.Delete(id);
            TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = 
                deleteConfirmation.Message;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult DeleteEntry(int ID)
        {
            ActionConfirmation deleteConfirmation = entryManagementService.Delete(ID);

            return Json(deleteConfirmation.Message);
        }

        private readonly IEntryManagementService entryManagementService;
    }
}
