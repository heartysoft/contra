using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Contra.Tests.Support;
using NUnit.Framework;

namespace Contra.Tests
{
    [TestFixture]
    public class TypeRegistryTests
    {
        private TypeRegistry<Action<object>> _registry;

        [SetUp]
        public void Setup()
        {
            _registry = new TypeRegistry<Action<object>>();
        }

        [Test]
        public void should_handle_exact_type()
        {
            var handled = false;
            _registry.Register<PersonRegisteredEvent>(x => handled = true);
            execute(new PersonRegisteredEvent("foo"));

            Assert.True(handled);
        }

        [Test]
        public void should_handle_for_interface_type()
        {
            var handled = false;
            _registry.Register<RegisteredEvent>(x => handled = true);
            execute(new PersonRegisteredEvent("foo"));

            Assert.True(handled);
        }

        [Test]
        public void should_handle_for_exact_envelope_type()
        {
            var handled = false;
            _registry.Register<MessageEnvelope<PersonRegisteredEvent>>(x => handled = true);
            execute(new MessageEnvelope<PersonRegisteredEvent>(new PersonRegisteredEvent("foo")));

            Assert.True(handled);
        }

        [Test]
        public void should_handle_for_envelope_interface_on_exact_type()
        {
            var handled = false;
            _registry.Register<Envelope<PersonRegisteredEvent>>(x => handled = true);
            execute(new MessageEnvelope<PersonRegisteredEvent>(new PersonRegisteredEvent("foo")));

            Assert.True(handled);
        }

        [Test]
        public void should_handle_for_envelope_interface_on_interface_type()
        {
            var handled = false;
            _registry.Register<Envelope<RegisteredEvent>>(x => handled = true);
            execute(new MessageEnvelope<PersonRegisteredEvent>(new PersonRegisteredEvent("foo")));

            Assert.True(handled);
        }


        [Test]
        public void should_not_handle_for_envelope_interface_on_different_message_type()
        {
            var handled = false;
            _registry.Register<Envelope<CancelledEvent>>(x => handled = true);
            execute(new MessageEnvelope<PersonRegisteredEvent>(new PersonRegisteredEvent("foo")));

            Assert.False(handled);
        }

        [Test]
        public void handlers_shouldnt_freak_each_other_out()
        {
            var handledCancelled = false;
            var handledRegistered = false;
            _registry.Register<Envelope<CancelledEvent>>(x => handledCancelled = true);
            _registry.Register<Envelope<RegisteredEvent>>(x => handledRegistered = true);
            execute(new MessageEnvelope<PersonRegisteredEvent>(new PersonRegisteredEvent("foo")));

            Assert.False(handledCancelled);
            Assert.True(handledRegistered);
        }


        [Test]
        public void all_handlers_for_message_should_trigger()
        {
            var handledInterface = false;
            var handledClass = false;
            _registry.Register<Envelope<RegisteredEvent>>(x => handledInterface = true);
            _registry.Register<Envelope<PersonRegisteredEvent>>(x => handledClass = true);

            execute(new MessageEnvelope<PersonRegisteredEvent>(new PersonRegisteredEvent("foo")));

            Assert.True(handledInterface);
            Assert.True(handledClass);
        }

        [Test]
        public void envelopes_of_derived_messages_should_get_handled()
        {
            var handledInterface = false;
            var handledClass = false;
            _registry.Register<Envelope<RegisteredEvent>>(x => handledInterface = true);
            _registry.Register<Envelope<PersonRegisteredEvent>>(x => handledClass = true);

            execute(new MessageEnvelope<CriminalRegisteredEvent>(new CriminalRegisteredEvent("foo")));

            Assert.True(handledInterface);
            Assert.True(handledClass);
        }


        [Test]
        public void should_work_with_value_types()
        {
            var handled = false;
            _registry.Register<int>(x => handled = true);

            execute(42);

            Assert.True(handled);
        }

