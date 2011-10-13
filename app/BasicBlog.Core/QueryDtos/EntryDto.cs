using System;

namespace BasicBlog.Core.QueryDtos
{
    public class EntryDto
    {
        public int Id { get; set; }
		public string Content { get; set; }
		public DateTime PostingDateTime { get; set; }
    }
}
