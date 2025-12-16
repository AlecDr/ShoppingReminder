using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace ShoppingReminder.Infrastructure.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // Encontra o projeto API
        var currentDir = Directory.GetCurrentDirectory();
        var apiProjectPath = Path.GetFullPath(Path.Combine(currentDir, "..", "ShoppingReminder.API"));

        // Se não encontrar, tenta da raiz
        if (!Directory.Exists(apiProjectPath))
        {
            apiProjectPath = Path.GetFullPath(Path.Combine(currentDir, "ShoppingReminder.API"));
        }

        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiProjectPath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        // Primeiro tenta ler o UserSecretsId do .csproj
        var csprojPath = Path.Combine(apiProjectPath, "ShoppingReminder.API.csproj");
        if (File.Exists(csprojPath))
        {
            var csprojContent = File.ReadAllText(csprojPath);
            var userSecretsIdMatch = System.Text.RegularExpressions.Regex.Match(
                csprojContent,
                @"<UserSecretsId>(.*?)</UserSecretsId>"
            );

            if (userSecretsIdMatch.Success)
            {
                var userSecretsId = userSecretsIdMatch.Groups[1].Value;
                configuration = new ConfigurationBuilder()
                    .SetBasePath(apiProjectPath)
                    .AddJsonFile("appsettings.json", optional: true)
                    .AddJsonFile("appsettings.Development.json", optional: true)
                    .AddUserSecrets(userSecretsId)
                    .AddEnvironmentVariables()
                    .Build();
            }
        }

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException(
                $"Connection string 'DefaultConnection' not found.\n" +
                $"API Project Path: {apiProjectPath}\n" +
                $"Please set it using: dotnet user-secrets set \"ConnectionStrings:DefaultConnection\" \"your-connection-string\" --project ShoppingReminder.API/ShoppingReminder.API.csproj"
            );
        }

        Console.WriteLine($"Using connection string from: {apiProjectPath}");

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(
            connectionString,
            b => b.MigrationsAssembly("ShoppingReminder.Infrastructure")
        );

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}