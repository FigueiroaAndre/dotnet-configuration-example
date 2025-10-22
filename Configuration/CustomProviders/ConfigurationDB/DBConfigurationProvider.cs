using ConfigurationExample.Data;
using Npgsql;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationExample.Configuration;

public class DBConfigurationProvider : ConfigurationProvider, IDisposable
{
    private readonly ApplicationDBContext _dbContext;
    private NpgsqlConnection? _listenConnection;
    private CancellationTokenSource? _cancellationTokenSource;

    public DBConfigurationProvider(ApplicationDBContext dbContext)
    {
        _dbContext = dbContext;
        Data = new Dictionary<string, string?>();
    }

    public override void Load()
    {
        var settings = _dbContext.Settings.AsNoTracking().ToList();

        if (settings is not null)
        {
            Data.Clear();
            foreach (var setting in settings)
            {
                Data.Add(setting.Key, setting.Value);
            }
        }

        if (_listenConnection == null)
        {
            StartListening();
        }
    }

    private void StartListening()
    {
        var connectionString = _dbContext.Database.GetConnectionString();

        _listenConnection = new NpgsqlConnection(connectionString);
        _listenConnection.Open();

        using (var cmd = _listenConnection.CreateCommand())
        {
            cmd.CommandText = "LISTEN settings_changed;";
            cmd.ExecuteNonQuery();
        }

        _listenConnection.Notification += (o, e) =>
        {
            if (e.Channel == "settings_changed")
            {
                ReloadSettings();
            }
        };

        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;

        Task.Run(async () =>
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    // Wait for notifications
                    await _listenConnection.WaitAsync(token);
                }
            }
            catch (OperationCanceledException)
            {
                // Task was cancelled, exit gracefully
            }
        }, token);
    }

    private void ReloadSettings()
    {
        Load();
        OnReload();
    }

    public void Dispose()
    {
        try
        {
            _cancellationTokenSource?.Cancel();
            _listenConnection?.Close();
            _listenConnection?.Dispose();
            _cancellationTokenSource?.Dispose();
        }
        catch
        {

        }
    }
}
