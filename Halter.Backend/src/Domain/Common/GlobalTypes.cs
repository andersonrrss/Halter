namespace Halter.Domain.Common;

public sealed class FieldErrors : Dictionary<string, string[]>
{
    public FieldErrors(string field, string errorCode) : base()
    {
        this[field] = [errorCode];
    }

    public FieldErrors(Dictionary<string, string[]> dict) : base(dict) {}
}