using System;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;

namespace BasicBlog.Core
{
    public class Entry : Entity
    {
		[DomainSignature]
		[NotNull, NotEmpty]
		public virtual string Content { get; set; }

		public virtual DateTime PostingDateTime { get; set; }
    }
}
