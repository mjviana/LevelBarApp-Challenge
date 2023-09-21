using System;
using System.Globalization;
using System.Windows.Data;

namespace LevelBarApp.Helpers
{
    /// <summary>
    /// Converter to setup the height of the level bar 
    /// </summary>
    public class LevelBarHeightConverter : IMultiValueConverter
    {
        /// <summary>
        /// Converts the input values representing a level and a maximum level bar height to a scaled level bar height.
        /// It scales the level value to fit within the maximum level bar height range.
        /// The resulting level bar height is used for visualizing the level within the level bar control.
        /// </summary>
        /// <param name="values">An array of input values containing the level and maximum level bar height.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>The scaled level bar height for visualizing the level within the level bar control.</returns>

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2 || !(values[0] is float) || !(values[1] is int))
            {
                // Return Binding.DoNothing if the condition is not met.
                return Binding.DoNothing;
            }

            float level = (float)values[0];
            int levelBarMaxHeight = (int)values[1];

            // Scale the level value to fit within the level car container height range.
            double scaledLevel = (level * 10 * levelBarMaxHeight);

            // Ensure that the scaled level does not exceed level bar container height.
            double levelBarHeight = Math.Min(scaledLevel, levelBarMaxHeight);

            return levelBarHeight;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetTypes"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}

