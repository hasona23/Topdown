using MonogameLibrary.EntitySystem;

namespace MainGame.Components;

public class HurtBox:Component
{
    private Collider _collider;
    public bool IsActive { get; set; }
    public Vector2 Center => _collider?.CollisionRect.Center.ToVector2()??Vector2.Zero;
    public Rectangle Box => _collider?.CollisionRect??new Rectangle(0,0,16,16);
    public override void Init()
    {
        _collider = Owner.GetComponent<Collider>();
    }
}