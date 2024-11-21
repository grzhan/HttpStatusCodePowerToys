using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.HttpStatusCodes.UnitTests
{
    [TestClass]
    public class MainTests
    {
        private Main main;

        [TestInitialize]
        public void TestInitialize()
        {
            main = new Main();
        }

        [TestMethod]
        public void Query_should_return_results()
        {
            var results = main.Query(new("search"));

            Assert.IsNotNull(results.First());
        }

    }
}
