using NUnit.Framework;
using Rhino.Mocks;
using SharpArch.Testing.NUnit;
using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport;
using BasicBlog.Core;
using BasicBlog.ApplicationServices;
using BasicBlog.ApplicationServices.ViewModels;
using BasicBlog.Core.QueryDtos;
using BasicBlog.Core.RepositoryInterfaces;
using Tests.BasicBlog.Core;
 

namespace Tests.BasicBlog.ApplicationServices
{
    [TestFixture]
    public class EntryManagementServiceTests
    {
        [SetUp]
        public void SetUp() {
            ServiceLocatorInitializer.Init();

            entryRepository = 
                MockRepository.GenerateMock<IEntryRepository>();
            entryRepository.Stub(r => r.DbContext)
                .Return(MockRepository.GenerateMock<IDbContext>());
            
            entryManagementService =
                new EntryManagementService(entryRepository);
        }

        [Test]
        public void CanGetEntry() {
            // Establish Context
            Entry entryToExpect = 
                EntryInstanceFactory.CreateValidTransientEntry();

            entryRepository.Expect(r => r.Get(1))
                .Return(entryToExpect);

            // Act
            Entry entryRetrieved = 
                entryManagementService.Get(1);

            // Assert
            entryRetrieved.ShouldNotBeNull();
            entryRetrieved.ShouldEqual(entryToExpect);
        }

        [Test]
        public void CanGetAllEntries() {
            // Establish Context
            IList<Entry> entriesToExpect = new List<Entry>();

            Entry entry = 
                EntryInstanceFactory.CreateValidTransientEntry();

            entriesToExpect.Add(entry);

            entryRepository.Expect(r => r.GetAll())
                .Return(entriesToExpect);

            // Act
            IList<Entry> entriesRetrieved =
                entryManagementService.GetAll();

            // Assert
            entriesRetrieved.ShouldNotBeNull();
            entriesRetrieved.Count.ShouldEqual(1);
            entriesRetrieved[0].ShouldNotBeNull();
            entriesRetrieved[0].ShouldEqual(entry);
        }

        [Test]
        public void CanGetEntrySummaries() {
            // Establish Context
            IList<EntryDto> entrySummariesToExpect = new List<EntryDto>();

            EntryDto entryDto = new EntryDto();
            entrySummariesToExpect.Add(entryDto);

            entryRepository.Expect(r => r.GetEntrySummaries())
                .Return(entrySummariesToExpect);

            // Act
            IList<EntryDto> entrySummariesRetrieved =
                entryManagementService.GetEntrySummaries();

            // Assert
            entrySummariesRetrieved.ShouldNotBeNull();
            entrySummariesRetrieved.Count.ShouldEqual(1);
            entrySummariesRetrieved[0].ShouldNotBeNull();
            entrySummariesRetrieved[0].ShouldEqual(entryDto);
        }

        [Test]
        public void CanCreateFormViewModel() {
            // Establish Context
            EntryFormViewModel viewModelToExpect = new EntryFormViewModel();

            // Act
            EntryFormViewModel viewModelRetrieved =
                entryManagementService.CreateFormViewModel();

            // Assert
            viewModelRetrieved.ShouldNotBeNull();
            viewModelRetrieved.Entry.ShouldBeNull();
        }

        [Test]
        public void CanCreateFormViewModelForEntry() {
            // Establish Context
            EntryFormViewModel viewModelToExpect = new EntryFormViewModel();

            Entry entry = 
                EntryInstanceFactory.CreateValidTransientEntry();

            entryRepository.Expect(r => r.Get(1))
                .Return(entry);

            // Act
            EntryFormViewModel viewModelRetrieved =
                entryManagementService.CreateFormViewModelFor(1);

            // Assert
            viewModelRetrieved.ShouldNotBeNull();
            viewModelRetrieved.Entry.ShouldNotBeNull();
            viewModelRetrieved.Entry.ShouldEqual(entry);
        }

        [Test]
        public void CanSaveOrUpdateValidEntry() {
            // Establish Context
            Entry validEntry = 
                EntryInstanceFactory.CreateValidTransientEntry();

            // Act
            ActionConfirmation confirmation =
                entryManagementService.SaveOrUpdate(validEntry);

            // Assert
            confirmation.ShouldNotBeNull();
            confirmation.WasSuccessful.ShouldBeTrue();
            confirmation.Value.ShouldNotBeNull();
            confirmation.Value.ShouldEqual(validEntry);
        }

        [Test]
        public void CannotSaveOrUpdateInvalidEntry() {
            // Establish Context
            Entry invalidEntry = new Entry();

            // Act
            ActionConfirmation confirmation =
                entryManagementService.SaveOrUpdate(invalidEntry);

            // Assert
            confirmation.ShouldNotBeNull();
            confirmation.WasSuccessful.ShouldBeFalse();
            confirmation.Value.ShouldBeNull();
        }

        [Test]
        public void CanUpdateWithValidEntryFromForm() {
            // Establish Context
            Entry validEntryFromForm = 
                EntryInstanceFactory.CreateValidTransientEntry();
            
            // Intentionally empty to ensure successful transfer of values
            Entry entryFromDb = new Entry();

            entryRepository.Expect(r => r.Get(1))
                .Return(entryFromDb);

            // Act
            ActionConfirmation confirmation =
                entryManagementService.UpdateWith(validEntryFromForm, 1);

            // Assert
            confirmation.ShouldNotBeNull();
            confirmation.WasSuccessful.ShouldBeTrue();
            confirmation.Value.ShouldNotBeNull();
            confirmation.Value.ShouldEqual(entryFromDb);
            confirmation.Value.ShouldEqual(validEntryFromForm);
        }

        [Test]
        public void CannotUpdateWithInvalidEntryFromForm() {
            // Establish Context
            Entry invalidEntryFromForm = new Entry();

            // Intentionally empty to ensure successful transfer of values
            Entry entryFromDb = new Entry();

            entryRepository.Expect(r => r.Get(1))
                .Return(entryFromDb);

            // Act
            ActionConfirmation confirmation =
                entryManagementService.UpdateWith(invalidEntryFromForm, 1);

            // Assert
            confirmation.ShouldNotBeNull();
            confirmation.WasSuccessful.ShouldBeFalse();
            confirmation.Value.ShouldBeNull();
        }

        [Test]
        public void CanDeleteEntry() {
            // Establish Context
            Entry entryToDelete = new Entry();

            entryRepository.Expect(r => r.Get(1))
                .Return(entryToDelete);

            // Act
            ActionConfirmation confirmation =
                entryManagementService.Delete(1);

            // Assert
            confirmation.ShouldNotBeNull();
            confirmation.WasSuccessful.ShouldBeTrue();
            confirmation.Value.ShouldBeNull();
        }

        [Test]
        public void CannotDeleteNonexistentEntry() {
            // Establish Context
            entryRepository.Expect(r => r.Get(1))
                .Return(null);

            // Act
            ActionConfirmation confirmation =
                entryManagementService.Delete(1);

            // Assert
            confirmation.ShouldNotBeNull();
            confirmation.WasSuccessful.ShouldBeFalse();
            confirmation.Value.ShouldBeNull();
        }

        private IEntryRepository entryRepository;
        private IEntryManagementService entryManagementService;
    }
}
