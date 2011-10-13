using SharpArch.Data.NHibernate;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Transform;
using BasicBlog.Core;
using BasicBlog.Core.QueryDtos;
using BasicBlog.Core.RepositoryInterfaces;

namespace BasicBlog.Data.Repositories
{
    public class EntryRepository : Repository<Entry>, IEntryRepository
    {
        public IList<EntryDto> GetEntrySummaries() {
            ISession session = SharpArch.Data.NHibernate.NHibernateSession.Current;

            IQuery query = session.GetNamedQuery("GetEntrySummaries")
                .SetResultTransformer(Transformers.AliasToBean<EntryDto>());

            return query.List<EntryDto>();
        }
    }
}
