using System.Collections.Generic;
using BasicBlog.Core;
using BasicBlog.ApplicationServices.ViewModels;
using BasicBlog.Core.QueryDtos;
 

namespace BasicBlog.ApplicationServices
{
    public interface IEntryManagementService
    {
        EntryFormViewModel CreateFormViewModel();
        EntryFormViewModel CreateFormViewModelFor(int entryId);
        EntryFormViewModel CreateFormViewModelFor(Entry entry);
        Entry Get(int id);
        IList<Entry> GetAll();
        IList<EntryDto> GetEntrySummaries();
        ActionConfirmation SaveOrUpdate(Entry entry);
        ActionConfirmation UpdateWith(Entry entryFromForm, int idOfEntryToUpdate);
        ActionConfirmation Delete(int id);
    }
}
