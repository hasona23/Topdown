using MonogameLibrary.EntitySystem;

namespace MainGame.Components.Weapons;

public abstract class WeaponBase:Component
{
    public abstract void Attack();
}