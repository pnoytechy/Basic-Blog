using System.Collections.Generic;
using System;
using SharpArch.Core;
using BasicBlog.Core;
using BasicBlog.ApplicationServices.ViewModels;
using BasicBlog.Core.QueryDtos;
using BasicBlog.Core.RepositoryInterfaces;
 

namespace BasicBlog.ApplicationServices
{
    public class EntryManagementService : IEntryManagementService
    {
        public EntryManagementService(IEntryRepository entryRepository) {
            Check.Require(entryRepository != null, "entryRepository may not be null");

            this.entryRepository = entryRepository;
        }

        public Entry Get(int id) {
            return entryRepository.Get(id);
        }

        public IList<Entry> GetAll() {
            return entryRepository.GetAll();
        }

        public IList<EntryDto> GetEntrySummaries() {
            return entryRepository.GetEntrySummaries();
        }

        public EntryFormViewModel CreateFormViewModel() {
            EntryFormViewModel viewModel = new EntryFormViewModel();
            return viewModel;
        }

        public EntryFormViewModel CreateFormViewModelFor(int entryId) {
            Entry entry = entryRepository.Get(entryId);
            return CreateFormViewModelFor(entry);
        }

        public EntryFormViewModel CreateFormViewModelFor(Entry entry) {
            EntryFormViewModel viewModel = CreateFormViewModel();
            viewModel.Entry = entry;
            return viewModel;
        }

        public ActionConfirmation SaveOrUpdate(Entry entry) {
            if (entry.IsValid()) {
                entryRepository.SaveOrUpdate(entry);

                ActionConfirmation saveOrUpdateConfirmation = ActionConfirmation.CreateSuccessConfirmation(
                    "The entry was successfully saved.");
                saveOrUpdateConfirmation.Value = entry;

                return saveOrUpdateConfirmation;
            }
            else {
                entryRepository.DbContext.RollbackTransaction();

                return ActionConfirmation.CreateFailureConfirmation(
                    "The entry could not be saved due to missing or invalid information.");
            }
        }

        public ActionConfirmation UpdateWith(Entry entryFromForm, int idOfEntryToUpdate) {
            Entry entryToUpdate = 
                entryRepository.Get(idOfEntryToUpdate);
            TransferFormValuesTo(entryToUpdate, entryFromForm);

            if (entryToUpdate.IsValid()) {
                ActionConfirmation updateConfirmation = ActionConfirmation.CreateSuccessConfirmation(
                    "The entry was successfully updated.");
                updateConfirmation.Value = entryToUpdate;

                return updateConfirmation;
            }
            else {
                entryRepository.DbContext.RollbackTransaction();

                return ActionConfirmation.CreateFailureConfirmation(
                    "The entry could not be saved due to missing or invalid information.");
            }
        }

        public ActionConfirmation Delete(int id) {
            Entry entryToDelete = entryRepository.Get(id);

            if (entryToDelete != null) {
                entryRepository.Delete(entryToDelete);

                try {
                    entryRepository.DbContext.CommitChanges();
                    
                    return ActionConfirmation.CreateSuccessConfirmation(
                        "The entry was successfully deleted.");
                }
                catch {
                    entryRepository.DbContext.RollbackTransaction();

                    return ActionConfirmation.CreateFailureConfirmation(
                        "A problem was encountered preventing the entry from being deleted. " +
                        "Another item likely depends on this entry.");
                }
            }
            else {
                return ActionConfirmation.CreateFailureConfirmation(
                    "The entry could not be found for deletion. It may already have been deleted.");
            }
        }

        private void TransferFormValuesTo(Entry entryToUpdate, Entry entryFromForm) {
		    entryToUpdate.Content = entryFromForm.Content;
			entryToUpdate.PostingDateTime = entryFromForm.PostingDateTime;
        }

        IEntryRepository entryRepository;
    }
}
