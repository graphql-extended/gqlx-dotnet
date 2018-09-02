namespace Gqlx.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class BindingTests
    {
        [Test]
        public void BasicBindingToJsDoesNotThrow()
        {
            var b = new Binding();
            Assert.IsNotNull(b.EngineInstance);
        }

        [Test]
        public void BasicBindingToJsObtainsDefaultApi()
        {
            var b = new Binding();
            var api = b.GetDefaultApi();
            Assert.IsNotNull(api);
            Assert.AreEqual(true, api["get"]);
            Assert.AreEqual(true, api["post"]);
            Assert.AreEqual(true, api["del"]);
            Assert.AreEqual(true, api["put"]);
            Assert.AreEqual(true, api["form"]);
            Assert.AreEqual(false, api["listen"]);
        }
    }
}
