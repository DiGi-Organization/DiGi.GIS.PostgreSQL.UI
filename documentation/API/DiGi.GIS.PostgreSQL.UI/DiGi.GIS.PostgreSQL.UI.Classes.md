#### [DiGi\.GIS\.PostgreSQL\.UI](DiGi.GIS.PostgreSQL.UI.Overview.md 'DiGi\.GIS\.PostgreSQL\.UI\.Overview')

## DiGi\.GIS\.PostgreSQL\.UI\.Classes Namespace
### Classes

<a name='DiGi.GIS.PostgreSQL.UI.Classes.BuildingModelsFromDatabaseOptions'></a>

## BuildingModelsFromDatabaseOptions Class

Provides configuration options for generating BuildingModels from CityGML Buildings stored in a database\.

Held by the user interface task rather than by a class library task, so it lives in this project: the run it configures is started from the user interface, through [BuildingModelsFromDatabaseOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.BuildingModelsFromDatabaseOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.BuildingModelsFromDatabaseOptionsWindow').

Carries the scope of a run only. The request pacing and the page size are tuning nobody changes between runs, so they stay properties of the task and are set where it is registered.

```csharp
public class BuildingModelsFromDatabaseOptions : DiGi.Core.Classes.SerializableOptions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.Object](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.object 'DiGi\.Core\.Classes\.Object') → [DiGi\.Core\.Classes\.SerializableObject](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.serializableobject 'DiGi\.Core\.Classes\.SerializableObject') → [DiGi\.Core\.Classes\.SerializableOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.serializableoptions 'DiGi\.Core\.Classes\.SerializableOptions') → BuildingModelsFromDatabaseOptions
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.BuildingModelsFromDatabaseOptions.BuildingModelsFromDatabaseOptions()'></a>

## BuildingModelsFromDatabaseOptions\(\) Constructor

Initializes a new instance of the [BuildingModelsFromDatabaseOptions](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.BuildingModelsFromDatabaseOptions 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.BuildingModelsFromDatabaseOptions') class\.

```csharp
public BuildingModelsFromDatabaseOptions();
```

<a name='DiGi.GIS.PostgreSQL.UI.Classes.BuildingModelsFromDatabaseOptions.BuildingModelsFromDatabaseOptions(DiGi.GIS.PostgreSQL.UI.Classes.BuildingModelsFromDatabaseOptions)'></a>

## BuildingModelsFromDatabaseOptions\(BuildingModelsFromDatabaseOptions\) Constructor

Initializes a new instance of the [BuildingModelsFromDatabaseOptions](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.BuildingModelsFromDatabaseOptions 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.BuildingModelsFromDatabaseOptions') class by copying the values from an existing [BuildingModelsFromDatabaseOptions](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.BuildingModelsFromDatabaseOptions 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.BuildingModelsFromDatabaseOptions') instance\.

```csharp
public BuildingModelsFromDatabaseOptions(DiGi.GIS.PostgreSQL.UI.Classes.BuildingModelsFromDatabaseOptions? buildingModelsFromDatabaseOptions);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.BuildingModelsFromDatabaseOptions.BuildingModelsFromDatabaseOptions(DiGi.GIS.PostgreSQL.UI.Classes.BuildingModelsFromDatabaseOptions).buildingModelsFromDatabaseOptions'></a>

`buildingModelsFromDatabaseOptions` [BuildingModelsFromDatabaseOptions](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.BuildingModelsFromDatabaseOptions 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.BuildingModelsFromDatabaseOptions')

The source [BuildingModelsFromDatabaseOptions](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.BuildingModelsFromDatabaseOptions 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.BuildingModelsFromDatabaseOptions') instance to copy settings from\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.BuildingModelsFromDatabaseOptions.BuildingModelsFromDatabaseOptions(System.Text.Json.Nodes.JsonObject)'></a>

## BuildingModelsFromDatabaseOptions\(JsonObject\) Constructor

Initializes a new instance of the [BuildingModelsFromDatabaseOptions](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.BuildingModelsFromDatabaseOptions 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.BuildingModelsFromDatabaseOptions') class using the provided JSON object\.

```csharp
public BuildingModelsFromDatabaseOptions(System.Text.Json.Nodes.JsonObject jsonObject);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.BuildingModelsFromDatabaseOptions.BuildingModelsFromDatabaseOptions(System.Text.Json.Nodes.JsonObject).jsonObject'></a>

`jsonObject` [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')

The [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject') containing the configuration data used to populate the options\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Classes.BuildingModelsFromDatabaseOptions.CountyIds'></a>

## BuildingModelsFromDatabaseOptions\.CountyIds Property

Gets or sets the county polygon part identifiers to be processed\. [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null') means every county\.

A county code is not a key - a multi-part county has one identifier per polygon part - so name every part that is wanted.

```csharp
public System.Collections.Generic.HashSet<int>? CountyIds { get; set; }
```

#### Property Value
[System\.Collections\.Generic\.HashSet&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1 'System\.Collections\.Generic\.HashSet\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1 'System\.Collections\.Generic\.HashSet\`1')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.BuildingModelsFromDatabaseOptions.ReportDirectory'></a>

## BuildingModelsFromDatabaseOptions\.ReportDirectory Property

Gets or sets the directory the checkpoint and the list of failed counties are written into\. When null the directory the application was launched from is used\.

```csharp
public string? ReportDirectory { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.BuildingModelsFromDatabaseOptions.Resume'></a>

## BuildingModelsFromDatabaseOptions\.Resume Property

Gets or sets a value indicating whether counties named in the checkpoint of an earlier run are skipped\.

A national pass is a matter of days, so it has to survive being interrupted. Turning this off starts from the first county in scope and truncates the checkpoint, which is what a deliberate re-run of an already-completed scope needs.

```csharp
public bool Resume { get; set; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.GISPostgreSQLConverterManagerConfigurationFile'></a>

## GISPostgreSQLConverterManagerConfigurationFile Class

Represents a configuration file specifically for the GIS PostgreSQL converter manager settings, extending the base configuration file functionality\.

```csharp
public class GISPostgreSQLConverterManagerConfigurationFile : DiGi.Core.Classes.ConfigurationFile, DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.Object](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.object 'DiGi\.Core\.Classes\.Object') → [DiGi\.Core\.Classes\.SerializableObject](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.serializableobject 'DiGi\.Core\.Classes\.SerializableObject') → [DiGi\.Core\.Classes\.ConfigurationFile](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.configurationfile 'DiGi\.Core\.Classes\.ConfigurationFile') → GISPostgreSQLConverterManagerConfigurationFile

Implements [IGISPostgreSQLUIObject](DiGi.GIS.PostgreSQL.UI.Interfaces.md#DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject 'DiGi\.GIS\.PostgreSQL\.UI\.Interfaces\.IGISPostgreSQLUIObject')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.GISPostgreSQLConverterManagerConfigurationFile.GISPostgreSQLConverterManagerConfigurationFile()'></a>

## GISPostgreSQLConverterManagerConfigurationFile\(\) Constructor

Initializes a new empty instance of the [GISPostgreSQLConverterManagerConfigurationFile](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.GISPostgreSQLConverterManagerConfigurationFile 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.GISPostgreSQLConverterManagerConfigurationFile') class\.

```csharp
public GISPostgreSQLConverterManagerConfigurationFile();
```

<a name='DiGi.GIS.PostgreSQL.UI.Classes.GISPostgreSQLConverterManagerConfigurationFile.GISPostgreSQLConverterManagerConfigurationFile(DiGi.Core.Classes.ConfigurationFile)'></a>

## GISPostgreSQLConverterManagerConfigurationFile\(ConfigurationFile\) Constructor

Initializes a new instance of the [GISPostgreSQLConverterManagerConfigurationFile](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.GISPostgreSQLConverterManagerConfigurationFile 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.GISPostgreSQLConverterManagerConfigurationFile') class by copying settings from another [DiGi\.Core\.Classes\.ConfigurationFile](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.configurationfile 'DiGi\.Core\.Classes\.ConfigurationFile')\.

```csharp
public GISPostgreSQLConverterManagerConfigurationFile(DiGi.Core.Classes.ConfigurationFile? configurationFile);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.GISPostgreSQLConverterManagerConfigurationFile.GISPostgreSQLConverterManagerConfigurationFile(DiGi.Core.Classes.ConfigurationFile).configurationFile'></a>

`configurationFile` [DiGi\.Core\.Classes\.ConfigurationFile](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.configurationfile 'DiGi\.Core\.Classes\.ConfigurationFile')

The source configuration file to copy settings from\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.GISPostgreSQLConverterManagerConfigurationFile.GISPostgreSQLConverterManagerConfigurationFile(System.Text.Json.Nodes.JsonObject)'></a>

## GISPostgreSQLConverterManagerConfigurationFile\(JsonObject\) Constructor

Initializes a new instance of the [GISPostgreSQLConverterManagerConfigurationFile](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.GISPostgreSQLConverterManagerConfigurationFile 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.GISPostgreSQLConverterManagerConfigurationFile') class from a [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')\.

```csharp
public GISPostgreSQLConverterManagerConfigurationFile(System.Text.Json.Nodes.JsonObject? jsonObject);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.GISPostgreSQLConverterManagerConfigurationFile.GISPostgreSQLConverterManagerConfigurationFile(System.Text.Json.Nodes.JsonObject).jsonObject'></a>

`jsonObject` [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')

The JSON object containing the configuration data\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Classes.GISPostgreSQLConverterManagerConfigurationFile.Key'></a>

## GISPostgreSQLConverterManagerConfigurationFile\.Key Property

Gets or sets the API authorization key used for authenticating requests to protected Web API endpoints\.

```csharp
public string? Key { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.GISPostgreSQLTrayApplicationContext'></a>

## GISPostgreSQLTrayApplicationContext Class

Provides the application context for the GIS PostgreSQL tray application, managing its lifecycle and dependencies\.

```csharp
public class GISPostgreSQLTrayApplicationContext : DiGi.UI.Windows.Classes.TrayApplicationContext<DiGi.GIS.PostgreSQL.UI.Windows.MainWindow>
```

Inheritance [System\.Windows\.Forms\.ApplicationContext](https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.applicationcontext 'System\.Windows\.Forms\.ApplicationContext') → [DiGi\.UI\.Windows\.Classes\.TrayApplicationContext&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.ui.windows.classes.trayapplicationcontext-1 'DiGi\.UI\.Windows\.Classes\.TrayApplicationContext\`1')[MainWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.MainWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.MainWindow')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.ui.windows.classes.trayapplicationcontext-1 'DiGi\.UI\.Windows\.Classes\.TrayApplicationContext\`1') → GISPostgreSQLTrayApplicationContext
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.GISPostgreSQLTrayApplicationContext.GISPostgreSQLTrayApplicationContext()'></a>

## GISPostgreSQLTrayApplicationContext\(\) Constructor

Initializes a new instance of the [GISPostgreSQLTrayApplicationContext](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.GISPostgreSQLTrayApplicationContext 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.GISPostgreSQLTrayApplicationContext') class\.

```csharp
public GISPostgreSQLTrayApplicationContext();
```
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Classes.GISPostgreSQLTrayApplicationContext.GetWindow()'></a>

## GISPostgreSQLTrayApplicationContext\.GetWindow\(\) Method

Creates and returns the main window associated with this application context\.

```csharp
protected override DiGi.GIS.PostgreSQL.UI.Windows.MainWindow GetWindow();
```

#### Returns
[MainWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.MainWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.MainWindow')  
An instance of the [MainWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.MainWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.MainWindow') class\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelBuildingInformationUpdateOptions'></a>

## PostgreSQLBuildingModelBuildingInformationUpdateOptions Class

Provides configuration options for stamping the WGS 84 coordinates and the UTC offset onto the `BuildingInformation` of the stored [DiGi\.Analytical\.Building\.Classes\.BuildingModel](https://learn.microsoft.com/en-us/dotnet/api/digi.analytical.building.classes.buildingmodel 'DiGi\.Analytical\.Building\.Classes\.BuildingModel') records\.

Held by the user interface task rather than by a class library task, so it lives in this project: the run it configures is started from the user interface, through [PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow').

Carries the scope of a run: the county polygon parts, the detail level, the page size, the statement timeout, the checkpoint behaviour and the report directory. A null county set means every county and a null level means every level whose table exists.

```csharp
public class PostgreSQLBuildingModelBuildingInformationUpdateOptions : DiGi.Core.Classes.SerializableOptions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.Object](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.object 'DiGi\.Core\.Classes\.Object') → [DiGi\.Core\.Classes\.SerializableObject](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.serializableobject 'DiGi\.Core\.Classes\.SerializableObject') → [DiGi\.Core\.Classes\.SerializableOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.serializableoptions 'DiGi\.Core\.Classes\.SerializableOptions') → PostgreSQLBuildingModelBuildingInformationUpdateOptions
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelBuildingInformationUpdateOptions.PostgreSQLBuildingModelBuildingInformationUpdateOptions()'></a>

## PostgreSQLBuildingModelBuildingInformationUpdateOptions\(\) Constructor

Initializes a new instance of the [PostgreSQLBuildingModelBuildingInformationUpdateOptions](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelBuildingInformationUpdateOptions 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.PostgreSQLBuildingModelBuildingInformationUpdateOptions') class\.

```csharp
public PostgreSQLBuildingModelBuildingInformationUpdateOptions();
```

<a name='DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelBuildingInformationUpdateOptions.PostgreSQLBuildingModelBuildingInformationUpdateOptions(DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelBuildingInformationUpdateOptions)'></a>

## PostgreSQLBuildingModelBuildingInformationUpdateOptions\(PostgreSQLBuildingModelBuildingInformationUpdateOptions\) Constructor

Initializes a new instance of the [PostgreSQLBuildingModelBuildingInformationUpdateOptions](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelBuildingInformationUpdateOptions 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.PostgreSQLBuildingModelBuildingInformationUpdateOptions') class by copying the values from an existing [PostgreSQLBuildingModelBuildingInformationUpdateOptions](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelBuildingInformationUpdateOptions 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.PostgreSQLBuildingModelBuildingInformationUpdateOptions') instance\.

```csharp
public PostgreSQLBuildingModelBuildingInformationUpdateOptions(DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelBuildingInformationUpdateOptions? options);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelBuildingInformationUpdateOptions.PostgreSQLBuildingModelBuildingInformationUpdateOptions(DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelBuildingInformationUpdateOptions).options'></a>

`options` [PostgreSQLBuildingModelBuildingInformationUpdateOptions](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelBuildingInformationUpdateOptions 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.PostgreSQLBuildingModelBuildingInformationUpdateOptions')

The source [PostgreSQLBuildingModelBuildingInformationUpdateOptions](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelBuildingInformationUpdateOptions 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.PostgreSQLBuildingModelBuildingInformationUpdateOptions') instance to copy settings from\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelBuildingInformationUpdateOptions.PostgreSQLBuildingModelBuildingInformationUpdateOptions(System.Text.Json.Nodes.JsonObject)'></a>

## PostgreSQLBuildingModelBuildingInformationUpdateOptions\(JsonObject\) Constructor

Initializes a new instance of the [PostgreSQLBuildingModelBuildingInformationUpdateOptions](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelBuildingInformationUpdateOptions 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.PostgreSQLBuildingModelBuildingInformationUpdateOptions') class using the provided JSON object\.

```csharp
public PostgreSQLBuildingModelBuildingInformationUpdateOptions(System.Text.Json.Nodes.JsonObject jsonObject);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelBuildingInformationUpdateOptions.PostgreSQLBuildingModelBuildingInformationUpdateOptions(System.Text.Json.Nodes.JsonObject).jsonObject'></a>

`jsonObject` [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')

The [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject') containing the configuration data used to populate the options\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelBuildingInformationUpdateOptions.BatchSize'></a>

## PostgreSQLBuildingModelBuildingInformationUpdateOptions\.BatchSize Property

Gets or sets the number of rows read and classified per page\.

```csharp
public int BatchSize { get; set; }
```

#### Property Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelBuildingInformationUpdateOptions.BuildingModelDetailLevel'></a>

## PostgreSQLBuildingModelBuildingInformationUpdateOptions\.BuildingModelDetailLevel Property

Gets or sets the detail level of the building model table to walk\. [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null') walks every level whose table exists\.

```csharp
public System.Nullable<DiGi.Analytical.Building.Enums.BuildingModelDetailLevel> BuildingModelDetailLevel { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[DiGi\.Analytical\.Building\.Enums\.BuildingModelDetailLevel](https://learn.microsoft.com/en-us/dotnet/api/digi.analytical.building.enums.buildingmodeldetaillevel 'DiGi\.Analytical\.Building\.Enums\.BuildingModelDetailLevel')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelBuildingInformationUpdateOptions.CommandTimeout'></a>

## PostgreSQLBuildingModelBuildingInformationUpdateOptions\.CommandTimeout Property

Gets or sets the timeout in seconds applied to the statements\. A value of 0 disables the timeout\.

```csharp
public int CommandTimeout { get; set; }
```

#### Property Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelBuildingInformationUpdateOptions.CountyIds'></a>

## PostgreSQLBuildingModelBuildingInformationUpdateOptions\.CountyIds Property

Gets or sets the county polygon part identifiers to be processed\. [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null') means every county\.

A county code is not a key - a multi-part county has one identifier per polygon part - so name every part that is wanted.

```csharp
public System.Collections.Generic.HashSet<int>? CountyIds { get; set; }
```

#### Property Value
[System\.Collections\.Generic\.HashSet&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1 'System\.Collections\.Generic\.HashSet\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1 'System\.Collections\.Generic\.HashSet\`1')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelBuildingInformationUpdateOptions.DryRun'></a>

## PostgreSQLBuildingModelBuildingInformationUpdateOptions\.DryRun Property

Gets or sets a value indicating whether the run only reports what it would stamp\. Defaults to [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool'); nothing is written until it is turned off\.

```csharp
public bool DryRun { get; set; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelBuildingInformationUpdateOptions.ReportDirectory'></a>

## PostgreSQLBuildingModelBuildingInformationUpdateOptions\.ReportDirectory Property

Gets or sets the directory the checkpoint and the reports are written into\. When null the directory the application was launched from is used\.

```csharp
public string? ReportDirectory { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelBuildingInformationUpdateOptions.Resume'></a>

## PostgreSQLBuildingModelBuildingInformationUpdateOptions\.Resume Property

Gets or sets a value indicating whether the parts named in the checkpoint of an earlier run are skipped\. Defaults to [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

The national pass has to survive being interrupted mid-country, so it resumes rather than restarting from the first part. Turning this off starts from the first part in scope and truncates the checkpoint, which is what a deliberate re-run of an already-completed scope needs.

```csharp
public bool Resume { get; set; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelCleanupTask'></a>

## PostgreSQLBuildingModelCleanupTask Class

Removes the [DiGi\.Analytical\.Building\.Classes\.BuildingModel](https://learn.microsoft.com/en-us/dotnet/api/digi.analytical.building.classes.buildingmodel 'DiGi\.Analytical\.Building\.Classes\.BuildingModel') rows whose building no longer exists under the county part holding them\.

An orphan is a model held under a part whose `building_2d` no longer holds the building it describes, which is what a county part repair run can leave behind when it re-files a building under the part its footprint lies in.

<b>Reports by default and writes nothing.</b>[DryRun](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelCleanupTask.DryRun 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.PostgreSQLBuildingModelCleanupTask\.DryRun') has to be turned off deliberately, and the counts it reports first are what the delete should be reviewed against - the rows removed here have no undo.

The report is written as files into [ReportDirectory](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelCleanupTask.ReportDirectory 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.PostgreSQLBuildingModelCleanupTask\.ReportDirectory') as well as to the log: `BuildingModels_Cleanup.csv` naming every orphaned reference, and `BuildingModels_Cleanup_Summary.txt` carrying the totals. The files are what the decision to delete should rest on - a log is shared with whatever else the application is doing and rolls by day.

```csharp
public class PostgreSQLBuildingModelCleanupTask : DiGi.Core.Classes.ReportableBackgroundTask<long>, DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → PostgreSQLBuildingModelCleanupTask

Implements [IGISPostgreSQLUIObject](DiGi.GIS.PostgreSQL.UI.Interfaces.md#DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject 'DiGi\.GIS\.PostgreSQL\.UI\.Interfaces\.IGISPostgreSQLUIObject')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelCleanupTask.PostgreSQLBuildingModelCleanupTask(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager)'></a>

## PostgreSQLBuildingModelCleanupTask\(GISPostgreSQLConverterManager\) Constructor

Initializes a new instance of the [PostgreSQLBuildingModelCleanupTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelCleanupTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.PostgreSQLBuildingModelCleanupTask') class\.

```csharp
public PostgreSQLBuildingModelCleanupTask(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager gISPostgreSQLConverterManager);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelCleanupTask.PostgreSQLBuildingModelCleanupTask(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager).gISPostgreSQLConverterManager'></a>

`gISPostgreSQLConverterManager` [DiGi\.GIS\.PostgreSQL\.Classes\.GISPostgreSQLConverterManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.gispostgresqlconvertermanager 'DiGi\.GIS\.PostgreSQL\.Classes\.GISPostgreSQLConverterManager')

The manager holding the PostgreSQL converters\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelCleanupTask.CountyIds'></a>

## PostgreSQLBuildingModelCleanupTask\.CountyIds Property

Gets or sets the identifiers of the county rows to clean\. When null every county row is examined\.

These are polygon parts, not counties - a multi-part county holds one row per part and each is cleaned on its own.

```csharp
public System.Collections.Generic.IEnumerable<int>? CountyIds { get; set; }
```

#### Property Value
[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelCleanupTask.DryRun'></a>

## PostgreSQLBuildingModelCleanupTask\.DryRun Property

Gets or sets a value indicating whether the task only reports what it would do\. Defaults to [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool'); nothing is written until it is turned off\.

```csharp
public bool DryRun { get; set; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelCleanupTask.ReportDirectory'></a>

## PostgreSQLBuildingModelCleanupTask\.ReportDirectory Property

Gets or sets the directory the two report files are written into\. When null the directory the application was launched from is used\.

Deliberately not a folder dialog: this runs on a thread pool thread, where a WPF common dialog needs an STA apartment and throws instead of opening.

```csharp
public string? ReportDirectory { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelCleanupTask.VoivodeshipCodes'></a>

## PostgreSQLBuildingModelCleanupTask\.VoivodeshipCodes Property

Gets or sets the two\-digit voivodeship codes to be cleaned\. A county row is in scope when its code starts with one of them\. When null every voivodeship is cleaned\. Combined with [CountyIds](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelCleanupTask.CountyIds 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.PostgreSQLBuildingModelCleanupTask\.CountyIds') both filters have to admit the row\.

This is what makes the national regeneration affordable: a voivodeship is regenerated and then cleaned before the next one starts, so the storage tablespace only ever carries a second copy of one voivodeship rather than of the whole country.

```csharp
public System.Collections.Generic.IEnumerable<string>? VoivodeshipCodes { get; set; }
```

#### Property Value
[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelCleanupTask.ReferencesOrphanedAsync(DiGi.GIS.PostgreSQL.Classes.BuildingModelPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,int,System.Threading.CancellationToken)'></a>

## PostgreSQLBuildingModelCleanupTask\.ReferencesOrphanedAsync\(BuildingModelPostgreSQLConverter, Building2DPostgreSQLConverter, int, CancellationToken\) Method

Returns the references a county row holds a model for but no longer holds a building for\.

```csharp
private static System.Threading.Tasks.Task<System.Collections.Generic.List<string>> ReferencesOrphanedAsync(DiGi.GIS.PostgreSQL.Classes.BuildingModelPostgreSQLConverter buildingModelPostgreSQLConverter, DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter building2DPostgreSQLConverter, int countyId, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelCleanupTask.ReferencesOrphanedAsync(DiGi.GIS.PostgreSQL.Classes.BuildingModelPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,int,System.Threading.CancellationToken).buildingModelPostgreSQLConverter'></a>

`buildingModelPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.BuildingModelPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.buildingmodelpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.BuildingModelPostgreSQLConverter')

The converter reading the model table\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelCleanupTask.ReferencesOrphanedAsync(DiGi.GIS.PostgreSQL.Classes.BuildingModelPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,int,System.Threading.CancellationToken).building2DPostgreSQLConverter'></a>

`building2DPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.Building2DPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.building2dpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.Building2DPostgreSQLConverter')

The converter reading the building table, which lives in a different database\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelCleanupTask.ReferencesOrphanedAsync(DiGi.GIS.PostgreSQL.Classes.BuildingModelPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,int,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The identifier of the county row to compare\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelCleanupTask.ReferencesOrphanedAsync(DiGi.GIS.PostgreSQL.Classes.BuildingModelPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. The task result contains the orphaned references, empty when either side could not be read\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIAddressPointsFromFilePostTask'></a>

## UIAddressPointsFromFilePostTask Class

Imports PRG address points from GML files selected through the user interface and posts them to the server\.

The PRG is published as one GML file per voivodeship (0.7 - 1.6 GB each), not per county, so the county of every point is taken from its own TERYT commune code (see [CountyCode\(string\)](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Query.CountyCode(string) 'DiGi\.GIS\.PostgreSQL\.UI\.Query\.CountyCode\(string\)')). Each code is resolved to every polygon part of the county through [DiGi\.GIS\.WebAPI\.Classes\.AdministrativeAreal2DController\.GetIdsByCodeAsync\(System\.String,System\.Nullable\{DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType\},System\.Threading\.CancellationToken\)](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.administrativeareal2dcontroller.getidsbycodeasync#digi-gis-webapi-classes-administrativeareal2dcontroller-getidsbycodeasync(system-string-system-nullable{digi-gis-postgresql-enums-administrativearealtype}-system-threading-cancellationtoken) 'DiGi\.GIS\.WebAPI\.Classes\.AdministrativeAreal2DController\.GetIdsByCodeAsync\(System\.String,System\.Nullable\{DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType\},System\.Threading\.CancellationToken\)') and the points are posted with those identifiers; the server files each point under the part it lies in.

Files are loaded one at a time and released before the next, because a loaded file holds a clone of its street and locality on every point (about 3 GB for a 714 MB file). A county that fails - its parts cannot be resolved, or its upload is refused or times out - is named, recorded in the report and skipped, and the run continues; so does a file that fails to load. Only a user cancel stops the run.

A report is written to [ReportDirectory](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIAddressPointsFromFilePostTask.ReportDirectory 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIAddressPointsFromFilePostTask\.ReportDirectory') and names the server the run posted to, so it cannot be mistaken for a description of another database. It is rewritten after every file and every county and once more when the run ends however it ends, so a stopped run still shows which counties were posted.

```csharp
public class UIAddressPointsFromFilePostTask : DiGi.GIS.WebAPI.Classes.AddressPointsPostTask, DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.serializableobjectsposttask-1 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\`1')[DiGi\.GIS\.Classes\.AddressPoint](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.addresspoint 'DiGi\.GIS\.Classes\.AddressPoint')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.serializableobjectsposttask-1 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.AddressPointsPostTask](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.addresspointsposttask 'DiGi\.GIS\.WebAPI\.Classes\.AddressPointsPostTask') → UIAddressPointsFromFilePostTask

Implements [IGISPostgreSQLUIObject](DiGi.GIS.PostgreSQL.UI.Interfaces.md#DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject 'DiGi\.GIS\.PostgreSQL\.UI\.Interfaces\.IGISPostgreSQLUIObject')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIAddressPointsFromFilePostTask.UIAddressPointsFromFilePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager)'></a>

## UIAddressPointsFromFilePostTask\(GISWebAPIManager\) Constructor

Initializes a new instance of the [UIAddressPointsFromFilePostTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIAddressPointsFromFilePostTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIAddressPointsFromFilePostTask') class\.

```csharp
public UIAddressPointsFromFilePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager GISWebAPIManager);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIAddressPointsFromFilePostTask.UIAddressPointsFromFilePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager).GISWebAPIManager'></a>

`GISWebAPIManager` [DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.giswebapimanager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager used to communicate with the GIS Web API\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIAddressPointsFromFilePostTask.ReportDirectory'></a>

## UIAddressPointsFromFilePostTask\.ReportDirectory Property

Gets or sets the directory the report is written into\. When null the directory the application was launched from is used\.

Deliberately not a folder dialog: this runs on a thread pool thread, where a WPF common dialog needs an STA apartment.

```csharp
public string? ReportDirectory { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIAddressPointsFromFilePostTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken)'></a>

## UIAddressPointsFromFilePostTask\.ExecuteAsync\(IProgress\<long\>, CancellationToken\) Method

Concrete implementation of the background work\.

```csharp
protected override System.Threading.Tasks.Task<bool> ExecuteAsync(System.IProgress<long> progress, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIAddressPointsFromFilePostTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).progress'></a>

`progress` [System\.IProgress&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')

The provider for reporting progress of the operation\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIAddressPointsFromFilePostTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The token to monitor for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. The task result is true if every county was posted; otherwise, false\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIAdministrativeAreal2DFromFilePostTask'></a>

## UIAdministrativeAreal2DFromFilePostTask Class

Provides functionality to post administrative areal 2D objects to a PostgreSQL database by selecting GIS model files through the user interface\.

```csharp
public class UIAdministrativeAreal2DFromFilePostTask : DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DsPostTask, DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.serializableobjectsposttask-1 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\`1')[DiGi\.GIS\.Classes\.AdministrativeAreal2D](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.administrativeareal2d 'DiGi\.GIS\.Classes\.AdministrativeAreal2D')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.serializableobjectsposttask-1 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.AdministrativeAreal2DsPostTask](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.administrativeareal2dsposttask 'DiGi\.GIS\.WebAPI\.Classes\.AdministrativeAreal2DsPostTask') → UIAdministrativeAreal2DFromFilePostTask

Implements [IGISPostgreSQLUIObject](DiGi.GIS.PostgreSQL.UI.Interfaces.md#DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject 'DiGi\.GIS\.PostgreSQL\.UI\.Interfaces\.IGISPostgreSQLUIObject')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIAdministrativeAreal2DFromFilePostTask.UIAdministrativeAreal2DFromFilePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager)'></a>

## UIAdministrativeAreal2DFromFilePostTask\(GISWebAPIManager\) Constructor

Initializes a new instance of the [UIAdministrativeAreal2DFromFilePostTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIAdministrativeAreal2DFromFilePostTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIAdministrativeAreal2DFromFilePostTask') class\.

```csharp
public UIAdministrativeAreal2DFromFilePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager GISWebAPIManager);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIAdministrativeAreal2DFromFilePostTask.UIAdministrativeAreal2DFromFilePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager).GISWebAPIManager'></a>

`GISWebAPIManager` [DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.giswebapimanager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager used to communicate with the GIS PostgreSQL Web API\.
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIAdministrativeAreal2DFromFilePostTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken)'></a>

## UIAdministrativeAreal2DFromFilePostTask\.ExecuteAsync\(IProgress\<long\>, CancellationToken\) Method

Concrete implementation of the background work\.

```csharp
protected override System.Threading.Tasks.Task<bool> ExecuteAsync(System.IProgress<long> progress, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIAdministrativeAreal2DFromFilePostTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).progress'></a>

`progress` [System\.IProgress&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')

The provider for a value and elapsed time to report progress\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIAdministrativeAreal2DFromFilePostTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. The task result is true if the operation succeeded; otherwise, false\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuilding2DsFromFilePostTask'></a>

## UIBuilding2DsFromFilePostTask Class

Represents a task for posting Building 2D objects to a PostgreSQL database from GIS model files selected through the user interface\.

```csharp
public class UIBuilding2DsFromFilePostTask : DiGi.GIS.WebAPI.Classes.Building2DsPostTask, DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.serializableobjectsposttask-1 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\`1')[DiGi\.GIS\.Classes\.Building2D](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.building2d 'DiGi\.GIS\.Classes\.Building2D')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.serializableobjectsposttask-1 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.Building2DsPostTask](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.building2dsposttask 'DiGi\.GIS\.WebAPI\.Classes\.Building2DsPostTask') → UIBuilding2DsFromFilePostTask

Implements [IGISPostgreSQLUIObject](DiGi.GIS.PostgreSQL.UI.Interfaces.md#DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject 'DiGi\.GIS\.PostgreSQL\.UI\.Interfaces\.IGISPostgreSQLUIObject')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuilding2DsFromFilePostTask.UIBuilding2DsFromFilePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager)'></a>

## UIBuilding2DsFromFilePostTask\(GISWebAPIManager\) Constructor

Initializes a new instance of the [UIBuilding2DsFromFilePostTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIBuilding2DsFromFilePostTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIBuilding2DsFromFilePostTask') class\.

```csharp
public UIBuilding2DsFromFilePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager GISWebAPIManager);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuilding2DsFromFilePostTask.UIBuilding2DsFromFilePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager).GISWebAPIManager'></a>

`GISWebAPIManager` [DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.giswebapimanager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager used to communicate with the GIS PostgreSQL Web API\.
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuilding2DsFromFilePostTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken)'></a>

## UIBuilding2DsFromFilePostTask\.ExecuteAsync\(IProgress\<long\>, CancellationToken\) Method

Concrete implementation of the background work\.

```csharp
protected override System.Threading.Tasks.Task<bool> ExecuteAsync(System.IProgress<long> progress, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuilding2DsFromFilePostTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).progress'></a>

`progress` [System\.IProgress&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')

The provider for reporting progress of the operation\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuilding2DsFromFilePostTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The token to monitor for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. The task result is true if the operation succeeded; otherwise, false\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsCourtyardAuditTask'></a>

## UIBuildingModelsCourtyardAuditTask Class

A read\-only task that locates the stored building models whose courtyard the storey split filled with a solid face\.

For every county in scope it walks every 2D building reference and classifies it against the defect fixed by DiGi.Geometry#7: the source CityGML [DiGi\.CityGML\.Classes\.Building](https://learn.microsoft.com/en-us/dotnet/api/digi.citygml.classes.building 'DiGi\.CityGML\.Classes\.Building') carries a `GroundSurface` with an interior ring (a courtyard) while the stored [DiGi\.GIS\.PostgreSQL\.Classes\.BuildingModel](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.buildingmodel 'DiGi\.GIS\.PostgreSQL\.Classes\.BuildingModel') reports no internal edge on its `Footprints`. That is the model that renders a floor over the courtyard and drives the terrain cut DiGi.GIS.WebAPI.UI#45 reported.

Nothing is uploaded and nothing is repaired - the task exists to name the affected set and tally it per county, which is the scope the regeneration (DiGi.GIS.PostgreSQL.UI#13) must run against. A county's tally is recorded only once every reference of it has been audited, so a county interrupted part way is not checkpointed and is redone in full on the next run rather than left half-counted.

Two files are written into [ReportDirectory](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsCourtyardAuditTask.ReportDirectory 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIBuildingModelsCourtyardAuditTask\.ReportDirectory'): one row per reference in `BuildingModels_CourtyardAudit.csv`, and per-county plus national totals in `BuildingModels_CourtyardAudit_Summary.txt`, the latter carrying an `AffectedCountyIds:` line that is the ready-to-paste [CountyIds](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsCourtyardAuditTask.CountyIds 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIBuildingModelsCourtyardAuditTask\.CountyIds') scope for the regeneration.

```csharp
public class UIBuildingModelsCourtyardAuditTask : DiGi.Core.Classes.ReportableBackgroundTask<long>, DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → UIBuildingModelsCourtyardAuditTask

Implements [IGISPostgreSQLUIObject](DiGi.GIS.PostgreSQL.UI.Interfaces.md#DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject 'DiGi\.GIS\.PostgreSQL\.UI\.Interfaces\.IGISPostgreSQLUIObject')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsCourtyardAuditTask.UIBuildingModelsCourtyardAuditTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager)'></a>

## UIBuildingModelsCourtyardAuditTask\(GISWebAPIManager\) Constructor

Initializes a new instance of the [UIBuildingModelsCourtyardAuditTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsCourtyardAuditTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIBuildingModelsCourtyardAuditTask') class\.

```csharp
public UIBuildingModelsCourtyardAuditTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager GISWebAPIManager);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsCourtyardAuditTask.UIBuildingModelsCourtyardAuditTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager).GISWebAPIManager'></a>

`GISWebAPIManager` [DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.giswebapimanager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIBuildingModelsCourtyardAuditTask\.GISWebAPIManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.ui.classes.uibuildingmodelscourtyardaudittask.giswebapimanager 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIBuildingModelsCourtyardAuditTask\.GISWebAPIManager') instance used to communicate with the server\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsCourtyardAuditTask.BatchSize'></a>

## UIBuildingModelsCourtyardAuditTask\.BatchSize Property

Gets or sets the number of source\-bearing references asked for in a single stored\-model request\. Those references travel in the query string, so a batch far above this risks the URL length limit of the server\.

```csharp
public int BatchSize { get; set; }
```

#### Property Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsCourtyardAuditTask.CountyIds'></a>

## UIBuildingModelsCourtyardAuditTask\.CountyIds Property

Gets or sets the identifiers of the counties to be audited\. When null every county held on the server is audited\.

```csharp
public System.Collections.Generic.IEnumerable<int>? CountyIds { get; set; }
```

#### Property Value
[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsCourtyardAuditTask.MaxConcurrentRequests'></a>

## UIBuildingModelsCourtyardAuditTask\.MaxConcurrentRequests Property

Gets or sets how many source\-building requests are allowed to be in flight at once\. The source pull is one request per building, so an unbounded national pass would take weeks when they are issued one after another; the requests are independent, so they go out in groups of this size\.

```csharp
public int MaxConcurrentRequests { get; set; }
```

#### Property Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsCourtyardAuditTask.MaxReferencesPerCounty'></a>

## UIBuildingModelsCourtyardAuditTask\.MaxReferencesPerCounty Property

Gets or sets the maximum number of references audited per county\. A value of zero or less audits every reference of the county \- the estate\-wide behaviour\. Capping a run keeps a sanity\-check bounded without changing that default\.

```csharp
public int MaxReferencesPerCounty { get; set; }
```

#### Property Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsCourtyardAuditTask.ReportDirectory'></a>

## UIBuildingModelsCourtyardAuditTask\.ReportDirectory Property

Gets or sets the directory the two report files are written into\. When null the user is asked for one\.

```csharp
public string? ReportDirectory { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsCourtyardAuditTask.Resume'></a>

## UIBuildingModelsCourtyardAuditTask\.Resume Property

Gets or sets a value indicating whether counties named in the checkpoint of an earlier run are skipped\. Defaults to [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\. A county is checkpointed only once every reference of it has been audited, so turning this off re\-audits every county in scope from the first\.

```csharp
public bool Resume { get; set; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsCourtyardAuditTask.Tolerance'></a>

## UIBuildingModelsCourtyardAuditTask\.Tolerance Property

Gets or sets the distance tolerance the `Footprints` of a stored model are required to hold their internal edges at\.

```csharp
public double Tolerance { get; set; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsCourtyardAuditTask.VoivodeshipCodes'></a>

## UIBuildingModelsCourtyardAuditTask\.VoivodeshipCodes Property

Gets or sets the two\-digit voivodeship codes to be audited\. A county is in scope when its code starts with one of them\. When null every voivodeship is audited\. Combined with [CountyIds](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsCourtyardAuditTask.CountyIds 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIBuildingModelsCourtyardAuditTask\.CountyIds') both filters have to admit the county\.

```csharp
public System.Collections.Generic.IEnumerable<string>? VoivodeshipCodes { get; set; }
```

#### Property Value
[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsCourtyardAuditTask.IsAffected(DiGi.CityGML.Classes.Building,DiGi.Analytical.Building.Classes.BuildingModel,double)'></a>

## UIBuildingModelsCourtyardAuditTask\.IsAffected\(Building, BuildingModel, double\) Method

Combines the two halves: the model is affected when the source carries a courtyard, a stored model exists, and that stored model's outline has none\. A source courtyard with no stored model is a missing model, not an affected one \- the regional view falls back to the 2D footprint, which keeps the courtyard\.

```csharp
public static bool IsAffected(DiGi.CityGML.Classes.Building? source, DiGi.Analytical.Building.Classes.BuildingModel? stored, double tolerance);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsCourtyardAuditTask.IsAffected(DiGi.CityGML.Classes.Building,DiGi.Analytical.Building.Classes.BuildingModel,double).source'></a>

`source` [DiGi\.CityGML\.Classes\.Building](https://learn.microsoft.com/en-us/dotnet/api/digi.citygml.classes.building 'DiGi\.CityGML\.Classes\.Building')

The stored CityGML building, or null when the server holds none\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsCourtyardAuditTask.IsAffected(DiGi.CityGML.Classes.Building,DiGi.Analytical.Building.Classes.BuildingModel,double).stored'></a>

`stored` [DiGi\.Analytical\.Building\.Classes\.BuildingModel](https://learn.microsoft.com/en-us/dotnet/api/digi.analytical.building.classes.buildingmodel 'DiGi\.Analytical\.Building\.Classes\.BuildingModel')

The stored building model, or null when the server holds none\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsCourtyardAuditTask.IsAffected(DiGi.CityGML.Classes.Building,DiGi.Analytical.Building.Classes.BuildingModel,double).tolerance'></a>

`tolerance` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The distance tolerance the footprints are required to hold their internal edges at\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') when the source has a courtyard the stored model lost\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsCourtyardAuditTask.SourceHasRing(DiGi.CityGML.Classes.Building)'></a>

## UIBuildingModelsCourtyardAuditTask\.SourceHasRing\(Building\) Method

Decides whether the source CityGML building carries a courtyard \- one of its [DiGi\.CityGML\.Classes\.GroundSurface](https://learn.microsoft.com/en-us/dotnet/api/digi.citygml.classes.groundsurface 'DiGi\.CityGML\.Classes\.GroundSurface') surfaces holds an interior ring\. This is the source half of the affected test; a wall or roof surface never counts, so only ground surfaces are examined\.

```csharp
public static bool SourceHasRing(DiGi.CityGML.Classes.Building? source);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsCourtyardAuditTask.SourceHasRing(DiGi.CityGML.Classes.Building).source'></a>

`source` [DiGi\.CityGML\.Classes\.Building](https://learn.microsoft.com/en-us/dotnet/api/digi.citygml.classes.building 'DiGi\.CityGML\.Classes\.Building')

The stored CityGML building, or null when the server holds none\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') when a ground surface of the building has at least one interior ring\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsCourtyardAuditTask.StoredHasHole(DiGi.Analytical.Building.Classes.BuildingModel,double)'></a>

## UIBuildingModelsCourtyardAuditTask\.StoredHasHole\(BuildingModel, double\) Method

Decides whether the stored model's outline carries a courtyard \- one of its `Footprints` faces holds an internal edge\. This is the stored half of the affected test: a model the storey split filled solid has none\.

```csharp
public static bool StoredHasHole(DiGi.Analytical.Building.Classes.BuildingModel? stored, double tolerance);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsCourtyardAuditTask.StoredHasHole(DiGi.Analytical.Building.Classes.BuildingModel,double).stored'></a>

`stored` [DiGi\.Analytical\.Building\.Classes\.BuildingModel](https://learn.microsoft.com/en-us/dotnet/api/digi.analytical.building.classes.buildingmodel 'DiGi\.Analytical\.Building\.Classes\.BuildingModel')

The stored building model, or null when the server holds none\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsCourtyardAuditTask.StoredHasHole(DiGi.Analytical.Building.Classes.BuildingModel,double).tolerance'></a>

`tolerance` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The distance tolerance the footprints are required to hold their internal edges at\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') when at least one footprint face carries an internal edge\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsFromDatabasePostTask'></a>

## UIBuildingModelsFromDatabasePostTask Class

A post task that generates [DiGi\.Analytical\.Building\.Classes\.BuildingModel](https://learn.microsoft.com/en-us/dotnet/api/digi.analytical.building.classes.buildingmodel 'DiGi\.Analytical\.Building\.Classes\.BuildingModel') instances entirely from data already held on the server \- the CityGML [DiGi\.CityGML\.Classes\.Building](https://learn.microsoft.com/en-us/dotnet/api/digi.citygml.classes.building 'DiGi\.CityGML\.Classes\.Building') records stored in the database are used instead of CityGML archives read from a local directory\.

For every county in scope the task pages through the county's [DiGi\.GIS\.PostgreSQL\.Classes\.Building2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.building2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.Building2DReference') records and, per page, downloads the [DiGi\.GIS\.Classes\.Building2D](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.building2d 'DiGi\.GIS\.Classes\.Building2D') data, so a whole county's buildings are never held in memory at once.

Each [DiGi\.GIS\.Classes\.Building2D](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.building2d 'DiGi\.GIS\.Classes\.Building2D') is then processed individually: its single best ranked CityGML [DiGi\.CityGML\.Classes\.Building](https://learn.microsoft.com/en-us/dotnet/api/digi.citygml.classes.building 'DiGi\.CityGML\.Classes\.Building') is pulled by reference through [DiGi\.GIS\.WebAPI\.Classes\.BuildingController\.GetItemByReferenceAsync\(System\.String,System\.Nullable\{System\.Int32\},System\.Nullable\{System\.Double\},System\.Nullable\{System\.Double\},System\.Nullable\{System\.Double\},System\.Nullable\{System\.Double\},System\.Threading\.CancellationToken\)](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.buildingcontroller.getitembyreferenceasync#digi-gis-webapi-classes-buildingcontroller-getitembyreferenceasync(system-string-system-nullable{system-int32}-system-nullable{system-double}-system-nullable{system-double}-system-nullable{system-double}-system-nullable{system-double}-system-threading-cancellationtoken) 'DiGi\.GIS\.WebAPI\.Classes\.BuildingController\.GetItemByReferenceAsync\(System\.String,System\.Nullable\{System\.Int32\},System\.Nullable\{System\.Double\},System\.Nullable\{System\.Double\},System\.Nullable\{System\.Double\},System\.Nullable\{System\.Double\},System\.Threading\.CancellationToken\)') and refined into storeys by the matching `Analytical.Create.BuildingModel` overload. A 2D building whose reference has no stored CityGML building, no reference at all, or whose pull fails is modelled from an extruded footprint instead.

<b>"County" here means one polygon part.</b> The county listing returns 406 references for 380 codes, because a county whose territory is disconnected is stored as one row per part. The task reads and uploads by `Id`, so each part is filled from its own `building_2d` rows; uploading by `Code` instead would let the server file every part's models under a single one, which is what left three counties reading back empty. The county code is still written onto each model as descriptive metadata.

Because `building_2d` holds the same building under every part it was imported under, a building shared by two parts is modelled once per part. That is inherent to keying by part and is not a duplicate to suppress here - it mirrors the underlying table.

<b>A national pass takes days, so a county is the unit of both failure and progress.</b> A county whose pages cannot be read or uploaded is named, recorded in `BuildingModels_Regeneration_Failed.txt` and skipped, rather than ending the run and discarding every county after it. A county that completes in full is appended to `BuildingModels_Regeneration_Checkpoint.txt`, which [Resume](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsFromDatabasePostTask.Resume 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIBuildingModelsFromDatabasePostTask\.Resume') reads on the next run - so an interrupted pass continues where it stopped, and a county interrupted part way is simply redone.

The scope - the county parts, the checkpoint behaviour and the report directory - is asked for each time the task starts, through [BuildingModelsFromDatabaseOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.BuildingModelsFromDatabaseOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.BuildingModelsFromDatabaseOptionsWindow'); a cancelled dialog ends the run with nothing written. The pacing and the voivodeship filter stay as set where the task is registered.

```csharp
public class UIBuildingModelsFromDatabasePostTask : DiGi.GIS.WebAPI.Classes.BuildingModelsPostTask, DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.serializableobjectsposttask-1 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\`1')[DiGi\.Analytical\.Building\.Classes\.BuildingModel](https://learn.microsoft.com/en-us/dotnet/api/digi.analytical.building.classes.buildingmodel 'DiGi\.Analytical\.Building\.Classes\.BuildingModel')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.serializableobjectsposttask-1 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.BuildingModelsPostTask](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.buildingmodelsposttask 'DiGi\.GIS\.WebAPI\.Classes\.BuildingModelsPostTask') → UIBuildingModelsFromDatabasePostTask

Implements [IGISPostgreSQLUIObject](DiGi.GIS.PostgreSQL.UI.Interfaces.md#DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject 'DiGi\.GIS\.PostgreSQL\.UI\.Interfaces\.IGISPostgreSQLUIObject')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsFromDatabasePostTask.UIBuildingModelsFromDatabasePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager)'></a>

## UIBuildingModelsFromDatabasePostTask\(GISWebAPIManager\) Constructor

Initializes a new instance of the [UIBuildingModelsFromDatabasePostTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsFromDatabasePostTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIBuildingModelsFromDatabasePostTask') class\.

```csharp
public UIBuildingModelsFromDatabasePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager GISWebAPIManager);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsFromDatabasePostTask.UIBuildingModelsFromDatabasePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager).GISWebAPIManager'></a>

`GISWebAPIManager` [DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.giswebapimanager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.giswebapimanager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance used to communicate with the server\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsFromDatabasePostTask.MaxConcurrentRequests'></a>

## UIBuildingModelsFromDatabasePostTask\.MaxConcurrentRequests Property

Gets or sets how many CityGML and terrain requests are allowed to be in flight at once\.

One CityGML request per building at roughly 60 ms each makes a national pass a matter of weeks when they are issued one after another. The requests are independent, so they are issued in groups of this size. Lower it if the server or the terrain service starts refusing.

```csharp
public int MaxConcurrentRequests { get; set; }
```

#### Property Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsFromDatabasePostTask.PageSize'></a>

## UIBuildingModelsFromDatabasePostTask\.PageSize Property

Gets or sets the number of [DiGi\.GIS\.PostgreSQL\.Classes\.Building2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.building2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.Building2DReference') items requested per page while downloading a county's buildings\.

```csharp
public int PageSize { get; set; }
```

#### Property Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsFromDatabasePostTask.ReportDirectory'></a>

## UIBuildingModelsFromDatabasePostTask\.ReportDirectory Property

Gets or sets the directory the checkpoint and the list of failed counties are written into\. When null the directory the application was launched from is used\.

Deliberately not a folder dialog: this runs on a thread pool thread, where a WPF common dialog needs an STA apartment and throws instead of opening.

```csharp
public string? ReportDirectory { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsFromDatabasePostTask.Resume'></a>

## UIBuildingModelsFromDatabasePostTask\.Resume Property

Gets or sets a value indicating whether counties named in the checkpoint of an earlier run are skipped\. Defaults to [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

A national pass is a matter of days, so it has to survive being interrupted. Turning this off starts from the first county in scope and truncates the checkpoint, which is what a deliberate re-run of an already-completed scope needs.

```csharp
public bool Resume { get; set; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsFromDatabasePostTask.VoivodeshipCodes'></a>

## UIBuildingModelsFromDatabasePostTask\.VoivodeshipCodes Property

Gets or sets the two\-digit voivodeship codes to be processed\. A county is in scope when its code starts with one of them\. When null every voivodeship is processed\. Combined with [DiGi\.GIS\.WebAPI\.Classes\.BuildingModelsPostTask\.CountyIds](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.buildingmodelsposttask.countyids 'DiGi\.GIS\.WebAPI\.Classes\.BuildingModelsPostTask\.CountyIds') both filters have to admit the county\.

Regenerating one voivodeship at a time is what keeps the storage tablespace within reach: a county's models are written beside the ones they supersede until [PostgreSQLBuildingModelCleanupTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelCleanupTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.PostgreSQLBuildingModelCleanupTask') removes them, so a national pass in one go would need room for a second copy of the whole table.

```csharp
public System.Collections.Generic.IEnumerable<string>? VoivodeshipCodes { get; set; }
```

#### Property Value
[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsFromDirectoryPostTask'></a>

## UIBuildingModelsFromDirectoryPostTask Class

A UI\-driven post task that prompts the user to select a directory containing CityGML archives, enumerates the counties held on the server, and for every county whose code matches a CityGML archive in that directory downloads the county's [DiGi\.GIS\.Classes\.Building2D](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.building2d 'DiGi\.GIS\.Classes\.Building2D') data page by page, generates [DiGi\.Analytical\.Building\.Classes\.BuildingModel](https://learn.microsoft.com/en-us/dotnet/api/digi.analytical.building.classes.buildingmodel 'DiGi\.Analytical\.Building\.Classes\.BuildingModel') instances and uploads them\.

Both the CityGML archives and the Building2D data are processed one county - and within a county one page - at a time, so neither the whole country's CityGML nor a whole county's buildings are ever held in memory at once.

```csharp
public class UIBuildingModelsFromDirectoryPostTask : DiGi.GIS.WebAPI.Classes.BuildingModelsPostTask, DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.serializableobjectsposttask-1 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\`1')[DiGi\.Analytical\.Building\.Classes\.BuildingModel](https://learn.microsoft.com/en-us/dotnet/api/digi.analytical.building.classes.buildingmodel 'DiGi\.Analytical\.Building\.Classes\.BuildingModel')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.serializableobjectsposttask-1 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.BuildingModelsPostTask](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.buildingmodelsposttask 'DiGi\.GIS\.WebAPI\.Classes\.BuildingModelsPostTask') → UIBuildingModelsFromDirectoryPostTask

Implements [IGISPostgreSQLUIObject](DiGi.GIS.PostgreSQL.UI.Interfaces.md#DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject 'DiGi\.GIS\.PostgreSQL\.UI\.Interfaces\.IGISPostgreSQLUIObject')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsFromDirectoryPostTask.UIBuildingModelsFromDirectoryPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager)'></a>

## UIBuildingModelsFromDirectoryPostTask\(GISWebAPIManager\) Constructor

Initializes a new instance of the [UIBuildingModelsFromDirectoryPostTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsFromDirectoryPostTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIBuildingModelsFromDirectoryPostTask') class\.

```csharp
public UIBuildingModelsFromDirectoryPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager GISWebAPIManager);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsFromDirectoryPostTask.UIBuildingModelsFromDirectoryPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager).GISWebAPIManager'></a>

`GISWebAPIManager` [DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.giswebapimanager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.giswebapimanager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance used to communicate with the server\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsFromDirectoryPostTask.PageSize'></a>

## UIBuildingModelsFromDirectoryPostTask\.PageSize Property

Gets or sets the number of [DiGi\.GIS\.PostgreSQL\.Classes\.Building2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.building2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.Building2DReference') items requested per page while downloading a county's buildings\.

```csharp
public int PageSize { get; set; }
```

#### Property Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsVerificationTask'></a>

## UIBuildingModelsVerificationTask Class

A UI\-driven task that reads the [DiGi\.Analytical\.Building\.Classes\.BuildingModel](https://learn.microsoft.com/en-us/dotnet/api/digi.analytical.building.classes.buildingmodel 'DiGi\.Analytical\.Building\.Classes\.BuildingModel') records already stored on the server and reports how complete and how sound they are\.

Read-only. Nothing is uploaded, nothing is repaired - the task exists to say what the stored data looks like, which is the state the upload path itself never reported: a model whose spaces are not enclosed is accepted by the server today and stored without a word.

For every county in scope a sample of [SampleSize](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsVerificationTask.SampleSize 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIBuildingModelsVerificationTask\.SampleSize') 2D building references is drawn with [RandomSeed](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsVerificationTask.RandomSeed 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIBuildingModelsVerificationTask\.RandomSeed'), so a run is reproducible and two runs can be compared. The models behind those references are pulled in batches and each one is passed through `Analytical.Create.BuildingModelValidationResult`. A reference the server holds no model for is recorded as missing, which is the completeness half of the answer.

The seed is combined with the county row identifier rather than shared across counties, so a county draws the same sample whether it is verified on its own, with its voivodeship, or nationally. A single generator advanced across counties made every county's draw depend on how many references each preceding county held, which the 2026-08-14 county part repair changed - and with it the sample of every county after the repaired three.

Two files are written into [ReportDirectory](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsVerificationTask.ReportDirectory 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIBuildingModelsVerificationTask\.ReportDirectory'): one row per reference in `BuildingModels_Verification.csv`, and per county plus national totals in `BuildingModels_Verification_Summary.txt`. The row file is flushed county by county, so a run interrupted late still leaves everything it had already measured.

The CSV carries an `UnlocatedBuildingInformation` column, true for a model whose `BuildingInformation` carries no located coordinates and defined UTC offset - the state the [UIPostgreSQLBuildingModelBuildingInformationUpdateTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingModelBuildingInformationUpdateTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIPostgreSQLBuildingModelBuildingInformationUpdateTask') exists to remove, so the column is 0 after the backfill and on freshly regenerated models. It is a separate column rather than a [DiGi\.GIS\.Analytical\.Enums\.BuildingModelValidationCode](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.analytical.enums.buildingmodelvalidationcode 'DiGi\.GIS\.Analytical\.Enums\.BuildingModelValidationCode'), deliberately: a code would make every unstamped model invalid and break comparison with the 2026-08-11 enclosure baseline.

```csharp
public class UIBuildingModelsVerificationTask : DiGi.Core.Classes.ReportableBackgroundTask<long>, DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → UIBuildingModelsVerificationTask

Implements [IGISPostgreSQLUIObject](DiGi.GIS.PostgreSQL.UI.Interfaces.md#DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject 'DiGi\.GIS\.PostgreSQL\.UI\.Interfaces\.IGISPostgreSQLUIObject')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsVerificationTask.UIBuildingModelsVerificationTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager)'></a>

## UIBuildingModelsVerificationTask\(GISWebAPIManager\) Constructor

Initializes a new instance of the [UIBuildingModelsVerificationTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsVerificationTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIBuildingModelsVerificationTask') class\.

```csharp
public UIBuildingModelsVerificationTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager GISWebAPIManager);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsVerificationTask.UIBuildingModelsVerificationTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager).GISWebAPIManager'></a>

`GISWebAPIManager` [DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.giswebapimanager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIBuildingModelsVerificationTask\.GISWebAPIManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.ui.classes.uibuildingmodelsverificationtask.giswebapimanager 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIBuildingModelsVerificationTask\.GISWebAPIManager') instance used to communicate with the server\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsVerificationTask.BatchSize'></a>

## UIBuildingModelsVerificationTask\.BatchSize Property

Gets or sets the number of references asked for in a single request\. The references travel in the query string, so a batch far above this risks the URL length limit of the server\.

```csharp
public int BatchSize { get; set; }
```

#### Property Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsVerificationTask.CountyIds'></a>

## UIBuildingModelsVerificationTask\.CountyIds Property

Gets or sets the identifiers of the counties to be processed\. When null every county held on the server is processed\.

```csharp
public System.Collections.Generic.IEnumerable<int>? CountyIds { get; set; }
```

#### Property Value
[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsVerificationTask.RandomSeed'></a>

## UIBuildingModelsVerificationTask\.RandomSeed Property

Gets or sets the seed of the sampling\. Two runs sharing a seed draw the same references, which is what lets a run before a change be compared with one after it\. The seed is combined per county by [DiGi\.GIS\.PostgreSQL\.Query\.RandomSeed\(System\.Int32,System\.Int32\)](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.query.randomseed#digi-gis-postgresql-query-randomseed(system-int32-system-int32) 'DiGi\.GIS\.PostgreSQL\.Query\.RandomSeed\(System\.Int32,System\.Int32\)'), so a county's draw does not depend on the scope of the run or on what any other county holds\.

```csharp
public int RandomSeed { get; set; }
```

#### Property Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsVerificationTask.ReportDirectory'></a>

## UIBuildingModelsVerificationTask\.ReportDirectory Property

Gets or sets the directory the two report files are written into\. When null the user is asked for one\.

```csharp
public string? ReportDirectory { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsVerificationTask.SampleSize'></a>

## UIBuildingModelsVerificationTask\.SampleSize Property

Gets or sets the number of references drawn per county\. A value of zero or less takes every reference of the county\.

```csharp
public int SampleSize { get; set; }
```

#### Property Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsVerificationTask.Tolerance'></a>

## UIBuildingModelsVerificationTask\.Tolerance Property

Gets or sets the distance tolerance the enclosure of a space is required to hold at\.

```csharp
public double Tolerance { get; set; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsVerificationTask.VoivodeshipCodes'></a>

## UIBuildingModelsVerificationTask\.VoivodeshipCodes Property

Gets or sets the two\-digit voivodeship codes to be processed\. A county is in scope when its code starts with one of them\. When null every voivodeship is processed\. Combined with [CountyIds](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsVerificationTask.CountyIds 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIBuildingModelsVerificationTask\.CountyIds') both filters have to admit the county\.

```csharp
public System.Collections.Generic.IEnumerable<string>? VoivodeshipCodes { get; set; }
```

#### Property Value
[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsVerificationTask.Text(double)'></a>

## UIBuildingModelsVerificationTask\.Text\(double\) Method

Formats a value for the report, writing an empty cell rather than the word for not a number\.

```csharp
private static string Text(double value);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingModelsVerificationTask.Text(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The value to format\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The formatted value, or an empty string when it is not a number\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask'></a>

## UIBuildingsFromDirectoryPostTask Class

A UI\-driven post task that prompts the user to select a directory, reads CityGML city models from it, extracts [DiGi\.CityGML\.Classes\.Building](https://learn.microsoft.com/en-us/dotnet/api/digi.citygml.classes.building 'DiGi\.CityGML\.Classes\.Building') instances, determines the county code from the file path, and uploads them to the server in batches\.

```csharp
public class UIBuildingsFromDirectoryPostTask : DiGi.GIS.WebAPI.Classes.BuildingsPostTask, DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.serializableobjectsposttask-1 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\`1')[DiGi\.CityGML\.Classes\.Building](https://learn.microsoft.com/en-us/dotnet/api/digi.citygml.classes.building 'DiGi\.CityGML\.Classes\.Building')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.serializableobjectsposttask-1 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.BuildingsPostTask](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.buildingsposttask 'DiGi\.GIS\.WebAPI\.Classes\.BuildingsPostTask') → UIBuildingsFromDirectoryPostTask

Implements [IGISPostgreSQLUIObject](DiGi.GIS.PostgreSQL.UI.Interfaces.md#DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject 'DiGi\.GIS\.PostgreSQL\.UI\.Interfaces\.IGISPostgreSQLUIObject')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.UIBuildingsFromDirectoryPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager)'></a>

## UIBuildingsFromDirectoryPostTask\(GISWebAPIManager\) Constructor

Initializes a new instance of the [UIBuildingsFromDirectoryPostTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIBuildingsFromDirectoryPostTask') class\.

```csharp
public UIBuildingsFromDirectoryPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager GISWebAPIManager);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.UIBuildingsFromDirectoryPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager).GISWebAPIManager'></a>

`GISWebAPIManager` [DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.giswebapimanager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.giswebapimanager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance used to communicate with the server\.
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.LatestSourceAsync(DiGi.GIS.WebAPI.Classes.GISWebAPIManager)'></a>

## UIBuildingsFromDirectoryPostTask\.LatestSourceAsync\(GISWebAPIManager\) Method

Asynchronously reads the source path recorded on the most recently written building\.

```csharp
private static System.Threading.Tasks.Task<string?> LatestSourceAsync(DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.LatestSourceAsync(DiGi.GIS.WebAPI.Classes.GISWebAPIManager).GISWebAPIManager'></a>

`GISWebAPIManager` [DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.giswebapimanager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager used to reach the server\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task whose result is the recorded source path, or null when the server holds no buildings or the path was not recorded\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.ProduceAsync(string,System.Threading.Channels.ChannelWriter_DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.BuildingsBatch_,DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.ResumeFilter,System.Threading.CancellationToken)'></a>

## UIBuildingsFromDirectoryPostTask\.ProduceAsync\(string, ChannelWriter\<BuildingsBatch\>, ResumeFilter, CancellationToken\) Method

Walks the directory, parses each city model and publishes its tagged buildings to the channel\.

```csharp
private static System.Threading.Tasks.Task<bool> ProduceAsync(string directory, System.Threading.Channels.ChannelWriter<DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.BuildingsBatch> channelWriter, DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.ResumeFilter? resumeFilter, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.ProduceAsync(string,System.Threading.Channels.ChannelWriter_DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.BuildingsBatch_,DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.ResumeFilter,System.Threading.CancellationToken).directory'></a>

`directory` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The directory to walk\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.ProduceAsync(string,System.Threading.Channels.ChannelWriter_DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.BuildingsBatch_,DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.ResumeFilter,System.Threading.CancellationToken).channelWriter'></a>

`channelWriter` [System\.Threading\.Channels\.ChannelWriter&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.channels.channelwriter-1 'System\.Threading\.Channels\.ChannelWriter\`1')[BuildingsBatch](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.BuildingsBatch 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIBuildingsFromDirectoryPostTask\.BuildingsBatch')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.channels.channelwriter-1 'System\.Threading\.Channels\.ChannelWriter\`1')

The writer the parsed batches are published to\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.ProduceAsync(string,System.Threading.Channels.ChannelWriter_DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.BuildingsBatch_,DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.ResumeFilter,System.Threading.CancellationToken).resumeFilter'></a>

`resumeFilter` [ResumeFilter](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.ResumeFilter 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIBuildingsFromDirectoryPostTask\.ResumeFilter')

An optional filter that skips files preceding a recorded resume point, or null to walk everything\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.ProduceAsync(string,System.Threading.Channels.ChannelWriter_DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.BuildingsBatch_,DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.ResumeFilter,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A token observed before each file is parsed\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. The task result is true if the walk completed successfully; otherwise, false\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.BuildingsBatch'></a>

## UIBuildingsFromDirectoryPostTask\.BuildingsBatch Class

A parsed city model's buildings together with the county code derived from its file path\.

```csharp
private sealed class UIBuildingsFromDirectoryPostTask.BuildingsBatch
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → BuildingsBatch
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.BuildingsBatch.BuildingsBatch(System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,string)'></a>

## BuildingsBatch\(IEnumerable\<Building\>, string\) Constructor

Initializes a new instance of the [BuildingsBatch](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.BuildingsBatch 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIBuildingsFromDirectoryPostTask\.BuildingsBatch') class\.

```csharp
public BuildingsBatch(System.Collections.Generic.IEnumerable<DiGi.CityGML.Classes.Building> buildings, string? code);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.BuildingsBatch.BuildingsBatch(System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,string).buildings'></a>

`buildings` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.CityGML\.Classes\.Building](https://learn.microsoft.com/en-us/dotnet/api/digi.citygml.classes.building 'DiGi\.CityGML\.Classes\.Building')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The buildings parsed from a single city model\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.BuildingsBatch.BuildingsBatch(System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,string).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The county code derived from the source file path, or null when it could not be determined\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.BuildingsBatch.Buildings'></a>

## UIBuildingsFromDirectoryPostTask\.BuildingsBatch\.Buildings Property

Gets the buildings parsed from a single city model\.

```csharp
public System.Collections.Generic.IEnumerable<DiGi.CityGML.Classes.Building> Buildings { get; }
```

#### Property Value
[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.CityGML\.Classes\.Building](https://learn.microsoft.com/en-us/dotnet/api/digi.citygml.classes.building 'DiGi\.CityGML\.Classes\.Building')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.BuildingsBatch.Code'></a>

## UIBuildingsFromDirectoryPostTask\.BuildingsBatch\.Code Property

Gets the county code derived from the source file path\.

```csharp
public string? Code { get; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.ResumeFilter'></a>

## UIBuildingsFromDirectoryPostTask\.ResumeFilter Class

Skips walked files until a recorded source path is reached, then admits everything from that file onward\.

The recorded file is admitted rather than skipped: the run that wrote it was interrupted, so it was probably only partly uploaded. Re-importing it is safe because the server upserts on (county, reference, lod, year).

```csharp
private sealed class UIBuildingsFromDirectoryPostTask.ResumeFilter
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → ResumeFilter
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.ResumeFilter.ResumeFilter(string)'></a>

## ResumeFilter\(string\) Constructor

Initializes a new instance of the [ResumeFilter](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.ResumeFilter 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIBuildingsFromDirectoryPostTask\.ResumeFilter') class\.

```csharp
public ResumeFilter(string source);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.ResumeFilter.ResumeFilter(string).source'></a>

`source` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The recorded source path, relative to the walked directory, to resume from\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.ResumeFilter.Matched'></a>

## UIBuildingsFromDirectoryPostTask\.ResumeFilter\.Matched Property

Gets a value indicating whether the recorded source path was ever reached during the walk\.

```csharp
public bool Matched { get; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.ResumeFilter.Admit(string)'></a>

## UIBuildingsFromDirectoryPostTask\.ResumeFilter\.Admit\(string\) Method

Decides whether a walked file should be parsed\.

```csharp
public bool Admit(string path);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIBuildingsFromDirectoryPostTask.ResumeFilter.Admit(string).path'></a>

`path` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The walked file's path, relative to the walked directory\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
True once the recorded source path has been reached; otherwise, false\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIEPWFileFromFilePostTask'></a>

## UIEPWFileFromFilePostTask Class

Represents a task for posting [DiGi\.EPW\.Classes\.EPWFile](https://learn.microsoft.com/en-us/dotnet/api/digi.epw.classes.epwfile 'DiGi\.EPW\.Classes\.EPWFile') objects to a PostgreSQL database from EPW files selected through the user interface\.

```csharp
public class UIEPWFileFromFilePostTask : DiGi.GIS.WebAPI.Classes.EPWFilesPostTask, DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.serializableobjectsposttask-1 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\`1')[DiGi\.EPW\.Classes\.EPWFile](https://learn.microsoft.com/en-us/dotnet/api/digi.epw.classes.epwfile 'DiGi\.EPW\.Classes\.EPWFile')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.serializableobjectsposttask-1 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.EPWFilesPostTask](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.epwfilesposttask 'DiGi\.GIS\.WebAPI\.Classes\.EPWFilesPostTask') → UIEPWFileFromFilePostTask

Implements [IGISPostgreSQLUIObject](DiGi.GIS.PostgreSQL.UI.Interfaces.md#DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject 'DiGi\.GIS\.PostgreSQL\.UI\.Interfaces\.IGISPostgreSQLUIObject')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIEPWFileFromFilePostTask.UIEPWFileFromFilePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager)'></a>

## UIEPWFileFromFilePostTask\(GISWebAPIManager\) Constructor

Initializes a new instance of the [UIEPWFileFromFilePostTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIEPWFileFromFilePostTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIEPWFileFromFilePostTask') class\.

```csharp
public UIEPWFileFromFilePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager GISWebAPIManager);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIEPWFileFromFilePostTask.UIEPWFileFromFilePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager).GISWebAPIManager'></a>

`GISWebAPIManager` [DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.giswebapimanager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager used to communicate with the GIS PostgreSQL Web API\.
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIEPWFileFromFilePostTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken)'></a>

## UIEPWFileFromFilePostTask\.ExecuteAsync\(IProgress\<long\>, CancellationToken\) Method

Concrete implementation of the background work\.

```csharp
protected override System.Threading.Tasks.Task<bool> ExecuteAsync(System.IProgress<long> progress, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIEPWFileFromFilePostTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).progress'></a>

`progress` [System\.IProgress&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')

The provider for reporting progress of the operation\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIEPWFileFromFilePostTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The token to monitor for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. The task result is true if the operation succeeded; otherwise, false\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIOccupancyDatasFromFilePostTask'></a>

## UIOccupancyDatasFromFilePostTask Class

Represents a task that handles the process of posting occupancy data extracted from GIS model files to the PostgreSQL database through the user interface\.

```csharp
public class UIOccupancyDatasFromFilePostTask : DiGi.GIS.WebAPI.Classes.OccupancyDatasPostTask, DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.serializableobjectsposttask-1 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\`1')[DiGi\.GIS\.Classes\.OccupancyData](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.occupancydata 'DiGi\.GIS\.Classes\.OccupancyData')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.serializableobjectsposttask-1 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.OccupancyDatasPostTask](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.occupancydatasposttask 'DiGi\.GIS\.WebAPI\.Classes\.OccupancyDatasPostTask') → UIOccupancyDatasFromFilePostTask

Implements [IGISPostgreSQLUIObject](DiGi.GIS.PostgreSQL.UI.Interfaces.md#DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject 'DiGi\.GIS\.PostgreSQL\.UI\.Interfaces\.IGISPostgreSQLUIObject')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIOccupancyDatasFromFilePostTask.UIOccupancyDatasFromFilePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager)'></a>

## UIOccupancyDatasFromFilePostTask\(GISWebAPIManager\) Constructor

Initializes a new instance of the [UIOccupancyDatasFromFilePostTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIOccupancyDatasFromFilePostTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIOccupancyDatasFromFilePostTask') class\.

```csharp
public UIOccupancyDatasFromFilePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager GISWebAPIManager);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIOccupancyDatasFromFilePostTask.UIOccupancyDatasFromFilePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager).GISWebAPIManager'></a>

`GISWebAPIManager` [DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.giswebapimanager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager used to interact with the GIS PostgreSQL Web API\.
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIOccupancyDatasFromFilePostTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken)'></a>

## UIOccupancyDatasFromFilePostTask\.ExecuteAsync\(IProgress\<long\>, CancellationToken\) Method

Concrete implementation of the background work\.

```csharp
protected override System.Threading.Tasks.Task<bool> ExecuteAsync(System.IProgress<long> progress, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIOccupancyDatasFromFilePostTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).progress'></a>

`progress` [System\.IProgress&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')

The provider for reporting progress of the operation\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIOccupancyDatasFromFilePostTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The token to monitor for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. The task result is true if the operation completed successfully; otherwise, false\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIOrtoDatasFromFilePostTask'></a>

## UIOrtoDatasFromFilePostTask Class

Represents a task for posting orthodata from files to a PostgreSQL database, specifically designed for use within the UI layer\.

```csharp
public class UIOrtoDatasFromFilePostTask : DiGi.GIS.WebAPI.Classes.OrtoDatasPostTask, DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.serializableobjectsposttask-1 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\`1')[DiGi\.GIS\.Classes\.OrtoDatas](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.ortodatas 'DiGi\.GIS\.Classes\.OrtoDatas')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.serializableobjectsposttask-1 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.OrtoDatasPostTask](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.ortodatasposttask 'DiGi\.GIS\.WebAPI\.Classes\.OrtoDatasPostTask') → UIOrtoDatasFromFilePostTask

Implements [IGISPostgreSQLUIObject](DiGi.GIS.PostgreSQL.UI.Interfaces.md#DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject 'DiGi\.GIS\.PostgreSQL\.UI\.Interfaces\.IGISPostgreSQLUIObject')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIOrtoDatasFromFilePostTask.UIOrtoDatasFromFilePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager)'></a>

## UIOrtoDatasFromFilePostTask\(GISWebAPIManager\) Constructor

Initializes a new instance of the [UIOrtoDatasFromFilePostTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIOrtoDatasFromFilePostTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIOrtoDatasFromFilePostTask') class\.

```csharp
public UIOrtoDatasFromFilePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager GISWebAPIManager);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIOrtoDatasFromFilePostTask.UIOrtoDatasFromFilePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager).GISWebAPIManager'></a>

`GISWebAPIManager` [DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.giswebapimanager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager used to communicate with the GIS PostgreSQL Web API\.
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIOrtoDatasFromFilePostTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken)'></a>

## UIOrtoDatasFromFilePostTask\.ExecuteAsync\(IProgress\<long\>, CancellationToken\) Method

Concrete implementation of the background work\.

```csharp
protected override System.Threading.Tasks.Task<bool> ExecuteAsync(System.IProgress<long> progress, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIOrtoDatasFromFilePostTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).progress'></a>

`progress` [System\.IProgress&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')

The progress reporter used to track the operation's completion percentage\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIOrtoDatasFromFilePostTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token used to observe while writing the task to stop executing\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. The task result is true if the operation succeeded; otherwise, false\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuilding2DRefreshTask'></a>

## UIPostgreSQLBuilding2DRefreshTask Class

A Building2D refresh that is scoped from the user interface: the batch size, whether existing subdivision identifiers are re\-derived, whether the run is limited to counties whose subdivision layer nests, and which counties are walked are asked for through [PostgreSQLBuilding2DRefreshOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuilding2DRefreshOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.PostgreSQLBuilding2DRefreshOptionsWindow') each time the task is started, and only then is the run handed to [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuilding2DRefreshTask](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlbuilding2drefreshtask 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuilding2DRefreshTask')\.

That is what the scope is worth asking for. Overriding and unscoped, the refresh re-derives the subdivision of every building in the country - a polygon lookup and an intersection per building. The counties that need it after DiGi.GIS.PostgreSQL#77 are the ones whose subdivision layer nests, which the task can find on its own - but that is nearly every county, a village and its named parts nesting as a city and its districts do, so the county list is what makes a trial run small.

```csharp
public class UIPostgreSQLBuilding2DRefreshTask : DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuilding2DRefreshTask, DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuilding2DRefreshTask](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlbuilding2drefreshtask 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuilding2DRefreshTask') → UIPostgreSQLBuilding2DRefreshTask

Implements [IGISPostgreSQLUIObject](DiGi.GIS.PostgreSQL.UI.Interfaces.md#DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject 'DiGi\.GIS\.PostgreSQL\.UI\.Interfaces\.IGISPostgreSQLUIObject')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuilding2DRefreshTask.UIPostgreSQLBuilding2DRefreshTask(DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter)'></a>

## UIPostgreSQLBuilding2DRefreshTask\(Building2DPostgreSQLConverter, AdministrativeAreal2DPostgreSQLConverter\) Constructor

Initializes a new instance of the [UIPostgreSQLBuilding2DRefreshTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuilding2DRefreshTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIPostgreSQLBuilding2DRefreshTask') class\.

```csharp
public UIPostgreSQLBuilding2DRefreshTask(DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter building2DPostgreSQLConverter, DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter? administrativeAreal2DPostgreSQLConverter);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuilding2DRefreshTask.UIPostgreSQLBuilding2DRefreshTask(DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).building2DPostgreSQLConverter'></a>

`building2DPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.Building2DPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.building2dpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.Building2DPostgreSQLConverter')

The Building2D PostgreSQL converter used to refresh the data\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuilding2DRefreshTask.UIPostgreSQLBuilding2DRefreshTask(DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).administrativeAreal2DPostgreSQLConverter'></a>

`administrativeAreal2DPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DPostgreSQLConverter')

The administrative areal converter the counties are read from, so the run can be scoped\. When null the dialog offers no counties and the run is nationwide\.
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuilding2DRefreshTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken)'></a>

## UIPostgreSQLBuilding2DRefreshTask\.ExecuteAsync\(IProgress\<long\>, CancellationToken\) Method

Executes the background task to refresh the PostgreSQL data for Building2D\.

```csharp
protected override System.Threading.Tasks.Task<bool> ExecuteAsync(System.IProgress<long> progress, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuilding2DRefreshTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).progress'></a>

`progress` [System\.IProgress&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')

A progress reporter for reporting the number of processed items\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuilding2DRefreshTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task representing the asynchronous operation\. Returns true if the refresh was successful; otherwise, false\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingDataExternalComponentsUpdateTask'></a>

## UIPostgreSQLBuildingDataExternalComponentsUpdateTask Class

An external components area run that is scoped from the user interface: the counties and the statement timeout are asked for through [PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow') each time the task is started, and only then is the run handed to [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataExternalComponentsUpdateTask](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlbuildingdataexternalcomponentsupdatetask 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataExternalComponentsUpdateTask')\.

That is what the counties are worth asking for. The run reads every stored building model of the counties in scope and classifies their components into the wall, roof and floor area columns, so unscoped it walks all 406 county parts and their models, while over one county it is minutes. A county whose buildings carry no stored model is processed, not failed, so the scoping decides what is touched and how long it takes, not whether the run succeeds.

```csharp
public class UIPostgreSQLBuildingDataExternalComponentsUpdateTask : DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataExternalComponentsUpdateTask, DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataExternalComponentsUpdateTask](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlbuildingdataexternalcomponentsupdatetask 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataExternalComponentsUpdateTask') → UIPostgreSQLBuildingDataExternalComponentsUpdateTask

Implements [IGISPostgreSQLUIObject](DiGi.GIS.PostgreSQL.UI.Interfaces.md#DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject 'DiGi\.GIS\.PostgreSQL\.UI\.Interfaces\.IGISPostgreSQLUIObject')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingDataExternalComponentsUpdateTask.UIPostgreSQLBuildingDataExternalComponentsUpdateTask(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager)'></a>

## UIPostgreSQLBuildingDataExternalComponentsUpdateTask\(GISPostgreSQLConverterManager\) Constructor

Initializes a new instance of the [UIPostgreSQLBuildingDataExternalComponentsUpdateTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingDataExternalComponentsUpdateTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIPostgreSQLBuildingDataExternalComponentsUpdateTask') class\.

```csharp
public UIPostgreSQLBuildingDataExternalComponentsUpdateTask(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager GISPostgreSQLConverterManager);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingDataExternalComponentsUpdateTask.UIPostgreSQLBuildingDataExternalComponentsUpdateTask(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager).GISPostgreSQLConverterManager'></a>

`GISPostgreSQLConverterManager` [DiGi\.GIS\.PostgreSQL\.Classes\.GISPostgreSQLConverterManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.gispostgresqlconvertermanager 'DiGi\.GIS\.PostgreSQL\.Classes\.GISPostgreSQLConverterManager')

The GIS PostgreSQL converter manager used to read the stored building models and write the external components area columns\.
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingDataExternalComponentsUpdateTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken)'></a>

## UIPostgreSQLBuildingDataExternalComponentsUpdateTask\.ExecuteAsync\(IProgress\<long\>, CancellationToken\) Method

Executes the background task that fills the external components area columns of the building data table from the stored building models\.

```csharp
protected override System.Threading.Tasks.Task<bool> ExecuteAsync(System.IProgress<long> progress, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingDataExternalComponentsUpdateTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).progress'></a>

`progress` [System\.IProgress&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')

A progress reporter for reporting the number of rows written\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingDataExternalComponentsUpdateTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task representing the asynchronous operation\. Returns true when every county in scope was processed without error and no model was refused by the classification; otherwise, false\. Degenerate models do not make it false\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingDataUpdateTask'></a>

## UIPostgreSQLBuildingDataUpdateTask Class

A building data run that is scoped from the user interface: the counties, the kinds of column to write and the statement timeout are asked for through [PostgreSQLBuildingDataUpdateOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataUpdateOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.PostgreSQLBuildingDataUpdateOptionsWindow') each time the task is started, and only then is the run handed to [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataUpdateTask](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlbuildingdataupdatetask 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataUpdateTask')\.

That is what the counties are worth asking for. Unscoped the run walks every subdivision in the country - around a hundred thousand of them - reading each subdivision's buildings and writing a row per building; over one named county it is minutes. Neither is a default the other would tolerate, and the base task takes an unset county collection as the whole country.

```csharp
public class UIPostgreSQLBuildingDataUpdateTask : DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataUpdateTask, DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataUpdateTask](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlbuildingdataupdatetask 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataUpdateTask') → UIPostgreSQLBuildingDataUpdateTask

Implements [IGISPostgreSQLUIObject](DiGi.GIS.PostgreSQL.UI.Interfaces.md#DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject 'DiGi\.GIS\.PostgreSQL\.UI\.Interfaces\.IGISPostgreSQLUIObject')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingDataUpdateTask.UIPostgreSQLBuildingDataUpdateTask(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager)'></a>

## UIPostgreSQLBuildingDataUpdateTask\(GISPostgreSQLConverterManager\) Constructor

Initializes a new instance of the [UIPostgreSQLBuildingDataUpdateTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingDataUpdateTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIPostgreSQLBuildingDataUpdateTask') class\.

```csharp
public UIPostgreSQLBuildingDataUpdateTask(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager GISPostgreSQLConverterManager);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingDataUpdateTask.UIPostgreSQLBuildingDataUpdateTask(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager).GISPostgreSQLConverterManager'></a>

`GISPostgreSQLConverterManager` [DiGi\.GIS\.PostgreSQL\.Classes\.GISPostgreSQLConverterManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.gispostgresqlconvertermanager 'DiGi\.GIS\.PostgreSQL\.Classes\.GISPostgreSQLConverterManager')

The GIS PostgreSQL converter manager used to read the areas and buildings and write the building data\.
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingDataUpdateTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken)'></a>

## UIPostgreSQLBuildingDataUpdateTask\.ExecuteAsync\(IProgress\<long\>, CancellationToken\) Method

Executes the background task to update building data from AdministrativeAreal2D and Building2D sources\.

```csharp
protected override System.Threading.Tasks.Task<bool> ExecuteAsync(System.IProgress<long> progress, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingDataUpdateTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).progress'></a>

`progress` [System\.IProgress&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')

A progress reporter for reporting the number of processed items\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingDataUpdateTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task representing the asynchronous operation\. Returns true when the run could be attempted, every subdivision in scope was updated without error, every selected update type was written and every subdivision the radial ratios were asked for could be measured; otherwise, false \- including when a selected update type was counted against [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataUpdateTask\.UnfulfilledUpdateTypeCount](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlbuildingdataupdatetask.unfulfilledupdatetypecount 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataUpdateTask\.UnfulfilledUpdateTypeCount') or a subdivision against [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataUpdateTask\.RadialRatiosUnmeasuredSubdivisionCount](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlbuildingdataupdatetask.radialratiosunmeasuredsubdivisioncount 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataUpdateTask\.RadialRatiosUnmeasuredSubdivisionCount')\. A county's unassigned buildings whose radial ratios could not be measured are counted against [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataUpdateTask\.RadialRatiosUnmeasuredUnassignedCountyCount](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlbuildingdataupdatetask.radialratiosunmeasuredunassignedcountycount 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataUpdateTask\.RadialRatiosUnmeasuredUnassignedCountyCount') and do not affect the result; neither do [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataUpdateTask\.YearBuiltWrittenCount](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlbuildingdataupdatetask.yearbuiltwrittencount 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataUpdateTask\.YearBuiltWrittenCount') and [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataUpdateTask\.YearBuiltMissingBuildingCount](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlbuildingdataupdatetask.yearbuiltmissingbuildingcount 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataUpdateTask\.YearBuiltMissingBuildingCount'), since a county without stored year built entries is not a fault of the run\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingModelBuildingInformationUpdateTask'></a>

## UIPostgreSQLBuildingModelBuildingInformationUpdateTask Class

Stamps the WGS 84 coordinates and the Polish standard\-time UTC onto the `BuildingInformation` of the [DiGi\.Analytical\.Building\.Classes\.BuildingModel](https://learn.microsoft.com/en-us/dotnet/api/digi.analytical.building.classes.buildingmodel 'DiGi\.Analytical\.Building\.Classes\.BuildingModel') rows stored in the building model tables\.

The rows the national regeneration wrote before the creation paths started stamping carry no coordinates at all, so every stored model computes its sun path at (0, 0). The task walks each county part of each level table by identifier, classifies every row, and writes back the one JSON key it changes - `BuildingInformation` - leaving the rest of the stored object exactly as it is.

<b>Reports by default and writes nothing.</b>[DryRun](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingModelBuildingInformationUpdateTask.DryRun 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIPostgreSQLBuildingModelBuildingInformationUpdateTask\.DryRun') has to be turned off deliberately, and the summary it reports first - how many rows would be stamped and which are rejected - is what the real run should be reviewed against.

<b>The run is idempotent and checkpointed.</b> A row whose stored values already equal the computed ones is reported `AlreadyCorrect` and not written, so a second run reports 0 updated. Finished parts are appended to `BuildingModels_BuildingInformation_Checkpoint.txt` and skipped on a resumed run; a dry run neither reads nor writes it, so a dry run followed by a real run with [Resume](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingModelBuildingInformationUpdateTask.Resume 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIPostgreSQLBuildingModelBuildingInformationUpdateTask\.Resume') still touches every part.

The scope - the county parts, the detail level, the page size, the statement timeout, the checkpoint behaviour and the report directory - is asked for each time the task starts, through [PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow'); a cancelled dialog ends the run with nothing written.

```csharp
public class UIPostgreSQLBuildingModelBuildingInformationUpdateTask : DiGi.Core.Classes.ReportableBackgroundTask<long>, DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → UIPostgreSQLBuildingModelBuildingInformationUpdateTask

Implements [IGISPostgreSQLUIObject](DiGi.GIS.PostgreSQL.UI.Interfaces.md#DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject 'DiGi\.GIS\.PostgreSQL\.UI\.Interfaces\.IGISPostgreSQLUIObject')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingModelBuildingInformationUpdateTask.UIPostgreSQLBuildingModelBuildingInformationUpdateTask(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager)'></a>

## UIPostgreSQLBuildingModelBuildingInformationUpdateTask\(GISPostgreSQLConverterManager\) Constructor

Initializes a new instance of the [UIPostgreSQLBuildingModelBuildingInformationUpdateTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingModelBuildingInformationUpdateTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIPostgreSQLBuildingModelBuildingInformationUpdateTask') class\.

```csharp
public UIPostgreSQLBuildingModelBuildingInformationUpdateTask(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager gISPostgreSQLConverterManager);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingModelBuildingInformationUpdateTask.UIPostgreSQLBuildingModelBuildingInformationUpdateTask(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager).gISPostgreSQLConverterManager'></a>

`gISPostgreSQLConverterManager` [DiGi\.GIS\.PostgreSQL\.Classes\.GISPostgreSQLConverterManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.gispostgresqlconvertermanager 'DiGi\.GIS\.PostgreSQL\.Classes\.GISPostgreSQLConverterManager')

The manager holding the PostgreSQL converters\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingModelBuildingInformationUpdateTask.BatchSize'></a>

## UIPostgreSQLBuildingModelBuildingInformationUpdateTask\.BatchSize Property

Gets or sets the number of rows read and classified per page\.

```csharp
public int BatchSize { get; set; }
```

#### Property Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingModelBuildingInformationUpdateTask.BuildingModelDetailLevel'></a>

## UIPostgreSQLBuildingModelBuildingInformationUpdateTask\.BuildingModelDetailLevel Property

Gets or sets the detail level of the building model table to walk\. When null every level whose table exists is walked\.

```csharp
public System.Nullable<DiGi.Analytical.Building.Enums.BuildingModelDetailLevel> BuildingModelDetailLevel { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[DiGi\.Analytical\.Building\.Enums\.BuildingModelDetailLevel](https://learn.microsoft.com/en-us/dotnet/api/digi.analytical.building.enums.buildingmodeldetaillevel 'DiGi\.Analytical\.Building\.Enums\.BuildingModelDetailLevel')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingModelBuildingInformationUpdateTask.CommandTimeout'></a>

## UIPostgreSQLBuildingModelBuildingInformationUpdateTask\.CommandTimeout Property

Gets or sets the timeout in seconds applied to the statements\. A value of 0 disables the timeout\.

```csharp
public int CommandTimeout { get; set; }
```

#### Property Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingModelBuildingInformationUpdateTask.CountyIds'></a>

## UIPostgreSQLBuildingModelBuildingInformationUpdateTask\.CountyIds Property

Gets or sets the identifiers of the county rows to stamp\. When null every county row is in scope\.

These are polygon parts, not counties - a multi-part county holds one row per part and each is walked on its own.

```csharp
public System.Collections.Generic.IEnumerable<int>? CountyIds { get; set; }
```

#### Property Value
[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingModelBuildingInformationUpdateTask.DryRun'></a>

## UIPostgreSQLBuildingModelBuildingInformationUpdateTask\.DryRun Property

Gets or sets a value indicating whether the task only reports what it would stamp\. Defaults to [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool'); nothing is written until it is turned off\.

```csharp
public bool DryRun { get; set; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingModelBuildingInformationUpdateTask.ReportDirectory'></a>

## UIPostgreSQLBuildingModelBuildingInformationUpdateTask\.ReportDirectory Property

Gets or sets the directory the checkpoint and the reports are written into\. When null the directory the application was launched from is used\.

Deliberately not a folder dialog: this runs on a thread pool thread, where a WPF common dialog needs an STA apartment and throws instead of opening.

```csharp
public string? ReportDirectory { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingModelBuildingInformationUpdateTask.Resume'></a>

## UIPostgreSQLBuildingModelBuildingInformationUpdateTask\.Resume Property

Gets or sets a value indicating whether the parts named in the checkpoint of an earlier run are skipped\. Defaults to [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

The national pass has to survive being interrupted mid-country, so it resumes rather than restarting from the first part. Turning this off starts from the first part in scope and truncates the checkpoint, which is what a deliberate re-run of an already-completed scope needs.

```csharp
public bool Resume { get; set; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingModelBuildingInformationUpdateTask.SkipOptionsDialog'></a>

## UIPostgreSQLBuildingModelBuildingInformationUpdateTask\.SkipOptionsDialog Property

Gets or sets a value indicating whether the options window is skipped and the properties of this task are used as they are\. Defaults to [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

A run started from the tray always asks for its scope through [PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow'). A caller that sets the options itself - a test, or a host that runs the task without the user interface - turns this on so [DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIPostgreSQLBuildingModelBuildingInformationUpdateTask\.ExecuteAsync\(System\.IProgress\{System\.Int64\},System\.Threading\.CancellationToken\)](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.ui.classes.uipostgresqlbuildingmodelbuildinginformationupdatetask.executeasync#digi-gis-postgresql-ui-classes-uipostgresqlbuildingmodelbuildinginformationupdatetask-executeasync(system-iprogress{system-int64}-system-threading-cancellationtoken) 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIPostgreSQLBuildingModelBuildingInformationUpdateTask\.ExecuteAsync\(System\.IProgress\{System\.Int64\},System\.Threading\.CancellationToken\)') never asks for them.

```csharp
public bool SkipOptionsDialog { get; set; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingModelBuildingInformationUpdateTask.VoivodeshipCodes'></a>

## UIPostgreSQLBuildingModelBuildingInformationUpdateTask\.VoivodeshipCodes Property

Gets or sets the two\-digit voivodeship codes to be stamped\. A county row is in scope when its code starts with one of them\. When null every voivodeship is in scope\. Combined with [CountyIds](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingModelBuildingInformationUpdateTask.CountyIds 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIPostgreSQLBuildingModelBuildingInformationUpdateTask\.CountyIds') both filters have to admit the row\.

```csharp
public System.Collections.Generic.IEnumerable<string>? VoivodeshipCodes { get; set; }
```

#### Property Value
[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingModelBuildingInformationUpdateTask.Classify(DiGi.GIS.PostgreSQL.Classes.BuildingModel)'></a>

## UIPostgreSQLBuildingModelBuildingInformationUpdateTask\.Classify\(BuildingModel\) Method

Classifies one stored row: what the run would do with it and, for the outcomes that are reported, why\.

The stored values are taken before `UpdateBuildingInformation` is called, because the method always overwrites: the comparison against them is what makes a row already at its target values `AlreadyCorrect` rather than `ToUpdate`, and a second run report 0 updated.

```csharp
private static DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingModelBuildingInformationUpdateTask.RowResult Classify(DiGi.GIS.PostgreSQL.Classes.BuildingModel buildingModel);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingModelBuildingInformationUpdateTask.Classify(DiGi.GIS.PostgreSQL.Classes.BuildingModel).buildingModel'></a>

`buildingModel` [DiGi\.GIS\.PostgreSQL\.Classes\.BuildingModel](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.buildingmodel 'DiGi\.GIS\.PostgreSQL\.Classes\.BuildingModel')

The stored row to classify\.

#### Returns
[RowResult](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingModelBuildingInformationUpdateTask.RowResult 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIPostgreSQLBuildingModelBuildingInformationUpdateTask\.RowResult')  
The classification: `Failed` with a deserialization or exception reason, `Rejected` when the model cannot be located, `AlreadyCorrect` when the stored values already equal the computed ones, or `ToUpdate` carrying the value to write\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingModelBuildingInformationUpdateTask.RowResult'></a>

## UIPostgreSQLBuildingModelBuildingInformationUpdateTask\.RowResult Class

The classification of one stored row: its status, the reason for the outcomes that are reported, and, for `ToUpdate` rows, the value to write\.

```csharp
private class UIPostgreSQLBuildingModelBuildingInformationUpdateTask.RowResult
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → RowResult
### Fields

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingModelBuildingInformationUpdateTask.RowResult.Reason'></a>

## UIPostgreSQLBuildingModelBuildingInformationUpdateTask\.RowResult\.Reason Field

The reason for a `Failed` or `Rejected` row; null otherwise\.

```csharp
public string? Reason;
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingModelBuildingInformationUpdateTask.RowResult.Status'></a>

## UIPostgreSQLBuildingModelBuildingInformationUpdateTask\.RowResult\.Status Field

One of `Failed`, `Rejected`, `AlreadyCorrect` or `ToUpdate`\.

```csharp
public string Status;
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLBuildingModelBuildingInformationUpdateTask.RowResult.Value'></a>

## UIPostgreSQLBuildingModelBuildingInformationUpdateTask\.RowResult\.Value Field

The value to write for a `ToUpdate` row; null otherwise\.

```csharp
public JsonNode? Value;
```

#### Field Value
[System\.Text\.Json\.Nodes\.JsonNode](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonnode 'System\.Text\.Json\.Nodes\.JsonNode')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLStatisticalDataCollectionCreateTableTask'></a>

## UIPostgreSQLStatisticalDataCollectionCreateTableTask Class

Represents a task for creating the statistical data collection table in PostgreSQL and populating it from \.sdcf files in a directory selected through the user interface\.

```csharp
public class UIPostgreSQLStatisticalDataCollectionCreateTableTask : DiGi.GIS.PostgreSQL.Classes.PostgreSQLStatisticalDataCollectionCreateTableTask, DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLStatisticalDataCollectionCreateTableTask](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlstatisticaldatacollectioncreatetabletask 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLStatisticalDataCollectionCreateTableTask') → UIPostgreSQLStatisticalDataCollectionCreateTableTask

Implements [IGISPostgreSQLUIObject](DiGi.GIS.PostgreSQL.UI.Interfaces.md#DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject 'DiGi\.GIS\.PostgreSQL\.UI\.Interfaces\.IGISPostgreSQLUIObject')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLStatisticalDataCollectionCreateTableTask.UIPostgreSQLStatisticalDataCollectionCreateTableTask(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager)'></a>

## UIPostgreSQLStatisticalDataCollectionCreateTableTask\(GISPostgreSQLConverterManager\) Constructor

Initializes a new instance of the [UIPostgreSQLStatisticalDataCollectionCreateTableTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLStatisticalDataCollectionCreateTableTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIPostgreSQLStatisticalDataCollectionCreateTableTask') class from a manager\.

```csharp
public UIPostgreSQLStatisticalDataCollectionCreateTableTask(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager gISPostgreSQLConverterManager);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLStatisticalDataCollectionCreateTableTask.UIPostgreSQLStatisticalDataCollectionCreateTableTask(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager).gISPostgreSQLConverterManager'></a>

`gISPostgreSQLConverterManager` [DiGi\.GIS\.PostgreSQL\.Classes\.GISPostgreSQLConverterManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.gispostgresqlconvertermanager 'DiGi\.GIS\.PostgreSQL\.Classes\.GISPostgreSQLConverterManager')

The GIS PostgreSQL converter manager containing the statistical data collection converter\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLStatisticalDataCollectionCreateTableTask.UIPostgreSQLStatisticalDataCollectionCreateTableTask(DiGi.GIS.PostgreSQL.Classes.StatisticalDataCollectionPostgreSQLConverter)'></a>

## UIPostgreSQLStatisticalDataCollectionCreateTableTask\(StatisticalDataCollectionPostgreSQLConverter\) Constructor

Initializes a new instance of the [UIPostgreSQLStatisticalDataCollectionCreateTableTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLStatisticalDataCollectionCreateTableTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIPostgreSQLStatisticalDataCollectionCreateTableTask') class with a statistical data collection PostgreSQL converter\.

```csharp
public UIPostgreSQLStatisticalDataCollectionCreateTableTask(DiGi.GIS.PostgreSQL.Classes.StatisticalDataCollectionPostgreSQLConverter statisticalDataCollectionPostgreSQLConverter);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLStatisticalDataCollectionCreateTableTask.UIPostgreSQLStatisticalDataCollectionCreateTableTask(DiGi.GIS.PostgreSQL.Classes.StatisticalDataCollectionPostgreSQLConverter).statisticalDataCollectionPostgreSQLConverter'></a>

`statisticalDataCollectionPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.StatisticalDataCollectionPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.statisticaldatacollectionpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.StatisticalDataCollectionPostgreSQLConverter')

The statistical data collection PostgreSQL converter used to create and populate the table\.
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLStatisticalDataCollectionCreateTableTask.ExecuteAsync()'></a>

## UIPostgreSQLStatisticalDataCollectionCreateTableTask\.ExecuteAsync\(\) Method

Executes the background task to create the statistical data collection table in PostgreSQL\.

```csharp
protected override System.Threading.Tasks.Task<bool> ExecuteAsync();
```

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task representing the asynchronous operation\. Returns true if the table was created successfully; otherwise, false\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLStatisticalDataCollectionPopulateTask'></a>

## UIPostgreSQLStatisticalDataCollectionPopulateTask Class

Represents a task for populating statistical data collections into PostgreSQL from \.sdcf files in a directory selected through the user interface\.

```csharp
public class UIPostgreSQLStatisticalDataCollectionPopulateTask : DiGi.GIS.PostgreSQL.Classes.PostgreSQLStatisticalDataCollectionPopulateTask, DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLStatisticalDataCollectionPopulateTask](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlstatisticaldatacollectionpopulatetask 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLStatisticalDataCollectionPopulateTask') → UIPostgreSQLStatisticalDataCollectionPopulateTask

Implements [IGISPostgreSQLUIObject](DiGi.GIS.PostgreSQL.UI.Interfaces.md#DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject 'DiGi\.GIS\.PostgreSQL\.UI\.Interfaces\.IGISPostgreSQLUIObject')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLStatisticalDataCollectionPopulateTask.UIPostgreSQLStatisticalDataCollectionPopulateTask(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager)'></a>

## UIPostgreSQLStatisticalDataCollectionPopulateTask\(GISPostgreSQLConverterManager\) Constructor

Initializes a new instance of the [UIPostgreSQLStatisticalDataCollectionPopulateTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLStatisticalDataCollectionPopulateTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIPostgreSQLStatisticalDataCollectionPopulateTask') class from a manager\.

```csharp
public UIPostgreSQLStatisticalDataCollectionPopulateTask(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager gISPostgreSQLConverterManager);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLStatisticalDataCollectionPopulateTask.UIPostgreSQLStatisticalDataCollectionPopulateTask(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager).gISPostgreSQLConverterManager'></a>

`gISPostgreSQLConverterManager` [DiGi\.GIS\.PostgreSQL\.Classes\.GISPostgreSQLConverterManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.gispostgresqlconvertermanager 'DiGi\.GIS\.PostgreSQL\.Classes\.GISPostgreSQLConverterManager')

The GIS PostgreSQL converter manager containing the statistical data collection converter\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLStatisticalDataCollectionPopulateTask.UIPostgreSQLStatisticalDataCollectionPopulateTask(DiGi.GIS.PostgreSQL.Classes.StatisticalDataCollectionPostgreSQLConverter)'></a>

## UIPostgreSQLStatisticalDataCollectionPopulateTask\(StatisticalDataCollectionPostgreSQLConverter\) Constructor

Initializes a new instance of the [UIPostgreSQLStatisticalDataCollectionPopulateTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLStatisticalDataCollectionPopulateTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIPostgreSQLStatisticalDataCollectionPopulateTask') class with a converter\.

```csharp
public UIPostgreSQLStatisticalDataCollectionPopulateTask(DiGi.GIS.PostgreSQL.Classes.StatisticalDataCollectionPostgreSQLConverter statisticalDataCollectionPostgreSQLConverter);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLStatisticalDataCollectionPopulateTask.UIPostgreSQLStatisticalDataCollectionPopulateTask(DiGi.GIS.PostgreSQL.Classes.StatisticalDataCollectionPostgreSQLConverter).statisticalDataCollectionPostgreSQLConverter'></a>

`statisticalDataCollectionPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.StatisticalDataCollectionPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.statisticaldatacollectionpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.StatisticalDataCollectionPostgreSQLConverter')

The statistical data collection PostgreSQL converter used to populate the table\.
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLStatisticalDataCollectionPopulateTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken)'></a>

## UIPostgreSQLStatisticalDataCollectionPopulateTask\.ExecuteAsync\(IProgress\<long\>, CancellationToken\) Method

Executes the background task to read statistical data collections from \.sdcf files and insert them into PostgreSQL\.

```csharp
protected override System.Threading.Tasks.Task<bool> ExecuteAsync(System.IProgress<long> progress, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLStatisticalDataCollectionPopulateTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).progress'></a>

`progress` [System\.IProgress&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')

A progress reporter for reporting the number of processed items\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLStatisticalDataCollectionPopulateTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task representing the asynchronous operation\. Returns true if the population was successful; otherwise, false\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLTerrainPointFillGapsTask'></a>

## UIPostgreSQLTerrainPointFillGapsTask Class

A terrain point repair that is scoped from the user interface: the counties and the spacing they were sampled at are asked for through [PostgreSQLTerrainPointFillGapsOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointFillGapsOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.PostgreSQLTerrainPointFillGapsOptionsWindow') each time the task is started, and only then is the run handed to [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointFillGapsTask](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlterrainpointfillgapstask 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointFillGapsTask')\.

The spacing is why the dialog is worth showing at all. It is what decides which nodes count as missing, and a value finer than a county actually holds turns a repair of a few thousand points into a densification of the whole country - so the measured spacing of each county is put in front of whoever is choosing it.

```csharp
public class UIPostgreSQLTerrainPointFillGapsTask : DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointFillGapsTask, DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointFillGapsTask](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlterrainpointfillgapstask 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointFillGapsTask') → UIPostgreSQLTerrainPointFillGapsTask

Implements [IGISPostgreSQLUIObject](DiGi.GIS.PostgreSQL.UI.Interfaces.md#DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject 'DiGi\.GIS\.PostgreSQL\.UI\.Interfaces\.IGISPostgreSQLUIObject')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLTerrainPointFillGapsTask.UIPostgreSQLTerrainPointFillGapsTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager)'></a>

## UIPostgreSQLTerrainPointFillGapsTask\(GISWebAPIManager, GISPostgreSQLConverterManager\) Constructor

Initializes a new instance of the [UIPostgreSQLTerrainPointFillGapsTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLTerrainPointFillGapsTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIPostgreSQLTerrainPointFillGapsTask') class\.

```csharp
public UIPostgreSQLTerrainPointFillGapsTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager? GISPostgreSQLConverterManager);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLTerrainPointFillGapsTask.UIPostgreSQLTerrainPointFillGapsTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager).GISWebAPIManager'></a>

`GISWebAPIManager` [DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.giswebapimanager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager the elevation service client is built from\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLTerrainPointFillGapsTask.UIPostgreSQLTerrainPointFillGapsTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager).GISPostgreSQLConverterManager'></a>

`GISPostgreSQLConverterManager` [DiGi\.GIS\.PostgreSQL\.Classes\.GISPostgreSQLConverterManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.gispostgresqlconvertermanager 'DiGi\.GIS\.PostgreSQL\.Classes\.GISPostgreSQLConverterManager')

The GIS PostgreSQL converter manager used to read the areas and write the points\.
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLTerrainPointFillGapsTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken)'></a>

## UIPostgreSQLTerrainPointFillGapsTask\.ExecuteAsync\(IProgress\<long\>, CancellationToken\) Method

Executes the background task, measuring each county against the lattice and sampling only the nodes it is short of\.

```csharp
protected override System.Threading.Tasks.Task<bool> ExecuteAsync(System.IProgress<long> progress, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLTerrainPointFillGapsTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).progress'></a>

`progress` [System\.IProgress&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')

A progress reporter carrying the running total of points stored\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLTerrainPointFillGapsTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task representing the asynchronous operation\. Returns true unless the run was cancelled\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLTerrainPointUpdateTask'></a>

## UIPostgreSQLTerrainPointUpdateTask Class

A terrain point run that is scoped from the user interface: the counties, the spacing of the sampling grid and whether points already stored are sampled again are asked for through [PostgreSQLTerrainPointUpdateOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointUpdateOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.PostgreSQLTerrainPointUpdateOptionsWindow') each time the task is started, and only then is the run handed to [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointUpdateTask](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlterrainpointupdatetask 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointUpdateTask')\.

That is what the settings are worth asking for: a national pass at 50 m is about 125 million points and one request to the elevation service each, while the same task over a named county at 10 m is an afternoon. Neither is a default the other would tolerate.

```csharp
public class UIPostgreSQLTerrainPointUpdateTask : DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointUpdateTask, DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointUpdateTask](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlterrainpointupdatetask 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointUpdateTask') → UIPostgreSQLTerrainPointUpdateTask

Implements [IGISPostgreSQLUIObject](DiGi.GIS.PostgreSQL.UI.Interfaces.md#DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject 'DiGi\.GIS\.PostgreSQL\.UI\.Interfaces\.IGISPostgreSQLUIObject')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLTerrainPointUpdateTask.UIPostgreSQLTerrainPointUpdateTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager)'></a>

## UIPostgreSQLTerrainPointUpdateTask\(GISWebAPIManager, GISPostgreSQLConverterManager\) Constructor

Initializes a new instance of the [UIPostgreSQLTerrainPointUpdateTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLTerrainPointUpdateTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIPostgreSQLTerrainPointUpdateTask') class\.

```csharp
public UIPostgreSQLTerrainPointUpdateTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager? GISPostgreSQLConverterManager);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLTerrainPointUpdateTask.UIPostgreSQLTerrainPointUpdateTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager).GISWebAPIManager'></a>

`GISWebAPIManager` [DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.giswebapimanager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager the elevation service client is built from\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLTerrainPointUpdateTask.UIPostgreSQLTerrainPointUpdateTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager).GISPostgreSQLConverterManager'></a>

`GISPostgreSQLConverterManager` [DiGi\.GIS\.PostgreSQL\.Classes\.GISPostgreSQLConverterManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.gispostgresqlconvertermanager 'DiGi\.GIS\.PostgreSQL\.Classes\.GISPostgreSQLConverterManager')

The GIS PostgreSQL converter manager used to read the areas and write the points\.
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLTerrainPointUpdateTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken)'></a>

## UIPostgreSQLTerrainPointUpdateTask\.ExecuteAsync\(IProgress\<long\>, CancellationToken\) Method

Executes the background task, sampling elevations county by county and writing them to the terrain point table\.

```csharp
protected override System.Threading.Tasks.Task<bool> ExecuteAsync(System.IProgress<long> progress, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLTerrainPointUpdateTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).progress'></a>

`progress` [System\.IProgress&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')

A progress reporter carrying the running total of points stored\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLTerrainPointUpdateTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task representing the asynchronous operation\. Returns true unless the run was cancelled\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLUnitInsertFromFileTask'></a>

## UIPostgreSQLUnitInsertFromFileTask Class

Represents a task for populating territorial units into a PostgreSQL database from a JSON file selected through the user interface\.

```csharp
public class UIPostgreSQLUnitInsertFromFileTask : DiGi.GIS.PostgreSQL.Classes.PostgreSQLUnitInsertFromFileTask, DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLUnitInsertFromFileTask](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlunitinsertfromfiletask 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLUnitInsertFromFileTask') → UIPostgreSQLUnitInsertFromFileTask

Implements [IGISPostgreSQLUIObject](DiGi.GIS.PostgreSQL.UI.Interfaces.md#DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject 'DiGi\.GIS\.PostgreSQL\.UI\.Interfaces\.IGISPostgreSQLUIObject')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLUnitInsertFromFileTask.UIPostgreSQLUnitInsertFromFileTask(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager)'></a>

## UIPostgreSQLUnitInsertFromFileTask\(GISPostgreSQLConverterManager\) Constructor

Initializes a new instance of the [UIPostgreSQLUnitInsertFromFileTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLUnitInsertFromFileTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIPostgreSQLUnitInsertFromFileTask') class from a manager\.

```csharp
public UIPostgreSQLUnitInsertFromFileTask(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager gISPostgreSQLConverterManager);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLUnitInsertFromFileTask.UIPostgreSQLUnitInsertFromFileTask(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager).gISPostgreSQLConverterManager'></a>

`gISPostgreSQLConverterManager` [DiGi\.GIS\.PostgreSQL\.Classes\.GISPostgreSQLConverterManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.gispostgresqlconvertermanager 'DiGi\.GIS\.PostgreSQL\.Classes\.GISPostgreSQLConverterManager')

The GIS PostgreSQL converter manager containing the unit converter\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLUnitInsertFromFileTask.UIPostgreSQLUnitInsertFromFileTask(DiGi.GIS.PostgreSQL.Classes.UnitPostgreSQLConverter)'></a>

## UIPostgreSQLUnitInsertFromFileTask\(UnitPostgreSQLConverter\) Constructor

Initializes a new instance of the [UIPostgreSQLUnitInsertFromFileTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLUnitInsertFromFileTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIPostgreSQLUnitInsertFromFileTask') class with a unit PostgreSQL converter\.

```csharp
public UIPostgreSQLUnitInsertFromFileTask(DiGi.GIS.PostgreSQL.Classes.UnitPostgreSQLConverter unitPostgreSQLConverter);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLUnitInsertFromFileTask.UIPostgreSQLUnitInsertFromFileTask(DiGi.GIS.PostgreSQL.Classes.UnitPostgreSQLConverter).unitPostgreSQLConverter'></a>

`unitPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.UnitPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.unitpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.UnitPostgreSQLConverter')

The unit PostgreSQL converter used to populate the table\.
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLUnitInsertFromFileTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken)'></a>

## UIPostgreSQLUnitInsertFromFileTask\.ExecuteAsync\(IProgress\<long\>, CancellationToken\) Method

Executes the background task to read territorial units from JSON file\(s\) and insert them into PostgreSQL\.

```csharp
protected override System.Threading.Tasks.Task<bool> ExecuteAsync(System.IProgress<long> progress, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLUnitInsertFromFileTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).progress'></a>

`progress` [System\.IProgress&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')

A progress reporter for reporting the number of processed items\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLUnitInsertFromFileTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task representing the asynchronous operation\. Returns true if the population was successful; otherwise, false\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLUpdateOccupancyTask'></a>

## UIPostgreSQLUpdateOccupancyTask Class

An occupancy update that is scoped from the user interface: which side runs, whether the stored rows are cleared first and which counties the building side is limited to are asked for through [PostgreSQLUpdateOccupancyOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUpdateOccupancyOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.PostgreSQLUpdateOccupancyOptionsWindow') each time the task is started, and only then is the run handed to [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLUpdateOccupancyTask](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlupdateoccupancytask 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLUpdateOccupancyTask')\.

The counties are what make a re-run of one county affordable after its subdivision identifiers were re-derived (DiGi.GIS.PostgreSQL#77): unscoped, the building side reads and rewrites every building in the country.

```csharp
public class UIPostgreSQLUpdateOccupancyTask : DiGi.GIS.PostgreSQL.Classes.PostgreSQLUpdateOccupancyTask, DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLUpdateOccupancyTask](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlupdateoccupancytask 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLUpdateOccupancyTask') → UIPostgreSQLUpdateOccupancyTask

Implements [IGISPostgreSQLUIObject](DiGi.GIS.PostgreSQL.UI.Interfaces.md#DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject 'DiGi\.GIS\.PostgreSQL\.UI\.Interfaces\.IGISPostgreSQLUIObject')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLUpdateOccupancyTask.UIPostgreSQLUpdateOccupancyTask(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager)'></a>

## UIPostgreSQLUpdateOccupancyTask\(GISPostgreSQLConverterManager\) Constructor

Initializes a new instance of the [UIPostgreSQLUpdateOccupancyTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLUpdateOccupancyTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIPostgreSQLUpdateOccupancyTask') class\.

```csharp
public UIPostgreSQLUpdateOccupancyTask(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager gISPostgreSQLConverterManager);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLUpdateOccupancyTask.UIPostgreSQLUpdateOccupancyTask(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager).gISPostgreSQLConverterManager'></a>

`gISPostgreSQLConverterManager` [DiGi\.GIS\.PostgreSQL\.Classes\.GISPostgreSQLConverterManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.gispostgresqlconvertermanager 'DiGi\.GIS\.PostgreSQL\.Classes\.GISPostgreSQLConverterManager')

The GIS PostgreSQL converter manager used to read the areas and buildings and write the occupancy\.
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLUpdateOccupancyTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken)'></a>

## UIPostgreSQLUpdateOccupancyTask\.ExecuteAsync\(IProgress\<long\>, CancellationToken\) Method

Executes the background task to update occupancy data for administrative areal units and buildings\.

```csharp
protected override System.Threading.Tasks.Task<bool> ExecuteAsync(System.IProgress<long> progress, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLUpdateOccupancyTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).progress'></a>

`progress` [System\.IProgress&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')

A progress reporter for reporting the number of processed items\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLUpdateOccupancyTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task representing the asynchronous operation\. Returns true if the update was successful; otherwise, false\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLUserCreateTask'></a>

## UIPostgreSQLUserCreateTask Class

Creates one user in the user database, asking for it through [PostgreSQLUserCreateOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUserCreateOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.PostgreSQLUserCreateOptionsWindow') each time the task is started\.

The password is stored only as a PBKDF2 derived key with its salt and iteration count. The text the operator typed reaches the derivation and nothing else - not the row, not the log, and not a field on this task.

The user database is not the database the rest of this application works against: users live where `User_PostgreSQL_Main.conf` points, which is what the Web API reads when it authenticates a login. A user written anywhere else could not log in.

There is no base task to hand the run to. [DiGi\.User\.PostgreSQL\.Classes\.UserPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.user.postgresql.classes.userpostgresqlconverter 'DiGi\.User\.PostgreSQL\.Classes\.UserPostgreSQLConverter') already carries every database step, and creating a user is three of them.

Every refusal - a taken email, a cancelled dialog, a database that cannot be reached - is thrown as a [DiGi\.Core\.Classes\.BackgroundTaskFailureException](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtaskfailureexception 'DiGi\.Core\.Classes\.BackgroundTaskFailureException') rather than returned, so the task row carries the reason on hover instead of a bare Failed with the reason buried in the log file.

```csharp
public class UIPostgreSQLUserCreateTask : DiGi.Core.Classes.BackgroundTask, DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → UIPostgreSQLUserCreateTask

Implements [IGISPostgreSQLUIObject](DiGi.GIS.PostgreSQL.UI.Interfaces.md#DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject 'DiGi\.GIS\.PostgreSQL\.UI\.Interfaces\.IGISPostgreSQLUIObject')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLUserCreateTask.UIPostgreSQLUserCreateTask(DiGi.User.PostgreSQL.Classes.UserPostgreSQLConverter)'></a>

## UIPostgreSQLUserCreateTask\(UserPostgreSQLConverter\) Constructor

Initializes a new instance of the [UIPostgreSQLUserCreateTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLUserCreateTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIPostgreSQLUserCreateTask') class\.

```csharp
public UIPostgreSQLUserCreateTask(DiGi.User.PostgreSQL.Classes.UserPostgreSQLConverter userPostgreSQLConverter);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLUserCreateTask.UIPostgreSQLUserCreateTask(DiGi.User.PostgreSQL.Classes.UserPostgreSQLConverter).userPostgreSQLConverter'></a>

`userPostgreSQLConverter` [DiGi\.User\.PostgreSQL\.Classes\.UserPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.user.postgresql.classes.userpostgresqlconverter 'DiGi\.User\.PostgreSQL\.Classes\.UserPostgreSQLConverter')

The converter addressing the user database the new user is written to\.
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIPostgreSQLUserCreateTask.ExecuteAsync()'></a>

## UIPostgreSQLUserCreateTask\.ExecuteAsync\(\) Method

When overridden in a derived class, defines the work to be executed by the background task\.

```csharp
protected override System.Threading.Tasks.Task<bool> ExecuteAsync();
```

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
True if the task succeeded; otherwise, false\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIUpdateFromFilePostTask'></a>

## UIUpdateFromFilePostTask Class

Represents a task that handles the process of updating GIS data from a file via user interface interactions\.

```csharp
public class UIUpdateFromFilePostTask : DiGi.GIS.WebAPI.Classes.Building2DsPostTask, DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.serializableobjectsposttask-1 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\`1')[DiGi\.GIS\.Classes\.Building2D](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.building2d 'DiGi\.GIS\.Classes\.Building2D')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.serializableobjectsposttask-1 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.Building2DsPostTask](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.building2dsposttask 'DiGi\.GIS\.WebAPI\.Classes\.Building2DsPostTask') → UIUpdateFromFilePostTask

Implements [IGISPostgreSQLUIObject](DiGi.GIS.PostgreSQL.UI.Interfaces.md#DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject 'DiGi\.GIS\.PostgreSQL\.UI\.Interfaces\.IGISPostgreSQLUIObject')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIUpdateFromFilePostTask.UIUpdateFromFilePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager)'></a>

## UIUpdateFromFilePostTask\(GISWebAPIManager\) Constructor

Initializes a new instance of the [UIUpdateFromFilePostTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIUpdateFromFilePostTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIUpdateFromFilePostTask') class\.

```csharp
public UIUpdateFromFilePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager GISWebAPIManager);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIUpdateFromFilePostTask.UIUpdateFromFilePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager).GISWebAPIManager'></a>

`GISWebAPIManager` [DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.giswebapimanager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager used to handle PostgreSQL Web API communications\.
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIUpdateFromFilePostTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken)'></a>

## UIUpdateFromFilePostTask\.ExecuteAsync\(IProgress\<long\>, CancellationToken\) Method

Concrete implementation of the background work\.

```csharp
protected override System.Threading.Tasks.Task<bool> ExecuteAsync(System.IProgress<long> progress, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIUpdateFromFilePostTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).progress'></a>

`progress` [System\.IProgress&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')

The provider for reporting progress of the operation\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIUpdateFromFilePostTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The token to monitor for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. The task result is true if the update was successful; otherwise, false\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYearBuiltDatasFromFilePostTask'></a>

## UIYearBuiltDatasFromFilePostTask Class

Represents a task that extracts year built data from GIS model files and posts it to the PostgreSQL database\.

```csharp
public class UIYearBuiltDatasFromFilePostTask : DiGi.GIS.WebAPI.Classes.YearBuiltDatasPostTask, DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.serializableobjectsposttask-1 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\`1')[DiGi\.GIS\.Classes\.YearBuiltData](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.yearbuiltdata 'DiGi\.GIS\.Classes\.YearBuiltData')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.serializableobjectsposttask-1 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.YearBuiltDatasPostTask](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.yearbuiltdatasposttask 'DiGi\.GIS\.WebAPI\.Classes\.YearBuiltDatasPostTask') → UIYearBuiltDatasFromFilePostTask

Implements [IGISPostgreSQLUIObject](DiGi.GIS.PostgreSQL.UI.Interfaces.md#DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject 'DiGi\.GIS\.PostgreSQL\.UI\.Interfaces\.IGISPostgreSQLUIObject')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYearBuiltDatasFromFilePostTask.UIYearBuiltDatasFromFilePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager)'></a>

## UIYearBuiltDatasFromFilePostTask\(GISWebAPIManager\) Constructor

Initializes a new instance of the [UIYearBuiltDatasFromFilePostTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIYearBuiltDatasFromFilePostTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIYearBuiltDatasFromFilePostTask') class\.

```csharp
public UIYearBuiltDatasFromFilePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager GISWebAPIManager);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYearBuiltDatasFromFilePostTask.UIYearBuiltDatasFromFilePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager).GISWebAPIManager'></a>

`GISWebAPIManager` [DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.giswebapimanager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager used to handle communication with the PostgreSQL Web API\.
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYearBuiltDatasFromFilePostTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken)'></a>

## UIYearBuiltDatasFromFilePostTask\.ExecuteAsync\(IProgress\<long\>, CancellationToken\) Method

Concrete implementation of the background work\.

```csharp
protected override System.Threading.Tasks.Task<bool> ExecuteAsync(System.IProgress<long> progress, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYearBuiltDatasFromFilePostTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).progress'></a>

`progress` [System\.IProgress&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')

The provider for reporting progress of the operation\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYearBuiltDatasFromFilePostTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The token to monitor for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. The task result is true if the process succeeded; otherwise, false\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYearBuiltPredictionsTask'></a>

## UIYearBuiltPredictionsTask Class

A Year Built prediction run that is scoped from the user interface: the counties, the scratch directory, the interpreter, the request concurrency and the scratch cleanup are asked for through [YearBuiltPredictionsOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.YearBuiltPredictionsOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.YearBuiltPredictionsOptionsWindow') each time the task is started\.

<b>A tray run has one shape, and it writes.</b> The pipeline's steps are not offered - DiGi-Organization/DiGi.GIS.YOLO.UI#8 made the six of them a single run per county and left the granular flags to the options class and the console app - so the dialog settles the scope and the run does the rest. Because OK now means the deployed data of every county selected, the scope is named back in a confirmation before anything is launched.

<b>The run happens in another process.</b> The pipeline needs a regressor, and the only implementation of it carries the machine learning closure - about a gigabyte of native libraries, against an application that publishes self-contained and single-file. The `IYearBuiltPredictor` seam exists to keep that weight out of hosts that only need to start a run, so this task writes the options out and hands them to `DiGi.GIS.YOLO.UI.ConsoleApp`, which already hosts the pipeline and is already exercised end to end.

Two consequences of that are worth knowing before a run. <b>The runner authorizes with its own key</b>, read from the `GIS_WebAPI_Client.conf` beside its executable rather than from this application's - a run that ends in [DiGi\.GIS\.YOLO\.UI\.Enums\.YearBuiltPredictionExitCode\.Authorization](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.enums.yearbuiltpredictionexitcode.authorization 'DiGi\.GIS\.YOLO\.UI\.Enums\.YearBuiltPredictionExitCode\.Authorization') is usually that file rather than the one this application uses. And <b>stopping the task kills the run rather than winding it down</b>: the whole process tree goes, the detector included, so a batch that was being written may be half written. Every step of the pipeline is idempotent and a stopped run is re-runnable, but its tallies are not a record of what was stored.

The environment preflight runs here, before anything is launched, so a machine with no CPython carrying ultralytics says so in front of whoever opened the dialog instead of an hour later as an exit code. The pipeline repeats the check inside the run; that costs one interpreter start and is what makes the reason legible.

<b>Every refusal before launch names its reason on the task row.</b> Each check below runs before anything is started, logs its reason and throws a [DiGi\.Core\.Classes\.BackgroundTaskFailureException](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtaskfailureexception 'DiGi\.Core\.Classes\.BackgroundTaskFailureException') worded exactly as the log line ([RenderRefusal\(string, object\[\]\)](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Query.RenderRefusal(string,object[]) 'DiGi\.GIS\.PostgreSQL\.UI\.Query\.RenderRefusal\(string, object\[\]\)') renders it), so nobody has to open the logs folder to learn why nothing started. A cancelled dialog or a declined confirmation is not a refusal - nobody scoped a run, so the task reports a completed run that started nothing. A run that fails after launch is reported with its exit code, the last `[ERROR]`/`[FATAL]` line the runner printed and the step that failed ([FailureMessage\(YearBuiltPredictionExitCode, IEnumerable&lt;string&gt;\)](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIYearBuiltPredictionsTask.FailureMessage(DiGi.GIS.YOLO.UI.Enums.YearBuiltPredictionExitCode,System.Collections.Generic.IEnumerable_string_) 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIYearBuiltPredictionsTask\.FailureMessage\(DiGi\.GIS\.YOLO\.UI\.Enums\.YearBuiltPredictionExitCode, System\.Collections\.Generic\.IEnumerable\<string\>\)')).

```csharp
public class UIYearBuiltPredictionsTask : DiGi.Core.Classes.ReportableBackgroundTask<long>, DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → UIYearBuiltPredictionsTask

Implements [IGISPostgreSQLUIObject](DiGi.GIS.PostgreSQL.UI.Interfaces.md#DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject 'DiGi\.GIS\.PostgreSQL\.UI\.Interfaces\.IGISPostgreSQLUIObject')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYearBuiltPredictionsTask.UIYearBuiltPredictionsTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager)'></a>

## UIYearBuiltPredictionsTask\(GISWebAPIManager\) Constructor

Initializes a new instance of the [UIYearBuiltPredictionsTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIYearBuiltPredictionsTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIYearBuiltPredictionsTask') class\.

```csharp
public UIYearBuiltPredictionsTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager GISWebAPIManager);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYearBuiltPredictionsTask.UIYearBuiltPredictionsTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager).GISWebAPIManager'></a>

`GISWebAPIManager` [DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.giswebapimanager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.giswebapimanager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance the county rows behind the dialog are read with\. The run itself authorizes with the runner's own key, not with this one\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYearBuiltPredictionsTask.ConsoleAppPath'></a>

## UIYearBuiltPredictionsTask\.ConsoleAppPath Property

Gets or sets the path of the headless runner\. When null it is resolved by [YearBuiltPredictionConsoleAppPath\(string, string\)](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Query.YearBuiltPredictionConsoleAppPath(string,string) 'DiGi\.GIS\.PostgreSQL\.UI\.Query\.YearBuiltPredictionConsoleAppPath\(string, string\)'), which probes this application's own output, the runner's folder beside it, and the runner's build output in a workspace checkout\.

```csharp
public string? ConsoleAppPath { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYearBuiltPredictionsTask.YearBuiltPredictionPipelineOptions'></a>

## UIYearBuiltPredictionsTask\.YearBuiltPredictionPipelineOptions Property

Gets or sets the options the dialog opens with, and which it writes back to when it is closed with OK\. When null the defaults are used, which name no county and therefore ask for nothing\.

```csharp
public DiGi.GIS.YOLO.UI.Classes.YearBuiltPredictionPipelineOptions? YearBuiltPredictionPipelineOptions { get; set; }
```

#### Property Value
[DiGi\.GIS\.YOLO\.UI\.Classes\.YearBuiltPredictionPipelineOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.classes.yearbuiltpredictionpipelineoptions 'DiGi\.GIS\.YOLO\.UI\.Classes\.YearBuiltPredictionPipelineOptions')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYearBuiltPredictionsTask.FailureMessage(DiGi.GIS.YOLO.UI.Enums.YearBuiltPredictionExitCode,System.Collections.Generic.IEnumerable_string_)'></a>

## UIYearBuiltPredictionsTask\.FailureMessage\(YearBuiltPredictionExitCode, IEnumerable\<string\>\) Method

Builds the message a failed Year Built prediction run is reported with, so the task row names the last reason instead of the generic "reported failure without an exception" text\.

The runner marks its standard output with prefixes: `[ERROR]` and `[FATAL]` for what went wrong, `[PROGRESS]` for the item counts, `[INFO]`/`[NOTE]` for narration, and two spaces plus a dash for each entry under a `… failed step(s):` header. The last `[ERROR]`/`[FATAL]` line is the cause the run ended with, and the step entries under it - when it is such a header - say which step died; everything else is narration the log already carries. Without a named cause the exit code alone is reported.

```csharp
internal static string FailureMessage(DiGi.GIS.YOLO.UI.Enums.YearBuiltPredictionExitCode exitCode, System.Collections.Generic.IEnumerable<string> lines);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYearBuiltPredictionsTask.FailureMessage(DiGi.GIS.YOLO.UI.Enums.YearBuiltPredictionExitCode,System.Collections.Generic.IEnumerable_string_).exitCode'></a>

`exitCode` [DiGi\.GIS\.YOLO\.UI\.Enums\.YearBuiltPredictionExitCode](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.enums.yearbuiltpredictionexitcode 'DiGi\.GIS\.YOLO\.UI\.Enums\.YearBuiltPredictionExitCode')

The exit code the runner ended with\. It is never [DiGi\.GIS\.YOLO\.UI\.Enums\.YearBuiltPredictionExitCode\.Succeeded](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.enums.yearbuiltpredictionexitcode.succeeded 'DiGi\.GIS\.YOLO\.UI\.Enums\.YearBuiltPredictionExitCode\.Succeeded') or [DiGi\.GIS\.YOLO\.UI\.Enums\.YearBuiltPredictionExitCode\.Cancelled](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.enums.yearbuiltpredictionexitcode.cancelled 'DiGi\.GIS\.YOLO\.UI\.Enums\.YearBuiltPredictionExitCode\.Cancelled') here \- both are handled by the caller\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYearBuiltPredictionsTask.FailureMessage(DiGi.GIS.YOLO.UI.Enums.YearBuiltPredictionExitCode,System.Collections.Generic.IEnumerable_string_).lines'></a>

`lines` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The standard output lines the runner printed, in order\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The failure message for the task row\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYearBuiltPredictionsTask.WriteOptions(DiGi.GIS.YOLO.UI.Classes.YearBuiltPredictionPipelineOptions,string)'></a>

## UIYearBuiltPredictionsTask\.WriteOptions\(YearBuiltPredictionPipelineOptions, string\) Method

Writes the options of a scoped run beside its scratch directory \- the record of what the run was asked to do \- and reports why it could not be written when it could not\.

```csharp
private static string? WriteOptions(DiGi.GIS.YOLO.UI.Classes.YearBuiltPredictionPipelineOptions yearBuiltPredictionPipelineOptions, out string? reason);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYearBuiltPredictionsTask.WriteOptions(DiGi.GIS.YOLO.UI.Classes.YearBuiltPredictionPipelineOptions,string).yearBuiltPredictionPipelineOptions'></a>

`yearBuiltPredictionPipelineOptions` [DiGi\.GIS\.YOLO\.UI\.Classes\.YearBuiltPredictionPipelineOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.classes.yearbuiltpredictionpipelineoptions 'DiGi\.GIS\.YOLO\.UI\.Classes\.YearBuiltPredictionPipelineOptions')

The options the run is handed\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYearBuiltPredictionsTask.WriteOptions(DiGi.GIS.YOLO.UI.Classes.YearBuiltPredictionPipelineOptions,string).reason'></a>

`reason` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The refusal to write, worded as it is logged, or null when the options were written\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The path of the written options file; null when it could not be written\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYOLOTrainingTask'></a>

## UIYOLOTrainingTask Class

A YOLO detector training run started from the tray: dataset build, label check, training, validation on the Test split and the detector evaluation that gates the new weights, with the inputs asked for through [YOLOTrainingOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.YOLOTrainingOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.YOLOTrainingOptionsWindow') each time the task is started\.

<b>Two scenarios.</b>[Retrain](DiGi.GIS.PostgreSQL.UI.Enums.md#DiGi.GIS.PostgreSQL.UI.Enums.YOLOTrainingScenario.Retrain 'DiGi\.GIS\.PostgreSQL\.UI\.Enums\.YOLOTrainingScenario\.Retrain') continues from the deployed model.pt and appends the chosen counties to the existing dataset folder; [Fresh](DiGi.GIS.PostgreSQL.UI.Enums.md#DiGi.GIS.PostgreSQL.UI.Enums.YOLOTrainingScenario.Fresh 'DiGi\.GIS\.PostgreSQL\.UI\.Enums\.YOLOTrainingScenario\.Fresh') starts from the pretrained yolo26x.pt and builds the dataset fresh into a new folder. The scenario sets defaults only ([YOLOTrainingRunOptions\(YOLOTrainingScenario, string, YOLOTrainingRunOptions\)](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Create.YOLOTrainingRunOptions(DiGi.GIS.PostgreSQL.UI.Enums.YOLOTrainingScenario,string,DiGi.GIS.YOLO.UI.Classes.YOLOTrainingRunOptions) 'DiGi\.GIS\.PostgreSQL\.UI\.Create\.YOLOTrainingRunOptions\(DiGi\.GIS\.PostgreSQL\.UI\.Enums\.YOLOTrainingScenario, string, DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingRunOptions\)')).

<b>The run happens in another process</b>, `DiGi.GIS.YOLO.UI.ConsoleApp --train`, for the same reason as [UIYearBuiltPredictionsTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIYearBuiltPredictionsTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIYearBuiltPredictionsTask'): this application publishes self-contained and single-file and does not carry the runner's closure. The options are written beside the run folder - not inside it, because the runner refuses a run whose folder already exists - and that file is the record of what the run was asked to do, so it is never overwritten: a run with the Training step writes `<RunName>.YOLOTrainingRunOptions.json` (a taken run name is refused before launch), a resume writes `<RunName>.resume-<yyyyMMdd_HHmmss>.YOLOTrainingRunOptions.json` so the original run's file is untouched, and a run without training always writes `yyyyMMdd_HHmmss.YOLOTrainingRunOptions.json`, whatever the run name box says ([YOLOTrainingRunOptionsFile\(string, string, bool, string, DateTimeOffset, bool\)](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Create.YOLOTrainingRunOptionsFile(string,string,bool,string,System.DateTimeOffset,bool) 'DiGi\.GIS\.PostgreSQL\.UI\.Create\.YOLOTrainingRunOptionsFile\(string, string, bool, string, System\.DateTimeOffset, bool\)')).

<b>Every path is made absolute here</b>, against the runner for the files its defaults name (the weights, the legacy references, the reports folder) and against this process for the folders the operator typed, so the file the runner reads names what this application checked and named back.

<b>The run never overwrites model.pt.</b> The trained weights stay under the run folder and are copied to `<ProjectDirectory>\<RunName>\<RunName>.pt`; shipping or holding them is a manual decision from the evaluation table (DiGi-Organization/DiGi.GIS.YOLO.UI#12).

The dataset step authorizes with <b>the runner's own key</b>, read from the `GIS_WebAPI_Client.conf` beside its executable - a run that ends in [DiGi\.GIS\.YOLO\.UI\.Enums\.YearBuiltPredictionExitCode\.Authorization](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.enums.yearbuiltpredictionexitcode.authorization 'DiGi\.GIS\.YOLO\.UI\.Enums\.YearBuiltPredictionExitCode\.Authorization') is usually that file. <b>Stopping the task kills the process tree</b>, the interpreter included; the dataset manifest lets a Re-train run continue an interrupted build (a Start from yolo26x.pt run does not resume, so a fresh build that was stopped is continued as Re-train with the start weights set back to yolo26x.pt), and a training that was stopped or crashed is <b>offered for resume</b>: the dialog names its epoch and ceiling, the preflight repeats the runner's refusals before launch, and the resume continues `weights\last.pt` at the next epoch in the same folder. Closing this application or a power cut still stops the run - resume shortens the recovery, it does not prevent the interruption. A training that stalls while its runner is alive is also <b>resumed automatically</b>, up to [DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingRunOptions\.AutoResumeCount](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.classes.yolotrainingrunoptions.autoresumecount 'DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingRunOptions\.AutoResumeCount') times (the dialog offers 3) and after [DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingRunOptions\.InactivityTimeout](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.classes.yolotrainingrunoptions.inactivitytimeout 'DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingRunOptions\.InactivityTimeout') without output (15 minutes by default); a run that exhausts them fails with the last reason named, and a stop from the tray is never resumed.

```csharp
public class UIYOLOTrainingTask : DiGi.Core.Classes.ReportableBackgroundTask<long>, DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → UIYOLOTrainingTask

Implements [IGISPostgreSQLUIObject](DiGi.GIS.PostgreSQL.UI.Interfaces.md#DiGi.GIS.PostgreSQL.UI.Interfaces.IGISPostgreSQLUIObject 'DiGi\.GIS\.PostgreSQL\.UI\.Interfaces\.IGISPostgreSQLUIObject')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYOLOTrainingTask.UIYOLOTrainingTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager)'></a>

## UIYOLOTrainingTask\(GISWebAPIManager\) Constructor

Initializes a new instance of the [UIYOLOTrainingTask](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIYOLOTrainingTask 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIYOLOTrainingTask') class\.

```csharp
public UIYOLOTrainingTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager GISWebAPIManager);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYOLOTrainingTask.UIYOLOTrainingTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager).GISWebAPIManager'></a>

`GISWebAPIManager` [DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.giswebapimanager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.giswebapimanager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance the county rows behind the dialog are read with\. The run itself authorizes with the runner's own key, not with this one\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYOLOTrainingTask.ConsoleAppPath'></a>

## UIYOLOTrainingTask\.ConsoleAppPath Property

Gets or sets the path of the headless runner\. When null it is resolved by [YearBuiltPredictionConsoleAppPath\(string, string\)](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Query.YearBuiltPredictionConsoleAppPath(string,string) 'DiGi\.GIS\.PostgreSQL\.UI\.Query\.YearBuiltPredictionConsoleAppPath\(string, string\)') \- the training mode is hosted by the same executable as the Year Built prediction\.

```csharp
public string? ConsoleAppPath { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYOLOTrainingTask.YOLOTrainingRunOptions'></a>

## UIYOLOTrainingTask\.YOLOTrainingRunOptions Property

Gets or sets the options the dialog opens with, and which it writes back to \- with every path made absolute \- when a run is confirmed\. When null the defaults of [YOLOTrainingScenario](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.UIYOLOTrainingTask.YOLOTrainingScenario 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.UIYOLOTrainingTask\.YOLOTrainingScenario') are used, which name no county and no folder and therefore ask for them\.

```csharp
public DiGi.GIS.YOLO.UI.Classes.YOLOTrainingRunOptions? YOLOTrainingRunOptions { get; set; }
```

#### Property Value
[DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingRunOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.classes.yolotrainingrunoptions 'DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingRunOptions')

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYOLOTrainingTask.YOLOTrainingScenario'></a>

## UIYOLOTrainingTask\.YOLOTrainingScenario Property

Gets or sets the scenario the dialog opens with, and which it writes back to when it is closed with OK\.

```csharp
public DiGi.GIS.PostgreSQL.UI.Enums.YOLOTrainingScenario YOLOTrainingScenario { get; set; }
```

#### Property Value
[YOLOTrainingScenario](DiGi.GIS.PostgreSQL.UI.Enums.md#DiGi.GIS.PostgreSQL.UI.Enums.YOLOTrainingScenario 'DiGi\.GIS\.PostgreSQL\.UI\.Enums\.YOLOTrainingScenario')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYOLOTrainingTask.FailureMessage(System.Nullable_DiGi.GIS.YOLO.UI.Enums.YearBuiltPredictionExitCode_,System.Collections.Concurrent.ConcurrentQueue_string_)'></a>

## UIYOLOTrainingTask\.FailureMessage\(Nullable\<YearBuiltPredictionExitCode\>, ConcurrentQueue\<string\>\) Method

Builds the message a failed YOLO training run is reported with, so the task row names the last reason instead of the generic "reported failure without an exception" text\.

The runner prints one line per automatic resume (`... - automatic resume N of M`), a closing summary (`[NOTE] Resumed automatically N time(s) ...`) and, when a stall or a crash ends the run with no resume left, the cause of that last attempt (`Training stalled at epoch E ... - no automatic resume left`, or `- automatic resume is off`). That last cause is named first, with the exit code and the summary after it; without it the summary or the last resume line is named with the exit code, and otherwise the exit code alone is.

```csharp
private static string FailureMessage(System.Nullable<DiGi.GIS.YOLO.UI.Enums.YearBuiltPredictionExitCode> yearBuiltPredictionExitCode, System.Collections.Concurrent.ConcurrentQueue<string> lines);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYOLOTrainingTask.FailureMessage(System.Nullable_DiGi.GIS.YOLO.UI.Enums.YearBuiltPredictionExitCode_,System.Collections.Concurrent.ConcurrentQueue_string_).yearBuiltPredictionExitCode'></a>

`yearBuiltPredictionExitCode` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[DiGi\.GIS\.YOLO\.UI\.Enums\.YearBuiltPredictionExitCode](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.enums.yearbuiltpredictionexitcode 'DiGi\.GIS\.YOLO\.UI\.Enums\.YearBuiltPredictionExitCode')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The exit code the runner ended with\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYOLOTrainingTask.FailureMessage(System.Nullable_DiGi.GIS.YOLO.UI.Enums.YearBuiltPredictionExitCode_,System.Collections.Concurrent.ConcurrentQueue_string_).lines'></a>

`lines` [System\.Collections\.Concurrent\.ConcurrentQueue&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentqueue-1 'System\.Collections\.Concurrent\.ConcurrentQueue\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentqueue-1 'System\.Collections\.Concurrent\.ConcurrentQueue\`1')

The standard output lines the runner printed\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The failure message for the task row\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYOLOTrainingTask.Refuse(string,string,string)'></a>

## UIYOLOTrainingTask\.Refuse\(string, string, string\) Method

Logs a pre\-launch refusal the operator is refused with and throws it, so the task row shows the reason instead of the generic "reported failure without an exception" text; it never returns normally\.

```csharp
private static void Refuse(string name, string message, string? value=null);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYOLOTrainingTask.Refuse(string,string,string).name'></a>

`name` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The field the refusal is about\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYOLOTrainingTask.Refuse(string,string,string).message'></a>

`message` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The reason, with a Serilog property placeholder for the value, when there is one\.

<a name='DiGi.GIS.PostgreSQL.UI.Classes.UIYOLOTrainingTask.Refuse(string,string,string).value'></a>

`value` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The value the reason names, when there is one\.