using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace StudentCoursesApp.Converters;

public class StringIsNotNullConverter : IValueConverter
{
    public static readonly StringIsNotNullConverter Instance = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is string str && !string.IsNullOrEmpty(str);

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}