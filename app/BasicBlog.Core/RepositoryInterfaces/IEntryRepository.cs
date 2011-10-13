using SharpArch.Core.PersistenceSupport;
using System.Collections.Generic;
using BasicBlog.Core;
using BasicBlog.Core.QueryDtos;

namespace BasicBlog.Core.RepositoryInterfaces
{
    public interface IEntryRepository : IRepository<Entry>
    {
        IList<EntryDto> GetEntrySummaries();
    }
}
