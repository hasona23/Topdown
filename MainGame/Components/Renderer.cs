using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.EntitySystem;
using MonogameLibrary.Utils;
namespace MainGame.Components;
public class Renderer(Color color,int size = 32) : Component
{
    public override void Draw(SpriteBatch spriteBatch, float dt)
    {
        base.Draw(spriteBatch, dt);
        DebugRect.DrawSolid(new Rectangle(Owner.Pos.ToPoint(),new Point(size)),color);
    }
}