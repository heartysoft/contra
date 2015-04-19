
# Contra

Contra is a type registry to map .NET types to anything. Types can be registered against values, and the values can be looked up by type.
Type resolution honours contravariance, so values registered against interfaces, base classes, etc. resolve correctly. Even structures like
Envelope<Message> work as expected.
    
Read the [Getting started tutorial](http://heartysoft.github.io/contra/index.html#Getting-started) to learn more.

Documentation: http://heartysoft.github.io/contra

Example
-------

Assuming PersonRegistered implements Started, the following code will trigger both handlers.

```
var registry = new TypeRegistry<Action<object>>()
    .Register<PersonRegistered>(x => Console.WriteLine("Person registered."))
    .Register<Started>(x => Console.WriteLine("Something started."));

var msg = new PersonRegistered(...)
var handlers = registry.GetValuesFor(msg);

foreach(var h in handlers) h(msg);

```

Assuming PersonRegistered implements Started, the following code will trigger both handlers. Here, Envelope<T> is an interface implemented by
MessageEnvelope<T>.

```
var registry = new TypeRegistry<Action<object>>()
    .Register<Envelope<PersonRegistered>>(x => Console.WriteLine("Person registered."))
    .Register<Envelope<Started>>(x => Console.WriteLine("Something started."));

var msg = new MessageEnvelope<PersonRegistered>(new PersonRegistered(...))
var handlers = registry.GetValuesFor(msg);

foreach(var h in handlers) h(msg);

```

Inverse loopuks can be used for type mappings to keys:

```
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

```

## Maintainer(s)

- [@ashic](https://github.com/ashic)

