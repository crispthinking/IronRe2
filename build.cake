#tool "dotnet:https://api.nuget.org/v3/index.json?package=GitVersion.Tool&version=6.1.0"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var nugetConfigFile = Argument("nuget-config", "nuget.config");

var slnFile = File("IronRe2.sln");

// Used for copyright and ownership information.
public const string CrispGroupName = "Crisp Thinking Group Ltd.";

Setup<GitVersion>(_ => GitVersion(new GitVersionSettings{
  UpdateAssemblyInfo = false,
  NoFetch = true,
}));

Task("Restore")
  .Does(() =>
  {
    DotNetCoreRestore(slnFile, new DotNetCoreRestoreSettings{
      ConfigFile = nugetConfigFile,
    });
  });

Task("Build")
  .IsDependentOn("Restore")
  .Does<GitVersion>(versionInfo =>
  {
    DotNetCoreBuild(slnFile, new DotNetCoreBuildSettings{
      Configuration = configuration,
      NoRestore = true,
      VersionSuffix = versionInfo.FullBuildMetaData,
      ArgumentCustomization = args => args
        .Append($"/p:Version={versionInfo.AssemblySemVer}")
        .Append($"/p:InformationalVersion={versionInfo.InformationalVersion}")
    });
  });

// Remove the build artifacts
Task("Clean")
  .Does(() =>
  {
    DeleteDirectory("bin/", new DeleteDirectorySettings {
      Recursive = true,
      Force = true
    });
    StartProcess("make", new ProcessSettings {
      Arguments = "clean",
      WorkingDirectory = Directory("thirdparty/re2/"),
    });
  });

Task("Test")
  .IsDependentOn("Build")
  .Does(() =>
  {
    DotNetCoreTest(slnFile, new DotNetCoreTestSettings{
      Configuration = configuration,
      NoRestore = true,
      NoBuild = true,
      Loggers =
      {
        "trx",
        "console;verbosity=normal"
      }
    });
  });

Task("Pack")
  .IsDependentOn("Build")
  .Does<GitVersion>(versionInfo =>
  {
    DotNetCorePack(slnFile, new DotNetCorePackSettings{
      Configuration = configuration,
      NoRestore = true,
      NoBuild = true,
      OutputDirectory = "./Artifacts",
      ArgumentCustomization = args => args
        .Append($"/p:Version={versionInfo.AssemblySemVer}")
        .Append($"/p:PackageVersion={versionInfo.NuGetVersionV2}")
    });
  });

// Phony target to trigger all the things we _usually_ want done
Task("Default")
  .IsDependentOn("Build");

Task("CI")
  .IsDependentOn("Test");

Task("CI-pack")
  .IsDependentOn("Test")
  .IsDependentOn("Pack");

RunTarget(target);
