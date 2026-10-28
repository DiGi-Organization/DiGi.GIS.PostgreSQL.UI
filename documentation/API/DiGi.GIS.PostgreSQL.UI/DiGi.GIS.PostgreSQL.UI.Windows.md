#### [DiGi\.GIS\.PostgreSQL\.UI](DiGi.GIS.PostgreSQL.UI.Overview.md 'DiGi\.GIS\.PostgreSQL\.UI\.Overview')

## DiGi\.GIS\.PostgreSQL\.UI\.Windows Namespace
### Classes

<a name='DiGi.GIS.PostgreSQL.UI.Windows.BuildingModelsFromDatabaseOptionsWindow'></a>

## BuildingModelsFromDatabaseOptionsWindow Class

Interaction logic for BuildingModelsFromDatabaseOptionsWindow\.xaml

Asks what decides the reach of a BuildingModel regeneration from the database: which county polygon parts are walked, whether the checkpoint of an earlier run is honoured, and where the checkpoint and the failed-county list are written. The request pacing stays with the task - it is tuning nobody changes between runs, and is set where the task is registered.

No county selected means every county, which is what the task has always meant.

The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.

```csharp
public class BuildingModelsFromDatabaseOptionsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [System\.Windows\.Threading\.DispatcherObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcherobject 'System\.Windows\.Threading\.DispatcherObject') → [System\.Windows\.DependencyObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.dependencyobject 'System\.Windows\.DependencyObject') → [System\.Windows\.Media\.Visual](https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.visual 'System\.Windows\.Media\.Visual') → [System\.Windows\.UIElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.uielement 'System\.Windows\.UIElement') → [System\.Windows\.FrameworkElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement 'System\.Windows\.FrameworkElement') → [System\.Windows\.Controls\.Control](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.control 'System\.Windows\.Controls\.Control') → [System\.Windows\.Controls\.ContentControl](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.contentcontrol 'System\.Windows\.Controls\.ContentControl') → [System\.Windows\.Window](https://learn.microsoft.com/en-us/dotnet/api/system.windows.window 'System\.Windows\.Window') → BuildingModelsFromDatabaseOptionsWindow

Implements [System\.Windows\.Markup\.IComponentConnector](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector 'System\.Windows\.Markup\.IComponentConnector')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Windows.BuildingModelsFromDatabaseOptionsWindow.BuildingModelsFromDatabaseOptionsWindow(DiGi.GIS.PostgreSQL.UI.Classes.BuildingModelsFromDatabaseOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_)'></a>

## BuildingModelsFromDatabaseOptionsWindow\(BuildingModelsFromDatabaseOptions, IEnumerable\<AdministrativeAreal2DReference\>\) Constructor

Initializes a new instance of the [BuildingModelsFromDatabaseOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.BuildingModelsFromDatabaseOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.BuildingModelsFromDatabaseOptionsWindow') class\.

```csharp
public BuildingModelsFromDatabaseOptionsWindow(DiGi.GIS.PostgreSQL.UI.Classes.BuildingModelsFromDatabaseOptions? buildingModelsFromDatabaseOptions, System.Collections.Generic.IEnumerable<DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference>? administrativeAreal2DReferences);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.BuildingModelsFromDatabaseOptionsWindow.BuildingModelsFromDatabaseOptionsWindow(DiGi.GIS.PostgreSQL.UI.Classes.BuildingModelsFromDatabaseOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_).buildingModelsFromDatabaseOptions'></a>

