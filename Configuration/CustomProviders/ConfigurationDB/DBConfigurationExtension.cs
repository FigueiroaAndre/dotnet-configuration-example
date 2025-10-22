using ConfigurationExample.Data;

namespace ConfigurationExample.Configuration;

public static class DBConfigurationExtension
{
    public static IConfigurationBuilder AddDBConfiguration(this IConfigurationBuilder builder, ApplicationDBContext dbContext)
    {
        return builder.Add(new DBConfigurationSource(dbContext));
    }
}
