using UnityEngine;

[CreateAssetMenu(menuName = "Skills/BattleHardened")]
public class BattleHardened : BaseSkill
{
    public override void OnGain() => Debug.Log("BattleHardened: OnGain");
}
