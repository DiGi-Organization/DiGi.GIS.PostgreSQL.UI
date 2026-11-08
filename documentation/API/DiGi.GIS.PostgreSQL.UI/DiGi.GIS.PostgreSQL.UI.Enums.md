#### [DiGi\.GIS\.PostgreSQL\.UI](DiGi.GIS.PostgreSQL.UI.Overview.md 'DiGi\.GIS\.PostgreSQL\.UI\.Overview')

## DiGi\.GIS\.PostgreSQL\.UI\.Enums Namespace
### Enums

<a name='DiGi.GIS.PostgreSQL.UI.Enums.Mode'></a>

## Mode Enum

Specifies the operational mode for the GIS PostgreSQL UI\.

```csharp
public enum Mode
```
### Fields

<a name='DiGi.GIS.PostgreSQL.UI.Enums.Mode.Server'></a>

`Server` 0

Indicates that the operation is performed on the server side\.

<a name='DiGi.GIS.PostgreSQL.UI.Enums.Mode.Client'></a>

`Client` 1

Indicates that the operation is performed on the client side\.

<a name='DiGi.GIS.PostgreSQL.UI.Enums.Mode.ServerAndClient'></a>

`ServerAndClient` 2

Indicates that the operation is performed on both the server and client sides\.

<a name='DiGi.GIS.PostgreSQL.UI.Enums.YOLOTrainingScenario'></a>

## YOLOTrainingScenario Enum

Specifies where a YOLO detector training run started from the tray starts: from the detector already in use, or from the pretrained base checkpoint\.

The scenario decides only the defaults [YOLOTrainingRunOptions\(YOLOTrainingScenario, string, YOLOTrainingRunOptions\)](DiGi.GIS.PostgreSQL.UI.md#DiGi.GIS.PostgreSQL.UI.Create.YOLOTrainingRunOptions(DiGi.GIS.PostgreSQL.UI.Enums.YOLOTrainingScenario,string,DiGi.GIS.YOLO.UI.Classes.YOLOTrainingRunOptions) 'DiGi\.GIS\.PostgreSQL\.UI\.Create\.YOLOTrainingRunOptions\(DiGi\.GIS\.PostgreSQL\.UI\.Enums\.YOLOTrainingScenario, string, DiGi\.GIS\.YOLO\.UI\.Classes\.YOLOTrainingRunOptions\)') applies - the start weights, the epoch ceiling and whether the dataset folder is appended to - and every one of them can still be changed in the dialog.

```csharp
public enum YOLOTrainingScenario
```
### Fields

<a name='DiGi.GIS.PostgreSQL.UI.Enums.YOLOTrainingScenario.Retrain'></a>

`Retrain` 0

Continues the existing training: starts from the deployed model\.pt and appends the chosen counties to the existing dataset folder, whose builder is resumable and keyed by reference\.

<a name='DiGi.GIS.PostgreSQL.UI.Enums.YOLOTrainingScenario.Fresh'></a>

`Fresh` 1

Starts from the pretrained yolo26x\.pt: the dataset is built fresh into a new folder, and a folder that already holds a dataset is refused\.