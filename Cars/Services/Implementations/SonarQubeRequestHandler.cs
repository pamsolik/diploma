﻿using System.Text;
using Core.SonarQubeDataModels;
using Newtonsoft.Json;
using RestSharp;
    
namespace Services.Implementations;

public class SonarQubeRequestHandler
{
    private const string Metrics =
        "complexity,cognitive_complexity,duplicated_lines,duplicated_lines_density,violations,code_smells," +
        "sqale_index,sqale_rating,bugs,reliability_rating,coverage,tests,security_hotspots,security_rating,lines";

    public const string SonarLoc = "D:/SonarScan";

    private const string MvnLoc = $"{SonarLoc}\\dependencies\\mvn\\bin\\mvn";

    private const string GradleLoc = $"{SonarLoc}\\dependencies\\gradle\\bin\\gradle";

    private readonly string _key;
    private readonly string _password;

    private readonly string _userName;

    public readonly string BasePath;

    public SonarQubeRequestHandler(string basePath, string key, string user, string pwd)
    {
        _key = key;
        BasePath = basePath;
        _userName = user;
        _password = pwd;
    }

    private string MetricsUriBase => $"{BasePath}/api/measures/component?metricKeys={Metrics}&component=";

    private string CreateProjectUriBase => $"{BasePath}/api/projects/create";

    private async Task<T?> GetResponse<T>(string url, Method method = Method.Get)
    {
        try
        {
            var encoded =
                Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes($"{_userName}:{_password}"));
            var client = new RestClient(url);
            var request = new RestRequest(url ,method);
            request.AddHeader("Authorization", $"Basic {encoded}");
            var response = await client.ExecuteAsync(request);
            return response.Content is null ? default : JsonConvert.DeserializeObject<T>(response.Content);
        }
        catch
        {
            return default;
        }
    }

    public async Task<Projects?> GetExistingProjects()
    {
        return await GetResponse<Projects>(GetProjectsUri());
    }

    public async Task<CodeAnalysis?> GetCodeAnalysis(string projectKey)
    {
        return await GetResponse<CodeAnalysis>(GetMetricsUri(projectKey));
    }

    public async Task<string?> DeleteProject(string projectKey)
    {
        return await GetResponse<string>(GetDeleteProjectUri(projectKey), Method.Post);
    }

    public async Task<ProjectCreate?> CreateProject(string projectKey)
    {
        return await GetResponse<ProjectCreate>(
            GetCreateProjectUri(projectKey), Method.Post);
    }

    private string GetMetricsUri(string project)
    {
        return MetricsUriBase + project;
    }

    private string GetProjectsUri()
    {
        return $"{BasePath}/api/projects/search";
    }

    private string GetCreateProjectUri(string project)
    {
        return $"{CreateProjectUriBase}?name={project}&project={project}";
    }

    private string GetDeleteProjectUri(string project)
    {
        return $"{BasePath}/api/projects/delete?project={project}";
    }

    public string GetNormalScanCommand(string projectKey)
    {
        return
            $"sonar-scanner.bat -D\"sonar.projectKey={projectKey}\" -D\"sonar.sources=.\" -D\"sonar.host.url={BasePath}\" -D\"sonar.login={_key}\"";
    }

    public string GetMvnScanCommand(string projectKey)
    {
        return
            $@"{MvnLoc} compile & {MvnLoc} sonar:sonar -Dsonar.projectKey={projectKey} -Dsonar.host.url={BasePath} -Dsonar.login={_key}";
    }

    //Add plugin automatically
    // plugins {
    //     id "org.sonarqube" version "3.3"
    // }
    public string GetGradleScanCommand(string projectKey)
    {
        return
            $@"{GradleLoc} sonarqube -Dsonar.projectKey={projectKey} -Dsonar.host.url={BasePath} -Dsonar.login={_key}";
    }

    public string GetDotNetScanCommand(string projectKey)
    {
        return
            $"dotnet sonarscanner begin /k:\"{projectKey}\" /d:sonar.host.url=\"{BasePath}\"  /d:sonar.login=\"{_key}\" & " +
            $"dotnet build & dotnet sonarscanner end /d:sonar.login=\"{_key}\"";
    }
}