`buildingModelsFromDatabaseOptions` [BuildingModelsFromDatabaseOptions](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.BuildingModelsFromDatabaseOptions 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.BuildingModelsFromDatabaseOptions')

The options the controls are filled from\. When null the defaults are used\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.BuildingModelsFromDatabaseOptionsWindow.BuildingModelsFromDatabaseOptionsWindow(DiGi.GIS.PostgreSQL.UI.Classes.BuildingModelsFromDatabaseOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_).administrativeAreal2DReferences'></a>

`administrativeAreal2DReferences` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The counties to choose from\. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Windows.BuildingModelsFromDatabaseOptionsWindow.BuildingModelsFromDatabaseOptions'></a>

## BuildingModelsFromDatabaseOptionsWindow\.BuildingModelsFromDatabaseOptions Property

Gets the options the window holds\. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with\.

```csharp
public DiGi.GIS.PostgreSQL.UI.Classes.BuildingModelsFromDatabaseOptions BuildingModelsFromDatabaseOptions { get; }
```

#### Property Value
[BuildingModelsFromDatabaseOptions](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.BuildingModelsFromDatabaseOptions 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.BuildingModelsFromDatabaseOptions')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Windows.BuildingModelsFromDatabaseOptionsWindow.InitializeComponent()'></a>

## BuildingModelsFromDatabaseOptionsWindow\.InitializeComponent\(\) Method

InitializeComponent

```csharp
public void InitializeComponent();
```

Implements [InitializeComponent\(\)](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector.initializecomponent 'System\.Windows\.Markup\.IComponentConnector\.InitializeComponent')

<a name='DiGi.GIS.PostgreSQL.UI.Windows.MainWindow'></a>

## MainWindow Class

MainWindow

```csharp
public class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [System\.Windows\.Threading\.DispatcherObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcherobject 'System\.Windows\.Threading\.DispatcherObject') → [System\.Windows\.DependencyObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.dependencyobject 'System\.Windows\.DependencyObject') → [System\.Windows\.Media\.Visual](https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.visual 'System\.Windows\.Media\.Visual') → [System\.Windows\.UIElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.uielement 'System\.Windows\.UIElement') → [System\.Windows\.FrameworkElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement 'System\.Windows\.FrameworkElement') → [System\.Windows\.Controls\.Control](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.control 'System\.Windows\.Controls\.Control') → [System\.Windows\.Controls\.ContentControl](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.contentcontrol 'System\.Windows\.Controls\.ContentControl') → [System\.Windows\.Window](https://learn.microsoft.com/en-us/dotnet/api/system.windows.window 'System\.Windows\.Window') → MainWindow

Implements [System\.Windows\.Markup\.IComponentConnector](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector 'System\.Windows\.Markup\.IComponentConnector')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Windows.MainWindow.MainWindow(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager,DiGi.User.PostgreSQL.Classes.UserPostgreSQLConverterManager,DiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Nullable_DiGi.GIS.PostgreSQL.UI.Enums.Mode_)'></a>

## MainWindow\(GISPostgreSQLConverterManager, UserPostgreSQLConverterManager, GISWebAPIManager, Nullable\<Mode\>\) Constructor

Initializes a new instance of the [MainWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.MainWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.MainWindow') class\.

```csharp
public MainWindow(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager? gISPostgreSQLConverterManager, DiGi.User.PostgreSQL.Classes.UserPostgreSQLConverterManager? userPostgreSQLConverterManager, DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, System.Nullable<DiGi.GIS.PostgreSQL.UI.Enums.Mode> mode=null);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.MainWindow.MainWindow(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager,DiGi.User.PostgreSQL.Classes.UserPostgreSQLConverterManager,DiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Nullable_DiGi.GIS.PostgreSQL.UI.Enums.Mode_).gISPostgreSQLConverterManager'></a>

`gISPostgreSQLConverterManager` [DiGi\.GIS\.PostgreSQL\.Classes\.GISPostgreSQLConverterManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.gispostgresqlconvertermanager 'DiGi\.GIS\.PostgreSQL\.Classes\.GISPostgreSQLConverterManager')

The manager responsible for GIS PostgreSQL conversion processes\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.MainWindow.MainWindow(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager,DiGi.User.PostgreSQL.Classes.UserPostgreSQLConverterManager,DiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Nullable_DiGi.GIS.PostgreSQL.UI.Enums.Mode_).userPostgreSQLConverterManager'></a>

`userPostgreSQLConverterManager` [DiGi\.User\.PostgreSQL\.Classes\.UserPostgreSQLConverterManager](https://learn.microsoft.com/en-us/dotnet/api/digi.user.postgresql.classes.userpostgresqlconvertermanager 'DiGi\.User\.PostgreSQL\.Classes\.UserPostgreSQLConverterManager')

The manager holding the converter for the user database, or null where no User\_PostgreSQL\_Main\.conf names one\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.MainWindow.MainWindow(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager,DiGi.User.PostgreSQL.Classes.UserPostgreSQLConverterManager,DiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Nullable_DiGi.GIS.PostgreSQL.UI.Enums.Mode_).GISWebAPIManager'></a>

`GISWebAPIManager` [DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.giswebapimanager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager responsible for GIS PostgreSQL Web API interactions\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.MainWindow.MainWindow(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager,DiGi.User.PostgreSQL.Classes.UserPostgreSQLConverterManager,DiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Nullable_DiGi.GIS.PostgreSQL.UI.Enums.Mode_).mode'></a>

`mode` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[Mode](DiGi.GIS.PostgreSQL.UI.Enums.md#DiGi.GIS.PostgreSQL.UI.Enums.Mode 'DiGi\.GIS\.PostgreSQL\.UI\.Enums\.Mode')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The operational mode of the application\. If null, it is determined based on converter availability\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Windows.MainWindow.Mode'></a>

## MainWindow\.Mode Property

Gets the current operational mode of the application\.

```csharp
public DiGi.GIS.PostgreSQL.UI.Enums.Mode Mode { get; }
```

#### Property Value
[Mode](DiGi.GIS.PostgreSQL.UI.Enums.md#DiGi.GIS.PostgreSQL.UI.Enums.Mode 'DiGi\.GIS\.PostgreSQL\.UI\.Enums\.Mode')

<a name='DiGi.GIS.PostgreSQL.UI.Windows.MainWindow.VisualBackgroundTasks_Client'></a>

## MainWindow\.VisualBackgroundTasks\_Client Property

Gets or sets the collection of visual background tasks associated with the client operational mode\.

```csharp
public System.Collections.ObjectModel.ObservableCollection<DiGi.UI.WPF.Interfaces.IVisualBackgroundTask>? VisualBackgroundTasks_Client { get; set; }
```

#### Property Value
[System\.Collections\.ObjectModel\.ObservableCollection&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1 'System\.Collections\.ObjectModel\.ObservableCollection\`1')[DiGi\.UI\.WPF\.Interfaces\.IVisualBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.ui.wpf.interfaces.ivisualbackgroundtask 'DiGi\.UI\.WPF\.Interfaces\.IVisualBackgroundTask')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1 'System\.Collections\.ObjectModel\.ObservableCollection\`1')

<a name='DiGi.GIS.PostgreSQL.UI.Windows.MainWindow.VisualBackgroundTasks_Server'></a>

## MainWindow\.VisualBackgroundTasks\_Server Property

Gets or sets the collection of visual background tasks associated with the server operational mode\.

```csharp
public System.Collections.ObjectModel.ObservableCollection<DiGi.UI.WPF.Interfaces.IVisualBackgroundTask>? VisualBackgroundTasks_Server { get; set; }
```

#### Property Value
[System\.Collections\.ObjectModel\.ObservableCollection&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1 'System\.Collections\.ObjectModel\.ObservableCollection\`1')[DiGi\.UI\.WPF\.Interfaces\.IVisualBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.ui.wpf.interfaces.ivisualbackgroundtask 'DiGi\.UI\.WPF\.Interfaces\.IVisualBackgroundTask')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1 'System\.Collections\.ObjectModel\.ObservableCollection\`1')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Windows.MainWindow.InitializeComponent()'></a>

## MainWindow\.InitializeComponent\(\) Method

InitializeComponent

```csharp
public void InitializeComponent();
```

Implements [InitializeComponent\(\)](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector.initializecomponent 'System\.Windows\.Markup\.IComponentConnector\.InitializeComponent')

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuilding2DRefreshOptionsWindow'></a>

## PostgreSQLBuilding2DRefreshOptionsWindow Class

Interaction logic for PostgreSQLBuilding2DRefreshOptionsWindow\.xaml

Asks for what decides the cost and the reach of a Building2D refresh: whether buildings that already carry a `subdivision_id` are re-derived, whether the run is limited to the counties whose subdivision layer nests, and which counties are walked at all. Unscoped and overriding, the run re-derives every building in the country.

No county selected means every county - the scope the refresh has always had. The nested-layer box names the counties where the previous tie-break produced an arbitrary value (DiGi.GIS.PostgreSQL#77); expect it to exclude very little, because a village and its named parts nest as a city and its districts do. The county list is what makes a trial run small.

The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.

```csharp
public class PostgreSQLBuilding2DRefreshOptionsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [System\.Windows\.Threading\.DispatcherObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcherobject 'System\.Windows\.Threading\.DispatcherObject') → [System\.Windows\.DependencyObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.dependencyobject 'System\.Windows\.DependencyObject') → [System\.Windows\.Media\.Visual](https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.visual 'System\.Windows\.Media\.Visual') → [System\.Windows\.UIElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.uielement 'System\.Windows\.UIElement') → [System\.Windows\.FrameworkElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement 'System\.Windows\.FrameworkElement') → [System\.Windows\.Controls\.Control](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.control 'System\.Windows\.Controls\.Control') → [System\.Windows\.Controls\.ContentControl](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.contentcontrol 'System\.Windows\.Controls\.ContentControl') → [System\.Windows\.Window](https://learn.microsoft.com/en-us/dotnet/api/system.windows.window 'System\.Windows\.Window') → PostgreSQLBuilding2DRefreshOptionsWindow

Implements [System\.Windows\.Markup\.IComponentConnector](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector 'System\.Windows\.Markup\.IComponentConnector')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuilding2DRefreshOptionsWindow.PostgreSQLBuilding2DRefreshOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuilding2DRefreshOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_)'></a>

## PostgreSQLBuilding2DRefreshOptionsWindow\(PostgreSQLBuilding2DRefreshOptions, IEnumerable\<AdministrativeAreal2DReference\>\) Constructor

Initializes a new instance of the [PostgreSQLBuilding2DRefreshOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuilding2DRefreshOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.PostgreSQLBuilding2DRefreshOptionsWindow') class\.

```csharp
public PostgreSQLBuilding2DRefreshOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuilding2DRefreshOptions? postgreSQLBuilding2DRefreshOptions, System.Collections.Generic.IEnumerable<DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference>? administrativeAreal2DReferences);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuilding2DRefreshOptionsWindow.PostgreSQLBuilding2DRefreshOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuilding2DRefreshOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_).postgreSQLBuilding2DRefreshOptions'></a>

`postgreSQLBuilding2DRefreshOptions` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuilding2DRefreshOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlbuilding2drefreshoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuilding2DRefreshOptions')

The options the controls are filled from\. When null the defaults are used\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuilding2DRefreshOptionsWindow.PostgreSQLBuilding2DRefreshOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuilding2DRefreshOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_).administrativeAreal2DReferences'></a>

`administrativeAreal2DReferences` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The counties to choose from\. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuilding2DRefreshOptionsWindow.PostgreSQLBuilding2DRefreshOptions'></a>

## PostgreSQLBuilding2DRefreshOptionsWindow\.PostgreSQLBuilding2DRefreshOptions Property

Gets the options the window holds\. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with\.

```csharp
public DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuilding2DRefreshOptions PostgreSQLBuilding2DRefreshOptions { get; }
```

#### Property Value
[DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuilding2DRefreshOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlbuilding2drefreshoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuilding2DRefreshOptions')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuilding2DRefreshOptionsWindow.InitializeComponent()'></a>

## PostgreSQLBuilding2DRefreshOptionsWindow\.InitializeComponent\(\) Method

InitializeComponent

```csharp
public void InitializeComponent();
```

Implements [InitializeComponent\(\)](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector.initializecomponent 'System\.Windows\.Markup\.IComponentConnector\.InitializeComponent')

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow'></a>

## PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow Class

Interaction logic for PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow\.xaml

Asks for the two settings that decide what a run touches and how long a statement may take - which counties, and the command timeout. The run reads the stored building models of the counties in scope, so the counties decide its reach and its cost; left unscoped it walks every county part in the country.

Every county is selected by default, so opening the dialog confirms the national scope rather than inviting it; deselecting everything is still refused at OK, because an empty selection is not the same as no selection and neither is a safe thing to leave this window with by accident.

The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.

```csharp
public class PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [System\.Windows\.Threading\.DispatcherObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcherobject 'System\.Windows\.Threading\.DispatcherObject') → [System\.Windows\.DependencyObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.dependencyobject 'System\.Windows\.DependencyObject') → [System\.Windows\.Media\.Visual](https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.visual 'System\.Windows\.Media\.Visual') → [System\.Windows\.UIElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.uielement 'System\.Windows\.UIElement') → [System\.Windows\.FrameworkElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement 'System\.Windows\.FrameworkElement') → [System\.Windows\.Controls\.Control](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.control 'System\.Windows\.Controls\.Control') → [System\.Windows\.Controls\.ContentControl](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.contentcontrol 'System\.Windows\.Controls\.ContentControl') → [System\.Windows\.Window](https://learn.microsoft.com/en-us/dotnet/api/system.windows.window 'System\.Windows\.Window') → PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow

Implements [System\.Windows\.Markup\.IComponentConnector](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector 'System\.Windows\.Markup\.IComponentConnector')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow.PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataExternalComponentsUpdateOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_)'></a>

## PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow\(PostgreSQLBuildingDataExternalComponentsUpdateOptions, IEnumerable\<AdministrativeAreal2DReference\>\) Constructor

Initializes a new instance of the [PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow') class\.

```csharp
public PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataExternalComponentsUpdateOptions? postgreSQLBuildingDataExternalComponentsUpdateOptions, System.Collections.Generic.IEnumerable<DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference>? administrativeAreal2DReferences);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow.PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataExternalComponentsUpdateOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_).postgreSQLBuildingDataExternalComponentsUpdateOptions'></a>

`postgreSQLBuildingDataExternalComponentsUpdateOptions` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataExternalComponentsUpdateOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlbuildingdataexternalcomponentsupdateoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataExternalComponentsUpdateOptions')

The options the controls are filled from\. When null the defaults are used\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow.PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataExternalComponentsUpdateOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_).administrativeAreal2DReferences'></a>

