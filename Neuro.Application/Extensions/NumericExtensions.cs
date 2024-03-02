namespace Neuro.Application.Extensions
{
    public static class NumericExtensions
    {
        /// <summary>
        /// Checks if the given nullable value is null.
        /// </summary>
        /// <typeparam name="T">Value type that is nullable and implements IComparable<T>.</typeparam>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the value is null, False otherwise.</returns>
        public static bool IsNull<T>(this T? value)
            where T : struct, IComparable<T> => !value.HasValue;

        /// <summary>
        /// Checks if the given nullable integer is null.
        /// </summary>
        /// <param name="value">The integer value to check.</param>
        /// <returns>True if the integer value is null, False otherwise.</returns>
        public static bool IsNull(this int? value) => !value.HasValue;

        /// <summary>
        /// Checks whether the given numeric value is null or zero.
        /// </summary>
        /// <typeparam name="T">Numeric data type.</typeparam>
        /// <param name="value">The numeric value</param>
        /// <returns>True if the value is null or zero, False otherwise.</returns>
        public static bool IsNullOrZero<T>(this T? value)
            where T : struct, IComparable<T>
        {
            if (!value.HasValue)
                return true;

            return Convert.ToDecimal(value.Value) == 0;
        }

        /// <summary>
        /// Checks whether the given numeric value is null or below zero.
        /// </summary>
        /// <typeparam name="T">Numeric data type.</typeparam>
        /// <param name="value">The numeric value</param>
        /// <returns>True if the vahue is null or below zero, False otherwise.</returns>
        public static bool IsNullOrBelowZero<T>(this T? value)
            where T : struct, IComparable<T>
        {
            if (!value.HasValue)
                return true;
            //below or equal to zero
            return Convert.ToDecimal(value.Value) <= 0;
        }

        /// <summary>
        /// Checks whether the given nullable integer value is null or zero.
        /// </summary>
        /// <param name="value">The nullable integer value to check.</param>
        /// <returns>True if the value is null or zero, False otherwise.</returns>
        public static bool IsNullOrZero(this int? value)
        {
            if (!value.HasValue)
                return true;

            return value.Value == 0;
        }

        public static decimal Percentage(decimal numerator, decimal denominator, int decimalPlaces = 2,
            decimal? upperLimit = null, decimal? lowerLimit = null)
        {
            if (denominator == 0)
                return 0;

            decimal result = (numerator / denominator) * 100;
            result = Math.Round(result, decimalPlaces);

            if (upperLimit.HasValue)
            {
                decimal roundedUpperLimit = Math.Round(upperLimit.Value, decimalPlaces);
                if (result > roundedUpperLimit)
                    return roundedUpperLimit;
            }

            if (lowerLimit.HasValue)
            {
                decimal roundedLowerLimit = Math.Round(lowerLimit.Value, decimalPlaces); 
                if (result < roundedLowerLimit)
                    return roundedLowerLimit;
            }

            return result; 
        }
    }
}