public interface ISkill : IVisual
{
    void OnGain();
    void OnTick();
    void OnExpire();
    void OnRoll();
}
