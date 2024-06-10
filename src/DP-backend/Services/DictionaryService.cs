using System.ComponentModel;
using System.Reflection;
using DP_backend.Common;
using DP_backend.Common.EntityType;
using DP_backend.Database;
using LazyCache;
using Mapster.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace DP_backend.Services;

public interface IEnumDictionaryService
{
    public record Entry(long Value, string Description);

    IEnumerable<Entry> DescribeEnum<TEnum>() where TEnum : Enum;
    IEnumerable<Entry> DescribeEnum(Type enumType);
}

public class DictionaryService : IEntityTypesProvider, IEnumDictionaryService
{
    private readonly IAppCache _cache;
    private readonly ApplicationDbContext _dbContext;

    public DictionaryService(IAppCache cache, ApplicationDbContext dbContext)
    {
        _cache = cache;
        _dbContext = dbContext;
    }

    private static string KeyFor(Type type) => $"dictionary/{type.FullName}";
    private static string KeyFor<T>() => KeyFor(typeof(T));

    public IEnumerable<IEnumDictionaryService.Entry> DescribeEnum<TEnum>() where TEnum : Enum => DescribeEnum(typeof(TEnum));

    public IEnumerable<IEnumDictionaryService.Entry> DescribeEnum(Type enumType)
    {
        return _cache.GetOrAdd(
            KeyFor(enumType),
            () => DescribeEnumInternal(enumType).ToArray());
    }

    private IEnumerable<IEnumDictionaryService.Entry> DescribeEnumInternal(Type enumType)
    {
        foreach (var enumRawValue in enumType.GetEnumValuesAsUnderlyingType())
        {
            var enumName = Enum.GetName(enumType, enumRawValue)!;
            var enumField = enumType.GetField(enumName)!;

            var enumFieldName = enumField.GetCustomAttribute<DescriptionAttribute>()?.Description
                                ?? enumName;

            yield return new(Convert.ToInt64(enumRawValue), enumFieldName);
        }
    }

    public Task<Dictionary<string, EntityType>> GetEntityTypes(CancellationToken ct = default)
    {
        return _cache.GetOrAddAsync(
            KeyFor<EntityType>(),
            () => _dbContext.Set<EntityType>().AsNoTracking().ToDictionaryAsync(x => x.Id, ct));
    }
}