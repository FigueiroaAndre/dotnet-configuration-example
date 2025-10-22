# .NET Configuration Example
This repository contains an example of how to use configuration in a .NET application. It demonstrates how to read settings from various sources such as JSON files, environment variables, command-line arguments and custom configuration providers and how to use option pattern to encapsulate configuration settings.

## Explanation
The example application will define multiple values for the section "GeneralSettings" from different configuration sources.

`appsettings.json` defines values from "Settings1" to "Settings7".

`appsettings.Development.json` overrides "Settings2" to "Settings7".

.NET user secrets overrides "Settings3" to "Settings7".

Environment variables override "Settings4" to "Settings7".

Command-line arguments override "Settings5" to "Settings7".

Custom configuration provider using a PostgreSQL overrides "Settings6" and "Settings7".

## Setup
### User-Secrets
User-secrets were already initialized for this project. Use the following commands to set "Settings3" to "Settings7" in "GeneralSettings":
```bash
dotnet user-secrets set "GeneralSettings:Settings3" "From user-secrets"
dotnet user-secrets set "GeneralSettings:Settings3" "From user-secrets"
dotnet user-secrets set "GeneralSettings:Settings4" "From user-secrets"
dotnet user-secrets set "GeneralSettings:Settings5" "From user-secrets"
dotnet user-secrets set "GeneralSettings:Settings6" "From user-secrets"
dotnet user-secrets set "GeneralSettings:Settings7" "From user-secrets"
```
### Environment Variables
Set the following environment variables to override "Settings4" to "Settings7":
```bash
export GeneralSettings__Settings4="From environment-variables"
export GeneralSettings__Settings5="From environment-variables"
export GeneralSettings__Settings6="From environment-variables"
export GeneralSettings__Settings7="From environment-variables"
```

### Command-Line Arguments
When running the application, provide the following command-line arguments to override "Settings5" to "Settings7":
```bash
dotnet run -e "GeneralSettings:Settings7"="From cli-args" -e "GeneralSettings:Settings6"="From cli-args" -e "GeneralSettings:Settings5"="From cli-args"
```

### PostgreSQL
1. Ensure you have a PostgreSQL database running.

For this example lets create one with Docker:
```bash
docker volume create general-settings-db-volume

docker run -d \
	-e POSTGRES_PASSWORD=123456 \
	-e POSTGRES_DB=general-settings-db \
	-e POSTGRES_USER=root \
	-p 5432:5432 \
	--name general-settings-db \
	-v general-settings-db-volume:/var/lib/postgresql/data \
	postgres
```
2. Setup the connection string:

The application uses the value stored in the configuration "ConnectionStrings:DefaultConnection" to connect to the PostgreSQL database. You can set this configuration whatever way you prefer (appsettings.json, environment variable, user-secrets, etc). Here is an example of setting it using user-secrets. Notice that the connection string points to the Docker container created in the previous step, any changes on the command in the last step should be reflected here.
```
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=general-settings-db;Username=root;Password=123456"
```

3. Create and Run migrations

The model and DbContext for the custom configuration provider is already defined in the project. You just need to create and run the migrations:
```bash
dotnet ef migrations add CreateSettingsTable --context ApplicationDBContext
dotnet ef database update --context ApplicationDBContext
```

4. Insert the settings into the database:

Run the following SQL commands to insert "Settings6" and "Settings7" into the database:
```sql
insert into public."Settings" ("Id", "Key", "Value")
values
    (0, 'GeneralSettings:Settings6', 'From postgreSQL'),
    (1, 'GeneralSettings:Settings7', 'From postgreSQL');
```

5. [Optional] Setup a database trigger to automatically reload configuration on changes:

You can create a trigger function and a trigger to notify the application when the "Settings" table is updated.
```sql
create or replace function notify_settings_change()
returns trigger as $$
begin
    perform pg_notify('settings_changed', 'settings updated');
    return new;
end;
$$ language plpgsql;

drop trigger if exists settings_change_trigger on public."Settings";

create trigger settings_change_trigger
after insert or update or delete on public."Settings"
for each row
execute function notify_settings_change();
```

## Running the Application
Remember to run the application with the command-line arguments mentioned above to see the full effect of configuration overrides:
```bash
dotnet run -e "GeneralSettings:Settings7"="From cli-args" -e "GeneralSettings:Settings6"="From cli-args" -e "GeneralSettings:Settings5"="From cli-args"
```

## Expected output
The application define 3 endpoints to see the configuration values:
- `/general` - Uses `IOptions` interface to get configuration values, never updates.
- `/general-snapshot` - Uses `IOptionsSnapshot` interface to get configuration values, reads updated values on scope creation (every request).
- `/general-monitor` - Uses `IOptionsMonitor` interface to get configuration values, updates on change notification.

Initially all the endpoints should return the following output:
```json
{
    "settings1":"From appsettings.json",
    "settings2":"From appsettings.Development.json",
    "settings3":"From user-secrets",
    "settings4":"From environment-variables",
    "settings5":"From cli-args",
    "settings6":"From postgreSQL",
    "settings7":"From postgreSQL"
}
```

The output of `/general` will remain the same until the application is restarted.

The output of `/general-snapshot` and `/general-monitor` will update automatically when changes are made to any of the configuration sources, including changes made directly to the PostgreSQL database if the trigger is set up.
