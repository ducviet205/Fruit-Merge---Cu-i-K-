public sealed class AchievementDefinition
{
    public AchievementDefinition(string id, string title, string description, int targetValue)
    {
        Id = id;
        Title = title;
        Description = description;
        TargetValue = targetValue;
    }

    public string Id { get; }

    public string Title { get; }

    public string Description { get; }

    public int TargetValue { get; }
}