`administrativeAreal2DReferences` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The counties to choose from\. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own\. All of them are selected when the options carry no county set\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow.PostgreSQLBuildingDataExternalComponentsUpdateOptions'></a>

## PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow\.PostgreSQLBuildingDataExternalComponentsUpdateOptions Property

Gets the options the window holds\. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with\.

```csharp
public DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataExternalComponentsUpdateOptions PostgreSQLBuildingDataExternalComponentsUpdateOptions { get; }
```

#### Property Value
[DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataExternalComponentsUpdateOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlbuildingdataexternalcomponentsupdateoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataExternalComponentsUpdateOptions')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow.InitializeComponent()'></a>

## PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow\.InitializeComponent\(\) Method

InitializeComponent

```csharp
public void InitializeComponent();
```

Implements [InitializeComponent\(\)](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector.initializecomponent 'System\.Windows\.Markup\.IComponentConnector\.InitializeComponent')

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataUpdateOptionsWindow'></a>

## PostgreSQLBuildingDataUpdateOptionsWindow Class

Interaction logic for PostgreSQLBuildingDataUpdateOptionsWindow\.xaml

Asks for the three settings that decide what a run costs and touches - which counties, which kinds of column, and how long a statement may take. The radiuses are carried over from the instance it was given untouched: each one names its own pair of stored columns, so changing them from a dialog would change the shape of the table rather than the numbers in it.

The counties matter most. Left unscoped the run walks every subdivision in the country, which is the pass nobody wants to start by accident while trying one county.

The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.

```csharp
public class PostgreSQLBuildingDataUpdateOptionsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [System\.Windows\.Threading\.DispatcherObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcherobject 'System\.Windows\.Threading\.DispatcherObject') → [System\.Windows\.DependencyObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.dependencyobject 'System\.Windows\.DependencyObject') → [System\.Windows\.Media\.Visual](https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.visual 'System\.Windows\.Media\.Visual') → [System\.Windows\.UIElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.uielement 'System\.Windows\.UIElement') → [System\.Windows\.FrameworkElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement 'System\.Windows\.FrameworkElement') → [System\.Windows\.Controls\.Control](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.control 'System\.Windows\.Controls\.Control') → [System\.Windows\.Controls\.ContentControl](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.contentcontrol 'System\.Windows\.Controls\.ContentControl') → [System\.Windows\.Window](https://learn.microsoft.com/en-us/dotnet/api/system.windows.window 'System\.Windows\.Window') → PostgreSQLBuildingDataUpdateOptionsWindow

Implements [System\.Windows\.Markup\.IComponentConnector](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector 'System\.Windows\.Markup\.IComponentConnector')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataUpdateOptionsWindow.PostgreSQLBuildingDataUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataUpdateOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_)'></a>

## PostgreSQLBuildingDataUpdateOptionsWindow\(PostgreSQLBuildingDataUpdateOptions, IEnumerable\<AdministrativeAreal2DReference\>\) Constructor

Initializes a new instance of the [PostgreSQLBuildingDataUpdateOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataUpdateOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.PostgreSQLBuildingDataUpdateOptionsWindow') class\.

```csharp
public PostgreSQLBuildingDataUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataUpdateOptions? postgreSQLBuildingDataUpdateOptions, System.Collections.Generic.IEnumerable<DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference>? administrativeAreal2DReferences);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataUpdateOptionsWindow.PostgreSQLBuildingDataUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataUpdateOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_).postgreSQLBuildingDataUpdateOptions'></a>

`postgreSQLBuildingDataUpdateOptions` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataUpdateOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlbuildingdataupdateoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataUpdateOptions')

The options the controls are filled from\. When null the defaults are used\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataUpdateOptionsWindow.PostgreSQLBuildingDataUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataUpdateOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_).administrativeAreal2DReferences'></a>

`administrativeAreal2DReferences` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The counties to choose from\. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataUpdateOptionsWindow.PostgreSQLBuildingDataUpdateOptions'></a>

## PostgreSQLBuildingDataUpdateOptionsWindow\.PostgreSQLBuildingDataUpdateOptions Property

Gets the options the window holds\. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with\.

```csharp
public DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataUpdateOptions PostgreSQLBuildingDataUpdateOptions { get; }
```

