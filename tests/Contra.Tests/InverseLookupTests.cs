using System.Linq;
using Contra.Tests.Support;
using NUnit.Framework;

namespace Contra.Tests
{
    [TestFixture]
    public class InverseLookupTests
    {
        readonly TypeRegistry<string> _registry = 
            new TypeRegistry<string>()
                .Register<PersonRegisteredEvent>("person-registered")
                .Register<CriminalRegisteredEvent>("criminal-registered");

        [Test]
        public void should_get_type_from_value()
        {
            Assert.AreEqual(typeof(PersonRegisteredEvent), _registry.GetKeysFor("person-registered").Single().Key);
            Assert.AreEqual(typeof(CriminalRegisteredEvent), _registry.GetKeysFor("criminal-registered").Single().Key);
        }
    }
}