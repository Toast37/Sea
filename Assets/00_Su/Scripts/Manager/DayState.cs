public class DayState
{
    public int Day { get; private set; }
    public int ActionPoints { get; private set; }
    public int MaxActionPoints { get; private set; } = 6;

    public void NextDay()
    {
        Day++;
        ActionPoints = MaxActionPoints;
        GameManager.Instance.NotifyStateChanged();
    }

    public bool Spend(int cost)
    {
        if (ActionPoints < cost) return false;
        ActionPoints -= cost;
        GameManager.Instance.NotifyStateChanged();
        return true;
    }
}
