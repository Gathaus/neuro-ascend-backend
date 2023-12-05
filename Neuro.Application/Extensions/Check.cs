using Neuro.Application.Exceptions;

namespace Neuro.Application.Extensions;

public static class Check
{
    public static void EntityExists<TEntity>(TEntity? entity, string? errorMessage = null) where TEntity : class
    {
        if (entity == null)
            throw new EntityNotFoundException("Wrong Input",
                errorMessage ?? $"{typeof(TEntity).Name} could not be found. Please check your input.");
    }

    public static void IsNull(object? obj, string? errorMessage)
    {
        if (obj == null)
            throw new InputCannotBeNullException("Input Is Null", errorMessage );
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