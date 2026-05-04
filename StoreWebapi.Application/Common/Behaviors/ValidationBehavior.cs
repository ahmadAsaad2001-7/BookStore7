using FluentValidation;
using MediatR;
using StoreWebapi.Application.Features.Books.SearchBooks;

namespace StoreWebapi.Application.Common.Behaviors;

public class ValidationBehavior <TRequest,TResponse>(IEnumerable<IValidator<TRequest>> validators) : 
    IPipelineBehavior<TRequest,TResponse> where TRequest : notnull
{
 

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var Context= new ValidationContext<TRequest>(request);
        var results = await Task.WhenAll(validators.Select(validator => validator.ValidateAsync(Context, cancellationToken)));
        var failures = results.SelectMany(result => result.Errors).Where(error => error != null).ToList();
        if (request is GetBookByIdQuery query)
        {
            Console.WriteLine($"ValidationBehavior received query with Id: {query.Id}");
        }

        if (failures.Any())
        {
            
            var responseType = typeof(TResponse);
            var errorMessages = string.Join(" ; ", failures.Select(x => x.ErrorMessage));

            if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
            {
                var resultType = responseType.GetGenericArguments()[0];
                
                var failureMethod = typeof(Result)
                    .GetMethods()
                    .FirstOrDefault(m => m.Name == "Failure" && m.IsGenericMethod);

                if (failureMethod != null)
                {
                    var genericFailureMethod = failureMethod.MakeGenericMethod(resultType);
            
                   
                    var failureResult = genericFailureMethod.Invoke(null, new object[] { errorMessages });
            
                    return (TResponse)failureResult!;
                }
            }
            else if (typeof(TResponse) == typeof(Result))
            {
       
                var failureResult = Result.Failure(errorMessages);
                return (TResponse)(object)failureResult;
            }
            else
            {
                throw new ValidationException(failures);
            }
        }

        return await next(cancellationToken);
    }
    
}