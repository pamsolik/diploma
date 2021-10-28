namespace Cars.Services.Other
{
    public static class SonarQubeRequestHandler
    {
        private const string MetricsUriBase =
            "http://localhost:9000/api/measures/component?metricKeys=bugs,code_smells,duplicated_lines,duplicated_lines_density,complexity,cognitive_complexity,violations,coverage,lines,sqale_rating,reliability_rating,security_hotspots,security_rating&component=";
        
        private const string CreateProjectUriBase =
            "http://localhost:9000/api/todo";
        
        public static string GetMetricsUri(string project) => MetricsUriBase + project;
        
        public static string GetCreateProjectUri(string project) => CreateProjectUriBase + project;
    }
}