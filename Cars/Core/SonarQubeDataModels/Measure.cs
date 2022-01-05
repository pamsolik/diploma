namespace Core.SonarQubeDataModels;

public class Measure
{
    public string? Metric { get; set; }
    public string? Value { get; set; }
    public string? BestValue { get; set; }

    public float? GetValue()
    {
        if (Value != null) return float.Parse(Value);
        return null;
    }
}