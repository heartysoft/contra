(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use 
// it to define helpers that you do not want to show in the documentation.
#I "../../bin"

(**
contra
======================

Documentation

<div class="row">
  <div class="span1"></div>
  <div class="span6">
    <div class="well well-small" id="nuget">
      The contra library can be <a href="https://nuget.org/packages/contra">installed from NuGet</a>:
      <pre>PM> Install-Package contra</pre>
    </div>
  </div>
  <div class="span1"></div>
</div>

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

*)
