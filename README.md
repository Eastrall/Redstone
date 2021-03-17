<h1 align="center">
  <br>
  <a href="https://github.com/Eastrall/Redstone">
    <img src="resources/icon.png" alt="Markdownify" />
  </a>
  <br>
  Redstone
  <br>
</h1>

[![Build Status](https://dev.azure.com/eastrall/Redstone/_apis/build/status/Eastrall.Redstone?branchName=main)](https://dev.azure.com/eastrall/Redstone/_build/latest?definitionId=5&branchName=main)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/77c0faaec9834e4da541b459f9311879)](https://www.codacy.com/gh/Eastrall/Redstone/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Eastrall/Redstone&amp;utm_campaign=Badge_Grade)
[![codecov](https://codecov.io/gh/Eastrall/Redstone/branch/main/graph/badge.svg?token=RTU5NXR3DP)](https://codecov.io/gh/Eastrall/Redstone)


# Introduction

`Redstone` is an experimental Minecraft server built with C# and .NET 5.

The project has been created for learning purposes, about the network, game logic and Minecraft world generation problematics.

The goal is to provide a clean and simple API to develop highly performant Minecraft servers.

Redstone uses a [`HostBuilder`](https://docs.microsoft.com/en-us/dotnet/core/extensions/generic-host) to benefit the use of using modern design patterns such as [Dependency Injection](https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection), [Logging](https://docs.microsoft.com/en-us/dotnet/core/extensions/logging) and [Configuration](https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration) loading.

<h4 align="center">:warning: This project is not affiliated with Mojang Studios. :warning:</h4>

## Getting started

Before getting started, you will need to install the following softwares in order to contrribute to the Redstone project:

* [Git SCM](https://git-scm.com/)
* Visual Studio 2019 (or [Visual Studio Code](https://code.visualstudio.com/))
* [Docker](https://www.docker.com/get-started)
  * *With `docker-compose`*

> The solution is configured to run with Linux containers.

One you have checked out the repository, you can open the `Redstone.sln` solution in visual studio, set the `docker-compose` project as "Startup Project" and start the debug to get a working server in debug mode.

Note: The configuration files of the project are located in the `bin/config` folder, located at the root directory of the project.
