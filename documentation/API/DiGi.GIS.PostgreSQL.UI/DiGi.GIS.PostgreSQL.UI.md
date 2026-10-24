#### [DiGi\.GIS\.PostgreSQL\.UI](DiGi.GIS.PostgreSQL.UI.Overview.md 'DiGi\.GIS\.PostgreSQL\.UI\.Overview')

## DiGi\.GIS\.PostgreSQL\.UI Namespace
### Classes

<a name='DiGi.GIS.PostgreSQL.UI.Create'></a>

## Create Class

```csharp
public static class Create
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → Create
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Create.GISPostgreSQLConverterManagerConfigurationFile(string)'></a>

## Create\.GISPostgreSQLConverterManagerConfigurationFile\(string\) Method

Creates a new instance of a [GISPostgreSQLConverterManagerConfigurationFile\(string\)](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Create.GISPostgreSQLConverterManagerConfigurationFile(string) 'DiGi\.GIS\.PostgreSQL\.UI\.Create\.GISPostgreSQLConverterManagerConfigurationFile\(string\)') from the specified path or default location\.

```csharp
public static DiGi.GIS.PostgreSQL.UI.Classes.GISPostgreSQLConverterManagerConfigurationFile? GISPostgreSQLConverterManagerConfigurationFile(string? path=null);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Create.GISPostgreSQLConverterManagerConfigurationFile(string).path'></a>

`path` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional path to the configuration file\. If omitted, resolves from the executing assembly's location\.

