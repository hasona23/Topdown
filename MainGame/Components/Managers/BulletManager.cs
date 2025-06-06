using MonogameLibrary.EntitySystem;
using MonogameLibrary.Tiled;

namespace MainGame.Components.Managers;
public class BulletManager(TiledMap map) : Component
{
    //Variable used to signal to remove disables bullets instead of every frame
    private float _bulletCleaningCount = 32;
    public override void Update(float dt)
    {
        base.Update(dt);
        //BULLETS
        if (Owner.Children.Count > 32)
        {
            for (var i = Owner.Children.Count - 1; i >= 0; i--)
            {
                if (!Owner.Children[i].IsAlive)
                {
                    Owner.Children[i].Destroy();
                    Owner.Children.RemoveAt(i);
                }
            }
        }
    }
}