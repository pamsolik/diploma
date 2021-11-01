namespace Cars.Services.Other
{
    public static class SonarQubeRequestHandler
    {
        public const string Key = "2a717a00e2600f862f49a8fcc9b28f2029040369";
        
        public const string BasePath = "http://localhost:9000";

        private const string Metrics = "bugs,code_smells,duplicated_lines,duplicated_lines_density,complexity,cognitive_complexity,violations,coverage,lines,sqale_rating,reliability_rating,security_hotspots,security_rating";
        
        private static readonly string MetricsUriBase = $"{BasePath}/api/measures/component?metricKeys={Metrics}&component=";

        private static readonly string CreateProjectUriBase = $"{BasePath}/api/projects/create";
        
        public static string GetMetricsUri(string project) => MetricsUriBase + project;

        public static string GetProjectsUri() => $"{BasePath}/api/projects/search";
        
        public static string GetCreateProjectUri(string project) => $"{CreateProjectUriBase}?name={project}&project={project}";
        
        public static string GetDeleteProjectUri(string project) => $"{BasePath}/api/projects/delete?project={project}";
    }
}