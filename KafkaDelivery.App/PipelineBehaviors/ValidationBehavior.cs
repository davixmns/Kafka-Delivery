using FluentValidation;
using KafkaDelivery.App.Wrappers;
using MediatR;

namespace KafkaDelivery.App.PipelineBehaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
        Console.WriteLine(validators.Count());
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken))
        );

        var failures = validationResults
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Any())
        {
            var errorMessages = failures.Select(f => f.ErrorMessage).ToList();

            // Cria um AppResult<TResponse> sem aninhamento
            var appResultType = typeof(AppResult<>).MakeGenericType(typeof(TResponse).GetGenericArguments()[0]);
            var appResult = Activator.CreateInstance(appResultType);

            // Define as propriedades de erro
            var errorsProperty = appResultType.GetProperty("ErrorMessage");
            if (errorsProperty != null)
                errorsProperty.SetValue(appResult, string.Join("; ", errorMessages));

            var successProperty = appResultType.GetProperty("IsSuccess");
            if (successProperty != null)
            {
                successProperty.SetValue(appResult, false);
            }

            return (TResponse) appResult;
        }

        // Se não houver falhas, continua com o próximo handler
        return await next();
    }
}
