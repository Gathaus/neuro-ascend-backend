
using Neuro.Application.Exceptions;

namespace Neuro.Application.Extensions;

public static class Check
{
    public static void EntityExists<TEntity>(TEntity? entity, string? errorMessage = null) where TEntity : class
    {
        if (entity == null)
            throw EntityException.EntityExceptions.EntityNotFound;
    }

    public static void IsNull(object? obj, string? errorMessage)
    {
        if (obj == null)
            throw InputExceptions.InputCannotBeEmpty;
    }

    

    
   
    public static void IsNullOrEmpty(string value, string? errorMessage)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException(errorMessage);
    }

    public static void IsNullOrWhiteSpace(string value, string? errorMessage)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(errorMessage);
    }

}