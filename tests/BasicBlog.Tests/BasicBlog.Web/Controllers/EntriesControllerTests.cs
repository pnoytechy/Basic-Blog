using MvcContrib.TestHelper;
using NUnit.Framework;
using Rhino.Mocks;
using SharpArch.Testing.NUnit;
using System.Collections.Generic;
using System.Web.Mvc;
using BasicBlog.Core;
using BasicBlog.Core.QueryDtos;
using BasicBlog.ApplicationServices;
using BasicBlog.ApplicationServices.ViewModels;
using BasicBlog.Web.Controllers;
using Tests.BasicBlog.Core;
 

namespace Tests.BasicBlog.Web.Controllers
{
    [TestFixture]
    public class EntriesControllerTests
    {
        [SetUp]
        public void SetUp() {
            ServiceLocatorInitializer.Init();

            entryManagementService =
                MockRepository.GenerateMock<IEntryManagementService>();
            entriesController = 
                new EntriesController(entryManagementService);
        }

        [Test]
        public void CanListEntries() {
            // Establish Context
            IList<EntryDto> entrySummariesToExpect = new List<EntryDto>();

            EntryDto entryDto = new EntryDto();
            entrySummariesToExpect.Add(entryDto);

            entryManagementService.Expect(r => r.GetEntrySummaries())
                .Return(entrySummariesToExpect);

            // Act
            ViewResult result = entriesController.Index().AssertViewRendered();

            // Assert
            result.ViewData.Model.ShouldNotBeNull();
            (result.ViewData.Model as IList<EntryDto>).ShouldNotBeNull();
            (result.ViewData.Model as IList<EntryDto>).Count.ShouldEqual(1);
        }

        [Test]
        public void CanShowEntry() {
            // Establish Context
            Entry entry = 
                EntryInstanceFactory.CreateValidTransientEntry();

            entryManagementService.Expect(r => r.Get(1))
                .Return(entry);

            // Act
            ViewResult result = entriesController.Show(1).AssertViewRendered();

            // Assert
            result.ViewData.Model.ShouldNotBeNull();
            (result.ViewData.Model as Entry).ShouldNotBeNull();
            (result.ViewData.Model as Entry).ShouldEqual(entry);
        }

        [Test]
        public void CanInitCreate() {
            // Establish Context
            EntryFormViewModel viewModel = new EntryFormViewModel();

            entryManagementService.Expect(r => r.CreateFormViewModel())
                .Return(viewModel);

            // Act
            ViewResult result = entriesController.Create().AssertViewRendered();

            // Assert
            result.ViewData.Model.ShouldNotBeNull();
            (result.ViewData.Model as EntryFormViewModel).ShouldNotBeNull();
            (result.ViewData.Model as EntryFormViewModel).Entry.ShouldBeNull();
        }

        [Test]
        public void CanCreateValidEntryFromForm() {
            // Establish Context
            Entry entryFromForm = new Entry();

            entryManagementService.Expect(r => r.SaveOrUpdate(entryFromForm))
                .Return(ActionConfirmation.CreateSuccessConfirmation("saved"));

            // Act
            RedirectToRouteResult redirectResult =
                entriesController.Create(entryFromForm)
                .AssertActionRedirect().ToAction("Index");

            // Assert
            entriesController.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()].ToString()
				.ShouldEqual("saved");
        }

        [Test]
        public void CannotCreateInvalidEntryFromForm() {
            // Establish Context
            Entry entryFromForm = new Entry();
            EntryFormViewModel viewModelToExpect = new EntryFormViewModel();

            entryManagementService.Expect(r => r.SaveOrUpdate(entryFromForm))
                .Return(ActionConfirmation.CreateFailureConfirmation("not saved"));
            entryManagementService.Expect(r => r.CreateFormViewModelFor(entryFromForm))
                .Return(viewModelToExpect);

            // Act
            ViewResult result =
                entriesController.Create(entryFromForm).AssertViewRendered();

            // Assert
            result.ViewData.Model.ShouldNotBeNull();
            (result.ViewData.Model as EntryFormViewModel).ShouldNotBeNull();
        }

        [Test]
        public void CanInitEdit() {
            // Establish Context
            EntryFormViewModel viewModel = new EntryFormViewModel();

            entryManagementService.Expect(r => r.CreateFormViewModelFor(1))
                .Return(viewModel);

            // Act
            ViewResult result = entriesController.Edit(1).AssertViewRendered();

            // Assert
            result.ViewData.Model.ShouldNotBeNull();
            (result.ViewData.Model as EntryFormViewModel).ShouldNotBeNull();
        }

        [Test]
        public void CanUpdateValidEntryFromForm() {
            // Establish Context
            Entry entryFromForm = new Entry();

            entryManagementService.Expect(r => r.UpdateWith(entryFromForm, 0))
                .Return(ActionConfirmation.CreateSuccessConfirmation("updated"));

            // Act
            RedirectToRouteResult redirectResult =
                entriesController.Edit(entryFromForm)
                .AssertActionRedirect().ToAction("Index");

            // Assert
            entriesController.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()].ToString()
                .ShouldEqual("updated");
        }

        [Test]
        public void CannotUpdateInvalidEntryFromForm() {
            // Establish Context
            Entry entryFromForm = new Entry();
            EntryFormViewModel viewModelToExpect = new EntryFormViewModel();

            entryManagementService.Expect(r => r.UpdateWith(entryFromForm, 0))
                .Return(ActionConfirmation.CreateFailureConfirmation("not updated"));
            entryManagementService.Expect(r => r.CreateFormViewModelFor(entryFromForm))
                .Return(viewModelToExpect);

            // Act
            ViewResult result =
                entriesController.Edit(entryFromForm).AssertViewRendered();

            // Assert
            result.ViewData.Model.ShouldNotBeNull();
            (result.ViewData.Model as EntryFormViewModel).ShouldNotBeNull();
        }

        [Test]
        public void CanDeleteEntry() {
            // Establish Context
            entryManagementService.Expect(r => r.Delete(1))
                .Return(ActionConfirmation.CreateSuccessConfirmation("deleted"));
            
            // Act
            RedirectToRouteResult redirectResult =
                entriesController.Delete(1)
                .AssertActionRedirect().ToAction("Index");

            // Assert
            entriesController.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()].ToString()
                .ShouldEqual("deleted");
        }

        private IEntryManagementService entryManagementService;
        private EntriesController entriesController;
    }
}
