using System;
using BasicBlog.Core;

namespace Tests.BasicBlog.Core
{
    public class EntryInstanceFactory
    {
        public static Entry CreateValidTransientEntry() {
            return new Entry() {
			    Content = "This is an entry", 
				PostingDateTime = DateTime.Parse("1/1/1975 12:00:00 AM") 
            };
        }
    }
}