#### Property Value
[DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataUpdateOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlbuildingdataupdateoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataUpdateOptions')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataUpdateOptionsWindow.InitializeComponent()'></a>

## PostgreSQLBuildingDataUpdateOptionsWindow\.InitializeComponent\(\) Method

InitializeComponent

```csharp
public void InitializeComponent();
```

Implements [InitializeComponent\(\)](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector.initializecomponent 'System\.Windows\.Markup\.IComponentConnector\.InitializeComponent')

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow'></a>

## PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow Class

Interaction logic for PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow\.xaml

Asks what decides the reach of a BuildingInformation stamping run: which county polygon parts are walked, which detail level, the page size, the statement timeout, where the checkpoint and the reports are written, and whether a run that has already been started resumes or restarts.

Every county is selected by default, so opening the dialog confirms the national scope rather than inviting it; deselecting everything is still refused at OK, because an empty selection is not the same as no selection and neither is a safe thing to leave this window with by accident.

The dry run checkbox is checked by default: the run reports by default and writes nothing until it is turned off, and the window is where the operator makes that decision.

The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.

```csharp
public class PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [System\.Windows\.Threading\.DispatcherObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcherobject 'System\.Windows\.Threading\.DispatcherObject') → [System\.Windows\.DependencyObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.dependencyobject 'System\.Windows\.DependencyObject') → [System\.Windows\.Media\.Visual](https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.visual 'System\.Windows\.Media\.Visual') → [System\.Windows\.UIElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.uielement 'System\.Windows\.UIElement') → [System\.Windows\.FrameworkElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement 'System\.Windows\.FrameworkElement') → [System\.Windows\.Controls\.Control](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.control 'System\.Windows\.Controls\.Control') → [System\.Windows\.Controls\.ContentControl](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.contentcontrol 'System\.Windows\.Controls\.ContentControl') → [System\.Windows\.Window](https://learn.microsoft.com/en-us/dotnet/api/system.windows.window 'System\.Windows\.Window') → PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow

Implements [System\.Windows\.Markup\.IComponentConnector](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector 'System\.Windows\.Markup\.IComponentConnector')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow.PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow(DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelBuildingInformationUpdateOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_)'></a>

## PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow\(PostgreSQLBuildingModelBuildingInformationUpdateOptions, IEnumerable\<AdministrativeAreal2DReference\>\) Constructor

Initializes a new instance of the [PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow') class\.

```csharp
public PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow(DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelBuildingInformationUpdateOptions? postgreSQLBuildingModelBuildingInformationUpdateOptions, System.Collections.Generic.IEnumerable<DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference>? administrativeAreal2DReferences);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow.PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow(DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelBuildingInformationUpdateOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_).postgreSQLBuildingModelBuildingInformationUpdateOptions'></a>

`postgreSQLBuildingModelBuildingInformationUpdateOptions` [PostgreSQLBuildingModelBuildingInformationUpdateOptions](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelBuildingInformationUpdateOptions 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.PostgreSQLBuildingModelBuildingInformationUpdateOptions')

The options the controls are filled from\. When null the defaults are used\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow.PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow(DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelBuildingInformationUpdateOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_).administrativeAreal2DReferences'></a>

`administrativeAreal2DReferences` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The counties to choose from\. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own\. All of them are selected when the options carry no county set\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow.PostgreSQLBuildingModelBuildingInformationUpdateOptions'></a>

## PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow\.PostgreSQLBuildingModelBuildingInformationUpdateOptions Property

Gets the options the window holds\. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with\.

```csharp
public DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelBuildingInformationUpdateOptions PostgreSQLBuildingModelBuildingInformationUpdateOptions { get; }
```

#### Property Value
[PostgreSQLBuildingModelBuildingInformationUpdateOptions](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.PostgreSQLBuildingModelBuildingInformationUpdateOptions 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.PostgreSQLBuildingModelBuildingInformationUpdateOptions')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow.InitializeComponent()'></a>

## PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow\.InitializeComponent\(\) Method

InitializeComponent

```csharp
public void InitializeComponent();
```

Implements [InitializeComponent\(\)](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector.initializecomponent 'System\.Windows\.Markup\.IComponentConnector\.InitializeComponent')

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointFillGapsOptionsWindow'></a>

## PostgreSQLTerrainPointFillGapsOptionsWindow Class

Interaction logic for PostgreSQLTerrainPointFillGapsOptionsWindow\.xaml

Asks for the two settings that decide what a repair covers - the spacing the counties were sampled at, and the counties to measure. Every other option of the instance it was given is carried over untouched: the origin of the lattice and the tile size are what let this agree with the run it is repairing, and they are not settings to change from a dialog.

The spacing is the one that has to be right. Set finer than a county actually holds, every node in between reads as a gap and the repair turns into a densification of the whole country, so the equivalent spacing measured for each county is shown beside it.

The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.

```csharp
public class PostgreSQLTerrainPointFillGapsOptionsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [System\.Windows\.Threading\.DispatcherObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcherobject 'System\.Windows\.Threading\.DispatcherObject') → [System\.Windows\.DependencyObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.dependencyobject 'System\.Windows\.DependencyObject') → [System\.Windows\.Media\.Visual](https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.visual 'System\.Windows\.Media\.Visual') → [System\.Windows\.UIElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.uielement 'System\.Windows\.UIElement') → [System\.Windows\.FrameworkElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement 'System\.Windows\.FrameworkElement') → [System\.Windows\.Controls\.Control](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.control 'System\.Windows\.Controls\.Control') → [System\.Windows\.Controls\.ContentControl](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.contentcontrol 'System\.Windows\.Controls\.ContentControl') → [System\.Windows\.Window](https://learn.microsoft.com/en-us/dotnet/api/system.windows.window 'System\.Windows\.Window') → PostgreSQLTerrainPointFillGapsOptionsWindow

Implements [System\.Windows\.Markup\.IComponentConnector](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector 'System\.Windows\.Markup\.IComponentConnector')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointFillGapsOptionsWindow.PostgreSQLTerrainPointFillGapsOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointFillGapsOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.TerrainPointDensityResult_)'></a>

## PostgreSQLTerrainPointFillGapsOptionsWindow\(PostgreSQLTerrainPointFillGapsOptions, IEnumerable\<AdministrativeAreal2DReference\>, IEnumerable\<TerrainPointDensityResult\>\) Constructor

Initializes a new instance of the [PostgreSQLTerrainPointFillGapsOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointFillGapsOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.PostgreSQLTerrainPointFillGapsOptionsWindow') class\.

```csharp
public PostgreSQLTerrainPointFillGapsOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointFillGapsOptions? postgreSQLTerrainPointFillGapsOptions, System.Collections.Generic.IEnumerable<DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference>? administrativeAreal2DReferences, System.Collections.Generic.IEnumerable<DiGi.GIS.PostgreSQL.Classes.TerrainPointDensityResult>? terrainPointDensityResults=null);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointFillGapsOptionsWindow.PostgreSQLTerrainPointFillGapsOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointFillGapsOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.TerrainPointDensityResult_).postgreSQLTerrainPointFillGapsOptions'></a>

`postgreSQLTerrainPointFillGapsOptions` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointFillGapsOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlterrainpointfillgapsoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointFillGapsOptions')

The options the controls are filled from\. When null the defaults are used\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointFillGapsOptionsWindow.PostgreSQLTerrainPointFillGapsOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointFillGapsOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.TerrainPointDensityResult_).administrativeAreal2DReferences'></a>

`administrativeAreal2DReferences` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The counties to choose from\. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointFillGapsOptionsWindow.PostgreSQLTerrainPointFillGapsOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointFillGapsOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.TerrainPointDensityResult_).terrainPointDensityResults'></a>

