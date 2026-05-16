using UnityEngine;

[CreateAssetMenu(menuName = "Skills/DeepStudy")]
public class DeepStudy : BaseSkill
{
    public override void OnGain() => Debug.Log("DeepStudy: OnGain");
}
