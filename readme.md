# Fractal

Project based on dotnet Web API. 

## Workflow

<div hidden>
```
@startuml Fractal

autonumber "<b>[0]"

' skinparam classFontName Courier 
 
skinparam classFontName Helvetica

skinparam DefaultTextAlignment Center
skinparam sequenceMessageAlign center

skinparam shadowing false

skinparam ArrowColor #4b9fea
skinparam ArrowFontColor #4b9fea

skinparam ArrowThickness 1

skinparam participant {
  FontSize 14
  StartColor MediumBlue

  BackgroundColor #1e88e5
  BorderColor #1e88e5
  FontColor #fff
}

skinparam actor {
  FontSize 14
  StartColor MediumBlue

  BackgroundColor #1e88e5
  BorderColor #1e88e5
  FontColor #fff

}

skinparam sequence {
  LifeLineBorderColor #1e88e5
  LifeLineBorderThickness 2
  LifeLineBackgroundColor #fff
}

BrowserApp -> WebApi ++ : GetVehicleState
WebApi -> TaskSheduler : SheduleTask
return taskId

@enduml
```
</div>

![](Fractal.svg)