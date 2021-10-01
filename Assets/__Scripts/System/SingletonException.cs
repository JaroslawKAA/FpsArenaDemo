using System;

public class SingletonException : Exception
{
    public override string Message => "Try to instantiate second singleton.";

    public SingletonException()
    {
    }

    public SingletonException(string message) : base(message)
    {
    }
}
