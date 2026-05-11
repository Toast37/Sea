using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Warrior")]
public class Warrior : BaseCharacter
{
    public override string Name => "Warrior";
    public override int Strength => 8;
    public override int Wisdom   => 2;
    public override int Charisma => 3;
    public override void OnSpawn() { }
    public override void OnDead()  { }
}
