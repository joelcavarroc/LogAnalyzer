namespace LogAnalyzer
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Converter for time span to string. It displays the integer value of total number rounded to floor and the minutes...
    /// </summary>
    internal class TotalHoursTimeSpanConverter : IValueConverter
    {
        #region Public Methods and Operators

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ( (targetType != typeof(string)) || !(value is TimeSpan))
            {
                return value;
            }

            TimeSpan timespan = (TimeSpan)value;

            return $"{timespan.Hours + timespan.Days * 24:00}:{timespan.Minutes:00;00;00}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        #endregion
    }
}