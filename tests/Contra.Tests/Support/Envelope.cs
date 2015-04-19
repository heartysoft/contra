namespace Contra.Tests.Support
{
    public interface Envelope
    {
        object UntypedMessage { get; }
    }

    public interface Envelope<out T> : Envelope
    {
        T Message { get; }
    }
}