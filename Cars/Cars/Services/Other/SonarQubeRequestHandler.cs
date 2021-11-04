﻿namespace Cars.Services.Other
{
    public static class SonarQubeRequestHandler
    {
        public const string Key = "2a717a00e2600f862f49a8fcc9b28f2029040369";

        public const string BasePath = "http://localhost:9000";

        private const string Metrics =
            "bugs,code_smells,duplicated_lines,duplicated_lines_density,complexity,cognitive_complexity,violations,coverage,lines,sqale_rating,reliability_rating,security_hotspots,security_rating";

        private static readonly string MetricsUriBase =
            $"{BasePath}/api/measures/component?metricKeys={Metrics}&component=";

        private static readonly string CreateProjectUriBase = $"{BasePath}/api/projects/create";

        public static string GetMetricsUri(string project)
        {
            return MetricsUriBase + project;
        }

        public static string GetProjectsUri()
        {
            return $"{BasePath}/api/projects/search";
        }

        public static string GetCreateProjectUri(string project)
        {
            return $"{CreateProjectUriBase}?name={project}&project={project}";
        }

        public static string GetDeleteProjectUri(string project)
        {
            return $"{BasePath}/api/projects/delete?project={project}";
        }

        public static string GetNormalScanCommand(string projectKey)
        {
            return
                $"sonar-scanner.bat -D\"sonar.projectKey={projectKey}\" -D\"sonar.sources=.\" -D\"sonar.host.url={BasePath}\" -D\"sonar.login={Key}\"";
        }

        public static string GetMvnScanCommand(string projectKey)
        {
            return $@"mvn sonar:sonar \
                -Dsonar.projectKey={projectKey} \
                -Dsonar.host.url={BasePath} \
                -Dsonar.login={Key}";
        }

        // plugins {
        //     id "org.sonarqube" version "3.3"
        // }
        public static string GetGradleScanCommand(string projectKey)
        {
            return $@"./gradlew sonarqube \
                  -Dsonar.projectKey={projectKey} \
                  -Dsonar.host.url={BasePath} \
                  -Dsonar.login={Key}";
        }

        public static string GetDotNetScanCommand(string projectKey)
        {
            return
                $"dotnet sonarscanner begin /k:\"{projectKey}\" /d:sonar.host.url=\"{BasePath}\"  /d:sonar.login=\"{Key} &" +
                $"dotnet build & dotnet sonarscanner end /d:sonar.login=\"{Key}\"";
        }
    }
}