        [Test]
        public void should_work_with_strings()
        {
            var handled = false;
            _registry.Register<string>(x => handled = true);

            execute("42");

            Assert.True(handled);
        }

        [Test]
        public void should_work_with_envelope_of_value_types()
        {
            var handled = false;
            _registry.Register<Envelope<int>>(x => handled = true);

            execute(new MessageEnvelope<int>(42));

            Assert.True(handled);
        }


        [Test]
        public void should_work_with_envelope_of_strings()
        {
            var handled = false;
            _registry.Register<Envelope<string>>(x => handled = true);

            execute(new MessageEnvelope<string>("42"));

            Assert.True(handled);
        }

        [Test]
        public void should_work_with_enumerables_of_strings()
        {
            var handled = false;
            _registry.Register<Envelope<IEnumerable<string>>>(x => handled = true);

            execute(new MessageEnvelope<IEnumerable<string>>(new[] { "42" }));

            Assert.True(handled);
        }

        [Test]
        public void should_work_with_enumerables_of_objects()
        {
            var handled = false;
            _registry.Register<Envelope<IEnumerable<RegisteredEvent>>>(x => handled = true);

            execute(new MessageEnvelope<IEnumerable<PersonRegisteredEvent>>(new[] { new PersonRegisteredEvent("42") }));

            Assert.True(handled);
        }

        [Test]
        public void should_maintain_order_with_message_first()
        {
            var queue = new ConcurrentQueue<int>();
            _registry.Register<Envelope<PersonRegisteredEvent>>(x => queue.Enqueue(1));
            _registry.Register<Envelope<RegisteredEvent>>(x => queue.Enqueue(2));

            execute(new MessageEnvelope<CriminalRegisteredEvent>(new CriminalRegisteredEvent("foo")));


            Assert.AreEqual(1, queue.ElementAtOrDefault(0));
            Assert.AreEqual(2, queue.ElementAtOrDefault(1));
        }

        [Test]
        public void should_maintain_order_with_message_interface_first()
        {
            var queue = new ConcurrentQueue<int>();
            _registry.Register<Envelope<RegisteredEvent>>(x => queue.Enqueue(2));
            _registry.Register<Envelope<PersonRegisteredEvent>>(x => queue.Enqueue(1));

            execute(new MessageEnvelope<CriminalRegisteredEvent>(new CriminalRegisteredEvent("foo")));


            Assert.AreEqual(2, queue.ElementAtOrDefault(0));
            Assert.AreEqual(1, queue.ElementAtOrDefault(1));
        }

        [Test]
        public void should_handle_for_untyped_envelope_interface()
        {
            var handled = false;
            _registry.Register<Envelope>(x => handled = true);

            execute(new MessageEnvelope<CriminalRegisteredEvent>(new CriminalRegisteredEvent("foo")));

            Assert.True(handled);
        }

        [Test]
        public void should_handle_for_generic_concrete_envelop()
        {
            var handled1 = false;
            var handled2 = false;

            _registry.Register<ConcreteEnvelope<PersonRegisteredEvent>>(x => handled1 = true);
            _registry.Register<ConcreteEnvelope<CancelledEvent>>(x => handled2 = true);

            execute(new ConcreteEnvelope<PersonRegisteredEvent>(new PersonRegisteredEvent("john")));

            Assert.IsTrue(handled1);
            Assert.IsFalse(handled2);
        }

        [Test]
        public void should_handle_for_concrete_envelope_of_interface_of_message()
        {
            var handled = false;

            _registry.Register<ConcreteEnvelope<RegisteredEvent>>(x => handled = true);

            execute(new ConcreteEnvelope<PersonRegisteredEvent>(new PersonRegisteredEvent("john")));

            Assert.IsTrue(handled);
        }

        private void execute(object message)
        {
            var handlers = _registry.GetValuesFor(message);
            foreach (var handlerEntry in handlers)
            {
                handlerEntry.MappedValue(message);
            }
        }
    }
}