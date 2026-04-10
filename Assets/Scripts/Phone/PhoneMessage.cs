public enum SenderType
{
    Lost,
    Operator
}

public struct PhoneMessage
{
    public SenderType SenderType;
    public string Message;

    public override string ToString()
    {
        return $"<{SenderType}>\n{Message}";
    }
}
