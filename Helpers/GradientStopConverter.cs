using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace LevelBarApp.Helpers
{
    /// <summary>
    /// Converter to setup the gradients of level bar
    /// </summary>
    public class GradientStopConverter : IMultiValueConverter
    {
        /// <summary>
        /// Converts the input values representing a level and a container level bar height to a GradientStopCollection.
        /// It scales the level value and creates a gradient of colors based on the scaled height.
        /// The resulting GradientStopCollection represents a color gradient used for visualizing the level.
        /// </summary>
        /// <param name="values">An array of input values containing the level and container height.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>A GradientStopCollection representing the color gradient for the level bar visualization.</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2 || !(values[0] is float) || !(values[1] is int))
            {
                // Return Binding.DoNothing if the condition is not met.
                return Binding.DoNothing;
            }

            float level = (float)values[0];
            int levelBarContainerHeight = (int)values[1];

            // Scale the level value to fit within the level bar container height range.
            double scaledLevel = (level * 10 * levelBarContainerHeight);

            // Ensure that the scaled level does not exceed barLevelContainerHeight.
            double barLevelHeight = Math.Min(scaledLevel, levelBarContainerHeight);

            // Create a GradientStopCollection.
            GradientStopCollection stops = new GradientStopCollection();

            // Define GradientStops for the green, greenYellow, yellow, orange, and red colors.
            GradientStop greenGradientStop = new GradientStop(Colors.Green, 0.0);
            GradientStop greenYellowGradientStop = new GradientStop(Colors.GreenYellow, 0.25);
            GradientStop yellowGradientStop = new GradientStop(Colors.Yellow, 0.5);
            GradientStop orangeGradientStop = new GradientStop(Colors.Orange, 0.75);
            GradientStop redGradientStop = new GradientStop(Colors.Red, 1.0);

            // Determine which gradients to add based on level bar height.
            if (barLevelHeight >= levelBarContainerHeight * 0.75)
            {
                stops.Add(greenGradientStop);
                stops.Add(greenYellowGradientStop);
                stops.Add(yellowGradientStop);
                stops.Add(orangeGradientStop);
                stops.Add(redGradientStop);
            }
            else if (barLevelHeight >= levelBarContainerHeight * 0.5)
            {
                stops.Add(greenGradientStop);
                stops.Add(greenYellowGradientStop);
                stops.Add(yellowGradientStop);
                stops.Add(orangeGradientStop);
            }
            else if (barLevelHeight >= levelBarContainerHeight * 0.25)
            {
                stops.Add(greenGradientStop);
                stops.Add(greenYellowGradientStop);
                stops.Add(yellowGradientStop);
            }
            else if (barLevelHeight >= levelBarContainerHeight * 0.1)
            {
                stops.Add(greenGradientStop);
                stops.Add(greenYellowGradientStop);
            }
            else if (barLevelHeight >= 0)
            {
                stops.Add(greenGradientStop);
            }

            return stops;
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
