using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Scholar")]
public class Scholar : BaseCharacter
{
    public override string Name => "Scholar";
    public override int Strength => 2;
    public override int Wisdom   => 8;
    public override int Charisma => 3;
    public override void OnSpawn() { }
    public override void OnDead()  { }
}
