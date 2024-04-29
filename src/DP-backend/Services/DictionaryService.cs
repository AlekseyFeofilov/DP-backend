using System.ComponentModel;
using System.Reflection;

namespace DP_backend.Services;

public record DictionaryEntry(long Value, string Description);

public static class DictionaryService
{
    public static IEnumerable<DictionaryEntry> DescribeEnum<TEnum>() where TEnum : Enum => DescribeEnum(typeof(TEnum)); 
    
    public static IEnumerable<DictionaryEntry> DescribeEnum(Type enumType)
    {
        // todo cache
        foreach (var enumRawValue in enumType.GetEnumValuesAsUnderlyingType())
        {
            var enumName = Enum.GetName(enumType, enumRawValue)!;
            var enumField = enumType.GetField(enumName)!;

            var enumFieldName = enumField.GetCustomAttribute<DescriptionAttribute>()?.Description
                                ?? enumName;

            yield return new DictionaryEntry(Convert.ToInt64(enumRawValue), enumFieldName);
        }
    }
}