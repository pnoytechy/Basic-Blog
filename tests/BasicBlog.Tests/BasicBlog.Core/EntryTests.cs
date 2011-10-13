using NUnit.Framework;
using SharpArch.Testing;
using SharpArch.Testing.NUnit;
using BasicBlog.Core;

namespace Tests.BasicBlog.Core
{
	[TestFixture]
    public class EntryTests
    {
        [Test]
        public void CanCompareEntries() {
            Entry instance = new Entry();
			instance.Content = "This is an entry";

            Entry instanceToCompareTo = new Entry();
			instanceToCompareTo.Content = "This is an entry";

			instance.ShouldEqual(instanceToCompareTo);
        }
    }
}