`terrainPointDensityResults` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.TerrainPointDensityResult](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.terrainpointdensityresult 'DiGi\.GIS\.PostgreSQL\.Classes\.TerrainPointDensityResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The density measurements of the county partitions\. When provided, point count, density and equivalent spacing are shown for each county\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointFillGapsOptionsWindow.PostgreSQLTerrainPointFillGapsOptions'></a>

## PostgreSQLTerrainPointFillGapsOptionsWindow\.PostgreSQLTerrainPointFillGapsOptions Property

Gets the options the window holds\. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with\.

```csharp
public DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointFillGapsOptions PostgreSQLTerrainPointFillGapsOptions { get; }
```

#### Property Value
[DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointFillGapsOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlterrainpointfillgapsoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointFillGapsOptions')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointFillGapsOptionsWindow.InitializeComponent()'></a>

## PostgreSQLTerrainPointFillGapsOptionsWindow\.InitializeComponent\(\) Method

InitializeComponent

```csharp
public void InitializeComponent();
```

Implements [InitializeComponent\(\)](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector.initializecomponent 'System\.Windows\.Markup\.IComponentConnector\.InitializeComponent')

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointUpdateOptionsWindow'></a>

## PostgreSQLTerrainPointUpdateOptionsWindow Class

Interaction logic for PostgreSQLTerrainPointUpdateOptionsWindow\.xaml

Asks for the three settings that decide what a terrain point run costs and covers - the spacing of the sampling grid, whether points already stored are sampled again, and the counties to sample. Every other option of the instance it was given is carried over untouched, which is deliberate: the origin of the grid and the tile size are what let separate runs share their points, and they are not settings to change from a dialog.

The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.

```csharp
public class PostgreSQLTerrainPointUpdateOptionsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [System\.Windows\.Threading\.DispatcherObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcherobject 'System\.Windows\.Threading\.DispatcherObject') → [System\.Windows\.DependencyObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.dependencyobject 'System\.Windows\.DependencyObject') → [System\.Windows\.Media\.Visual](https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.visual 'System\.Windows\.Media\.Visual') → [System\.Windows\.UIElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.uielement 'System\.Windows\.UIElement') → [System\.Windows\.FrameworkElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement 'System\.Windows\.FrameworkElement') → [System\.Windows\.Controls\.Control](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.control 'System\.Windows\.Controls\.Control') → [System\.Windows\.Controls\.ContentControl](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.contentcontrol 'System\.Windows\.Controls\.ContentControl') → [System\.Windows\.Window](https://learn.microsoft.com/en-us/dotnet/api/system.windows.window 'System\.Windows\.Window') → PostgreSQLTerrainPointUpdateOptionsWindow

Implements [System\.Windows\.Markup\.IComponentConnector](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector 'System\.Windows\.Markup\.IComponentConnector')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointUpdateOptionsWindow.PostgreSQLTerrainPointUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointUpdateOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.TerrainPointDensityResult_)'></a>

## PostgreSQLTerrainPointUpdateOptionsWindow\(PostgreSQLTerrainPointUpdateOptions, IEnumerable\<AdministrativeAreal2DReference\>, IEnumerable\<TerrainPointDensityResult\>\) Constructor

Initializes a new instance of the [PostgreSQLTerrainPointUpdateOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointUpdateOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.PostgreSQLTerrainPointUpdateOptionsWindow') class\.

```csharp
public PostgreSQLTerrainPointUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointUpdateOptions? postgreSQLTerrainPointUpdateOptions, System.Collections.Generic.IEnumerable<DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference>? administrativeAreal2DReferences, System.Collections.Generic.IEnumerable<DiGi.GIS.PostgreSQL.Classes.TerrainPointDensityResult>? terrainPointDensityResults=null);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointUpdateOptionsWindow.PostgreSQLTerrainPointUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointUpdateOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.TerrainPointDensityResult_).postgreSQLTerrainPointUpdateOptions'></a>

`postgreSQLTerrainPointUpdateOptions` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointUpdateOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlterrainpointupdateoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointUpdateOptions')

The options the controls are filled from\. When null the defaults are used\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointUpdateOptionsWindow.PostgreSQLTerrainPointUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointUpdateOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.TerrainPointDensityResult_).administrativeAreal2DReferences'></a>

`administrativeAreal2DReferences` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The counties to choose from\. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointUpdateOptionsWindow.PostgreSQLTerrainPointUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointUpdateOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.TerrainPointDensityResult_).terrainPointDensityResults'></a>

`terrainPointDensityResults` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.TerrainPointDensityResult](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.terrainpointdensityresult 'DiGi\.GIS\.PostgreSQL\.Classes\.TerrainPointDensityResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The density measurements of the county partitions\. When provided, point count, density and equivalent spacing are shown for each county\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointUpdateOptionsWindow.PostgreSQLTerrainPointUpdateOptions'></a>

## PostgreSQLTerrainPointUpdateOptionsWindow\.PostgreSQLTerrainPointUpdateOptions Property

Gets the options the window holds\. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with\.

```csharp
public DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointUpdateOptions PostgreSQLTerrainPointUpdateOptions { get; }
```

#### Property Value
[DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointUpdateOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlterrainpointupdateoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointUpdateOptions')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointUpdateOptionsWindow.InitializeComponent()'></a>

## PostgreSQLTerrainPointUpdateOptionsWindow\.InitializeComponent\(\) Method

InitializeComponent

```csharp
public void InitializeComponent();
```

Implements [InitializeComponent\(\)](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector.initializecomponent 'System\.Windows\.Markup\.IComponentConnector\.InitializeComponent')

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUpdateOccupancyOptionsWindow'></a>

## PostgreSQLUpdateOccupancyOptionsWindow Class

Interaction logic for PostgreSQLUpdateOccupancyOptionsWindow\.xaml

Asks which side of the occupancy update runs - the administrative roll-up, the per-building distribution, or both - whether the stored rows are cleared first, and which counties the building side is limited to. The roll-up is a sum over the whole hierarchy and stays nationwide whatever counties are selected.

No county selected means every county, which is what the task has always meant; with counties selected a clear removes only their buildings' rows, so the rest of the table is left as it stands.

The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.

```csharp
public class PostgreSQLUpdateOccupancyOptionsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [System\.Windows\.Threading\.DispatcherObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcherobject 'System\.Windows\.Threading\.DispatcherObject') → [System\.Windows\.DependencyObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.dependencyobject 'System\.Windows\.DependencyObject') → [System\.Windows\.Media\.Visual](https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.visual 'System\.Windows\.Media\.Visual') → [System\.Windows\.UIElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.uielement 'System\.Windows\.UIElement') → [System\.Windows\.FrameworkElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement 'System\.Windows\.FrameworkElement') → [System\.Windows\.Controls\.Control](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.control 'System\.Windows\.Controls\.Control') → [System\.Windows\.Controls\.ContentControl](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.contentcontrol 'System\.Windows\.Controls\.ContentControl') → [System\.Windows\.Window](https://learn.microsoft.com/en-us/dotnet/api/system.windows.window 'System\.Windows\.Window') → PostgreSQLUpdateOccupancyOptionsWindow

Implements [System\.Windows\.Markup\.IComponentConnector](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector 'System\.Windows\.Markup\.IComponentConnector')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUpdateOccupancyOptionsWindow.PostgreSQLUpdateOccupancyOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLUpdateOccupancyOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_)'></a>

## PostgreSQLUpdateOccupancyOptionsWindow\(PostgreSQLUpdateOccupancyOptions, IEnumerable\<AdministrativeAreal2DReference\>\) Constructor

Initializes a new instance of the [PostgreSQLUpdateOccupancyOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUpdateOccupancyOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.PostgreSQLUpdateOccupancyOptionsWindow') class\.

```csharp
public PostgreSQLUpdateOccupancyOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLUpdateOccupancyOptions? postgreSQLUpdateOccupancyOptions, System.Collections.Generic.IEnumerable<DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference>? administrativeAreal2DReferences);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUpdateOccupancyOptionsWindow.PostgreSQLUpdateOccupancyOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLUpdateOccupancyOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_).postgreSQLUpdateOccupancyOptions'></a>

`postgreSQLUpdateOccupancyOptions` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLUpdateOccupancyOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlupdateoccupancyoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLUpdateOccupancyOptions')

The options the controls are filled from\. When null the defaults are used\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUpdateOccupancyOptionsWindow.PostgreSQLUpdateOccupancyOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLUpdateOccupancyOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_).administrativeAreal2DReferences'></a>

`administrativeAreal2DReferences` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The counties to choose from\. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUpdateOccupancyOptionsWindow.PostgreSQLUpdateOccupancyOptions'></a>

## PostgreSQLUpdateOccupancyOptionsWindow\.PostgreSQLUpdateOccupancyOptions Property

Gets the options the window holds\. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with\.

```csharp
public DiGi.GIS.PostgreSQL.Classes.PostgreSQLUpdateOccupancyOptions PostgreSQLUpdateOccupancyOptions { get; }
```

