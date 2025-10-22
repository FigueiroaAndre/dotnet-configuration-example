using ConfigurationExample.Data;

namespace ConfigurationExample.Configuration;

public class DBConfigurationSource : IConfigurationSource
{
    private readonly ApplicationDBContext _dbContext;
    public DBConfigurationSource(ApplicationDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new DBConfigurationProvider(_dbContext);
    }
}
