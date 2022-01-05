namespace Core.SonarQubeDataModels;

public class Projects
{
    public Paging Paging { get; set; }

    public List<Component> Components { get; set; }
}