#### Returns
[GISPostgreSQLConverterManagerConfigurationFile](DiGi.GIS.PostgreSQL.UI.Classes.md#DiGi.GIS.PostgreSQL.UI.Classes.GISPostgreSQLConverterManagerConfigurationFile 'DiGi\.GIS\.PostgreSQL\.UI\.Classes\.GISPostgreSQLConverterManagerConfigurationFile')  
A [GISPostgreSQLConverterManagerConfigurationFile\(string\)](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Create.GISPostgreSQLConverterManagerConfigurationFile(string) 'DiGi\.GIS\.PostgreSQL\.UI\.Create\.GISPostgreSQLConverterManagerConfigurationFile\(string\)') instance if successful; otherwise, null\.

<a name='DiGi.GIS.PostgreSQL.UI.Create.VisualBackgroundTasks(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager,DiGi.User.PostgreSQL.Classes.UserPostgreSQLConverterManager,DiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.PostgreSQL.UI.Enums.Mode,string)'></a>

## Create\.VisualBackgroundTasks\(GISPostgreSQLConverterManager, UserPostgreSQLConverterManager, GISWebAPIManager, Mode, string\) Method

Creates and returns a sorted list of visual background tasks based on the specified operation mode and available managers\.

```csharp
public static System.Collections.Generic.List<DiGi.UI.WPF.Interfaces.IVisualBackgroundTask>? VisualBackgroundTasks(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager? gISPostgreSQLConverterManager, DiGi.User.PostgreSQL.Classes.UserPostgreSQLConverterManager? userPostgreSQLConverterManager, DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, DiGi.GIS.PostgreSQL.UI.Enums.Mode mode, string? yearBuiltPredictionConsoleAppPath=null);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Create.VisualBackgroundTasks(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager,DiGi.User.PostgreSQL.Classes.UserPostgreSQLConverterManager,DiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.PostgreSQL.UI.Enums.Mode,string).gISPostgreSQLConverterManager'></a>

`gISPostgreSQLConverterManager` [DiGi\.GIS\.PostgreSQL\.Classes\.GISPostgreSQLConverterManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.gispostgresqlconvertermanager 'DiGi\.GIS\.PostgreSQL\.Classes\.GISPostgreSQLConverterManager')

The manager responsible for PostgreSQL conversion operations\.

<a name='DiGi.GIS.PostgreSQL.UI.Create.VisualBackgroundTasks(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager,DiGi.User.PostgreSQL.Classes.UserPostgreSQLConverterManager,DiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.PostgreSQL.UI.Enums.Mode,string).userPostgreSQLConverterManager'></a>

`userPostgreSQLConverterManager` [DiGi\.User\.PostgreSQL\.Classes\.UserPostgreSQLConverterManager](https://learn.microsoft.com/en-us/dotnet/api/digi.user.postgresql.classes.userpostgresqlconvertermanager 'DiGi\.User\.PostgreSQL\.Classes\.UserPostgreSQLConverterManager')

The manager holding the converter for the user database, or null where no User\_PostgreSQL\_Main\.conf names one\. Users live in their own database rather than in the one the rest of this application works against\.

<a name='DiGi.GIS.PostgreSQL.UI.Create.VisualBackgroundTasks(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager,DiGi.User.PostgreSQL.Classes.UserPostgreSQLConverterManager,DiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.PostgreSQL.UI.Enums.Mode,string).GISWebAPIManager'></a>

`GISWebAPIManager` [DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.giswebapimanager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager responsible for interacting with the PostgreSQL Web API\.

<a name='DiGi.GIS.PostgreSQL.UI.Create.VisualBackgroundTasks(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager,DiGi.User.PostgreSQL.Classes.UserPostgreSQLConverterManager,DiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.PostgreSQL.UI.Enums.Mode,string).mode'></a>

`mode` [Mode](DiGi.GIS.PostgreSQL.UI.Enums.md#DiGi.GIS.PostgreSQL.UI.Enums.Mode 'DiGi\.GIS\.PostgreSQL\.UI\.Enums\.Mode')

The operation mode \(Server, Client, or both\) that determines which tasks are instantiated\.

<a name='DiGi.GIS.PostgreSQL.UI.Create.VisualBackgroundTasks(DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager,DiGi.User.PostgreSQL.Classes.UserPostgreSQLConverterManager,DiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.PostgreSQL.UI.Enums.Mode,string).yearBuiltPredictionConsoleAppPath'></a>

`yearBuiltPredictionConsoleAppPath` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

An explicit path to the headless Year Built prediction runner, or null to let [YearBuiltPredictionConsoleAppPath\(string, string\)](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Query.YearBuiltPredictionConsoleAppPath(string,string) 'DiGi\.GIS\.PostgreSQL\.UI\.Query\.YearBuiltPredictionConsoleAppPath\(string, string\)') probe for it\. A test supplies one to decide whether that task is offered without deploying the runner, the same seam its resolver already carries\.

#### Returns
[System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[DiGi\.UI\.WPF\.Interfaces\.IVisualBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.ui.wpf.interfaces.ivisualbackgroundtask 'DiGi\.UI\.WPF\.Interfaces\.IVisualBackgroundTask')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')  
A list of [DiGi\.UI\.WPF\.Interfaces\.IVisualBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.ui.wpf.interfaces.ivisualbackgroundtask 'DiGi\.UI\.WPF\.Interfaces\.IVisualBackgroundTask') objects sorted by name, or null if not applicable\.

<a name='DiGi.GIS.PostgreSQL.UI.Create.YOLOTrainingRunOptions(DiGi.GIS.PostgreSQL.UI.Enums.YOLOTrainingScenario,string,DiGi.GIS.YOLO.UI.Classes.YOLOTrainingRunOptions)'></a>

## Create\.YOLOTrainingRunOptions\(YOLOTrainingScenario, string, YOLOTrainingRunOptions\) Method

Creates the options of a tray YOLO detector training run with the defaults of the given scenario applied\.

Starts from a copy of [yOLOTrainingRunOptions](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Create.YOLOTrainingRunOptions(DiGi.GIS.PostgreSQL.UI.Enums.YOLOTrainingScenario,string,DiGi.GIS.YOLO.UI.Classes.YOLOTrainingRunOptions).yOLOTrainingRunOptions 'DiGi\.GIS\.PostgreSQL\.UI\.Create\.YOLOTrainingRunOptions\(DiGi\.GIS\.PostgreSQL\.UI\.Enums\.YOLOTrainingScenario, string, DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingRunOptions\)\.yOLOTrainingRunOptions') - the run the operator had before - or from the class defaults, and overwrites only what the scenario decides, so the counties, the folders, the device and the interpreter survive a switch between scenarios:

[Retrain](DiGi.GIS.PostgreSQL.UI.Enums.md#DiGi.GIS.PostgreSQL.UI.Enums.YOLOTrainingScenario.Retrain 'DiGi\.GIS\.PostgreSQL\.UI\.Enums\.YOLOTrainingScenario\.Retrain'): the start weights are the deployed model.pt, the epoch ceiling is 150, and the dataset folder is resumed ([DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingDatasetOptions\.Resume](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.classes.yolotrainingdatasetoptions.resume 'DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingDatasetOptions\.Resume') on), so the chosen counties are appended to it.

[Fresh](DiGi.GIS.PostgreSQL.UI.Enums.md#DiGi.GIS.PostgreSQL.UI.Enums.YOLOTrainingScenario.Fresh 'DiGi\.GIS\.PostgreSQL\.UI\.Enums\.YOLOTrainingScenario\.Fresh'): the start weights are the pretrained yolo26x.pt, the epoch ceiling is 300, and the dataset folder is not resumed, so the runner refuses a folder that already holds a dataset and the dataset is built fresh.

Both weights files are named relative to the runner and resolved against it here ([ConsoleAppFilePath\(string, string\)](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Query.ConsoleAppFilePath(string,string) 'DiGi\.GIS\.PostgreSQL\.UI\.Query\.ConsoleAppFilePath\(string, string\)')), because the runner resolves a relative start path against its own directory and this application would otherwise check a different file. A file that is not found keeps the absolute path the runner's deployed layout gives it ([ConsoleAppDeployedPath\(string, string\)](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Query.ConsoleAppDeployedPath(string,string) 'DiGi\.GIS\.PostgreSQL\.UI\.Query\.ConsoleAppDeployedPath\(string, string\)')), so the preflight names the place it looked rather than a relative name.

The dialog offers no control for most dataset members, and their class defaults are this surface's answer: the confidence, the split, the label check sizes, the legacy cut-off and the request sizes are the values the runner's README describes. Two defaults are set here rather than taken: [DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingDatasetOptions\.CountOnly](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.classes.yolotrainingdatasetoptions.countonly 'DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingDatasetOptions\.CountOnly') is forced off, because the committed runner template ships it on and a counting run trains nothing; and an empty [DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingRunOptions\.Steps](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.classes.yolotrainingrunoptions.steps 'DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingRunOptions\.Steps') is replaced by all five steps written out, because the runner reads an empty list as "all steps" and the dialog would otherwise show no step ticked for a run that runs every one. [DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingDatasetOptions\.WeightsPaths](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.classes.yolotrainingdatasetoptions.weightspaths 'DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingDatasetOptions\.WeightsPaths') defaults to the incumbent model.pt; the runner adds the candidate itself.

```csharp
public static DiGi.GIS.YOLO.UI.Classes.YOLOTrainingRunOptions YOLOTrainingRunOptions(DiGi.GIS.PostgreSQL.UI.Enums.YOLOTrainingScenario yOLOTrainingScenario, string? consoleAppPath, DiGi.GIS.YOLO.UI.Classes.YOLOTrainingRunOptions? yOLOTrainingRunOptions=null);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Create.YOLOTrainingRunOptions(DiGi.GIS.PostgreSQL.UI.Enums.YOLOTrainingScenario,string,DiGi.GIS.YOLO.UI.Classes.YOLOTrainingRunOptions).yOLOTrainingScenario'></a>

`yOLOTrainingScenario` [YOLOTrainingScenario](DiGi.GIS.PostgreSQL.UI.Enums.md#DiGi.GIS.PostgreSQL.UI.Enums.YOLOTrainingScenario 'DiGi\.GIS\.PostgreSQL\.UI\.Enums\.YOLOTrainingScenario')

The scenario whose defaults are applied\.

<a name='DiGi.GIS.PostgreSQL.UI.Create.YOLOTrainingRunOptions(DiGi.GIS.PostgreSQL.UI.Enums.YOLOTrainingScenario,string,DiGi.GIS.YOLO.UI.Classes.YOLOTrainingRunOptions).consoleAppPath'></a>

`consoleAppPath` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The full path of the runner's executable the weights are resolved against, or null to leave relative names as they are\.

<a name='DiGi.GIS.PostgreSQL.UI.Create.YOLOTrainingRunOptions(DiGi.GIS.PostgreSQL.UI.Enums.YOLOTrainingScenario,string,DiGi.GIS.YOLO.UI.Classes.YOLOTrainingRunOptions).yOLOTrainingRunOptions'></a>

`yOLOTrainingRunOptions` [DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingRunOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.classes.yolotrainingrunoptions 'DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingRunOptions')

The options to start from, which are copied rather than changed; or null to start from the class defaults\.

#### Returns
[DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingRunOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.classes.yolotrainingrunoptions 'DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingRunOptions')  
A new options instance carrying the scenario's defaults\.

<a name='DiGi.GIS.PostgreSQL.UI.Create.YOLOTrainingRunOptionsFile(string,string,bool,string,System.DateTimeOffset,bool)'></a>

## Create\.YOLOTrainingRunOptionsFile\(string, string, bool, string, DateTimeOffset, bool\) Method

Writes a tray training run's options into a new file beside the run folder and never overwrites an existing one \- the file is the record of what a run was asked to do \(DiGi\-Organization/DiGi\.GIS\.PostgreSQL\.UI\#20\)\.

<b>A resume</b> writes `<RunName>.resume-<yyyyMMdd_HHmmss>.YOLOTrainingRunOptions.json`, so the original run's `<RunName>.YOLOTrainingRunOptions.json` is left untouched. Should two resumes start within the same second, `_2`, `_3` ... is appended to the later one.

<b>A run with the Training step</b> writes `<RunName>.YOLOTrainingRunOptions.json`. The preflight has already refused a taken run name, so that file cannot exist; if it does anyway, the run is refused here, naming the path, rather than written under another name.

<b>A run without the Training step</b> always writes `yyyyMMdd_HHmmss.YOLOTrainingRunOptions.json`, whatever the run name box says, because its run name describes nothing it creates. Should two such runs start within the same second, `_2`, `_3` ... is appended to the later one.

The file is opened with [System\.IO\.FileMode\.CreateNew](https://learn.microsoft.com/en-us/dotnet/api/system.io.filemode.createnew 'System\.IO\.FileMode\.CreateNew'), so a file that appears between the check and the write is not overwritten either.

```csharp
public static string? YOLOTrainingRunOptionsFile(string directory, string? runName, bool train, string contents, System.DateTimeOffset dateTimeOffset, bool resume=false);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Create.YOLOTrainingRunOptionsFile(string,string,bool,string,System.DateTimeOffset,bool).directory'></a>

`directory` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The existing directory the file is written into\.

<a name='DiGi.GIS.PostgreSQL.UI.Create.YOLOTrainingRunOptionsFile(string,string,bool,string,System.DateTimeOffset,bool).runName'></a>

`runName` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The run name; used when [train](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Create.YOLOTrainingRunOptionsFile(string,string,bool,string,System.DateTimeOffset,bool).train 'DiGi\.GIS\.PostgreSQL\.UI\.Create\.YOLOTrainingRunOptionsFile\(string, string, bool, string, System\.DateTimeOffset, bool\)\.train') or [resume](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Create.YOLOTrainingRunOptionsFile(string,string,bool,string,System.DateTimeOffset,bool).resume 'DiGi\.GIS\.PostgreSQL\.UI\.Create\.YOLOTrainingRunOptionsFile\(string, string, bool, string, System\.DateTimeOffset, bool\)\.resume') is true\.

<a name='DiGi.GIS.PostgreSQL.UI.Create.YOLOTrainingRunOptionsFile(string,string,bool,string,System.DateTimeOffset,bool).train'></a>

`train` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether the run includes the Training step\.

<a name='DiGi.GIS.PostgreSQL.UI.Create.YOLOTrainingRunOptionsFile(string,string,bool,string,System.DateTimeOffset,bool).contents'></a>

`contents` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The serialized options\.

<a name='DiGi.GIS.PostgreSQL.UI.Create.YOLOTrainingRunOptionsFile(string,string,bool,string,System.DateTimeOffset,bool).dateTimeOffset'></a>

`dateTimeOffset` [System\.DateTimeOffset](https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset 'System\.DateTimeOffset')

The time a run without training, or a resume, is named after\.

<a name='DiGi.GIS.PostgreSQL.UI.Create.YOLOTrainingRunOptionsFile(string,string,bool,string,System.DateTimeOffset,bool).resume'></a>

`resume` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether the run continues an interrupted one; a resume's own file is named after it and the original is never touched\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The path of the written file, or null when a training run's file already exists or no free name was found\.

<a name='DiGi.GIS.PostgreSQL.UI.Query'></a>

## Query Class

```csharp
public static class Query
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → Query
### Fields

<a name='DiGi.GIS.PostgreSQL.UI.Query.Count_AutoResumeMax'></a>

## Query\.Count\_AutoResumeMax Field

Gets the largest number of automatic resumes a tray run may ask for\.

```csharp
private const int Count_AutoResumeMax = 10;
```

#### Field Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')
### Methods

<a name='DiGi.GIS.PostgreSQL.UI.Query.ConsoleAppDeployedPath(string,string)'></a>

## Query\.ConsoleAppDeployedPath\(string, string\) Method

Returns the absolute path a headless runner will use for a path named relative to it, whether or not anything exists there yet \- a folder it writes into, such as `user files/reports`, or a file whose absence a preflight has to name\.

A rooted path is returned through [FullPath\(string\)](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Query.FullPath(string) 'DiGi\.GIS\.PostgreSQL\.UI\.Query\.FullPath\(string\)'). A relative one is placed under the runner's folder with a leading [UserFiles](DiGi.GIS.PostgreSQL.UI.Constants.md#DiGi.GIS.PostgreSQL.UI.Constants.DirectoryName.UserFiles 'DiGi\.GIS\.PostgreSQL\.UI\.Constants\.DirectoryName\.UserFiles') segment removed, which is the layout the build deploys: the git-ignored folder is flattened into the output root, both in a workspace `bin` and on a deployed machine.

Resolved here rather than left to the runner, so that the path written into an options file is the one this application checked and named back - not one the other process resolves against a different directory.

```csharp
public static string? ConsoleAppDeployedPath(string? consoleAppPath, string? path);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Query.ConsoleAppDeployedPath(string,string).consoleAppPath'></a>

`consoleAppPath` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The full path of the runner's executable\.

<a name='DiGi.GIS.PostgreSQL.UI.Query.ConsoleAppDeployedPath(string,string).path'></a>

`path` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The path, absolute or relative to the runner\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The absolute path; the path as [FullPath\(string\)](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Query.FullPath(string) 'DiGi\.GIS\.PostgreSQL\.UI\.Query\.FullPath\(string\)') returns it when there is no runner folder to place it under; or null when it is null or blank\.

<a name='DiGi.GIS.PostgreSQL.UI.Query.ConsoleAppExitCodeAsync(string,System.Collections.Generic.IEnumerable_string_,string,System.IProgress_long_,System.Action_string_,System.Threading.CancellationToken)'></a>

## Query\.ConsoleAppExitCodeAsync\(string, IEnumerable\<string\>, string, IProgress\<long\>, Action\<string\>, CancellationToken\) Method

Runs the headless `DiGi.GIS.YOLO.UI.ConsoleApp` with the given arguments, logs everything it prints, reports its progress lines and returns the exit code it ended with\.

Both output streams are read as they arrive - a child whose output nobody drains blocks on a full pipe, and these runs talk for hours. Every standard output line is logged, and a `[PROGRESS]` line is read through the runner's own [DiGi\.GIS\.YOLO\.UI\.Query\.ProgressCount\(System\.String\)](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.query.progresscount#digi-gis-yolo-ui-query-progresscount(system-string) 'DiGi\.GIS\.YOLO\.UI\.Query\.ProgressCount\(System\.String\)') rather than a format literal, so a format that ever drifts costs the progress reporting and nothing else. Standard error lines are logged as warnings.

The exit code is named rather than numbered, read off the runner's own [DiGi\.GIS\.YOLO\.UI\.Enums\.YearBuiltPredictionExitCode](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.enums.yearbuiltpredictionexitcode 'DiGi\.GIS\.YOLO\.UI\.Enums\.YearBuiltPredictionExitCode'); a code this application does not recognise is logged as the number it is and still returned.

<b>Cancelling kills the whole process tree</b> rather than winding the run down: the interpreter is a grandchild, and killing only the runner would leave it holding a graphics card with nothing waiting for it. Whatever the run was writing may be half written. A cancelled wait returns [DiGi\.GIS\.YOLO\.UI\.Enums\.YearBuiltPredictionExitCode\.Cancelled](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.enums.yearbuiltpredictionexitcode.cancelled 'DiGi\.GIS\.YOLO\.UI\.Enums\.YearBuiltPredictionExitCode\.Cancelled').

```csharp
public static System.Threading.Tasks.Task<System.Nullable<DiGi.GIS.YOLO.UI.Enums.YearBuiltPredictionExitCode>> ConsoleAppExitCodeAsync(string consoleAppPath, System.Collections.Generic.IEnumerable<string>? arguments, string name, System.IProgress<long>? progress=null, System.Action<string>? information=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Query.ConsoleAppExitCodeAsync(string,System.Collections.Generic.IEnumerable_string_,string,System.IProgress_long_,System.Action_string_,System.Threading.CancellationToken).consoleAppPath'></a>

`consoleAppPath` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The full path of the runner's executable\. Its folder is the working directory the runner starts in\.

<a name='DiGi.GIS.PostgreSQL.UI.Query.ConsoleAppExitCodeAsync(string,System.Collections.Generic.IEnumerable_string_,string,System.IProgress_long_,System.Action_string_,System.Threading.CancellationToken).arguments'></a>

`arguments` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The arguments, each passed through [System\.Diagnostics\.ProcessStartInfo\.ArgumentList](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.argumentlist 'System\.Diagnostics\.ProcessStartInfo\.ArgumentList') rather than a quoted string, so a path ending in a separator cannot escape its own closing quote\.

<a name='DiGi.GIS.PostgreSQL.UI.Query.ConsoleAppExitCodeAsync(string,System.Collections.Generic.IEnumerable_string_,string,System.IProgress_long_,System.Action_string_,System.Threading.CancellationToken).name'></a>

`name` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The name of the run the log lines are headed with, for example "Year built prediction"\.

<a name='DiGi.GIS.PostgreSQL.UI.Query.ConsoleAppExitCodeAsync(string,System.Collections.Generic.IEnumerable_string_,string,System.IProgress_long_,System.Action_string_,System.Threading.CancellationToken).progress'></a>

`progress` [System\.IProgress&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')

The receiver of the processed item counts the runner reports, or null\.

<a name='DiGi.GIS.PostgreSQL.UI.Query.ConsoleAppExitCodeAsync(string,System.Collections.Generic.IEnumerable_string_,string,System.IProgress_long_,System.Action_string_,System.Threading.CancellationToken).information'></a>

`information` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

The receiver of every standard output line, in order, or null\. It is called on the output pump, not on the caller's thread\.

<a name='DiGi.GIS.PostgreSQL.UI.Query.ConsoleAppExitCodeAsync(string,System.Collections.Generic.IEnumerable_string_,string,System.IProgress_long_,System.Action_string_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The token that stops the run by killing its process tree\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[DiGi\.GIS\.YOLO\.UI\.Enums\.YearBuiltPredictionExitCode](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.enums.yearbuiltpredictionexitcode 'DiGi\.GIS\.YOLO\.UI\.Enums\.YearBuiltPredictionExitCode')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
The exit code the runner ended with; [DiGi\.GIS\.YOLO\.UI\.Enums\.YearBuiltPredictionExitCode\.Cancelled](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.yolo.ui.enums.yearbuiltpredictionexitcode.cancelled 'DiGi\.GIS\.YOLO\.UI\.Enums\.YearBuiltPredictionExitCode\.Cancelled') when the token stopped it; or null when it could not be started, or failed while it was being watched\.

<a name='DiGi.GIS.PostgreSQL.UI.Query.ConsoleAppFilePath(string,string)'></a>

## Query\.ConsoleAppFilePath\(string, string\) Method

Finds an existing file named relative to a headless runner rather than to this application \- the weights and data files the runner's defaults name, such as `user files/YOLO/models/model.pt`\.

Three candidates in order: the path as given when it is absolute, then the path under the runner's folder, then the path under the runner's folder with a leading [UserFiles](DiGi.GIS.PostgreSQL.UI.Constants.md#DiGi.GIS.PostgreSQL.UI.Constants.DirectoryName.UserFiles 'DiGi\.GIS\.PostgreSQL\.UI\.Constants\.DirectoryName\.UserFiles') segment removed. The build flattens that git-ignored folder into the output root, so a file named through it sits one segment shallower once deployed; the runner's own resolver strips the segment the same way, and this mirrors it rather than guessing.

A relative path is never tried against this application's own current directory: the runner starts in its own folder, so that is the only directory a relative name means anything in, and a file found beside this application would be a different file with the same name.

Only an existing file is returned. A caller that needs a path to write into, or a path to name in a refusal, uses [ConsoleAppDeployedPath\(string, string\)](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Query.ConsoleAppDeployedPath(string,string) 'DiGi\.GIS\.PostgreSQL\.UI\.Query\.ConsoleAppDeployedPath\(string, string\)').

```csharp
public static string? ConsoleAppFilePath(string? consoleAppPath, string? path);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Query.ConsoleAppFilePath(string,string).consoleAppPath'></a>

`consoleAppPath` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The full path of the runner's executable\.

<a name='DiGi.GIS.PostgreSQL.UI.Query.ConsoleAppFilePath(string,string).path'></a>

`path` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The file path, absolute or relative to the runner\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The absolute path of the file found, or null when the path is blank or no candidate exists\.

<a name='DiGi.GIS.PostgreSQL.UI.Query.CountyCode(string)'></a>

## Query\.CountyCode\(string\) Method

Returns the four\-digit county code a PRG address point belongs to\.

The code is the first four digits of the seven-digit TERYT commune code (`TERYTGminy`): voivodeship, county, commune and commune type. It is a code, not a key - a county of disconnected territory holds one row per polygon part, so the identifiers are resolved from it with the `idsbycode` endpoint rather than assumed.

```csharp
public static string? CountyCode(string? TERYTGminy);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Query.CountyCode(string).TERYTGminy'></a>

`TERYTGminy` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The seven\-digit TERYT commune code of an address point\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The four\-digit county code; null when the input is null, shorter than four characters or its first four characters are not digits\.

<a name='DiGi.GIS.PostgreSQL.UI.Query.FullPath(string)'></a>

## Query\.FullPath\(string\) Method

Returns the absolute form of a path this application hands to another process\.

The two processes do not share a working directory - this one runs from wherever the tray application was started, a runner from beside its own executable - so a relative path names a different folder on each side, and neither would report anything wrong. Made absolute here, before it is written into an options file, it means the same thing to both.

A path this machine cannot even form is returned exactly as it was typed, so that the run fails naming what the operator wrote rather than something this method invented from it.

```csharp
public static string? FullPath(string? path);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Query.FullPath(string).path'></a>

`path` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The path to resolve against this process's current directory\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The absolute path; the path unchanged when it cannot be formed; or null when it is null or blank\.

<a name='DiGi.GIS.PostgreSQL.UI.Query.InterruptedYOLOTrainingRun(string,string,string,string,System.Threading.CancellationToken)'></a>

## Query\.InterruptedYOLOTrainingRun\(string, string, string, string, CancellationToken\) Method

Returns what an interrupted training run named by [projectDirectory](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Query.InterruptedYOLOTrainingRun(string,string,string,string,System.Threading.CancellationToken).projectDirectory 'DiGi\.GIS\.PostgreSQL\.UI\.Query\.InterruptedYOLOTrainingRun\(string, string, string, string, System\.Threading\.CancellationToken\)\.projectDirectory') and [runName](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Query.InterruptedYOLOTrainingRun(string,string,string,string,System.Threading.CancellationToken).runName 'DiGi\.GIS\.PostgreSQL\.UI\.Query\.InterruptedYOLOTrainingRun\(string, string, string, string, System\.Threading\.CancellationToken\)\.runName') holds when it can be resumed, or `null` when there is nothing to offer\.

A run is interruptible when its folder exists, holds `weights\last.pt`, has no completed `<RunName>.pt`, and the checkpoint can be read and is unfinished. [DiGi\.YOLO\.Query\.YOLOCheckpointInformation\(System\.String,System\.String,System\.String,System\.Threading\.CancellationToken\)](https://learn.microsoft.com/en-us/dotnet/api/digi.yolo.query.yolocheckpointinformation#digi-yolo-query-yolocheckpointinformation(system-string-system-string-system-string-system-threading-cancellationtoken) 'DiGi\.YOLO\.Query\.YOLOCheckpointInformation\(System\.String,System\.String,System\.String,System\.Threading\.CancellationToken\)') reads the epoch, the ceiling and the recorded arguments; the caller decides what to do with them.

This is the detection [YOLOTrainingOptionsWindow](DiGi.GIS.PostgreSQL.UI.Windows.md#DiGi.GIS.PostgreSQL.UI.Windows.YOLOTrainingOptionsWindow 'DiGi\.GIS\.PostgreSQL\.UI\.Windows\.YOLOTrainingOptionsWindow') offers and the task's preflight repeats, so both agree on which folders are interruptible. It runs the interpreter, so a machine that cannot read a checkpoint simply offers nothing.

```csharp
public static DiGi.YOLO.Classes.YOLOCheckpointInformation? InterruptedYOLOTrainingRun(string? projectDirectory, string? runName, string? pythonPath, string? workingDirectory, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Query.InterruptedYOLOTrainingRun(string,string,string,string,System.Threading.CancellationToken).projectDirectory'></a>

`projectDirectory` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The absolute directory the run folder is created in\.

<a name='DiGi.GIS.PostgreSQL.UI.Query.InterruptedYOLOTrainingRun(string,string,string,string,System.Threading.CancellationToken).runName'></a>

`runName` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The run name: the name of its folder under [projectDirectory](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Query.InterruptedYOLOTrainingRun(string,string,string,string,System.Threading.CancellationToken).projectDirectory 'DiGi\.GIS\.PostgreSQL\.UI\.Query\.InterruptedYOLOTrainingRun\(string, string, string, string, System\.Threading\.CancellationToken\)\.projectDirectory')\.

<a name='DiGi.GIS.PostgreSQL.UI.Query.InterruptedYOLOTrainingRun(string,string,string,string,System.Threading.CancellationToken).pythonPath'></a>

`pythonPath` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The path of the CPython interpreter, a command name on PATH, or `null` to search PATH\.

<a name='DiGi.GIS.PostgreSQL.UI.Query.InterruptedYOLOTrainingRun(string,string,string,string,System.Threading.CancellationToken).workingDirectory'></a>

`workingDirectory` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The directory the checkpoint script is written to and run in, or `null` to use [projectDirectory](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Query.InterruptedYOLOTrainingRun(string,string,string,string,System.Threading.CancellationToken).projectDirectory 'DiGi\.GIS\.PostgreSQL\.UI\.Query\.InterruptedYOLOTrainingRun\(string, string, string, string, System\.Threading\.CancellationToken\)\.projectDirectory')\.

<a name='DiGi.GIS.PostgreSQL.UI.Query.InterruptedYOLOTrainingRun(string,string,string,string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe\.

#### Returns
[DiGi\.YOLO\.Classes\.YOLOCheckpointInformation](https://learn.microsoft.com/en-us/dotnet/api/digi.yolo.classes.yolocheckpointinformation 'DiGi\.YOLO\.Classes\.YOLOCheckpointInformation')  
The checkpoint information of the interrupted run, or `null` when it is not one\.

<a name='DiGi.GIS.PostgreSQL.UI.Query.IsYOLOTrainingStallOptionsValid(int,System.Nullable_System.TimeSpan_,string)'></a>

## Query\.IsYOLOTrainingStallOptionsValid\(int, Nullable\<TimeSpan\>, string\) Method

Returns whether the automatic\-resume count and the stall limit are usable, and the reason when they are not, so the dialog and the task's preflight refuse with the same words\.

The count has to be 0 to [Count\_AutoResumeMax](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Query.Count_AutoResumeMax 'DiGi\.GIS\.PostgreSQL\.UI\.Query\.Count\_AutoResumeMax'): 0 keeps the runner's behaviour of no retries, and above the ceiling a run would retry a fault indefinitely rather than report it. The limit, when given, has to be at least one minute; empty means the runner's default of 15 minutes and is not validated here. A limit of zero would disable the stall detection, which is the opposite of what the box asks for, so it is refused.

```csharp
public static bool IsYOLOTrainingStallOptionsValid(int autoResumeCount, System.Nullable<System.TimeSpan> inactivityTimeout, out string? reason);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Query.IsYOLOTrainingStallOptionsValid(int,System.Nullable_System.TimeSpan_,string).autoResumeCount'></a>

`autoResumeCount` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number of automatic resumes allowed in one run\.

<a name='DiGi.GIS.PostgreSQL.UI.Query.IsYOLOTrainingStallOptionsValid(int,System.Nullable_System.TimeSpan_,string).inactivityTimeout'></a>

`inactivityTimeout` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The span without output after which the training is treated as stalled, or null for the runner's default\.

<a name='DiGi.GIS.PostgreSQL.UI.Query.IsYOLOTrainingStallOptionsValid(int,System.Nullable_System.TimeSpan_,string).reason'></a>

`reason` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The reason the pair cannot be used, or null when it can\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
True when the pair can be used; otherwise false\.

<a name='DiGi.GIS.PostgreSQL.UI.Query.PredictedYearBuiltCoverageFactors(System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.PredictedYearBuiltRunResult_,System.Collections.Generic.IReadOnlyDictionary_int,long_)'></a>

## Query\.PredictedYearBuiltCoverageFactors\(IEnumerable\<PredictedYearBuiltRunResult\>, IReadOnlyDictionary\<int,long\>\) Method

Returns, per county part, the share of its buildings carrying a predicted year built\.

The share is the count of the part's <b>largest</b> run over its building count, never the sum of its runs. A run does not replace the one before it - a building keeps an entry per run - so county 5 carried 10 437 objects from an old run and 33 584 from the current one out of 33 687 buildings, and summing would read 131 %.

```csharp
public static System.Collections.Generic.Dictionary<int,double> PredictedYearBuiltCoverageFactors(System.Collections.Generic.IEnumerable<DiGi.GIS.PostgreSQL.Classes.PredictedYearBuiltRunResult?>? predictedYearBuiltRunResults, System.Collections.Generic.IReadOnlyDictionary<int,long>? building2DCounts);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Query.PredictedYearBuiltCoverageFactors(System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.PredictedYearBuiltRunResult_,System.Collections.Generic.IReadOnlyDictionary_int,long_).predictedYearBuiltRunResults'></a>

`predictedYearBuiltRunResults` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.PredictedYearBuiltRunResult](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.predictedyearbuiltrunresult 'DiGi\.GIS\.PostgreSQL\.Classes\.PredictedYearBuiltRunResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The prediction runs, as `predictedyearbuiltruns` returns them\. Null entries are ignored\.

<a name='DiGi.GIS.PostgreSQL.UI.Query.PredictedYearBuiltCoverageFactors(System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.PredictedYearBuiltRunResult_,System.Collections.Generic.IReadOnlyDictionary_int,long_).building2DCounts'></a>

`building2DCounts` [System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')

The building count of each county part, keyed by part id\.

#### Returns
[System\.Collections\.Generic\.Dictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')  
The share keyed by part id, for every part that has a run and a positive building count \- a part without a count is left out rather than given a share it cannot have\. Empty when either input is null\.

<a name='DiGi.GIS.PostgreSQL.UI.Query.PredictedYearBuiltCoverageFactorsAsync(DiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Threading.CancellationToken)'></a>

## Query\.PredictedYearBuiltCoverageFactorsAsync\(GISWebAPIManager, IEnumerable\<int\>, CancellationToken\) Method

Asynchronously reads, from the deployed Web API, the share of each county part's buildings carrying a predicted year built\.

Two requests whatever the number of parts, sent together: `yearbuiltdata/predictedyearbuiltruns` for the runs and `building2d/estimatedcountsbycountyids` for the building counts, the latter a planner estimate read in one catalogue query. Combined by [PredictedYearBuiltCoverageFactors\(IEnumerable&lt;PredictedYearBuiltRunResult&gt;, IReadOnlyDictionary&lt;int,long&gt;\)](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Query.PredictedYearBuiltCoverageFactors(System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.PredictedYearBuiltRunResult_,System.Collections.Generic.IReadOnlyDictionary_int,long_) 'DiGi\.GIS\.PostgreSQL\.UI\.Query\.PredictedYearBuiltCoverageFactors\(System\.Collections\.Generic\.IEnumerable\<DiGi\.GIS\.PostgreSQL\.Classes\.PredictedYearBuiltRunResult\>, System\.Collections\.Generic\.IReadOnlyDictionary\<int,long\>\)').

Never throws for a failed read and is not retried: whatever goes wrong - an unreachable server, an error status, [CoverageTimeout](DiGi.GIS.PostgreSQL.UI.Constants.md#DiGi.GIS.PostgreSQL.UI.Constants.YearBuiltPrediction.CoverageTimeout 'DiGi\.GIS\.PostgreSQL\.UI\.Constants\.YearBuiltPrediction\.CoverageTimeout') elapsing, a body that does not parse - answers null, so a caller shows no marker rather than a false "not run". Only a cancellation through [cancellationToken](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Query.PredictedYearBuiltCoverageFactorsAsync(DiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Threading.CancellationToken).cancellationToken 'DiGi\.GIS\.PostgreSQL\.UI\.Query\.PredictedYearBuiltCoverageFactorsAsync\(DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager, System\.Collections\.Generic\.IEnumerable\<int\>, System\.Threading\.CancellationToken\)\.cancellationToken') is rethrown.

```csharp
public static System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<int,double>?> PredictedYearBuiltCoverageFactorsAsync(DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, System.Collections.Generic.IEnumerable<int>? countyIds, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Query.PredictedYearBuiltCoverageFactorsAsync(DiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Threading.CancellationToken).GISWebAPIManager'></a>

`GISWebAPIManager` [DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.classes.giswebapimanager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager that creates the HTTP clients\.

<a name='DiGi.GIS.PostgreSQL.UI.Query.PredictedYearBuiltCoverageFactorsAsync(DiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Threading.CancellationToken).countyIds'></a>

`countyIds` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The county part identifiers, one per part of a multi\-part county\.

<a name='DiGi.GIS.PostgreSQL.UI.Query.PredictedYearBuiltCoverageFactorsAsync(DiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Cancels both requests, which on the server cancels the queries behind them\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Collections\.Generic\.Dictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
The share keyed by part id, or null when it could not be read\.

<a name='DiGi.GIS.PostgreSQL.UI.Query.PredictedYearBuiltCoverageMarker(System.Nullable_double_)'></a>

## Query\.PredictedYearBuiltCoverageMarker\(Nullable\<double\>\) Method

Returns the marker shown after a county in the Year Built prediction options when its predicted coverage exceeds [CoverageMarkerThreshold](DiGi.GIS.PostgreSQL.UI.Constants.md#DiGi.GIS.PostgreSQL.UI.Constants.YearBuiltPrediction.CoverageMarkerThreshold 'DiGi\.GIS\.PostgreSQL\.UI\.Constants\.YearBuiltPrediction\.CoverageMarkerThreshold')\.

The text is built from the threshold, invariantly, so the two cannot drift apart and a Polish culture does not turn it into `(>10 %)`.

```csharp
public static string? PredictedYearBuiltCoverageMarker(System.Nullable<double> coverageFactor);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Query.PredictedYearBuiltCoverageMarker(System.Nullable_double_).coverageFactor'></a>

`coverageFactor` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The share of the county part's buildings carrying a predicted year built, as a fraction\. Null when it is not known\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
`(>10%)` when the factor strictly exceeds the threshold; null when it does not, when it is not a number, or when it is unknown \- no marker, never a false "not run"\.

<a name='DiGi.GIS.PostgreSQL.UI.Query.RenderRefusal(string,object[])'></a>

## Query\.RenderRefusal\(string, object\[\]\) Method

Renders a Serilog message template with its positional values into the plain text the task row shows, the same text Serilog writes to the log, so the refusal reason on the row and in the log stay word\-for\-word identical\.

The renderer is shared by every task that names a refusal on its row rather than kept per task: two copies of the token walk would be two places for the rendering rule to drift, and a drift costs nothing at compile time - only a row that no longer matches its own log line.

```csharp
internal static string RenderRefusal(string template, params object[] values);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Query.RenderRefusal(string,object[]).template'></a>

`template` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The Serilog message template, as it is handed to `Serilog.Modify.Log`\.

<a name='DiGi.GIS.PostgreSQL.UI.Query.RenderRefusal(string,object[]).values'></a>

`values` [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The positional values, in the order the template's property tokens appear\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The rendered text\.

<a name='DiGi.GIS.PostgreSQL.UI.Query.YearBuiltPredictionConsoleAppPath(string,string)'></a>

## Query\.YearBuiltPredictionConsoleAppPath\(string, string\) Method

Finds the headless Year Built prediction runner this application hands a run to\.

The runner is a separate deployment unit rather than an assembly this application loads, because hosting the pipeline here would mean referencing the machine learning closure - about a gigabyte of native libraries against an application that publishes self-contained and single-file. The cost of that choice is that the executable has to be found rather than linked, which is what this answers.

Five candidates in order: the path given, then beside this application's own output, then the optional extensions folder inside it, then the runner's own folder beside this one's, then the runner's build output in a workspace checkout. The last is what makes the task runnable from a development machine without deploying anything.

A candidate that does not exist is not returned. A path that only looks resolved would be discovered as a failure to start a process, after the counties had been chosen and the imagery scoped.

```csharp
public static string? YearBuiltPredictionConsoleAppPath(string? path=null, string? baseDirectory=null);
```
#### Parameters

<a name='DiGi.GIS.PostgreSQL.UI.Query.YearBuiltPredictionConsoleAppPath(string,string).path'></a>

`path` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

An explicit path to the runner, or null to search the candidates below it\.

<a name='DiGi.GIS.PostgreSQL.UI.Query.YearBuiltPredictionConsoleAppPath(string,string).baseDirectory'></a>

`baseDirectory` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The directory the candidates are resolved against, or null to use this application's own output\. A test supplies one to probe a laid\-out folder without deploying\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The full path of an executable that exists, or null when none of the candidates does\.