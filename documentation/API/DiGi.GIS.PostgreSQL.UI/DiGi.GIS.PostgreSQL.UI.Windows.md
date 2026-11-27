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
public MainWindow(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager? gISPostgreSQLConverterManager, DiGi.User.PostgreSQL.Classes.UserPostgreSQLConverterManager? userPostgreSQLConverterManager, DiGi.GIS.WebAPI.Classes.GISWebAPIManager? gISWebAPIManager, System.Nullable<DiGi.GIS.PostgreSQL.UI.Enums.Mode> mode=null);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.MainWindow.MainWindow(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager,DiGi.User.PostgreSQL.Classes.UserPostgreSQLConverterManager,DiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Nullable_DiGi.GIS.PostgreSQL.UI.Enums.Mode_).gISPostgreSQLConverterManager'></a>

`gISPostgreSQLConverterManager` [DiGi\.GIS\.PostgreSQL\.Classes\.GISPostgreSQLConverterManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.gispostgresqlconvertermanager 'DiGi\.GIS\.PostgreSQL\.Classes\.GISPostgreSQLConverterManager')

The manager responsible for GIS PostgreSQL conversion processes\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.MainWindow.MainWindow(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager,DiGi.User.PostgreSQL.Classes.UserPostgreSQLConverterManager,DiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Nullable_DiGi.GIS.PostgreSQL.UI.Enums.Mode_).userPostgreSQLConverterManager'></a>

