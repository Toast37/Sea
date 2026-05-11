using System;

[Serializable]
public class MetaStats
{
    public int Money;
    public int Luck;

    public void Modify(string stat, int value)
    {
        switch (stat)
        {
            case "Money": Money += (int)value; break;
            case "Luck": Luck += (int)value; break;
        }
        GameManager.Instance.NotifyStateChanged();
    }
}
