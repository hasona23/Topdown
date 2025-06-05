using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary;
using MonogameLibrary.EntitySystem;

namespace MainGame.Components;

public class SpriteRenderer(Texture2D texture,int spriteId):Component
{
    public override void Draw(SpriteBatch spriteBatch, float dt)
    {
        base.Draw(spriteBatch, dt);
        spriteBatch.Draw(texture,Owner.Pos,Core.GetSrcRect(texture,GameData.TileSize,spriteId),Color.White);
    }
}