namespace Core.SonarQubeDataModels;

public class CodeAnalysis
{
    public ScanComponent Component { get; set; }

    public float? GetValue(string param)
    {
        return Component.GetMeasure(param)?.GetValue();
    }
}