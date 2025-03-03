#tool "dotnet:https://api.nuget.org/v3/index.json?package=GitVersion.Tool&version=6.1.0"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

// Read GitHub credentials from arguments
var githubUser = Argument("github-user", "");
var githubPat = Argument("github-pat", "");

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
    // Add GitHub NuGet source using the passed credentials
    if (!string.IsNullOrEmpty(githubUser) && !string.IsNullOrEmpty(githubPat))
    {
      Information("🔑 Authenticating NuGet with GitHub...");
      StartProcess("dotnet", new ProcessSettings
      {
        Arguments = $"nuget add source \"https://nuget.pkg.github.com/crispthinking/index.json\" " +
                    $"--name github --username {githubUser} --password {githubPat} --store-password-in-clear-text"
      });
    }
    else
    {
      Warning("⚠️ GitHub credentials not provided. NuGet authentication may fail.");
    }

    // Restore dependencies
    DotNetRestore(slnFile);
    
    StartProcess("dotnet", new ProcessSettings {
      Arguments = "list package",
      RedirectStandardOutput = true
    });
    
    // Debug: Find cre2.so after restore
    Information("🔍 Checking if cre2.so exists after restore:");
    StartProcess("find", new ProcessSettings {
      Arguments = "./ -name 'cre2.so'"
    });
    
    StartProcess("find", new ProcessSettings {
      Arguments = "./ -name 'libcre2.so'"
    });
  });

  
Task("Build")
  .IsDependentOn("Restore")
  .Does<GitVersion>(versionInfo =>
  {
    DotNetBuild(slnFile, new DotNetBuildSettings{
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
    DotNetTest(slnFile, new DotNetTestSettings{
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
    DotNetPack(slnFile, new DotNetPackSettings{
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
