namespace KafkaDelivery.App.Wrappers;

public class AppResult<T> 
{
    public T? Data { get; private set; }
    public bool IsSuccess { get; private set; }
    public string? ErrorMessage { get; private set; }

    public AppResult()
    {
    }
    
    private AppResult(T? data, bool isSuccess, string? errorMessage)
    {
        Data = data;
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static AppResult<T> Success(T data) => new AppResult<T>(data, true, null);
    public static AppResult<T> Failure(string errorMessage) => new AppResult<T>(default, false, errorMessage);
}