#### Property Value
[DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLUpdateOccupancyOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlupdateoccupancyoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLUpdateOccupancyOptions')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUpdateOccupancyOptionsWindow.InitializeComponent()'></a>

## PostgreSQLUpdateOccupancyOptionsWindow\.InitializeComponent\(\) Method

InitializeComponent

```csharp
public void InitializeComponent();
```

Implements [InitializeComponent\(\)](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector.initializecomponent 'System\.Windows\.Markup\.IComponentConnector\.InitializeComponent')

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUserCreateOptionsWindow'></a>

## PostgreSQLUserCreateOptionsWindow Class

Interaction logic for PostgreSQLUserCreateOptionsWindow\.xaml

Asks for the user to be created: the email it is keyed by, its optional name, the password it is
            authenticated with and the permission level it is granted.

The email and the password are each asked for twice. Neither can be corrected afterwards by anything
            this application offers - the email is the natural key of the row, and the password is never stored in a form
            anything can read back - so a typo in either produces an account nobody can use.

Unlike the other options windows this one carries no options object. The password reaches it as text,
            and an options instance would keep that text alive on the task for the life of the process; the values live
            here instead, on a window the task creates, reads and closes within one run.

```csharp
public class PostgreSQLUserCreateOptionsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [System\.Windows\.Threading\.DispatcherObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcherobject 'System\.Windows\.Threading\.DispatcherObject') → [System\.Windows\.DependencyObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.dependencyobject 'System\.Windows\.DependencyObject') → [System\.Windows\.Media\.Visual](https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.visual 'System\.Windows\.Media\.Visual') → [System\.Windows\.UIElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.uielement 'System\.Windows\.UIElement') → [System\.Windows\.FrameworkElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement 'System\.Windows\.FrameworkElement') → [System\.Windows\.Controls\.Control](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.control 'System\.Windows\.Controls\.Control') → [System\.Windows\.Controls\.ContentControl](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.contentcontrol 'System\.Windows\.Controls\.ContentControl') → [System\.Windows\.Window](https://learn.microsoft.com/en-us/dotnet/api/system.windows.window 'System\.Windows\.Window') → PostgreSQLUserCreateOptionsWindow

Implements [System\.Windows\.Markup\.IComponentConnector](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector 'System\.Windows\.Markup\.IComponentConnector')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUserCreateOptionsWindow.PostgreSQLUserCreateOptionsWindow()'></a>

## PostgreSQLUserCreateOptionsWindow\(\) Constructor

Initializes a new instance of the [PostgreSQLUserCreateOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUserCreateOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.PostgreSQLUserCreateOptionsWindow') class\.

```csharp
public PostgreSQLUserCreateOptionsWindow();
```
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUserCreateOptionsWindow.Email'></a>

## PostgreSQLUserCreateOptionsWindow\.Email Property

Gets the email of the user to create\. It carries the value of the control only once the dialog has been closed with OK; until then, and after a cancellation, it is null\.

```csharp
public string? Email { get; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUserCreateOptionsWindow.FirstName'></a>

## PostgreSQLUserCreateOptionsWindow\.FirstName Property

Gets the optional first name of the user to create, or null when the field was left blank\.

```csharp
public string? FirstName { get; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUserCreateOptionsWindow.LastName'></a>

## PostgreSQLUserCreateOptionsWindow\.LastName Property

Gets the optional last name of the user to create, or null when the field was left blank\.

```csharp
public string? LastName { get; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUserCreateOptionsWindow.Password'></a>

## PostgreSQLUserCreateOptionsWindow\.Password Property

Gets the plain text password the credential is to be derived from\. It exists only between the dialog being closed with OK and the credential being derived, and is never written to the database or to a log\.

```csharp
public string? Password { get; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUserCreateOptionsWindow.UserLevel'></a>

## PostgreSQLUserCreateOptionsWindow\.UserLevel Property

Gets the permission level to grant the user\.

```csharp
public DiGi.User.Enums.UserLevel UserLevel { get; }
```

#### Property Value
[DiGi\.User\.Enums\.UserLevel](https://learn.microsoft.com/en-us/dotnet/api/digi.user.enums.userlevel 'DiGi\.User\.Enums\.UserLevel')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUserCreateOptionsWindow.InitializeComponent()'></a>

## PostgreSQLUserCreateOptionsWindow\.InitializeComponent\(\) Method

InitializeComponent

```csharp
public void InitializeComponent();
```

Implements [InitializeComponent\(\)](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector.initializecomponent 'System\.Windows\.Markup\.IComponentConnector\.InitializeComponent')

<a name='DiGi.GIS.PostgreSQL.UI.Windows.YearBuiltPredictionsOptionsWindow'></a>

## YearBuiltPredictionsOptionsWindow Class

Interaction logic for YearBuiltPredictionsOptionsWindow\.xaml

Asks for the scope of one Year Built prediction run and nothing else: the counties, where its imagery goes, which interpreter runs the detector, and how hard the export leans on the server.

<b>A tray run has one shape - the full six step flow - and it writes.</b> The steps are not offered because they are not a choice here: DiGi-Organization/DiGi.GIS.YOLO.UI#8 made export, detector, detection write, score, history write and column write a single run per county precisely so that no step could be left out of sequence, and it decided that the granular flags stay on the options class and the console app for hand-driven diagnostics while the tray driven flow collapses them. Eight checkboxes offered two hundred and fifty six combinations, of which three were real - and the rest failed late, after the half hour of export and the hour and a half of inference had already been paid for. The OK handler writes all six on, so the run the operator gets is the run the standing recipe describes.

<b>What settles the model is deliberately not here either.</b> The weights, the confidence threshold, the year range and the radiuses all decide what the regressor is handed, and a value that disagrees with what it was trained on is, for the year range and the radiuses, checked against the model's stated trained contract before the run reads a county - narrower is refused, wider is warned (DiGi-Organization/DiGi.GIS.ML#6) - while the weights and the confidence threshold still score without failing. They belong to the deployment and to the options file rather than to a dialog opened before every run. The working directory and the two batch sizes are out for a different reason: none of the three is a choice - see the comment in the OK handler.

The scratch cleanup is the one flag that survives, because its reason is about this run rather than about the sequence: a cancelled county is cleaned, so a run that is meant to be interrupted has to be able to say so beforehand.

The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were, and every member the window has no control for carries through untouched.

```csharp
public class YearBuiltPredictionsOptionsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [System\.Windows\.Threading\.DispatcherObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcherobject 'System\.Windows\.Threading\.DispatcherObject') → [System\.Windows\.DependencyObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.dependencyobject 'System\.Windows\.DependencyObject') → [System\.Windows\.Media\.Visual](https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.visual 'System\.Windows\.Media\.Visual') → [System\.Windows\.UIElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.uielement 'System\.Windows\.UIElement') → [System\.Windows\.FrameworkElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement 'System\.Windows\.FrameworkElement') → [System\.Windows\.Controls\.Control](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.control 'System\.Windows\.Controls\.Control') → [System\.Windows\.Controls\.ContentControl](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.contentcontrol 'System\.Windows\.Controls\.ContentControl') → [System\.Windows\.Window](https://learn.microsoft.com/en-us/dotnet/api/system.windows.window 'System\.Windows\.Window') → YearBuiltPredictionsOptionsWindow

Implements [System\.Windows\.Markup\.IComponentConnector](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector 'System\.Windows\.Markup\.IComponentConnector')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Windows.YearBuiltPredictionsOptionsWindow.YearBuiltPredictionsOptionsWindow(DiGi.GIS.YOLO.UI.Classes.YearBuiltPredictionPipelineOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,System.Func_System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.Dictionary_int,double___)'></a>

## YearBuiltPredictionsOptionsWindow\(YearBuiltPredictionPipelineOptions, IEnumerable\<AdministrativeAreal2DReference\>, Func\<CancellationToken,Task\<Dictionary\<int,double\>\>\>\) Constructor

Initializes a new instance of the [YearBuiltPredictionsOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.YearBuiltPredictionsOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.YearBuiltPredictionsOptionsWindow') class\.

```csharp
public YearBuiltPredictionsOptionsWindow(DiGi.GIS.YOLO.UI.Classes.YearBuiltPredictionPipelineOptions? yearBuiltPredictionPipelineOptions, System.Collections.Generic.IEnumerable<DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference>? administrativeAreal2DReferences, System.Func<System.Threading.CancellationToken,System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<int,double>?>>? coverageFactorsAsync=null);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.YearBuiltPredictionsOptionsWindow.YearBuiltPredictionsOptionsWindow(DiGi.GIS.YOLO.UI.Classes.YearBuiltPredictionPipelineOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,System.Func_System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.Dictionary_int,double___).yearBuiltPredictionPipelineOptions'></a>

`yearBuiltPredictionPipelineOptions` [DiGi\.GIS\.YOLO\.UI\.Classes\.YearBuiltPredictionPipelineOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.classes.yearbuiltpredictionpipelineoptions 'DiGi\.GIS\.YOLO\.UI\.Classes\.YearBuiltPredictionPipelineOptions')

The options the controls are filled from\. When null the defaults are used\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.YearBuiltPredictionsOptionsWindow.YearBuiltPredictionsOptionsWindow(DiGi.GIS.YOLO.UI.Classes.YearBuiltPredictionPipelineOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,System.Func_System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.Dictionary_int,double___).administrativeAreal2DReferences'></a>

`administrativeAreal2DReferences` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The counties to choose from\. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own \- a run names every part of a county so that each written row is filed under the part its reference belongs to\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.YearBuiltPredictionsOptionsWindow.YearBuiltPredictionsOptionsWindow(DiGi.GIS.YOLO.UI.Classes.YearBuiltPredictionPipelineOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,System.Func_System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.Dictionary_int,double___).coverageFactorsAsync'></a>

`coverageFactorsAsync` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Collections\.Generic\.Dictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

Reads the predicted year built coverage of each county part, keyed by part id, for the markers\. Started here and never awaited before the window opens: the county list is shown at once and its markers are filled in when the read completes, whenever that is\. Closing the window cancels it\. When null no coverage is read and nothing is marked\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Windows.YearBuiltPredictionsOptionsWindow.YearBuiltPredictionPipelineOptions'></a>

## YearBuiltPredictionsOptionsWindow\.YearBuiltPredictionPipelineOptions Property

Gets the options the window holds\. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with\.

```csharp
public DiGi.GIS.YOLO.UI.Classes.YearBuiltPredictionPipelineOptions YearBuiltPredictionPipelineOptions { get; }
```

#### Property Value
[DiGi\.GIS\.YOLO\.UI\.Classes\.YearBuiltPredictionPipelineOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.classes.yearbuiltpredictionpipelineoptions 'DiGi\.GIS\.YOLO\.UI\.Classes\.YearBuiltPredictionPipelineOptions')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Windows.YearBuiltPredictionsOptionsWindow.InitializeComponent()'></a>

## YearBuiltPredictionsOptionsWindow\.InitializeComponent\(\) Method

InitializeComponent

```csharp
public void InitializeComponent();
```

Implements [InitializeComponent\(\)](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector.initializecomponent 'System\.Windows\.Markup\.IComponentConnector\.InitializeComponent')

<a name='DiGi.GIS.PostgreSQL.UI.Windows.YearBuiltPredictionsOptionsWindow.SetCoverageFactors(System.Collections.Generic.IReadOnlyDictionary_int,double_)'></a>

## YearBuiltPredictionsOptionsWindow\.SetCoverageFactors\(IReadOnlyDictionary\<int,double\>\) Method

Sets the predicted year built coverage the county markers are drawn from, and redraws the county list in place\.

The selection and the scroll position are kept: the coverage normally arrives after the window is open, possibly while counties are being picked. Null means the coverage could not be read, which marks nothing and says so - never a false "not run".

```csharp
public void SetCoverageFactors(System.Collections.Generic.IReadOnlyDictionary<int,double>? coverageFactors);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.YearBuiltPredictionsOptionsWindow.SetCoverageFactors(System.Collections.Generic.IReadOnlyDictionary_int,double_).coverageFactors'></a>

`coverageFactors` [System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')

The share of each county part's buildings carrying a predicted year built, keyed by part id, or null when it could not be read\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.YOLOTrainingOptionsWindow'></a>

## YOLOTrainingOptionsWindow Class

Interaction logic for YOLOTrainingOptionsWindow\.xaml

Asks for the inputs of one YOLO detector training run: the scenario, the counties, the dataset folder, the start weights, the training hyperparameters, where the run is written, the interpreter, the steps and the weights the new detector is gated against.

<b>The scenario sets defaults, not rules.</b> Switching it resets the start weights and the epoch ceiling to that scenario's defaults ([YOLOTrainingRunOptions\(YOLOTrainingScenario, string, YOLOTrainingRunOptions\)](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Create.YOLOTrainingRunOptions(DiGi.GIS.PostgreSQL.UI.Enums.YOLOTrainingScenario,string,DiGi.GIS.YOLO.UI.Classes.YOLOTrainingRunOptions) 'DiGi\.GIS\.PostgreSQL\.UI\.Create\.YOLOTrainingRunOptions\(DiGi\.GIS\.PostgreSQL\.UI\.Enums\.YOLOTrainingScenario, string, DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingRunOptions\)')) and leaves every other control as it is; on OK it also decides whether the dataset folder is appended to (Re-train) or must be new (Start from yolo26x.pt).

<b>An interrupted run is offered for resume.</b> When the run name and project directory name a run whose folder holds an unfinished `weights\last.pt` and no completed `<RunName>.pt`, the dialog shows the checkpoint's epoch and ceiling and offers to resume it. Ticking [DiGi\.GIS\.YOLO\.UI\.Enums\.YOLOTrainingStep\.Train](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.enums.yolotrainingstep.train 'DiGi\.GIS\.YOLO\.UI\.Enums\.YOLOTrainingStep\.Train') is still required; the box locks what a resume cannot change - the hyperparameters, the start weights and the dataset build - because the checkpoint restores them, and the epoch ceiling is fixed by the checkpoint.

<b>Stalls are handled by the runner.</b>`Automatic resumes` and `Stall limit` set [DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingRunOptions\.AutoResumeCount](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.classes.yolotrainingrunoptions.autoresumecount 'DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingRunOptions\.AutoResumeCount') and [DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingRunOptions\.InactivityTimeout](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.classes.yolotrainingrunoptions.inactivitytimeout 'DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingRunOptions\.InactivityTimeout'): a training that stalls or crashes is continued from its own `last.pt` that many times, and a stall is what a silent training becomes after the limit. Both are enabled only with the Training step, and an empty stall limit keeps the runner's default of 15 minutes.

<b>What is not offered is the runner's to decide.</b> The dataset's confidence threshold, split, label check sizes, legacy cut-off and request sizes carry through the copy untouched at the values its README describes; they shape what the detector is trained on, and a control opened before every run invites changing them between runs that are then compared.

The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were, and every member the window has no control for carries through untouched.

```csharp
public class YOLOTrainingOptionsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [System\.Windows\.Threading\.DispatcherObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcherobject 'System\.Windows\.Threading\.DispatcherObject') → [System\.Windows\.DependencyObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.dependencyobject 'System\.Windows\.DependencyObject') → [System\.Windows\.Media\.Visual](https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.visual 'System\.Windows\.Media\.Visual') → [System\.Windows\.UIElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.uielement 'System\.Windows\.UIElement') → [System\.Windows\.FrameworkElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement 'System\.Windows\.FrameworkElement') → [System\.Windows\.Controls\.Control](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.control 'System\.Windows\.Controls\.Control') → [System\.Windows\.Controls\.ContentControl](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.contentcontrol 'System\.Windows\.Controls\.ContentControl') → [System\.Windows\.Window](https://learn.microsoft.com/en-us/dotnet/api/system.windows.window 'System\.Windows\.Window') → YOLOTrainingOptionsWindow

Implements [System\.Windows\.Markup\.IComponentConnector](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector 'System\.Windows\.Markup\.IComponentConnector')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Windows.YOLOTrainingOptionsWindow.YOLOTrainingOptionsWindow(DiGi.GIS.PostgreSQL.UI.Enums.YOLOTrainingScenario,DiGi.GIS.YOLO.UI.Classes.YOLOTrainingRunOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,string)'></a>

## YOLOTrainingOptionsWindow\(YOLOTrainingScenario, YOLOTrainingRunOptions, IEnumerable\<AdministrativeAreal2DReference\>, string\) Constructor

Initializes a new instance of the [YOLOTrainingOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.YOLOTrainingOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.YOLOTrainingOptionsWindow') class\.

```csharp
public YOLOTrainingOptionsWindow(DiGi.GIS.PostgreSQL.UI.Enums.YOLOTrainingScenario yOLOTrainingScenario, DiGi.GIS.YOLO.UI.Classes.YOLOTrainingRunOptions? yOLOTrainingRunOptions, System.Collections.Generic.IEnumerable<DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference>? administrativeAreal2DReferences, string? consoleAppPath);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.YOLOTrainingOptionsWindow.YOLOTrainingOptionsWindow(DiGi.GIS.PostgreSQL.UI.Enums.YOLOTrainingScenario,DiGi.GIS.YOLO.UI.Classes.YOLOTrainingRunOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,string).yOLOTrainingScenario'></a>

`yOLOTrainingScenario` [YOLOTrainingScenario](DiGi.GIS.PostgreSQL.UI.Enums.md#DiGi.GIS.PostgreSQL.UI.Enums.YOLOTrainingScenario 'DiGi\.GIS\.PostgreSQL\.UI\.Enums\.YOLOTrainingScenario')

The scenario the dialog opens with\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.YOLOTrainingOptionsWindow.YOLOTrainingOptionsWindow(DiGi.GIS.PostgreSQL.UI.Enums.YOLOTrainingScenario,DiGi.GIS.YOLO.UI.Classes.YOLOTrainingRunOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,string).yOLOTrainingRunOptions'></a>

`yOLOTrainingRunOptions` [DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingRunOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.classes.yolotrainingrunoptions 'DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingRunOptions')

The options the controls are filled from \- the previous run's, which are copied rather than changed\. When null the defaults of [yOLOTrainingScenario](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.YOLOTrainingOptionsWindow.YOLOTrainingOptionsWindow(DiGi.GIS.PostgreSQL.UI.Enums.YOLOTrainingScenario,DiGi.GIS.YOLO.UI.Classes.YOLOTrainingRunOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,string).yOLOTrainingScenario 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.YOLOTrainingOptionsWindow\.YOLOTrainingOptionsWindow\(DiGi\.GIS\.PostgreSQL\.UI\.Enums\.YOLOTrainingScenario, DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingRunOptions, System\.Collections\.Generic\.IEnumerable\<DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference\>, string\)\.yOLOTrainingScenario') are used\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.YOLOTrainingOptionsWindow.YOLOTrainingOptionsWindow(DiGi.GIS.PostgreSQL.UI.Enums.YOLOTrainingScenario,DiGi.GIS.YOLO.UI.Classes.YOLOTrainingRunOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,string).administrativeAreal2DReferences'></a>

`administrativeAreal2DReferences` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The counties to choose from\. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.YOLOTrainingOptionsWindow.YOLOTrainingOptionsWindow(DiGi.GIS.PostgreSQL.UI.Enums.YOLOTrainingScenario,DiGi.GIS.YOLO.UI.Classes.YOLOTrainingRunOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,string).consoleAppPath'></a>

`consoleAppPath` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The full path of the runner, which the default weights are named relative to\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Windows.YOLOTrainingOptionsWindow.YOLOTrainingRunOptions'></a>

## YOLOTrainingOptionsWindow\.YOLOTrainingRunOptions Property

Gets the options the window holds\. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with\.

```csharp
public DiGi.GIS.YOLO.UI.Classes.YOLOTrainingRunOptions YOLOTrainingRunOptions { get; }
```

#### Property Value
[DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingRunOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.classes.yolotrainingrunoptions 'DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingRunOptions')

<a name='DiGi.GIS.PostgreSQL.UI.Windows.YOLOTrainingOptionsWindow.YOLOTrainingScenario'></a>

## YOLOTrainingOptionsWindow\.YOLOTrainingScenario Property

Gets the scenario the window holds\. It is the one chosen only once the dialog has been closed with OK; until then, and after a cancellation, it is the one it was opened with\.

```csharp
public DiGi.GIS.PostgreSQL.UI.Enums.YOLOTrainingScenario YOLOTrainingScenario { get; }
```

#### Property Value
[YOLOTrainingScenario](DiGi.GIS.PostgreSQL.UI.Enums.md#DiGi.GIS.PostgreSQL.UI.Enums.YOLOTrainingScenario 'DiGi\.GIS\.PostgreSQL\.UI\.Enums\.YOLOTrainingScenario')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Windows.YOLOTrainingOptionsWindow.ApplyResumeMode()'></a>

## YOLOTrainingOptionsWindow\.ApplyResumeMode\(\) Method

Locks what a resume cannot change \- the hyperparameters, the start weights and the dataset build \- and shows the checkpoint's values while the resume box is ticked; unticking it puts the operator's own values back\. Device, interpreter, working directory, gate weights and the steps after training stay editable\.

```csharp
private void ApplyResumeMode();
```

<a name='DiGi.GIS.PostgreSQL.UI.Windows.YOLOTrainingOptionsWindow.DirectoryRefusal(string,string,bool)'></a>

## YOLOTrainingOptionsWindow\.DirectoryRefusal\(string, string, bool\) Method

Returns the warning a relative directory is refused with, or null when every directory it names is absolute: the dataset directory always, and the project directory when training is on\.

```csharp
internal static string? DirectoryRefusal(string? datasetDirectory, string? projectDirectory, bool train);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.YOLOTrainingOptionsWindow.DirectoryRefusal(string,string,bool).datasetDirectory'></a>

`datasetDirectory` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The dataset directory, when given\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.YOLOTrainingOptionsWindow.DirectoryRefusal(string,string,bool).projectDirectory'></a>

`projectDirectory` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The project directory, when training is on\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.YOLOTrainingOptionsWindow.DirectoryRefusal(string,string,bool).train'></a>

`train` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether the run trains, so the project directory is in play\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The warning to show, or null when the directories are absolute\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.YOLOTrainingOptionsWindow.EvaluateInterruptedRun()'></a>

## YOLOTrainingOptionsWindow\.EvaluateInterruptedRun\(\) Method

Re\-reads whether the run named by the run name and project directory boxes is an interrupted one, and shows or hides the resume offer accordingly\. The checkpoint is read through [InterruptedYOLOTrainingRun\(string, string, string, string, CancellationToken\)](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Query.InterruptedYOLOTrainingRun(string,string,string,string,System.Threading.CancellationToken) 'DiGi\.GIS\.PostgreSQL\.UI\.Query\.InterruptedYOLOTrainingRun\(string, string, string, string, System\.Threading\.CancellationToken\)'), so the offer names the epoch and ceiling the checkpoint holds\.

```csharp
private void EvaluateInterruptedRun();
```

<a name='DiGi.GIS.PostgreSQL.UI.Windows.YOLOTrainingOptionsWindow.InitializeComponent()'></a>

## YOLOTrainingOptionsWindow\.InitializeComponent\(\) Method

InitializeComponent

```csharp
public void InitializeComponent();
```

Implements [InitializeComponent\(\)](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector.initializecomponent 'System\.Windows\.Markup\.IComponentConnector\.InitializeComponent')

<a name='DiGi.GIS.PostgreSQL.UI.Windows.YOLOTrainingOptionsWindow.SetStallInputsEnabled()'></a>

## YOLOTrainingOptionsWindow\.SetStallInputsEnabled\(\) Method

Enables the automatic\-resume count and the stall limit only when the training step is ticked: without training there is nothing that can stall or be resumed, and the runner would ignore both\.

```csharp
private void SetStallInputsEnabled();
```