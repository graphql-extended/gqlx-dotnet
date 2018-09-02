#addin "Cake.FileHelpers"
#addin "Octokit"
using Octokit;

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var releaseNotes = ParseReleaseNotes("./CHANGELOG.md");
var version = releaseNotes.Version.ToString();
var buildResultDir = Directory("./dist") + Directory(version);

// Tasks
// ----------------------------------------

Task("Clean")
    .Does(() =>
    {
        CleanDirectories(new DirectoryPath[]
        {
            Directory("./src/Gqlx/bin"),
            buildResultDir,
        });
    });

Task("Update-Assembly-Version")
    .Does(() =>
    {
        var file = Directory("./src/Gqlx") + File("AssemblyInfo.cs");
        var company = "AngleVisions";

        CreateAssemblyInfo(file, new AssemblyInfoSettings
        {
            Product = "gqlx",
            Version = version,
            FileVersion = version,
            Company = company,
            Copyright = $"Copyright (c) {DateTime.Now.Year} {company}",
        });
    });

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Update-Assembly-Version")
    .Does(() =>
    {
        DotNetCoreBuild("./src/Gqlx/Gqlx.csproj", new DotNetCoreBuildSettings
        {
            OutputDirectory = buildResultDir,
        });

        MSBuild("./src/gqlx-dotnet.sln", new MSBuildSettings
        {
            Configuration = configuration,
            Verbosity = Verbosity.Minimal,
        });
    });

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
    {
        NUnit3($"./src/Gqlx.Test/bin/{configuration}/gqlx-dotnet-test.dll", new NUnit3Settings
        {
            Work = buildResultDir.Path.FullPath,
        });
    });

Task("Create-Package")
    .IsDependentOn("Build")
    .Does(() =>
    {
        NuGetPack(new NuGetPackSettings
        {
            Id = "gqlx",
            Authors = new []
            {
                "Florian Rappl",
            },
            Description = ".NET library for using the gqlx language.",
            Files = new []
            {
                new NuSpecContent
                {
                    Source = "Gqlx.dll",
                    Target = "lib/netstandard1.3",
                },
                new NuSpecContent
                {
                    Source = "Gqlx.pdb",
                    Target = "lib/netstandard1.3",
                },
                new NuSpecContent
                {
                    Source = "Gqlx.deps.json",
                    Target = "lib/netstandard1.3",
                },
            },
            Version = version,
            BasePath = buildResultDir,
            OutputDirectory = buildResultDir,
            Symbols = false,
            Properties = new Dictionary<String, String>
            {
                { "Configuration", configuration },
            },
        });
    });

Task("Publish-Package")
    .IsDependentOn("Create-Package")
    .Does(() =>
    {
        var apiKey = EnvironmentVariable("NUGET_API_KEY");

        if (String.IsNullOrEmpty(apiKey))
            throw new InvalidOperationException("Could not resolve the NuGet API key.");

        foreach (var nupkg in GetFiles($"{buildResultDir.Path.FullPath}/*.nupkg"))
        {
            NuGetPush(nupkg, new NuGetPushSettings
            {
                Source = "https://nuget.org/api/v2/package",
                ApiKey = apiKey,
            });
        }
    });

Task("Publish-Release")
    .IsDependentOn("Create-Package")
    .Does(() =>
    {
        var githubToken = EnvironmentVariable("GITHUB_API_TOKEN");

        if (String.IsNullOrEmpty(githubToken))
            throw new InvalidOperationException("Could not resolve GitHub API token.");

        var github = new GitHubClient(new ProductHeaderValue("GqlxCakeBuild"))
        {
            Credentials = new Credentials(githubToken),
        };

        var newRelease = github.Repository.Release;
        newRelease.Create("graphql-extended", "gqlx-dotnet", new NewRelease($"v{version}")
        {
            Name = version,
            Body = String.Join(Environment.NewLine, releaseNotes.Notes),
            Prerelease = false,
            TargetCommitish = "master",
        }).Wait();
    });

// Targets
// ----------------------------------------

Task("Package")
    .IsDependentOn("Run-Unit-Tests")
    .IsDependentOn("Create-Package");

Task("Default")
    .IsDependentOn("Package");

Task("Publish")
    .IsDependentOn("Publish-Package")
    .IsDependentOn("Publish-Release");

Task("Travis")
    .IsDependentOn("Publish");

// Execution
// ----------------------------------------

RunTarget(target);
