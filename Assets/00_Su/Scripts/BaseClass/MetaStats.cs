using System;
using System.Collections.Generic;

[Serializable]
public class MetaStats
{
    public int Money;
    public int Luck;
    public List<ISkill> Skills { get; } = new List<ISkill>();

    public void Modify(string stat, int value)
    {
        switch (stat)
        {
            case "Money": Money += value; break;
            case "Luck":  Luck  += value; break;
        }
        GameManager.Instance.NotifyStateChanged();
    }
}
