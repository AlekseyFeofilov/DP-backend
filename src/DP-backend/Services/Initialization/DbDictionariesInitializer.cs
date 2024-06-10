using DP_backend.Common;
using DP_backend.Common.EntityType;
using DP_backend.Database;
using DP_backend.Domain.Employment;
using Microsoft.EntityFrameworkCore;

namespace DP_backend.Services.Initialization;

public static class DbDictionariesInitializer
{
    public static async Task Initialize(this IServiceProvider serviceProvider, IConfiguration configuration)
    {
        await using (var serviceScope = serviceProvider.CreateAsyncScope())
        {
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await InitEntityTypes(dbContext);
        }
    }

    /// <summary>
    /// Заполняем справозник типов сущностей 
    /// </summary>
    /// <remarks>Существующие записи не обновляются, источник актуальных значений - БД</remarks>
    private static async Task InitEntityTypes(ApplicationDbContext context)
    {
        EntityType[] initialEntityTypes =
        [
            //@formatter:off
            new() { Id = EntityTypeIds.EmploymentRequest,      Description = "Заявка на трудоустройство",      Usage = EntityTypeUsage.LinkComment | EntityTypeUsage.LinkFile },
            new() { Id = EntityTypeIds.InternshipRequest,      Description = "Заявка на прохождение правтики", Usage = EntityTypeUsage.LinkComment | EntityTypeUsage.LinkFile },
            new() { Id = EntityTypeIds.InternshipDiaryRequest, Description = "Заявка для дневника практики",   Usage = EntityTypeUsage.LinkComment | EntityTypeUsage.LinkFile },
            new() { Id = EntityTypeIds.CourseWorkRequest,      Description = "Заявка для для курсовых и ВКР",  Usage = EntityTypeUsage.LinkComment | EntityTypeUsage.LinkFile },
            new() { Id = EntityTypeIds.EmploymentVariant,      Description = "Вариант трудоустройства",        Usage = EntityTypeUsage.LinkComment | EntityTypeUsage.LinkFile },
            
            // Сюда можно добавить новые типы 
            
            //@formatter:on
        ];

        var existingIds = await context.Set<EntityType>().Select(x => x.Id).ToListAsync();

        context.Set<EntityType>()
            .AddRange(initialEntityTypes.ExceptBy(existingIds, x => x.Id));

        await context.SaveChangesAsync();
    }
}