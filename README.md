# Visual Studio Report

## Problem description

Visual Studio 2022 doesn't report files added with `Add -> New Item`
via `IProjectChangeHintReceiver` for JavaScript Projects.

This repository contains a synthetic example for demonstration purposes, but the
problem was originally reproduced on a real-world source control plug-in.

## Steps to reproduce

1. Download the project.
2. Open the solution in Visual Studio.
3. `Debug -> Start Debugging (F5)`.
4. In the opened Visual Studio Experimental Instance:
    - Create a new C# Core Project (e.g. `ConsoleApp1`)
    - Add new source code file to the project via `Add -> New Item`
    - The output window should appear showing log messages for
      received `IProjectChangeHint` notifications
5. In contrast:
    - Create a new JavaScript Project (e.g. `NodeConsoleApp1`)
    - Add new source code file to the project via `Add -> New Item`
    - Nothing will happen

## Environment

- Microsoft Visual Studio Professional 2022 Version 17.9.0
- Microsoft Visual Studio Professional 2022 Version 17.10.0 Preview 1.0
- Windows 11 Version 10.0.22621 Build 22621
