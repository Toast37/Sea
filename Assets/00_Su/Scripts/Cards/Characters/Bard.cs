using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Bard")]
public class Bard : BaseCharacter
{
    public override string Name => "Bard";
    public override int Strength => 2;
    public override int Wisdom   => 3;
    public override int Charisma => 8;
    public override void OnSpawn() { }
    public override void OnDead()  { }
}
