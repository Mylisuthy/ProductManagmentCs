using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using webProducts.Infrastructure.Data;

namespace webProducts.Infrastructure.Extensions;

public static class MigrationExtensions
{
    // Usamos 'IHost' para aplicar las migraciones antes de que la aplicación corra.
    public static async Task ApplyMigrationsAsync(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                // Obtenemos una instancia de tu DbContext
                var dbContext = services.GetRequiredService<AppDbContext>();
                
                // Aplicamos cualquier migración pendiente a la base de datos
                await dbContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                // Registramos el error si la migración falla (es crucial en el despliegue)
                var logger = services.GetRequiredService<ILogger<IHost>>();
                logger.LogError(ex, "Ocurrió un error al aplicar las migraciones al iniciar la aplicación.");
                // En un escenario de despliegue crítico, podríamos relanzar la excepción.
                throw; 
            }
        }
    }
}