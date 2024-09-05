namespace KafkaDelivery.Domain.Wrappers;

public class DomainResult
{
    public bool IsSuccess { get; }
    public string ErrorMessage { get; }

    private DomainResult(bool isSuccess, string errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static DomainResult Success()
    {
        return new DomainResult(true, string.Empty);
    }

    public static DomainResult Failure(string errorMessage)
    {
        return new DomainResult(false, errorMessage);
    }
}