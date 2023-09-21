using System;
using System.Globalization;
using System.Windows.Data;

namespace LevelBarApp.Helpers
{
    /// <summary>
    /// Converter for linear values to decibels (bd)
    /// </summary>
    public class LinearToDecibelConverter : IValueConverter
    {
        /// <summary>
        /// Converts the input value representing a linear value level logarithmic value.
        /// </summary>
        /// <param name="value">The input value</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>A logarithmic value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is float)
            {
                double linearValue = (float)value;
                return 20 * Math.Log10(linearValue);
            }
            else
                return Binding.DoNothing;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //TODO
            throw new NotImplementedException();
        }
    }
}