`userPostgreSQLConverterManager` [DiGi\.User\.PostgreSQL\.Classes\.UserPostgreSQLConverterManager](https://learn.microsoft.com/en-us/dotnet/api/digi.user.postgresql.classes.userpostgresqlconvertermanager 'DiGi\.User\.PostgreSQL\.Classes\.UserPostgreSQLConverterManager')

The manager holding the converter for the user database, or null where no User\_PostgreSQL\_Main\.conf names one\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.MainWindow.MainWindow(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager,DiGi.User.PostgreSQL.Classes.UserPostgreSQLConverterManager,DiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Nullable_DiGi.GIS.PostgreSQL.UI.Enums.Mode_).gISWebAPIManager'></a>

`gISWebAPIManager` [DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.giswebapimanager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

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

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingImportOptionsWindow'></a>

## OpenBuildingImportOptionsWindow Class

Interaction logic for OpenBuildingImportOptionsWindow\.xaml

Asks what decides a run of the open building import (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#38): which source files the release came in, whether the scope is a county list or a bounding box, and the release metadata the register stores. The dataset itself is not asked - the row the operator started fixes it, so a row named for one release cannot import another.

The SHA-256 is never typed. The field is shown disabled with the caption saying when it is computed: the wrapper hashes the source files after this dialog closes and before the run starts.

Exactly one scope form has to be filled - a county selection or the four coordinates of an EPSG:2180 box - and OK still refuses when either is missing or both are given, because both is ambiguous and neither has no run. What the core task enforces is asked here first through [OpenBuildingImportOptionsMessage\(PostgreSQLOpenBuildingImportTaskOptions\)](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Query.OpenBuildingImportOptionsMessage(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOpenBuildingImportTaskOptions) 'DiGi\.GIS\.PostgreSQL\.UI\.Query\.OpenBuildingImportOptionsMessage\(DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOpenBuildingImportTaskOptions\)'), so the operator is told while the dialog is open rather than from a failed run's log.

The settings can be exported to a JSON file and imported again (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#58), so a run repeated after the tray was restarted is not typed again. Importing only fills the controls: nothing runs until OK, and OK validates the imported values like typed ones. A file written for another dataset is refused, because the row fixes the dataset.

The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.

```csharp
public class OpenBuildingImportOptionsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [System\.Windows\.Threading\.DispatcherObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcherobject 'System\.Windows\.Threading\.DispatcherObject') → [System\.Windows\.DependencyObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.dependencyobject 'System\.Windows\.DependencyObject') → [System\.Windows\.Media\.Visual](https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.visual 'System\.Windows\.Media\.Visual') → [System\.Windows\.UIElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.uielement 'System\.Windows\.UIElement') → [System\.Windows\.FrameworkElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement 'System\.Windows\.FrameworkElement') → [System\.Windows\.Controls\.Control](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.control 'System\.Windows\.Controls\.Control') → [System\.Windows\.Controls\.ContentControl](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.contentcontrol 'System\.Windows\.Controls\.ContentControl') → [System\.Windows\.Window](https://learn.microsoft.com/en-us/dotnet/api/system.windows.window 'System\.Windows\.Window') → OpenBuildingImportOptionsWindow

Implements [System\.Windows\.Markup\.IComponentConnector](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector 'System\.Windows\.Markup\.IComponentConnector')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingImportOptionsWindow.OpenBuildingImportOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOpenBuildingImportTaskOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_)'></a>

## OpenBuildingImportOptionsWindow\(PostgreSQLOpenBuildingImportTaskOptions, IEnumerable\<AdministrativeAreal2DReference\>\) Constructor

Initializes a new instance of the [OpenBuildingImportOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingImportOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.OpenBuildingImportOptionsWindow') class\.

```csharp
public OpenBuildingImportOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOpenBuildingImportTaskOptions? postgreSQLOpenBuildingImportTaskOptions, System.Collections.Generic.IEnumerable<DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference>? administrativeAreal2DReferences);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingImportOptionsWindow.OpenBuildingImportOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOpenBuildingImportTaskOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_).postgreSQLOpenBuildingImportTaskOptions'></a>

`postgreSQLOpenBuildingImportTaskOptions` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOpenBuildingImportTaskOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlopenbuildingimporttaskoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOpenBuildingImportTaskOptions')

The options the controls are filled from\. When null the defaults are used, and the dataset row then reports that no dataset was chosen\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingImportOptionsWindow.OpenBuildingImportOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOpenBuildingImportTaskOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_).administrativeAreal2DReferences'></a>

`administrativeAreal2DReferences` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The county parts to choose from\. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own\. Nothing is preselected: the scope is required, and a dialog that opens with every county ticked invites a national run as the default\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingImportOptionsWindow.PostgreSQLOpenBuildingImportTaskOptions'></a>

## OpenBuildingImportOptionsWindow\.PostgreSQLOpenBuildingImportTaskOptions Property

Gets the options the window holds\. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with\.

```csharp
public DiGi.GIS.PostgreSQL.Classes.PostgreSQLOpenBuildingImportTaskOptions PostgreSQLOpenBuildingImportTaskOptions { get; }
```

#### Property Value
[DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOpenBuildingImportTaskOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlopenbuildingimporttaskoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOpenBuildingImportTaskOptions')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingImportOptionsWindow.AddSourcePaths(System.Collections.Generic.IEnumerable_string_)'></a>

## OpenBuildingImportOptionsWindow\.AddSourcePaths\(IEnumerable\<string\>\) Method

Adds paths to the source list, skipping the ones it already holds, and keeps the list in the ordinal order the options keep\.

```csharp
private void AddSourcePaths(System.Collections.Generic.IEnumerable<string> paths_Added);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingImportOptionsWindow.AddSourcePaths(System.Collections.Generic.IEnumerable_string_).paths_Added'></a>

`paths_Added` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The paths to add\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingImportOptionsWindow.CreateOptions(bool)'></a>

## OpenBuildingImportOptionsWindow\.CreateOptions\(bool\) Method

Reads the controls into a new copy of the window's options, telling the operator what cannot be read\.

```csharp
private DiGi.GIS.PostgreSQL.Classes.PostgreSQLOpenBuildingImportTaskOptions? CreateOptions(bool complete);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingImportOptionsWindow.CreateOptions(bool).complete'></a>

`complete` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

True for OK: a scope is required\. False for an export: an empty county selection or four blank coordinates are kept as no scope\.

#### Returns
[DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOpenBuildingImportTaskOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlopenbuildingimporttaskoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOpenBuildingImportTaskOptions')  
The options the controls describe, or null when a value could not be read; the reason has been shown\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingImportOptionsWindow.DisplayName(string)'></a>

## OpenBuildingImportOptionsWindow\.DisplayName\(string\) Method

Maps a dataset key to the caption shown for it on the window \(DiGi\-Organization/DiGi\.GIS\.PostgreSQL\.UI\#38\)\.

```csharp
private static string DisplayName(string? datasetKey);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingImportOptionsWindow.DisplayName(string).datasetKey'></a>

`datasetKey` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The dataset key the row fixed, or null\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The display name, including the key itself so the two cannot be read apart\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingImportOptionsWindow.InitializeComponent()'></a>

## OpenBuildingImportOptionsWindow\.InitializeComponent\(\) Method

InitializeComponent

```csharp
public void InitializeComponent();
```

Implements [InitializeComponent\(\)](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector.initializecomponent 'System\.Windows\.Markup\.IComponentConnector\.InitializeComponent')

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingImportOptionsWindow.SetControls(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOpenBuildingImportTaskOptions)'></a>

## OpenBuildingImportOptionsWindow\.SetControls\(PostgreSQLOpenBuildingImportTaskOptions\) Method

Fills every control from the options \- on opening, and again on an import, so a value the options leave out clears its control rather than keeping what was typed before\.

```csharp
private void SetControls(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOpenBuildingImportTaskOptions options);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingImportOptionsWindow.SetControls(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOpenBuildingImportTaskOptions).options'></a>

`options` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOpenBuildingImportTaskOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlopenbuildingimporttaskoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOpenBuildingImportTaskOptions')

The options to show\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingImportOptionsWindow.SetCounties(System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_)'></a>

## OpenBuildingImportOptionsWindow\.SetCounties\(IEnumerable\<AdministrativeAreal2DReference\>\) Method

Fills the county list, sorted by code and then by identifier\. The selection is set by [SetControls\(PostgreSQLOpenBuildingImportTaskOptions\)](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingImportOptionsWindow.SetControls(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOpenBuildingImportTaskOptions) 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.OpenBuildingImportOptionsWindow\.SetControls\(DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOpenBuildingImportTaskOptions\)')\.

```csharp
private void SetCounties(System.Collections.Generic.IEnumerable<DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference>? administrativeAreal2DReferences);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingImportOptionsWindow.SetCounties(System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_).administrativeAreal2DReferences'></a>

`administrativeAreal2DReferences` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The county parts to list\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingImportOptionsWindow.SetScopeControls()'></a>

## OpenBuildingImportOptionsWindow\.SetScopeControls\(\) Method

Shows the scope form that is checked and hides the other one, so the window never offers a county selection and a box to fill at the same time\.

```csharp
private void SetScopeControls();
```

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingImportOptionsWindow.TrimOrNull(string)'></a>

## OpenBuildingImportOptionsWindow\.TrimOrNull\(string\) Method

Reads a control's value as trimmed text, mapping blank to null so an empty optional field is stored as absent rather than as whitespace\.

```csharp
private static string? TrimOrNull(string? value);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingImportOptionsWindow.TrimOrNull(string).value'></a>

`value` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The raw control value\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The trimmed text, or null when there was none\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingMatchOptionsWindow'></a>

## OpenBuildingMatchOptionsWindow Class

Interaction logic for OpenBuildingMatchOptionsWindow\.xaml

Asks what decides a run of the open building matching (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#57): which of the five footprint datasets to match to BDOT10k, the scope (a county list or an EPSG:2180 bounding box), the five parameters of the matching rule and whether the links replace the stored snapshot. GHS-OBAT is not offered: it holds centroids and the core task refuses it.

Every dataset checked is stored as "all" ([DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOpenBuildingMatchTaskOptions\.DatasetKeys](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlopenbuildingmatchtaskoptions.datasetkeys 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOpenBuildingMatchTaskOptions\.DatasetKeys') null), so a run started from this window keeps matching every footprint dataset if one is added later. Numbers are read in the invariant culture first and in the current one second, so `0.5` and `0,5` both work. What the core task and the rule need is asked through [OpenBuildingMatchOptionsMessage\(PostgreSQLOpenBuildingMatchTaskOptions\)](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Query.OpenBuildingMatchOptionsMessage(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOpenBuildingMatchTaskOptions) 'DiGi\.GIS\.PostgreSQL\.UI\.Query\.OpenBuildingMatchOptionsMessage\(DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOpenBuildingMatchTaskOptions\)') before OK returns, so the operator is told while the dialog is open.

The settings can be exported to a JSON file and imported again (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#60), so a run repeated after the tray was restarted is not typed again. Importing only fills the controls: nothing runs until OK, and OK validates the imported values like typed ones.

The window works on a copy - the options and their rule parameters - so a cancelled dialog leaves the settings of an earlier run exactly as they were.

```csharp
public class OpenBuildingMatchOptionsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [System\.Windows\.Threading\.DispatcherObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcherobject 'System\.Windows\.Threading\.DispatcherObject') → [System\.Windows\.DependencyObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.dependencyobject 'System\.Windows\.DependencyObject') → [System\.Windows\.Media\.Visual](https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.visual 'System\.Windows\.Media\.Visual') → [System\.Windows\.UIElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.uielement 'System\.Windows\.UIElement') → [System\.Windows\.FrameworkElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement 'System\.Windows\.FrameworkElement') → [System\.Windows\.Controls\.Control](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.control 'System\.Windows\.Controls\.Control') → [System\.Windows\.Controls\.ContentControl](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.contentcontrol 'System\.Windows\.Controls\.ContentControl') → [System\.Windows\.Window](https://learn.microsoft.com/en-us/dotnet/api/system.windows.window 'System\.Windows\.Window') → OpenBuildingMatchOptionsWindow

Implements [System\.Windows\.Markup\.IComponentConnector](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector 'System\.Windows\.Markup\.IComponentConnector')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingMatchOptionsWindow.OpenBuildingMatchOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOpenBuildingMatchTaskOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_)'></a>

## OpenBuildingMatchOptionsWindow\(PostgreSQLOpenBuildingMatchTaskOptions, IEnumerable\<AdministrativeAreal2DReference\>\) Constructor

Initializes a new instance of the [OpenBuildingMatchOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingMatchOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.OpenBuildingMatchOptionsWindow') class\.

```csharp
public OpenBuildingMatchOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOpenBuildingMatchTaskOptions? postgreSQLOpenBuildingMatchTaskOptions, System.Collections.Generic.IEnumerable<DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference>? administrativeAreal2DReferences);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingMatchOptionsWindow.OpenBuildingMatchOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOpenBuildingMatchTaskOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_).postgreSQLOpenBuildingMatchTaskOptions'></a>

`postgreSQLOpenBuildingMatchTaskOptions` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOpenBuildingMatchTaskOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlopenbuildingmatchtaskoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOpenBuildingMatchTaskOptions')

The options the controls are filled from; null uses the defaults \(every dataset, the proposed rule, links stored\)\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingMatchOptionsWindow.OpenBuildingMatchOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOpenBuildingMatchTaskOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_).administrativeAreal2DReferences'></a>

`administrativeAreal2DReferences` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The county parts to choose from, one entry per piece\. Nothing is preselected unless the options name parts: the scope is required, and a dialog that opens with every county ticked invites a national run as the default\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingMatchOptionsWindow.PostgreSQLOpenBuildingMatchTaskOptions'></a>

## OpenBuildingMatchOptionsWindow\.PostgreSQLOpenBuildingMatchTaskOptions Property

Gets the options the window holds\. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with\.

```csharp
public DiGi.GIS.PostgreSQL.Classes.PostgreSQLOpenBuildingMatchTaskOptions PostgreSQLOpenBuildingMatchTaskOptions { get; }
```

#### Property Value
[DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOpenBuildingMatchTaskOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlopenbuildingmatchtaskoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOpenBuildingMatchTaskOptions')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingMatchOptionsWindow.DatasetCheckBoxes()'></a>

## OpenBuildingMatchOptionsWindow\.DatasetCheckBoxes\(\) Method

The dataset check boxes with the key each one stands for, in the order they are shown\.

```csharp
private (System.Windows.Controls.CheckBox CheckBox,string DatasetKey)[] DatasetCheckBoxes();
```

#### Returns
[&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.valuetuple 'System\.ValueTuple')[System\.Windows\.Controls\.CheckBox](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.checkbox 'System\.Windows\.Controls\.CheckBox')[,](https://learn.microsoft.com/en-us/dotnet/api/system.valuetuple 'System\.ValueTuple')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.valuetuple 'System\.ValueTuple')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')  
The pairs of check box and dataset key\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingMatchOptionsWindow.InitializeComponent()'></a>

## OpenBuildingMatchOptionsWindow\.InitializeComponent\(\) Method

InitializeComponent

```csharp
public void InitializeComponent();
```

Implements [InitializeComponent\(\)](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector.initializecomponent 'System\.Windows\.Markup\.IComponentConnector\.InitializeComponent')

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingMatchOptionsWindow.SetControls(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOpenBuildingMatchTaskOptions)'></a>

## OpenBuildingMatchOptionsWindow\.SetControls\(PostgreSQLOpenBuildingMatchTaskOptions\) Method

Fills every control from the options \- on opening, and again on an import, so a value the options leave
out clears its control rather than keeping what was typed before\. Internal so a fact can fill a window
from a settings file the way Import does, without a file dialog\.

```csharp
internal void SetControls(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOpenBuildingMatchTaskOptions postgreSQLOpenBuildingMatchTaskOptions);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingMatchOptionsWindow.SetControls(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOpenBuildingMatchTaskOptions).postgreSQLOpenBuildingMatchTaskOptions'></a>

`postgreSQLOpenBuildingMatchTaskOptions` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOpenBuildingMatchTaskOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlopenbuildingmatchtaskoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOpenBuildingMatchTaskOptions')

The options to show\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingMatchOptionsWindow.SetParameters(DiGi.GIS.PostgreSQL.Classes.OpenBuildingMatchParameters)'></a>

## OpenBuildingMatchOptionsWindow\.SetParameters\(OpenBuildingMatchParameters\) Method

Shows the five rule parameters in the controls, in the invariant culture\.

```csharp
private void SetParameters(DiGi.GIS.PostgreSQL.Classes.OpenBuildingMatchParameters openBuildingMatchParameters);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingMatchOptionsWindow.SetParameters(DiGi.GIS.PostgreSQL.Classes.OpenBuildingMatchParameters).openBuildingMatchParameters'></a>

`openBuildingMatchParameters` [DiGi\.GIS\.PostgreSQL\.Classes\.OpenBuildingMatchParameters](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.openbuildingmatchparameters 'DiGi\.GIS\.PostgreSQL\.Classes\.OpenBuildingMatchParameters')

The parameters to show\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingMatchOptionsWindow.SetScopeControls()'></a>

## OpenBuildingMatchOptionsWindow\.SetScopeControls\(\) Method

Shows the scope form that is checked and hides the other one, so the window never offers a county selection and a box to fill at the same time\.

```csharp
private void SetScopeControls();
```

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingMatchOptionsWindow.TryGetOptions(string)'></a>

## OpenBuildingMatchOptionsWindow\.TryGetOptions\(string\) Method

Reads every control into the window's options and checks them, without closing the window or showing a message \- the part of OK a fact can drive, because a message box would hang it\.

The options are written only when every control reads and the result passes [OpenBuildingMatchOptionsMessage\(PostgreSQLOpenBuildingMatchTaskOptions\)](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Query.OpenBuildingMatchOptionsMessage(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOpenBuildingMatchTaskOptions) 'DiGi\.GIS\.PostgreSQL\.UI\.Query\.OpenBuildingMatchOptionsMessage\(DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOpenBuildingMatchTaskOptions\)'); a refusal leaves them as they were.

```csharp
internal bool TryGetOptions(out string? message);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingMatchOptionsWindow.TryGetOptions(string).message'></a>

`message` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The reason the controls cannot start a run, or null when they can\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
True when the options now hold the controls' values and can start a run\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingMatchOptionsWindow.TryParse(string,double)'></a>

## OpenBuildingMatchOptionsWindow\.TryParse\(string, double\) Method

Reads a number typed in the invariant culture \(`0.5`\) or, failing that, in the current one \(`0,5` under `pl-PL`\)\.

```csharp
private static bool TryParse(string? text, out double value);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingMatchOptionsWindow.TryParse(string,double).text'></a>

`text` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The typed text\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingMatchOptionsWindow.TryParse(string,double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The number read, or zero\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
True when the text is a finite number\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingMatchOptionsWindow.TryReadOptions(bool,DiGi.GIS.PostgreSQL.Classes.PostgreSQLOpenBuildingMatchTaskOptions,string)'></a>

## OpenBuildingMatchOptionsWindow\.TryReadOptions\(bool, PostgreSQLOpenBuildingMatchTaskOptions, string\) Method

Reads every control into a candidate set of options, writing nothing and showing no message\.

Complete (OK) requires at least one dataset and a scope, and finishes with [OpenBuildingMatchOptionsMessage\(PostgreSQLOpenBuildingMatchTaskOptions\)](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Query.OpenBuildingMatchOptionsMessage(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOpenBuildingMatchTaskOptions) 'DiGi\.GIS\.PostgreSQL\.UI\.Query\.OpenBuildingMatchOptionsMessage\(DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOpenBuildingMatchTaskOptions\)'), so the rule and the scope of the run are checked while the dialog can still be corrected. Export reads the same controls without those requirements - no dataset checked is an empty set, never "all", and an empty scope is left out - so a half-prepared window can be saved; what is still refused is what could not be written back as typed: a rule parameter that is not a finite number and a bounding box with only some of its four coordinates. A file out of the rule's range is refused by OK when it is read back, exactly as when it is typed.

```csharp
internal bool TryReadOptions(bool complete, out DiGi.GIS.PostgreSQL.Classes.PostgreSQLOpenBuildingMatchTaskOptions? options, out string? message);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingMatchOptionsWindow.TryReadOptions(bool,DiGi.GIS.PostgreSQL.Classes.PostgreSQLOpenBuildingMatchTaskOptions,string).complete'></a>

`complete` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

True for OK: a dataset and a scope are required and the whole options set is validated\. False for an export: an unfinished window is saved as it stands\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingMatchOptionsWindow.TryReadOptions(bool,DiGi.GIS.PostgreSQL.Classes.PostgreSQLOpenBuildingMatchTaskOptions,string).options'></a>

`options` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOpenBuildingMatchTaskOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlopenbuildingmatchtaskoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOpenBuildingMatchTaskOptions')

The options the controls describe, or null when they could not be read\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingMatchOptionsWindow.TryReadOptions(bool,DiGi.GIS.PostgreSQL.Classes.PostgreSQLOpenBuildingMatchTaskOptions,string).message'></a>

`message` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The reason the controls could not be read, or null when they could\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
True when the controls read into options\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingMatchOptionsWindow.TryReadSettings(string,string)'></a>

## OpenBuildingMatchOptionsWindow\.TryReadSettings\(string, string\) Method

Reads the settings an exported file holds\. Exactly one open building match options object is accepted: a
file of another window's settings carries a different type name and cannot be read as this one, and a
file holding several objects describes several settings, none of which is this window's\.

```csharp
internal static DiGi.GIS.PostgreSQL.Classes.PostgreSQLOpenBuildingMatchTaskOptions? TryReadSettings(string? json, out string? message);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingMatchOptionsWindow.TryReadSettings(string,string).json'></a>

`json` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The file content\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.OpenBuildingMatchOptionsWindow.TryReadSettings(string,string).message'></a>

`message` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The reason nothing was read, or null when exactly one setting was\.

#### Returns
[DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOpenBuildingMatchTaskOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlopenbuildingmatchtaskoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOpenBuildingMatchTaskOptions')  
The setting the file holds, or null when it holds none\.

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

Asks for the settings that decide what a run touches and how long it takes - which counties, the command timeout, the batch size, whether finished buildings are skipped, whether party walls are measured, and how many batches of a county run at once. The run reads the stored building models of the counties in scope, so the counties decide its reach and its cost; left unscoped it walks every county part in the country.

Every county is selected by default, so opening the dialog confirms the national scope rather than inviting it; deselecting everything is still refused at OK, because an empty selection is not the same as no selection and neither is a safe thing to leave this window with by accident.

Party walls decide most of the cost: with them the run also reads the outlines and the stored models of the neighbours of every batch, without them it only classifies the models of the counties in scope and leaves the party wall columns exactly as they were. The box starts from the options, so it is checked unless a run was configured otherwise.

The parallel batches decide how many batches of one county are read, classified and written at once - 0 takes half of the logical processors, 1 is the sequential run. The stored rows and the counters do not depend on it, but the memory of the run grows with that number times the batch size, so it is worth no more than the host can hold.

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

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow'></a>

## PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow Class

Interaction logic for PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow\.xaml

Asks for what a solar radiation summary run writes: the county parts, the radius the summary is built from, whether rows already carrying a total are stepped over, the batch size and the timeout.

The radius is the field to read twice. A building can carry several variants - the isolated building (0) and one or more neighbour radii (DiGi.GIS.PostgreSQL#107) - and left empty the variant written last wins, building by building.

The settings can be exported to a JSON file and imported again (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#66), so a run repeated after the tray was restarted is not typed again. Importing only fills the controls: nothing runs until OK, and OK validates the imported values like typed ones.

The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.

```csharp
public class PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [System\.Windows\.Threading\.DispatcherObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcherobject 'System\.Windows\.Threading\.DispatcherObject') → [System\.Windows\.DependencyObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.dependencyobject 'System\.Windows\.DependencyObject') → [System\.Windows\.Media\.Visual](https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.visual 'System\.Windows\.Media\.Visual') → [System\.Windows\.UIElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.uielement 'System\.Windows\.UIElement') → [System\.Windows\.FrameworkElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement 'System\.Windows\.FrameworkElement') → [System\.Windows\.Controls\.Control](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.control 'System\.Windows\.Controls\.Control') → [System\.Windows\.Controls\.ContentControl](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.contentcontrol 'System\.Windows\.Controls\.ContentControl') → [System\.Windows\.Window](https://learn.microsoft.com/en-us/dotnet/api/system.windows.window 'System\.Windows\.Window') → PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow

Implements [System\.Windows\.Markup\.IComponentConnector](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector 'System\.Windows\.Markup\.IComponentConnector')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow.PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataSolarRadiationUpdateOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_)'></a>

## PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow\(PostgreSQLBuildingDataSolarRadiationUpdateOptions, IEnumerable\<AdministrativeAreal2DReference\>\) Constructor

Initializes a new instance of the [PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow') class\.

```csharp
public PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataSolarRadiationUpdateOptions? postgreSQLBuildingDataSolarRadiationUpdateOptions, System.Collections.Generic.IEnumerable<DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference>? administrativeAreal2DReferences);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow.PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataSolarRadiationUpdateOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_).postgreSQLBuildingDataSolarRadiationUpdateOptions'></a>

`postgreSQLBuildingDataSolarRadiationUpdateOptions` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataSolarRadiationUpdateOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlbuildingdatasolarradiationupdateoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataSolarRadiationUpdateOptions')

The options the controls are filled from\. When null the defaults are used\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow.PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataSolarRadiationUpdateOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_).administrativeAreal2DReferences'></a>

`administrativeAreal2DReferences` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The counties to choose from\. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow.PostgreSQLBuildingDataSolarRadiationUpdateOptions'></a>

## PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow\.PostgreSQLBuildingDataSolarRadiationUpdateOptions Property

Gets the options the window holds\. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with\.

```csharp
public DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataSolarRadiationUpdateOptions PostgreSQLBuildingDataSolarRadiationUpdateOptions { get; }
```

#### Property Value
[DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataSolarRadiationUpdateOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlbuildingdatasolarradiationupdateoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataSolarRadiationUpdateOptions')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow.InitializeComponent()'></a>

## PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow\.InitializeComponent\(\) Method

InitializeComponent

```csharp
public void InitializeComponent();
```

Implements [InitializeComponent\(\)](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector.initializecomponent 'System\.Windows\.Markup\.IComponentConnector\.InitializeComponent')

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow.SetControls(DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataSolarRadiationUpdateOptions)'></a>

## PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow\.SetControls\(PostgreSQLBuildingDataSolarRadiationUpdateOptions\) Method

Fills every control from the options \- on opening, and again on an import, so a value the options leave
out clears its control rather than keeping what was typed before: options with no county list clear the
selection and options with no radius empty the radius box\. Internal so a fact can fill a window from a
settings file the way Import does, without a file dialog\.

```csharp
internal void SetControls(DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataSolarRadiationUpdateOptions postgreSQLBuildingDataSolarRadiationUpdateOptions);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow.SetControls(DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataSolarRadiationUpdateOptions).postgreSQLBuildingDataSolarRadiationUpdateOptions'></a>

`postgreSQLBuildingDataSolarRadiationUpdateOptions` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataSolarRadiationUpdateOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlbuildingdatasolarradiationupdateoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataSolarRadiationUpdateOptions')

The options to show\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow.TryGetOptions(string)'></a>

## PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow\.TryGetOptions\(string\) Method

Reads every control into the window's options and checks them, without closing the window or showing a message \- the part of OK a fact can drive, because a message box would hang it\.

The options are written only when every control reads and passes the full set of rules OK has always applied - at least one county, a radius of at least zero when given, a batch size of at least one, a timeout of at least zero; a refusal leaves them as they were.

```csharp
internal bool TryGetOptions(out string? message);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow.TryGetOptions(string).message'></a>

`message` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The reason the controls cannot start a run, or null when they can\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
True when the options now hold the controls' values and can start a run\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow.TryReadOptions(bool,DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataSolarRadiationUpdateOptions,string)'></a>

## PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow\.TryReadOptions\(bool, PostgreSQLBuildingDataSolarRadiationUpdateOptions, string\) Method

Reads every control into a candidate set of options, writing nothing and showing no message\.

Complete (OK) applies every rule the window has always applied, in the order it has always applied them, and finishes with those messages. Export reads the same controls without the range rules, so a half-prepared window can be saved: no county is written as no county list, an empty radius box as no radius - the latest stored result of any radius is a value of its own - and a radius below zero or a batch size below one still exports, to be refused by OK on read-back. What is refused on both paths is what could not be written back as typed: a radius box that does not hold a finite number, a batch size or timeout that is not a whole number.

```csharp
internal bool TryReadOptions(bool complete, out DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataSolarRadiationUpdateOptions? options, out string? message);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow.TryReadOptions(bool,DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataSolarRadiationUpdateOptions,string).complete'></a>

`complete` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

True for OK: every rule applies\. False for an export: an unfinished window is saved as it stands\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow.TryReadOptions(bool,DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataSolarRadiationUpdateOptions,string).options'></a>

`options` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataSolarRadiationUpdateOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlbuildingdatasolarradiationupdateoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataSolarRadiationUpdateOptions')

The options the controls describe, or null when they could not be read\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow.TryReadOptions(bool,DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataSolarRadiationUpdateOptions,string).message'></a>

`message` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The reason the controls could not be read, or null when they could\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
True when the controls read into options\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow.TryReadSettings(string,string)'></a>

## PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow\.TryReadSettings\(string, string\) Method

Reads the settings an exported file holds\. Exactly one building data solar summary options object is accepted: a
file of another window's settings carries a different type name and cannot be read as this one, and a
file holding several objects describes several settings, none of which is this window's\.

```csharp
internal static DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataSolarRadiationUpdateOptions? TryReadSettings(string? json, out string? message);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow.TryReadSettings(string,string).json'></a>

`json` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The file content\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow.TryReadSettings(string,string).message'></a>

`message` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The reason nothing was read, or null when exactly one setting was\.

#### Returns
[DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataSolarRadiationUpdateOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlbuildingdatasolarradiationupdateoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataSolarRadiationUpdateOptions')  
The setting the file holds, or null when it holds none\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataUpdateOptionsWindow'></a>

## PostgreSQLBuildingDataUpdateOptionsWindow Class

Interaction logic for PostgreSQLBuildingDataUpdateOptionsWindow\.xaml

Asks for the three settings that decide what a run costs and touches - which counties, which kinds of column, and how long a statement may take. The radiuses are carried over from the instance it was given untouched: each one names its own pair of stored columns, so changing them from a dialog would change the shape of the table rather than the numbers in it.

The counties matter most. Left unscoped the run walks every subdivision in the country, which is the pass nobody wants to start by accident while trying one county.

The settings can be exported to a JSON file and imported again (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#67), so a run repeated after the tray was restarted is not typed again. Importing fills the controls, and the two members no control shows - `Radiuses` and `Years` - travel in the file as the copy holds them, so an imported file can change which columns the run writes; this paragraph is the warning. Nothing runs until OK, and OK validates the imported values like typed ones.

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

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataUpdateOptionsWindow.SetControls(DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataUpdateOptions)'></a>

## PostgreSQLBuildingDataUpdateOptionsWindow\.SetControls\(PostgreSQLBuildingDataUpdateOptions\) Method

Fills every control from the options \- on opening, and again on an import, so a value the options leave
out clears its control rather than keeping what was typed before: options with no county list clear the
selection, options with no update types clear the type list\. The options shown are remembered as the
base every later read is built on, which is how the members with no control follow an import\. Internal
so a fact can fill a window from a settings file the way Import does, without a file dialog\.

```csharp
internal void SetControls(DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataUpdateOptions postgreSQLBuildingDataUpdateOptions);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataUpdateOptionsWindow.SetControls(DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataUpdateOptions).postgreSQLBuildingDataUpdateOptions'></a>

`postgreSQLBuildingDataUpdateOptions` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataUpdateOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlbuildingdataupdateoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataUpdateOptions')

The options to show\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataUpdateOptionsWindow.TryGetOptions(string)'></a>

## PostgreSQLBuildingDataUpdateOptionsWindow\.TryGetOptions\(string\) Method

Reads every control into the window's options and checks them, without closing the window or showing a message \- the part of OK a fact can drive, because a message box would hang it\.

The options are written only when every control reads and passes the full set of rules OK has always applied - at least one update type, at least one county, a timeout of at least zero - and they take the two members no control shows from whatever the controls were last filled from: the options the window was opened with, or the file an import read. A refusal leaves them as they were.

```csharp
internal bool TryGetOptions(out string? message);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataUpdateOptionsWindow.TryGetOptions(string).message'></a>

`message` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The reason the controls cannot start a run, or null when they can\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
True when the options now hold the controls' values and can start a run\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataUpdateOptionsWindow.TryReadOptions(bool,DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataUpdateOptions,string)'></a>

## PostgreSQLBuildingDataUpdateOptionsWindow\.TryReadOptions\(bool, PostgreSQLBuildingDataUpdateOptions, string\) Method

Reads every control into a candidate set of options, writing nothing and showing no message\.

Complete (OK) applies every rule the window has always applied - at least one update type, at least one county, a timeout of at least zero - and finishes with those messages. Export reads the same controls without those requirements, so a half-prepared window can be saved: nothing checked is an empty set (never "every type") and no county is no county list. A timeout box that does not hold a whole number is refused on both paths; a negative timeout exports, to be refused by OK on read-back. The candidate is built on what the controls were last filled from, so the two members with no control - `Radiuses` and `Years` - travel in the file exactly as that copy holds them, and after an import OK writes the file's, not the window's.

```csharp
internal bool TryReadOptions(bool complete, out DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataUpdateOptions? options, out string? message);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataUpdateOptionsWindow.TryReadOptions(bool,DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataUpdateOptions,string).complete'></a>

`complete` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

True for OK: every rule applies\. False for an export: an unfinished window is saved as it stands\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataUpdateOptionsWindow.TryReadOptions(bool,DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataUpdateOptions,string).options'></a>

`options` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataUpdateOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlbuildingdataupdateoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataUpdateOptions')

The options the controls describe, or null when they could not be read\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataUpdateOptionsWindow.TryReadOptions(bool,DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataUpdateOptions,string).message'></a>

`message` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The reason the controls could not be read, or null when they could\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
True when the controls read into options\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataUpdateOptionsWindow.TryReadSettings(string,string)'></a>

## PostgreSQLBuildingDataUpdateOptionsWindow\.TryReadSettings\(string, string\) Method

Reads the settings an exported file holds\. Exactly one building data options object is accepted: a
file of another window's settings carries a different type name and cannot be read as this one, and a
file holding several objects describes several settings, none of which is this window's\.

```csharp
internal static DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingDataUpdateOptions? TryReadSettings(string? json, out string? message);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataUpdateOptionsWindow.TryReadSettings(string,string).json'></a>

`json` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The file content\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingDataUpdateOptionsWindow.TryReadSettings(string,string).message'></a>

`message` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The reason nothing was read, or null when exactly one setting was\.

#### Returns
[DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataUpdateOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlbuildingdataupdateoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingDataUpdateOptions')  
The setting the file holds, or null when it holds none\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingHeatDemandSolveOptionsWindow'></a>

## PostgreSQLBuildingHeatDemandSolveOptionsWindow Class

Interaction logic for PostgreSQLBuildingHeatDemandSolveOptionsWindow\.xaml

Asks for everything that decides what a batch heat demand run costs and stores: the county parts, the construction flavour, the radius of the stored solar result the irradiation is taken from, whether buildings already calculated are stepped over, and the batch, ceiling, parallelism and timeout settings (DiGi.GIS.PostgreSQL.UI#49).

The counties matter most. An unscoped run is every building in the country - the window never leaves with no county chosen.

The options the window does not show - the engine options, the neighbour radius and the tile size - are carried through as they were. The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.

```csharp
public class PostgreSQLBuildingHeatDemandSolveOptionsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [System\.Windows\.Threading\.DispatcherObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcherobject 'System\.Windows\.Threading\.DispatcherObject') → [System\.Windows\.DependencyObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.dependencyobject 'System\.Windows\.DependencyObject') → [System\.Windows\.Media\.Visual](https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.visual 'System\.Windows\.Media\.Visual') → [System\.Windows\.UIElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.uielement 'System\.Windows\.UIElement') → [System\.Windows\.FrameworkElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement 'System\.Windows\.FrameworkElement') → [System\.Windows\.Controls\.Control](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.control 'System\.Windows\.Controls\.Control') → [System\.Windows\.Controls\.ContentControl](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.contentcontrol 'System\.Windows\.Controls\.ContentControl') → [System\.Windows\.Window](https://learn.microsoft.com/en-us/dotnet/api/system.windows.window 'System\.Windows\.Window') → PostgreSQLBuildingHeatDemandSolveOptionsWindow

Implements [System\.Windows\.Markup\.IComponentConnector](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector 'System\.Windows\.Markup\.IComponentConnector')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingHeatDemandSolveOptionsWindow.PostgreSQLBuildingHeatDemandSolveOptionsWindow(DiGi.GIS.PostgreSQL.Energy.Classes.PostgreSQLBuildingHeatDemandSolveOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_)'></a>

## PostgreSQLBuildingHeatDemandSolveOptionsWindow\(PostgreSQLBuildingHeatDemandSolveOptions, IEnumerable\<AdministrativeAreal2DReference\>\) Constructor

Initializes a new instance of the [PostgreSQLBuildingHeatDemandSolveOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingHeatDemandSolveOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.PostgreSQLBuildingHeatDemandSolveOptionsWindow') class\.

```csharp
public PostgreSQLBuildingHeatDemandSolveOptionsWindow(DiGi.GIS.PostgreSQL.Energy.Classes.PostgreSQLBuildingHeatDemandSolveOptions? postgreSQLBuildingHeatDemandSolveOptions, System.Collections.Generic.IEnumerable<DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference>? administrativeAreal2DReferences);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingHeatDemandSolveOptionsWindow.PostgreSQLBuildingHeatDemandSolveOptionsWindow(DiGi.GIS.PostgreSQL.Energy.Classes.PostgreSQLBuildingHeatDemandSolveOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_).postgreSQLBuildingHeatDemandSolveOptions'></a>

`postgreSQLBuildingHeatDemandSolveOptions` [DiGi\.GIS\.PostgreSQL\.Energy\.Classes\.PostgreSQLBuildingHeatDemandSolveOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.energy.classes.postgresqlbuildingheatdemandsolveoptions 'DiGi\.GIS\.PostgreSQL\.Energy\.Classes\.PostgreSQLBuildingHeatDemandSolveOptions')

The options the controls are filled from\. When null the defaults are used\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingHeatDemandSolveOptionsWindow.PostgreSQLBuildingHeatDemandSolveOptionsWindow(DiGi.GIS.PostgreSQL.Energy.Classes.PostgreSQLBuildingHeatDemandSolveOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_).administrativeAreal2DReferences'></a>

`administrativeAreal2DReferences` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The counties to choose from\. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingHeatDemandSolveOptionsWindow.PostgreSQLBuildingHeatDemandSolveOptions'></a>

## PostgreSQLBuildingHeatDemandSolveOptionsWindow\.PostgreSQLBuildingHeatDemandSolveOptions Property

Gets the options the window holds\. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with\.

```csharp
public DiGi.GIS.PostgreSQL.Energy.Classes.PostgreSQLBuildingHeatDemandSolveOptions PostgreSQLBuildingHeatDemandSolveOptions { get; }
```

#### Property Value
[DiGi\.GIS\.PostgreSQL\.Energy\.Classes\.PostgreSQLBuildingHeatDemandSolveOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.energy.classes.postgresqlbuildingheatdemandsolveoptions 'DiGi\.GIS\.PostgreSQL\.Energy\.Classes\.PostgreSQLBuildingHeatDemandSolveOptions')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingHeatDemandSolveOptionsWindow.InitializeComponent()'></a>

## PostgreSQLBuildingHeatDemandSolveOptionsWindow\.InitializeComponent\(\) Method

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

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingSolarRadiationContextExportOptionsWindow'></a>

## PostgreSQLBuildingSolarRadiationContextExportOptionsWindow Class

Interaction logic for PostgreSQLBuildingSolarRadiationContextExportOptionsWindow\.xaml

Asks for what the urban-context export writes and where: the county parts (one CSV per part), the stored radii in the column order they are exported, whether only residential buildings are kept, the batch size and timeout, and the output folder (DiGi.GIS.PostgreSQL#136).

The export refuses a run without a folder, so the window proposes [SolarContextDirectory\(string\)](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Query.SolarContextDirectory(string) 'DiGi\.GIS\.PostgreSQL\.UI\.Query\.SolarContextDirectory\(string\)') when the options carry none, and creates the folder when it is accepted.

The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.

```csharp
public class PostgreSQLBuildingSolarRadiationContextExportOptionsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [System\.Windows\.Threading\.DispatcherObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcherobject 'System\.Windows\.Threading\.DispatcherObject') → [System\.Windows\.DependencyObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.dependencyobject 'System\.Windows\.DependencyObject') → [System\.Windows\.Media\.Visual](https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.visual 'System\.Windows\.Media\.Visual') → [System\.Windows\.UIElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.uielement 'System\.Windows\.UIElement') → [System\.Windows\.FrameworkElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement 'System\.Windows\.FrameworkElement') → [System\.Windows\.Controls\.Control](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.control 'System\.Windows\.Controls\.Control') → [System\.Windows\.Controls\.ContentControl](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.contentcontrol 'System\.Windows\.Controls\.ContentControl') → [System\.Windows\.Window](https://learn.microsoft.com/en-us/dotnet/api/system.windows.window 'System\.Windows\.Window') → PostgreSQLBuildingSolarRadiationContextExportOptionsWindow

Implements [System\.Windows\.Markup\.IComponentConnector](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector 'System\.Windows\.Markup\.IComponentConnector')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingSolarRadiationContextExportOptionsWindow.PostgreSQLBuildingSolarRadiationContextExportOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingSolarRadiationContextExportOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_)'></a>

## PostgreSQLBuildingSolarRadiationContextExportOptionsWindow\(PostgreSQLBuildingSolarRadiationContextExportOptions, IEnumerable\<AdministrativeAreal2DReference\>\) Constructor

Initializes a new instance of the [PostgreSQLBuildingSolarRadiationContextExportOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingSolarRadiationContextExportOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.PostgreSQLBuildingSolarRadiationContextExportOptionsWindow') class\.

```csharp
public PostgreSQLBuildingSolarRadiationContextExportOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingSolarRadiationContextExportOptions? postgreSQLBuildingSolarRadiationContextExportOptions, System.Collections.Generic.IEnumerable<DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference>? administrativeAreal2DReferences);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingSolarRadiationContextExportOptionsWindow.PostgreSQLBuildingSolarRadiationContextExportOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingSolarRadiationContextExportOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_).postgreSQLBuildingSolarRadiationContextExportOptions'></a>

`postgreSQLBuildingSolarRadiationContextExportOptions` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingSolarRadiationContextExportOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlbuildingsolarradiationcontextexportoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingSolarRadiationContextExportOptions')

The options the controls are filled from\. When null the defaults are used\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingSolarRadiationContextExportOptionsWindow.PostgreSQLBuildingSolarRadiationContextExportOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingSolarRadiationContextExportOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_).administrativeAreal2DReferences'></a>

`administrativeAreal2DReferences` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The counties to choose from\. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingSolarRadiationContextExportOptionsWindow.PostgreSQLBuildingSolarRadiationContextExportOptions'></a>

## PostgreSQLBuildingSolarRadiationContextExportOptionsWindow\.PostgreSQLBuildingSolarRadiationContextExportOptions Property

Gets the options the window holds\. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with\.

```csharp
public DiGi.GIS.PostgreSQL.Classes.PostgreSQLBuildingSolarRadiationContextExportOptions PostgreSQLBuildingSolarRadiationContextExportOptions { get; }
```

#### Property Value
[DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingSolarRadiationContextExportOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlbuildingsolarradiationcontextexportoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLBuildingSolarRadiationContextExportOptions')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingSolarRadiationContextExportOptionsWindow.InitializeComponent()'></a>

## PostgreSQLBuildingSolarRadiationContextExportOptionsWindow\.InitializeComponent\(\) Method

InitializeComponent

```csharp
public void InitializeComponent();
```

Implements [InitializeComponent\(\)](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector.initializecomponent 'System\.Windows\.Markup\.IComponentConnector\.InitializeComponent')

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingSolarRadiationSolveOptionsWindow'></a>

## PostgreSQLBuildingSolarRadiationSolveOptionsWindow Class

Interaction logic for PostgreSQLBuildingSolarRadiationSolveOptionsWindow\.xaml

Asks for everything that decides what a batch solar radiation solve costs and stores: the county parts, the neighbour radii every building is solved for, whether results already stored are stepped over, and the batch, tile, ceiling and timeout settings (DiGi.GIS.PostgreSQL#136).

An optional limit of buildings per county part runs a pilot: empty solves every building (DiGi.GIS.PostgreSQL.UI#45).

The number of solves run at once: 0 is automatic, half the logical processors; 1 is one at a time. It changes the run time only, never the stored results (DiGi.GIS.PostgreSQL.UI#46, DiGi.GIS.PostgreSQL#139).

The counties matter most. A solve costs seconds per building and radius, so an unscoped run is the whole country several times over - the window never leaves with no county chosen.

The settings can be exported to a JSON file and imported again (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#59), so a run repeated after the tray was restarted is not typed again. Importing only fills the controls: nothing runs until OK, and OK validates the imported values like typed ones.

The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.

```csharp
public class PostgreSQLBuildingSolarRadiationSolveOptionsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [System\.Windows\.Threading\.DispatcherObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcherobject 'System\.Windows\.Threading\.DispatcherObject') → [System\.Windows\.DependencyObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.dependencyobject 'System\.Windows\.DependencyObject') → [System\.Windows\.Media\.Visual](https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.visual 'System\.Windows\.Media\.Visual') → [System\.Windows\.UIElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.uielement 'System\.Windows\.UIElement') → [System\.Windows\.FrameworkElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement 'System\.Windows\.FrameworkElement') → [System\.Windows\.Controls\.Control](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.control 'System\.Windows\.Controls\.Control') → [System\.Windows\.Controls\.ContentControl](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.contentcontrol 'System\.Windows\.Controls\.ContentControl') → [System\.Windows\.Window](https://learn.microsoft.com/en-us/dotnet/api/system.windows.window 'System\.Windows\.Window') → PostgreSQLBuildingSolarRadiationSolveOptionsWindow

Implements [System\.Windows\.Markup\.IComponentConnector](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector 'System\.Windows\.Markup\.IComponentConnector')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingSolarRadiationSolveOptionsWindow.PostgreSQLBuildingSolarRadiationSolveOptionsWindow(DiGi.GIS.PostgreSQL.Solar.Classes.PostgreSQLBuildingSolarRadiationSolveOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_)'></a>

## PostgreSQLBuildingSolarRadiationSolveOptionsWindow\(PostgreSQLBuildingSolarRadiationSolveOptions, IEnumerable\<AdministrativeAreal2DReference\>\) Constructor

Initializes a new instance of the [PostgreSQLBuildingSolarRadiationSolveOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingSolarRadiationSolveOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.PostgreSQLBuildingSolarRadiationSolveOptionsWindow') class\.

```csharp
public PostgreSQLBuildingSolarRadiationSolveOptionsWindow(DiGi.GIS.PostgreSQL.Solar.Classes.PostgreSQLBuildingSolarRadiationSolveOptions? postgreSQLBuildingSolarRadiationSolveOptions, System.Collections.Generic.IEnumerable<DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference>? administrativeAreal2DReferences);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingSolarRadiationSolveOptionsWindow.PostgreSQLBuildingSolarRadiationSolveOptionsWindow(DiGi.GIS.PostgreSQL.Solar.Classes.PostgreSQLBuildingSolarRadiationSolveOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_).postgreSQLBuildingSolarRadiationSolveOptions'></a>

`postgreSQLBuildingSolarRadiationSolveOptions` [DiGi\.GIS\.PostgreSQL\.Solar\.Classes\.PostgreSQLBuildingSolarRadiationSolveOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.solar.classes.postgresqlbuildingsolarradiationsolveoptions 'DiGi\.GIS\.PostgreSQL\.Solar\.Classes\.PostgreSQLBuildingSolarRadiationSolveOptions')

The options the controls are filled from\. When null the defaults are used\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingSolarRadiationSolveOptionsWindow.PostgreSQLBuildingSolarRadiationSolveOptionsWindow(DiGi.GIS.PostgreSQL.Solar.Classes.PostgreSQLBuildingSolarRadiationSolveOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_).administrativeAreal2DReferences'></a>

`administrativeAreal2DReferences` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The counties to choose from\. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingSolarRadiationSolveOptionsWindow.PostgreSQLBuildingSolarRadiationSolveOptions'></a>

## PostgreSQLBuildingSolarRadiationSolveOptionsWindow\.PostgreSQLBuildingSolarRadiationSolveOptions Property

Gets the options the window holds\. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with\.

```csharp
public DiGi.GIS.PostgreSQL.Solar.Classes.PostgreSQLBuildingSolarRadiationSolveOptions PostgreSQLBuildingSolarRadiationSolveOptions { get; }
```

#### Property Value
[DiGi\.GIS\.PostgreSQL\.Solar\.Classes\.PostgreSQLBuildingSolarRadiationSolveOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.solar.classes.postgresqlbuildingsolarradiationsolveoptions 'DiGi\.GIS\.PostgreSQL\.Solar\.Classes\.PostgreSQLBuildingSolarRadiationSolveOptions')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingSolarRadiationSolveOptionsWindow.CreateOptions(bool,string)'></a>

## PostgreSQLBuildingSolarRadiationSolveOptionsWindow\.CreateOptions\(bool, string\) Method

Reads the controls into a new copy of the window's options, telling what cannot be read\. The refusal is
returned rather than shown: [TryAccept\(string\)](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingSolarRadiationSolveOptionsWindow.TryAccept(string) 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.PostgreSQLBuildingSolarRadiationSolveOptionsWindow\.TryAccept\(string\)') hands it to OK to display, and Export
shows it itself, so a value is never reported twice\. Internal so a fact can read a half\-filled window the
way Export does, without a file dialog\.

```csharp
internal DiGi.GIS.PostgreSQL.Solar.Classes.PostgreSQLBuildingSolarRadiationSolveOptions? CreateOptions(bool complete, out string? message);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingSolarRadiationSolveOptionsWindow.CreateOptions(bool,string).complete'></a>

`complete` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

True for OK: a county and a radius are required\. False for an export: an empty county selection or an empty radii list is kept as no scope, so a half\-prepared window can still be saved\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingSolarRadiationSolveOptionsWindow.CreateOptions(bool,string).message'></a>

`message` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The reason the controls could not be read, or null when they could\.

#### Returns
[DiGi\.GIS\.PostgreSQL\.Solar\.Classes\.PostgreSQLBuildingSolarRadiationSolveOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.solar.classes.postgresqlbuildingsolarradiationsolveoptions 'DiGi\.GIS\.PostgreSQL\.Solar\.Classes\.PostgreSQLBuildingSolarRadiationSolveOptions')  
The options the controls describe, or null when a value could not be read\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingSolarRadiationSolveOptionsWindow.InitializeComponent()'></a>

## PostgreSQLBuildingSolarRadiationSolveOptionsWindow\.InitializeComponent\(\) Method

InitializeComponent

```csharp
public void InitializeComponent();
```

Implements [InitializeComponent\(\)](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector.initializecomponent 'System\.Windows\.Markup\.IComponentConnector\.InitializeComponent')

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingSolarRadiationSolveOptionsWindow.SetControls(DiGi.GIS.PostgreSQL.Solar.Classes.PostgreSQLBuildingSolarRadiationSolveOptions)'></a>

## PostgreSQLBuildingSolarRadiationSolveOptionsWindow\.SetControls\(PostgreSQLBuildingSolarRadiationSolveOptions\) Method

Fills every control from the options \- on opening, and again on an import, so a value the options leave
out clears its control rather than keeping what was typed before\. The captions stay in the XAML: the
designer does not run this code, only the values do\. Internal so a fact can fill a window from a settings
file the way Import does, without a file dialog\.

```csharp
internal void SetControls(DiGi.GIS.PostgreSQL.Solar.Classes.PostgreSQLBuildingSolarRadiationSolveOptions options);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingSolarRadiationSolveOptionsWindow.SetControls(DiGi.GIS.PostgreSQL.Solar.Classes.PostgreSQLBuildingSolarRadiationSolveOptions).options'></a>

`options` [DiGi\.GIS\.PostgreSQL\.Solar\.Classes\.PostgreSQLBuildingSolarRadiationSolveOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.solar.classes.postgresqlbuildingsolarradiationsolveoptions 'DiGi\.GIS\.PostgreSQL\.Solar\.Classes\.PostgreSQLBuildingSolarRadiationSolveOptions')

The options to show\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingSolarRadiationSolveOptionsWindow.TryAccept(string)'></a>

## PostgreSQLBuildingSolarRadiationSolveOptionsWindow\.TryAccept\(string\) Method

Validates the controls and, when they hold together, carries them into the options the window holds: the
selected counties, the radii list as edited, and the batch, tile, ceiling, timeout, pilot\-limit and
parallelism settings\. On refusal nothing is written and the reason is returned for the caller to show
\(DiGi\.GIS\.PostgreSQL\#158\)\.

```csharp
internal bool TryAccept(out string? message);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingSolarRadiationSolveOptionsWindow.TryAccept(string).message'></a>

`message` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The reason the dialog cannot be accepted, or null when it can\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
True when the options were written; false when a control was refused, leaving them untouched\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingSolarRadiationSolveOptionsWindow.TryReadSettings(string,string)'></a>

## PostgreSQLBuildingSolarRadiationSolveOptionsWindow\.TryReadSettings\(string, string\) Method

Reads the settings an exported file holds\. Exactly one solve options object is accepted: a file of another
window's settings answers a different type name and cannot be read as this one, and a file holding several
objects describes several settings, none of which is this window's\.

```csharp
internal static DiGi.GIS.PostgreSQL.Solar.Classes.PostgreSQLBuildingSolarRadiationSolveOptions? TryReadSettings(string? json, out string? message);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingSolarRadiationSolveOptionsWindow.TryReadSettings(string,string).json'></a>

`json` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The file content\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLBuildingSolarRadiationSolveOptionsWindow.TryReadSettings(string,string).message'></a>

`message` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The reason nothing was read, or null when exactly one setting was\.

#### Returns
[DiGi\.GIS\.PostgreSQL\.Solar\.Classes\.PostgreSQLBuildingSolarRadiationSolveOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.solar.classes.postgresqlbuildingsolarradiationsolveoptions 'DiGi\.GIS\.PostgreSQL\.Solar\.Classes\.PostgreSQLBuildingSolarRadiationSolveOptions')  
The setting the file holds, or null when it holds none\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow'></a>

## PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow Class

Interaction logic for PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow\.xaml

Asks for the county parts an acquisition-date backfill of `orto_datas` walks, the last year the stored series were fetched up to, and the paging of the walk. Unlike most county-scoped tasks, no selection means nothing rather than every county: the backfill reads every stored series it reaches, about 0.5 MB each, so the scope is always named.

The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.

```csharp
public class PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [System\.Windows\.Threading\.DispatcherObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcherobject 'System\.Windows\.Threading\.DispatcherObject') → [System\.Windows\.DependencyObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.dependencyobject 'System\.Windows\.DependencyObject') → [System\.Windows\.Media\.Visual](https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.visual 'System\.Windows\.Media\.Visual') → [System\.Windows\.UIElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.uielement 'System\.Windows\.UIElement') → [System\.Windows\.FrameworkElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement 'System\.Windows\.FrameworkElement') → [System\.Windows\.Controls\.Control](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.control 'System\.Windows\.Controls\.Control') → [System\.Windows\.Controls\.ContentControl](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.contentcontrol 'System\.Windows\.Controls\.ContentControl') → [System\.Windows\.Window](https://learn.microsoft.com/en-us/dotnet/api/system.windows.window 'System\.Windows\.Window') → PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow

Implements [System\.Windows\.Markup\.IComponentConnector](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector 'System\.Windows\.Markup\.IComponentConnector')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow.PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOrtoDatasAcquisitionUpdateOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,System.Func_System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.Dictionary_int,DiGi.GIS.PostgreSQL.Classes.OrtoDataAcquisitionCoverageResult___)'></a>

## PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow\(PostgreSQLOrtoDatasAcquisitionUpdateOptions, IEnumerable\<AdministrativeAreal2DReference\>, Func\<CancellationToken,Task\<Dictionary\<int,OrtoDataAcquisitionCoverageResult\>\>\>\) Constructor

Initializes a new instance of the [PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow') class\.

```csharp
public PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOrtoDatasAcquisitionUpdateOptions? postgreSQLOrtoDatasAcquisitionUpdateOptions, System.Collections.Generic.IEnumerable<DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference>? administrativeAreal2DReferences, System.Func<System.Threading.CancellationToken,System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<int,DiGi.GIS.PostgreSQL.Classes.OrtoDataAcquisitionCoverageResult>?>>? ortoDataAcquisitionCoverageResultsAsync=null);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow.PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOrtoDatasAcquisitionUpdateOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,System.Func_System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.Dictionary_int,DiGi.GIS.PostgreSQL.Classes.OrtoDataAcquisitionCoverageResult___).postgreSQLOrtoDatasAcquisitionUpdateOptions'></a>

`postgreSQLOrtoDatasAcquisitionUpdateOptions` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOrtoDatasAcquisitionUpdateOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlortodatasacquisitionupdateoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOrtoDatasAcquisitionUpdateOptions')

The options the controls are filled from\. When null the defaults are used\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow.PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOrtoDatasAcquisitionUpdateOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,System.Func_System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.Dictionary_int,DiGi.GIS.PostgreSQL.Classes.OrtoDataAcquisitionCoverageResult___).administrativeAreal2DReferences'></a>

`administrativeAreal2DReferences` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The county parts to choose from\. A county whose territory is in several pieces is one entry per piece, each with its own identifier\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow.PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOrtoDatasAcquisitionUpdateOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,System.Func_System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.Dictionary_int,DiGi.GIS.PostgreSQL.Classes.OrtoDataAcquisitionCoverageResult___).ortoDataAcquisitionCoverageResultsAsync'></a>

`ortoDataAcquisitionCoverageResultsAsync` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Collections\.Generic\.Dictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[DiGi\.GIS\.PostgreSQL\.Classes\.OrtoDataAcquisitionCoverageResult](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.ortodataacquisitioncoverageresult 'DiGi\.GIS\.PostgreSQL\.Classes\.OrtoDataAcquisitionCoverageResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

Reads how far the backfill has reached in each county part, keyed by part id, for the series, the backlog and the dated images shown beside each part \(DiGi\.GIS\.PostgreSQL\#134\)\. Started here and never awaited before the window opens: the county list is shown at once, with a progress bar in the bottom\-left corner, and the figures are filled in when the read completes\. Closing the window cancels it\. When null nothing is read, no part is annotated and the progress bar is never shown\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow.PostgreSQLOrtoDatasAcquisitionUpdateOptions'></a>

## PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow\.PostgreSQLOrtoDatasAcquisitionUpdateOptions Property

Gets the options the window holds\. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with\.

```csharp
public DiGi.GIS.PostgreSQL.Classes.PostgreSQLOrtoDatasAcquisitionUpdateOptions PostgreSQLOrtoDatasAcquisitionUpdateOptions { get; }
```

#### Property Value
[DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOrtoDatasAcquisitionUpdateOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlortodatasacquisitionupdateoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOrtoDatasAcquisitionUpdateOptions')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow.InitializeComponent()'></a>

## PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow\.InitializeComponent\(\) Method

InitializeComponent

```csharp
public void InitializeComponent();
```

Implements [InitializeComponent\(\)](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector.initializecomponent 'System\.Windows\.Markup\.IComponentConnector\.InitializeComponent')

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow.SetOrtoDataAcquisitionCoverageResults(System.Collections.Generic.IReadOnlyDictionary_int,DiGi.GIS.PostgreSQL.Classes.OrtoDataAcquisitionCoverageResult_)'></a>

## PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow\.SetOrtoDataAcquisitionCoverageResults\(IReadOnlyDictionary\<int,OrtoDataAcquisitionCoverageResult\>\) Method

Sets the acquisition\-date coverage shown beside the county parts, and redraws the county list in place\.

The selection and the scroll position are kept: the coverage normally arrives after the window is open, possibly while parts are being picked. Null means the coverage could not be read, which annotates nothing and says so - a bare part is never a measured one. A part absent from a dictionary that was read holds no OrtoDatas series, and says so. Either way the read is settled, so the progress bar is hidden.

```csharp
public void SetOrtoDataAcquisitionCoverageResults(System.Collections.Generic.IReadOnlyDictionary<int,DiGi.GIS.PostgreSQL.Classes.OrtoDataAcquisitionCoverageResult>? ortoDataAcquisitionCoverageResults);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow.SetOrtoDataAcquisitionCoverageResults(System.Collections.Generic.IReadOnlyDictionary_int,DiGi.GIS.PostgreSQL.Classes.OrtoDataAcquisitionCoverageResult_).ortoDataAcquisitionCoverageResults'></a>

`ortoDataAcquisitionCoverageResults` [System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[DiGi\.GIS\.PostgreSQL\.Classes\.OrtoDataAcquisitionCoverageResult](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.ortodataacquisitioncoverageresult 'DiGi\.GIS\.PostgreSQL\.Classes\.OrtoDataAcquisitionCoverageResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')

The coverage keyed by county part id, or null when it could not be read\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLOrtoSheetUpdateOptionsWindow'></a>

## PostgreSQLOrtoSheetUpdateOptionsWindow Class

Interaction logic for PostgreSQLOrtoSheetUpdateOptionsWindow\.xaml

Asks for the index years an orthophoto sheet import reads and for its pacing against the GUGiK index service. The whole index (49 years, 1957-2026, 1 337 341 rows on 2026-11-03) took 23 minutes to read; a single year is a quick trial.

The settings can be exported to a JSON file and imported again (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#62), so a run repeated after the tray was restarted is not typed again. Importing only fills the controls: nothing runs until OK, and OK validates the imported values like typed ones.

The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.

```csharp
public class PostgreSQLOrtoSheetUpdateOptionsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [System\.Windows\.Threading\.DispatcherObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcherobject 'System\.Windows\.Threading\.DispatcherObject') → [System\.Windows\.DependencyObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.dependencyobject 'System\.Windows\.DependencyObject') → [System\.Windows\.Media\.Visual](https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.visual 'System\.Windows\.Media\.Visual') → [System\.Windows\.UIElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.uielement 'System\.Windows\.UIElement') → [System\.Windows\.FrameworkElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement 'System\.Windows\.FrameworkElement') → [System\.Windows\.Controls\.Control](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.control 'System\.Windows\.Controls\.Control') → [System\.Windows\.Controls\.ContentControl](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.contentcontrol 'System\.Windows\.Controls\.ContentControl') → [System\.Windows\.Window](https://learn.microsoft.com/en-us/dotnet/api/system.windows.window 'System\.Windows\.Window') → PostgreSQLOrtoSheetUpdateOptionsWindow

Implements [System\.Windows\.Markup\.IComponentConnector](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector 'System\.Windows\.Markup\.IComponentConnector')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLOrtoSheetUpdateOptionsWindow.PostgreSQLOrtoSheetUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOrtoSheetUpdateOptions)'></a>

## PostgreSQLOrtoSheetUpdateOptionsWindow\(PostgreSQLOrtoSheetUpdateOptions\) Constructor

Initializes a new instance of the [PostgreSQLOrtoSheetUpdateOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLOrtoSheetUpdateOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.PostgreSQLOrtoSheetUpdateOptionsWindow') class\.

```csharp
public PostgreSQLOrtoSheetUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOrtoSheetUpdateOptions? postgreSQLOrtoSheetUpdateOptions);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLOrtoSheetUpdateOptionsWindow.PostgreSQLOrtoSheetUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOrtoSheetUpdateOptions).postgreSQLOrtoSheetUpdateOptions'></a>

`postgreSQLOrtoSheetUpdateOptions` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOrtoSheetUpdateOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlortosheetupdateoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOrtoSheetUpdateOptions')

The options the controls are filled from\. When null the defaults are used\.
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLOrtoSheetUpdateOptionsWindow.PostgreSQLOrtoSheetUpdateOptions'></a>

## PostgreSQLOrtoSheetUpdateOptionsWindow\.PostgreSQLOrtoSheetUpdateOptions Property

Gets the options the window holds\. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with\.

```csharp
public DiGi.GIS.PostgreSQL.Classes.PostgreSQLOrtoSheetUpdateOptions PostgreSQLOrtoSheetUpdateOptions { get; }
```

#### Property Value
[DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOrtoSheetUpdateOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlortosheetupdateoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOrtoSheetUpdateOptions')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLOrtoSheetUpdateOptionsWindow.InitializeComponent()'></a>

## PostgreSQLOrtoSheetUpdateOptionsWindow\.InitializeComponent\(\) Method

InitializeComponent

```csharp
public void InitializeComponent();
```

Implements [InitializeComponent\(\)](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector.initializecomponent 'System\.Windows\.Markup\.IComponentConnector\.InitializeComponent')

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLOrtoSheetUpdateOptionsWindow.SetControls(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOrtoSheetUpdateOptions)'></a>

## PostgreSQLOrtoSheetUpdateOptionsWindow\.SetControls\(PostgreSQLOrtoSheetUpdateOptions\) Method

Fills every control from the options \- on opening, and again on an import, so a value the options leave
out clears its control rather than keeping what was typed before\. Internal so a fact can fill a window
from a settings file the way Import does, without a file dialog\.

```csharp
internal void SetControls(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOrtoSheetUpdateOptions postgreSQLOrtoSheetUpdateOptions);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLOrtoSheetUpdateOptionsWindow.SetControls(DiGi.GIS.PostgreSQL.Classes.PostgreSQLOrtoSheetUpdateOptions).postgreSQLOrtoSheetUpdateOptions'></a>

`postgreSQLOrtoSheetUpdateOptions` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOrtoSheetUpdateOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlortosheetupdateoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOrtoSheetUpdateOptions')

The options to show\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLOrtoSheetUpdateOptionsWindow.TryGetOptions(string)'></a>

## PostgreSQLOrtoSheetUpdateOptionsWindow\.TryGetOptions\(string\) Method

Reads every control into the window's options and checks them, without closing the window or showing a message \- the part of OK a fact can drive, because a message box would hang it\.

The options are written only when every control reads and passes the full set of rules OK has always applied; a refusal leaves them as they were.

```csharp
internal bool TryGetOptions(out string? message);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLOrtoSheetUpdateOptionsWindow.TryGetOptions(string).message'></a>

`message` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The reason the controls cannot start a run, or null when they can\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
True when the options now hold the controls' values and can start a run\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLOrtoSheetUpdateOptionsWindow.TryReadOptions(bool,DiGi.GIS.PostgreSQL.Classes.PostgreSQLOrtoSheetUpdateOptions,string)'></a>

## PostgreSQLOrtoSheetUpdateOptionsWindow\.TryReadOptions\(bool, PostgreSQLOrtoSheetUpdateOptions, string\) Method

Reads every control into a candidate set of options, writing nothing and showing no message\.

Complete (OK) applies every rule the window has always applied - the years from 1900 on with the last not before the first, a page size of at least one row, a non-negative retry count and delay - and finishes with those messages, so the operator is told while the dialog can still be corrected. Export reads the same controls without the range rules, so a half-prepared window can be saved; what is still refused is what could not be written back as typed: a box that does not hold its number, or a retry delay that is not a finite number. A value out of range is refused by OK when it is read back, exactly as when it is typed.

```csharp
internal bool TryReadOptions(bool complete, out DiGi.GIS.PostgreSQL.Classes.PostgreSQLOrtoSheetUpdateOptions? options, out string? message);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLOrtoSheetUpdateOptionsWindow.TryReadOptions(bool,DiGi.GIS.PostgreSQL.Classes.PostgreSQLOrtoSheetUpdateOptions,string).complete'></a>

`complete` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

True for OK: every rule applies\. False for an export: an unfinished window is saved as it stands\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLOrtoSheetUpdateOptionsWindow.TryReadOptions(bool,DiGi.GIS.PostgreSQL.Classes.PostgreSQLOrtoSheetUpdateOptions,string).options'></a>

`options` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOrtoSheetUpdateOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlortosheetupdateoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOrtoSheetUpdateOptions')

The options the controls describe, or null when they could not be read\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLOrtoSheetUpdateOptionsWindow.TryReadOptions(bool,DiGi.GIS.PostgreSQL.Classes.PostgreSQLOrtoSheetUpdateOptions,string).message'></a>

`message` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The reason the controls could not be read, or null when they could\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
True when the controls read into options\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLOrtoSheetUpdateOptionsWindow.TryReadSettings(string,string)'></a>

## PostgreSQLOrtoSheetUpdateOptionsWindow\.TryReadSettings\(string, string\) Method

Reads the settings an exported file holds\. Exactly one orthophoto sheet index options object is accepted: a
file of another window's settings carries a different type name and cannot be read as this one, and a
file holding several objects describes several settings, none of which is this window's\.

```csharp
internal static DiGi.GIS.PostgreSQL.Classes.PostgreSQLOrtoSheetUpdateOptions? TryReadSettings(string? json, out string? message);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLOrtoSheetUpdateOptionsWindow.TryReadSettings(string,string).json'></a>

`json` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The file content\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLOrtoSheetUpdateOptionsWindow.TryReadSettings(string,string).message'></a>

`message` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The reason nothing was read, or null when exactly one setting was\.

#### Returns
[DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOrtoSheetUpdateOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlortosheetupdateoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLOrtoSheetUpdateOptions')  
The setting the file holds, or null when it holds none\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointFillGapsOptionsWindow'></a>

## PostgreSQLTerrainPointFillGapsOptionsWindow Class

Interaction logic for PostgreSQLTerrainPointFillGapsOptionsWindow\.xaml

Asks for the two settings that decide what a repair covers - the spacing the counties were sampled at, and the counties to measure. Every other option of the instance it was given is carried over untouched: the origin of the lattice and the tile size are what let this agree with the run it is repairing, and they are not settings to change from a dialog.

The spacing is the one that has to be right. Set finer than a county actually holds, every node in between reads as a gap and the repair turns into a densification of the whole country, so the equivalent spacing measured for each county is shown beside it.

The settings can be exported to a JSON file and imported again (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#64), so a run repeated after the tray was restarted is not typed again. Importing only fills the controls: nothing runs until OK, and OK validates the imported values like typed ones.

The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.

```csharp
public class PostgreSQLTerrainPointFillGapsOptionsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [System\.Windows\.Threading\.DispatcherObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcherobject 'System\.Windows\.Threading\.DispatcherObject') → [System\.Windows\.DependencyObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.dependencyobject 'System\.Windows\.DependencyObject') → [System\.Windows\.Media\.Visual](https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.visual 'System\.Windows\.Media\.Visual') → [System\.Windows\.UIElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.uielement 'System\.Windows\.UIElement') → [System\.Windows\.FrameworkElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement 'System\.Windows\.FrameworkElement') → [System\.Windows\.Controls\.Control](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.control 'System\.Windows\.Controls\.Control') → [System\.Windows\.Controls\.ContentControl](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.contentcontrol 'System\.Windows\.Controls\.ContentControl') → [System\.Windows\.Window](https://learn.microsoft.com/en-us/dotnet/api/system.windows.window 'System\.Windows\.Window') → PostgreSQLTerrainPointFillGapsOptionsWindow

Implements [System\.Windows\.Markup\.IComponentConnector](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector 'System\.Windows\.Markup\.IComponentConnector')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointFillGapsOptionsWindow.PostgreSQLTerrainPointFillGapsOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointFillGapsOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,System.Func_System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.Dictionary_int,DiGi.GIS.PostgreSQL.Classes.TerrainPointDensityResult___)'></a>

## PostgreSQLTerrainPointFillGapsOptionsWindow\(PostgreSQLTerrainPointFillGapsOptions, IEnumerable\<AdministrativeAreal2DReference\>, Func\<CancellationToken,Task\<Dictionary\<int,TerrainPointDensityResult\>\>\>\) Constructor

Initializes a new instance of the [PostgreSQLTerrainPointFillGapsOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointFillGapsOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.PostgreSQLTerrainPointFillGapsOptionsWindow') class\.

```csharp
public PostgreSQLTerrainPointFillGapsOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointFillGapsOptions? postgreSQLTerrainPointFillGapsOptions, System.Collections.Generic.IEnumerable<DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference>? administrativeAreal2DReferences, System.Func<System.Threading.CancellationToken,System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<int,DiGi.GIS.PostgreSQL.Classes.TerrainPointDensityResult>?>>? terrainPointDensityResultsAsync=null);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointFillGapsOptionsWindow.PostgreSQLTerrainPointFillGapsOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointFillGapsOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,System.Func_System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.Dictionary_int,DiGi.GIS.PostgreSQL.Classes.TerrainPointDensityResult___).postgreSQLTerrainPointFillGapsOptions'></a>

`postgreSQLTerrainPointFillGapsOptions` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointFillGapsOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlterrainpointfillgapsoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointFillGapsOptions')

The options the controls are filled from\. When null the defaults are used\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointFillGapsOptionsWindow.PostgreSQLTerrainPointFillGapsOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointFillGapsOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,System.Func_System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.Dictionary_int,DiGi.GIS.PostgreSQL.Classes.TerrainPointDensityResult___).administrativeAreal2DReferences'></a>

`administrativeAreal2DReferences` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The counties to choose from\. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointFillGapsOptionsWindow.PostgreSQLTerrainPointFillGapsOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointFillGapsOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,System.Func_System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.Dictionary_int,DiGi.GIS.PostgreSQL.Classes.TerrainPointDensityResult___).terrainPointDensityResultsAsync'></a>

`terrainPointDensityResultsAsync` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Collections\.Generic\.Dictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[DiGi\.GIS\.PostgreSQL\.Classes\.TerrainPointDensityResult](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.terrainpointdensityresult 'DiGi\.GIS\.PostgreSQL\.Classes\.TerrainPointDensityResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

Reads the density measurements of the county partitions, keyed by part id, for the point count, density and equivalent spacing shown beside each county\. Started here and never awaited before the window opens: the county list is shown at once, with a progress bar in the bottom\-left corner, and the figures are filled in when the read completes \(DiGi\.GIS\.PostgreSQL\.UI\#41\)\. Closing the window cancels it\. When null nothing is read, no county is annotated and the progress bar is never shown\.
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

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointFillGapsOptionsWindow.SetControls(DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointFillGapsOptions)'></a>

## PostgreSQLTerrainPointFillGapsOptionsWindow\.SetControls\(PostgreSQLTerrainPointFillGapsOptions\) Method

Fills every control from the options \- on opening, and again on an import, so a value the options leave
out clears its control rather than keeping what was typed before: options with no county list clear the
selection\. The members this window has no control for are left alone\. Internal so a fact can fill a
window from a settings file the way Import does, without a file dialog\.

```csharp
internal void SetControls(DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointFillGapsOptions postgreSQLTerrainPointFillGapsOptions);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointFillGapsOptionsWindow.SetControls(DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointFillGapsOptions).postgreSQLTerrainPointFillGapsOptions'></a>

`postgreSQLTerrainPointFillGapsOptions` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointFillGapsOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlterrainpointfillgapsoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointFillGapsOptions')

The options to show\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointFillGapsOptionsWindow.SetTerrainPointDensityResults(System.Collections.Generic.IReadOnlyDictionary_int,DiGi.GIS.PostgreSQL.Classes.TerrainPointDensityResult_)'></a>

## PostgreSQLTerrainPointFillGapsOptionsWindow\.SetTerrainPointDensityResults\(IReadOnlyDictionary\<int,TerrainPointDensityResult\>\) Method

Sets the terrain point densities shown beside the counties, and redraws the county list in place\.

The selection and the scroll position are kept: the densities normally arrive after the window is open, possibly while counties are being picked. Null means the densities could not be read, which annotates nothing and says so - a bare county is never a measured one. Either way the read is settled, so the progress bar is hidden.

```csharp
public void SetTerrainPointDensityResults(System.Collections.Generic.IReadOnlyDictionary<int,DiGi.GIS.PostgreSQL.Classes.TerrainPointDensityResult>? terrainPointDensityResults);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointFillGapsOptionsWindow.SetTerrainPointDensityResults(System.Collections.Generic.IReadOnlyDictionary_int,DiGi.GIS.PostgreSQL.Classes.TerrainPointDensityResult_).terrainPointDensityResults'></a>

`terrainPointDensityResults` [System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[DiGi\.GIS\.PostgreSQL\.Classes\.TerrainPointDensityResult](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.terrainpointdensityresult 'DiGi\.GIS\.PostgreSQL\.Classes\.TerrainPointDensityResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')

The densities keyed by county part id, or null when they could not be read\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointFillGapsOptionsWindow.TryGetOptions(string)'></a>

## PostgreSQLTerrainPointFillGapsOptionsWindow\.TryGetOptions\(string\) Method

Reads every control into the window's options and checks them, without closing the window or showing a message \- the part of OK a fact can drive, because a message box would hang it\.

The options are written only when every control reads, the grid size is greater than zero and at least one county is selected; a refusal leaves them as they were. The members the window has no control for keep the values it was opened with.

```csharp
internal bool TryGetOptions(out string? message);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointFillGapsOptionsWindow.TryGetOptions(string).message'></a>

`message` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The reason the controls cannot start a run, or null when they can\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
True when the options now hold the controls' values and can start a run\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointFillGapsOptionsWindow.TryReadOptions(bool,DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointFillGapsOptions,string)'></a>

## PostgreSQLTerrainPointFillGapsOptionsWindow\.TryReadOptions\(bool, PostgreSQLTerrainPointFillGapsOptions, string\) Method

Reads every control into a candidate set of options, writing nothing and showing no message\.

Complete (OK) requires a grid size greater than zero and at least one county, and finishes with the messages OK has always given. Export reads the same controls without those requirements - no county selected is written as no county list, never as an empty set - so a half-prepared window can be saved; what is still refused is what could not be written back as typed: a grid size box that does not hold a finite number. The candidate is built on a copy of the window's options, so the members with no control (the lattice origin, the tile size, the batch and retry settings) travel in the file exactly as the copy holds them and are never changed by a read.

```csharp
internal bool TryReadOptions(bool complete, out DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointFillGapsOptions? options, out string? message);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointFillGapsOptionsWindow.TryReadOptions(bool,DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointFillGapsOptions,string).complete'></a>

`complete` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

True for OK: the grid size range and a county are required\. False for an export: an unfinished window is saved as it stands\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointFillGapsOptionsWindow.TryReadOptions(bool,DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointFillGapsOptions,string).options'></a>

`options` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointFillGapsOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlterrainpointfillgapsoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointFillGapsOptions')

The options the controls describe, or null when they could not be read\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointFillGapsOptionsWindow.TryReadOptions(bool,DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointFillGapsOptions,string).message'></a>

`message` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The reason the controls could not be read, or null when they could\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
True when the controls read into options\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointFillGapsOptionsWindow.TryReadSettings(string,string)'></a>

## PostgreSQLTerrainPointFillGapsOptionsWindow\.TryReadSettings\(string, string\) Method

Reads the settings an exported file holds\. Exactly one terrain point gap options object is accepted: a
file of another window's settings carries a different type name and cannot be read as this one, and a
file holding several objects describes several settings, none of which is this window's\.

```csharp
internal static DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointFillGapsOptions? TryReadSettings(string? json, out string? message);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointFillGapsOptionsWindow.TryReadSettings(string,string).json'></a>

`json` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The file content\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointFillGapsOptionsWindow.TryReadSettings(string,string).message'></a>

`message` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The reason nothing was read, or null when exactly one setting was\.

#### Returns
[DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointFillGapsOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlterrainpointfillgapsoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointFillGapsOptions')  
The setting the file holds, or null when it holds none\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointUpdateOptionsWindow'></a>

## PostgreSQLTerrainPointUpdateOptionsWindow Class

Interaction logic for PostgreSQLTerrainPointUpdateOptionsWindow\.xaml

Asks for the three settings that decide what a terrain point run costs and covers - the spacing of the sampling grid, whether points already stored are sampled again, and the counties to sample. Every other option of the instance it was given is carried over untouched, which is deliberate: the origin of the grid and the tile size are what let separate runs share their points, and they are not settings to change from a dialog.

The settings can be exported to a JSON file and imported again (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#65), so a run repeated after the tray was restarted is not typed again. Importing only fills the controls: nothing runs until OK, and OK validates the imported values like typed ones.

The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.

```csharp
public class PostgreSQLTerrainPointUpdateOptionsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [System\.Windows\.Threading\.DispatcherObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcherobject 'System\.Windows\.Threading\.DispatcherObject') → [System\.Windows\.DependencyObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.dependencyobject 'System\.Windows\.DependencyObject') → [System\.Windows\.Media\.Visual](https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.visual 'System\.Windows\.Media\.Visual') → [System\.Windows\.UIElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.uielement 'System\.Windows\.UIElement') → [System\.Windows\.FrameworkElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement 'System\.Windows\.FrameworkElement') → [System\.Windows\.Controls\.Control](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.control 'System\.Windows\.Controls\.Control') → [System\.Windows\.Controls\.ContentControl](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.contentcontrol 'System\.Windows\.Controls\.ContentControl') → [System\.Windows\.Window](https://learn.microsoft.com/en-us/dotnet/api/system.windows.window 'System\.Windows\.Window') → PostgreSQLTerrainPointUpdateOptionsWindow

Implements [System\.Windows\.Markup\.IComponentConnector](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector 'System\.Windows\.Markup\.IComponentConnector')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointUpdateOptionsWindow.PostgreSQLTerrainPointUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointUpdateOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,System.Func_System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.Dictionary_int,DiGi.GIS.PostgreSQL.Classes.TerrainPointDensityResult___)'></a>

## PostgreSQLTerrainPointUpdateOptionsWindow\(PostgreSQLTerrainPointUpdateOptions, IEnumerable\<AdministrativeAreal2DReference\>, Func\<CancellationToken,Task\<Dictionary\<int,TerrainPointDensityResult\>\>\>\) Constructor

Initializes a new instance of the [PostgreSQLTerrainPointUpdateOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointUpdateOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.PostgreSQLTerrainPointUpdateOptionsWindow') class\.

```csharp
public PostgreSQLTerrainPointUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointUpdateOptions? postgreSQLTerrainPointUpdateOptions, System.Collections.Generic.IEnumerable<DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference>? administrativeAreal2DReferences, System.Func<System.Threading.CancellationToken,System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<int,DiGi.GIS.PostgreSQL.Classes.TerrainPointDensityResult>?>>? terrainPointDensityResultsAsync=null);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointUpdateOptionsWindow.PostgreSQLTerrainPointUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointUpdateOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,System.Func_System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.Dictionary_int,DiGi.GIS.PostgreSQL.Classes.TerrainPointDensityResult___).postgreSQLTerrainPointUpdateOptions'></a>

`postgreSQLTerrainPointUpdateOptions` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointUpdateOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlterrainpointupdateoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointUpdateOptions')

The options the controls are filled from\. When null the defaults are used\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointUpdateOptionsWindow.PostgreSQLTerrainPointUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointUpdateOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,System.Func_System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.Dictionary_int,DiGi.GIS.PostgreSQL.Classes.TerrainPointDensityResult___).administrativeAreal2DReferences'></a>

`administrativeAreal2DReferences` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The counties to choose from\. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointUpdateOptionsWindow.PostgreSQLTerrainPointUpdateOptionsWindow(DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointUpdateOptions,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_,System.Func_System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.Dictionary_int,DiGi.GIS.PostgreSQL.Classes.TerrainPointDensityResult___).terrainPointDensityResultsAsync'></a>

`terrainPointDensityResultsAsync` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Collections\.Generic\.Dictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[DiGi\.GIS\.PostgreSQL\.Classes\.TerrainPointDensityResult](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.terrainpointdensityresult 'DiGi\.GIS\.PostgreSQL\.Classes\.TerrainPointDensityResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

Reads the density measurements of the county partitions, keyed by part id, for the point count, density and equivalent spacing shown beside each county\. Started here and never awaited before the window opens: the county list is shown at once, with a progress bar in the bottom\-left corner, and the figures are filled in when the read completes \(DiGi\.GIS\.PostgreSQL\.UI\#41\)\. Closing the window cancels it\. When null nothing is read, no county is annotated and the progress bar is never shown\.
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

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointUpdateOptionsWindow.SetControls(DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointUpdateOptions)'></a>

## PostgreSQLTerrainPointUpdateOptionsWindow\.SetControls\(PostgreSQLTerrainPointUpdateOptions\) Method

Fills every control from the options \- on opening, and again on an import, so a value the options leave
out clears its control rather than keeping what was typed before: options with no county list clear the
selection\. The members this window has no control for are left alone\. Internal so a fact can fill a
window from a settings file the way Import does, without a file dialog\.

```csharp
internal void SetControls(DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointUpdateOptions postgreSQLTerrainPointUpdateOptions);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointUpdateOptionsWindow.SetControls(DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointUpdateOptions).postgreSQLTerrainPointUpdateOptions'></a>

`postgreSQLTerrainPointUpdateOptions` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointUpdateOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlterrainpointupdateoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointUpdateOptions')

The options to show\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointUpdateOptionsWindow.SetTerrainPointDensityResults(System.Collections.Generic.IReadOnlyDictionary_int,DiGi.GIS.PostgreSQL.Classes.TerrainPointDensityResult_)'></a>

## PostgreSQLTerrainPointUpdateOptionsWindow\.SetTerrainPointDensityResults\(IReadOnlyDictionary\<int,TerrainPointDensityResult\>\) Method

Sets the terrain point densities shown beside the counties, and redraws the county list in place\.

The selection and the scroll position are kept: the densities normally arrive after the window is open, possibly while counties are being picked. Null means the densities could not be read, which annotates nothing and says so - a bare county is never a measured one. Either way the read is settled, so the progress bar is hidden.

```csharp
public void SetTerrainPointDensityResults(System.Collections.Generic.IReadOnlyDictionary<int,DiGi.GIS.PostgreSQL.Classes.TerrainPointDensityResult>? terrainPointDensityResults);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointUpdateOptionsWindow.SetTerrainPointDensityResults(System.Collections.Generic.IReadOnlyDictionary_int,DiGi.GIS.PostgreSQL.Classes.TerrainPointDensityResult_).terrainPointDensityResults'></a>

`terrainPointDensityResults` [System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[DiGi\.GIS\.PostgreSQL\.Classes\.TerrainPointDensityResult](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.terrainpointdensityresult 'DiGi\.GIS\.PostgreSQL\.Classes\.TerrainPointDensityResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')

The densities keyed by county part id, or null when they could not be read\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointUpdateOptionsWindow.TryGetOptions(string)'></a>

## PostgreSQLTerrainPointUpdateOptionsWindow\.TryGetOptions\(string\) Method

Reads every control into the window's options and checks them, without closing the window or showing a message \- the part of OK a fact can drive, because a message box would hang it\.

The options are written only when every control reads, the grid size is greater than zero and at least one county is selected; a refusal leaves them as they were. The members the window has no control for keep the values it was opened with.

```csharp
internal bool TryGetOptions(out string? message);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointUpdateOptionsWindow.TryGetOptions(string).message'></a>

`message` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The reason the controls cannot start a run, or null when they can\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
True when the options now hold the controls' values and can start a run\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointUpdateOptionsWindow.TryReadOptions(bool,DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointUpdateOptions,string)'></a>

## PostgreSQLTerrainPointUpdateOptionsWindow\.TryReadOptions\(bool, PostgreSQLTerrainPointUpdateOptions, string\) Method

Reads every control into a candidate set of options, writing nothing and showing no message\.

Complete (OK) requires a grid size greater than zero and at least one county, and finishes with the messages OK has always given. Export reads the same controls without those requirements - no county selected is written as no county list, never as an empty set - so a half-prepared window can be saved; what is still refused is what could not be written back as typed: a grid size box that does not hold a finite number. The candidate is built on a copy of the window's options, so the members with no control (the grid origin, the tile size, the batch and retry settings) travel in the file exactly as the copy holds them and are never changed by a read.

```csharp
internal bool TryReadOptions(bool complete, out DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointUpdateOptions? options, out string? message);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointUpdateOptionsWindow.TryReadOptions(bool,DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointUpdateOptions,string).complete'></a>

`complete` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

True for OK: the grid size range and a county are required\. False for an export: an unfinished window is saved as it stands\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointUpdateOptionsWindow.TryReadOptions(bool,DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointUpdateOptions,string).options'></a>

`options` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointUpdateOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlterrainpointupdateoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointUpdateOptions')

The options the controls describe, or null when they could not be read\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointUpdateOptionsWindow.TryReadOptions(bool,DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointUpdateOptions,string).message'></a>

`message` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The reason the controls could not be read, or null when they could\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
True when the controls read into options\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointUpdateOptionsWindow.TryReadSettings(string,string)'></a>

## PostgreSQLTerrainPointUpdateOptionsWindow\.TryReadSettings\(string, string\) Method

Reads the settings an exported file holds\. Exactly one terrain point options object is accepted: a
file of another window's settings carries a different type name and cannot be read as this one, and a
file holding several objects describes several settings, none of which is this window's\.

```csharp
internal static DiGi.GIS.PostgreSQL.Classes.PostgreSQLTerrainPointUpdateOptions? TryReadSettings(string? json, out string? message);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointUpdateOptionsWindow.TryReadSettings(string,string).json'></a>

`json` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The file content\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLTerrainPointUpdateOptionsWindow.TryReadSettings(string,string).message'></a>

`message` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The reason nothing was read, or null when exactly one setting was\.

#### Returns
[DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointUpdateOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlterrainpointupdateoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLTerrainPointUpdateOptions')  
The setting the file holds, or null when it holds none\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUpdateOccupancyOptionsWindow'></a>

## PostgreSQLUpdateOccupancyOptionsWindow Class

Interaction logic for PostgreSQLUpdateOccupancyOptionsWindow\.xaml

Asks which side of the occupancy update runs - the administrative roll-up, the per-building distribution, or both - whether the stored rows are cleared first, and which counties the building side is limited to. The roll-up is a sum over the whole hierarchy and stays nationwide whatever counties are selected.

No county selected means every county, which is what the task has always meant; with counties selected a clear removes only their buildings' rows, so the rest of the table is left as it stands.

The settings can be exported to a JSON file and imported again (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#63), so a run repeated after the tray was restarted is not typed again. Importing only fills the controls: nothing runs until OK, and OK validates the imported values like typed ones.

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

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUpdateOccupancyOptionsWindow.SetControls(DiGi.GIS.PostgreSQL.Classes.PostgreSQLUpdateOccupancyOptions)'></a>

## PostgreSQLUpdateOccupancyOptionsWindow\.SetControls\(PostgreSQLUpdateOccupancyOptions\) Method

Fills every control from the options \- on opening, and again on an import, so a value the options leave
out clears its control rather than keeping what was typed before: options with no county list clear the
selection, which is this window's every\-county state\. Internal so a fact can fill a window from a
settings file the way Import does, without a file dialog\.

```csharp
internal void SetControls(DiGi.GIS.PostgreSQL.Classes.PostgreSQLUpdateOccupancyOptions postgreSQLUpdateOccupancyOptions);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUpdateOccupancyOptionsWindow.SetControls(DiGi.GIS.PostgreSQL.Classes.PostgreSQLUpdateOccupancyOptions).postgreSQLUpdateOccupancyOptions'></a>

`postgreSQLUpdateOccupancyOptions` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLUpdateOccupancyOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlupdateoccupancyoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLUpdateOccupancyOptions')

The options to show\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUpdateOccupancyOptionsWindow.TryGetOptions(string)'></a>

## PostgreSQLUpdateOccupancyOptionsWindow\.TryGetOptions\(string\) Method

Reads every control into the window's options and checks them, without closing the window or showing a message \- the part of OK a fact can drive, because a message box would hang it\.

The options are written only when every control reads and at least one side is selected; a refusal leaves them as they were.

```csharp
internal bool TryGetOptions(out string? message);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUpdateOccupancyOptionsWindow.TryGetOptions(string).message'></a>

`message` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The reason the controls cannot start a run, or null when they can\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
True when the options now hold the controls' values and can start a run\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUpdateOccupancyOptionsWindow.TryReadOptions(bool,DiGi.GIS.PostgreSQL.Classes.PostgreSQLUpdateOccupancyOptions,string)'></a>

## PostgreSQLUpdateOccupancyOptionsWindow\.TryReadOptions\(bool, PostgreSQLUpdateOccupancyOptions, string\) Method

Reads every control into a candidate set of options, writing nothing and showing no message\.

Complete (OK) requires at least one side to be selected. Export reads the same controls without that requirement - a window with neither side checked is saved as it stands - because a half-prepared window can be finished after the next import. This window holds no free numbers, so an export never fails on a value: what it writes is always what could be written back as typed. No county selected is written as no county list (`null`, which the task reads as every county), never as an empty set, on both paths.

```csharp
internal bool TryReadOptions(bool complete, out DiGi.GIS.PostgreSQL.Classes.PostgreSQLUpdateOccupancyOptions? options, out string? message);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUpdateOccupancyOptionsWindow.TryReadOptions(bool,DiGi.GIS.PostgreSQL.Classes.PostgreSQLUpdateOccupancyOptions,string).complete'></a>

`complete` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

True for OK: at least one side has to be selected\. False for an export: an unfinished window is saved as it stands\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUpdateOccupancyOptionsWindow.TryReadOptions(bool,DiGi.GIS.PostgreSQL.Classes.PostgreSQLUpdateOccupancyOptions,string).options'></a>

`options` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLUpdateOccupancyOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlupdateoccupancyoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLUpdateOccupancyOptions')

The options the controls describe, or null when they could not be read\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUpdateOccupancyOptionsWindow.TryReadOptions(bool,DiGi.GIS.PostgreSQL.Classes.PostgreSQLUpdateOccupancyOptions,string).message'></a>

`message` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The reason the controls could not be read, or null when they could\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
True when the controls read into options\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUpdateOccupancyOptionsWindow.TryReadSettings(string,string)'></a>

## PostgreSQLUpdateOccupancyOptionsWindow\.TryReadSettings\(string, string\) Method

Reads the settings an exported file holds\. Exactly one occupancy options object is accepted: a
file of another window's settings carries a different type name and cannot be read as this one, and a
file holding several objects describes several settings, none of which is this window's\.

```csharp
internal static DiGi.GIS.PostgreSQL.Classes.PostgreSQLUpdateOccupancyOptions? TryReadSettings(string? json, out string? message);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUpdateOccupancyOptionsWindow.TryReadSettings(string,string).json'></a>

`json` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The file content\.

<a name='DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUpdateOccupancyOptionsWindow.TryReadSettings(string,string).message'></a>

`message` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The reason nothing was read, or null when exactly one setting was\.

#### Returns
[DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLUpdateOccupancyOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlupdateoccupancyoptions 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLUpdateOccupancyOptions')  
The setting the file holds, or null when it holds none\.

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

<a name='DiGi.GIS.PostgreSQL.UI.Windows.UserSignInWindow'></a>

## UserSignInWindow Class

Interaction logic for UserSignInWindow\.xaml

Asks for the email and password of the user a task writes through the GIS Web API as (DiGi.GIS.PostgreSQL.UI#48). The window only collects them: the task signs in with them on its own thread ([UserTokenStringAsync\(this GISWebAPIManager, string, string, CancellationToken\)](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Query.UserTokenStringAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,string,string,System.Threading.CancellationToken) 'DiGi\.GIS\.PostgreSQL\.UI\.Query\.UserTokenStringAsync\(this DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager, string, string, System\.Threading\.CancellationToken\)')), because a request must not block the user interface thread.

Like [PostgreSQLUserCreateOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.PostgreSQLUserCreateOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.PostgreSQLUserCreateOptionsWindow'), it carries no options object: the password reaches it as text, and an options instance would keep that text alive on the task for the life of the process. The task reads the values, signs in and lets the window go within one run.

```csharp
public class UserSignInWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [System\.Windows\.Threading\.DispatcherObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcherobject 'System\.Windows\.Threading\.DispatcherObject') → [System\.Windows\.DependencyObject](https://learn.microsoft.com/en-us/dotnet/api/system.windows.dependencyobject 'System\.Windows\.DependencyObject') → [System\.Windows\.Media\.Visual](https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.visual 'System\.Windows\.Media\.Visual') → [System\.Windows\.UIElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.uielement 'System\.Windows\.UIElement') → [System\.Windows\.FrameworkElement](https://learn.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement 'System\.Windows\.FrameworkElement') → [System\.Windows\.Controls\.Control](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.control 'System\.Windows\.Controls\.Control') → [System\.Windows\.Controls\.ContentControl](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.contentcontrol 'System\.Windows\.Controls\.ContentControl') → [System\.Windows\.Window](https://learn.microsoft.com/en-us/dotnet/api/system.windows.window 'System\.Windows\.Window') → UserSignInWindow

Implements [System\.Windows\.Markup\.IComponentConnector](https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.icomponentconnector 'System\.Windows\.Markup\.IComponentConnector')
### Constructors

<a name='DiGi.GIS.PostgreSQL.UI.Windows.UserSignInWindow.UserSignInWindow()'></a>

## UserSignInWindow\(\) Constructor

Initializes a new instance of the [UserSignInWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.UserSignInWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.UserSignInWindow') class\.

```csharp
public UserSignInWindow();
```
### Properties

<a name='DiGi.GIS.PostgreSQL.UI.Windows.UserSignInWindow.Email'></a>

## UserSignInWindow\.Email Property

Gets the email to sign in with\. It carries the value of the control only once the dialog has been closed with OK; until then, and after a cancellation, it is null\.

```csharp
public string? Email { get; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.PostgreSQL.UI.Windows.UserSignInWindow.Password'></a>

## UserSignInWindow\.Password Property

Gets the password to sign in with\. It exists only between the dialog being closed with OK and the sign\-in request, and is never written anywhere\.

```csharp
public string? Password { get; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Windows.UserSignInWindow.InitializeComponent()'></a>

## UserSignInWindow\.InitializeComponent\(\) Method

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

Reads the predicted year built coverage of each county part, keyed by part id, for the markers\. Started here and never awaited before the window opens: the county list is shown at once and its markers are filled in when the read completes, whenever that is, with a progress bar in the bottom\-left corner running until it does\. Closing the window cancels it\. When null no coverage is read, nothing is marked and the progress bar is never shown\.
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

Either way the coverage is settled, so the progress bar in the bottom-left corner is hidden.

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

<b>Test areas are kept out of training.</b> The exclusion buffer and the exclusion boxes (DiGi.GIS.PostgreSQL.UI#39) edit [DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingDatasetOptions\.ExclusionBuffer](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.classes.yolotrainingdatasetoptions.exclusionbuffer 'DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingDatasetOptions\.ExclusionBuffer') and [DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingDatasetOptions\.ExclusionBoundingBoxes](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.classes.yolotrainingdatasetoptions.exclusionboundingboxes 'DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingDatasetOptions\.ExclusionBoundingBoxes'), parsed by [TryParseYOLOExclusionBuffer\(string, double, string\)](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Query.TryParseYOLOExclusionBuffer(string,double,string) 'DiGi\.GIS\.PostgreSQL\.UI\.Query\.TryParseYOLOExclusionBuffer\(string, double, string\)') and [TryParseYOLOExclusionBoundingBoxes\(string, List&lt;YOLOExclusionBoundingBox&gt;, string\)](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Query.TryParseYOLOExclusionBoundingBoxes(string,System.Collections.Generic.List_DiGi.GIS.YOLO.UI.Classes.YOLOExclusionBoundingBox_,string) 'DiGi\.GIS\.PostgreSQL\.UI\.Query\.TryParseYOLOExclusionBoundingBoxes\(string, System\.Collections\.Generic\.List\<DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOExclusionBoundingBox\>, string\)'): an empty buffer is the class default, an empty box list excludes nothing, and a malformed line refuses OK with its line number.

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