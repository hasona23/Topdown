using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary;
using MonogameLibrary.EntitySystem;
using MonogameLibrary.Tiled;
using MonogameLibrary.Utils;

namespace MainGame.Components;

public class Collider(TiledMap map):Component
{
    public TiledMap Map = map;
    public Rectangle CollisionRect;
    public Vector2 Offset;
    public Point Size = new (GameData.TileSize);
    public override void Init()
    {
        base.Init();
        CollisionRect.Location = Owner.Pos.ToPoint() ;
        CollisionRect.Size = Size;
    }

    public override void Update(float dt)
    {
        base.Update(dt);
        Move(Owner.Vel);
    }

    private void MoveX(float amount)
    {
        CollisionRect = new Rectangle((Owner.Pos + Offset).ToPoint(), Size);
        CollisionRect.X += (int)Math.Round(amount);
        
        foreach (var nearCollisionTileLayers in Map.GetNearCollisionTiles(CollisionRect.Location.ToVector2()))
        {
            foreach (var collisionTile in nearCollisionTileLayers.Value)
            {
                if (collisionTile.Intersects(CollisionRect))
                {
                    if (amount > 0)
                    {
                        CollisionRect.X = collisionTile.Left - CollisionRect.Width;
                    }
                    if (amount < 0)
                    {
                        CollisionRect.X = collisionTile.Right;
                    }
                }
            }
        }
        Owner.Pos = CollisionRect.Location.ToVector2()-Offset;
    }
    
    private void MoveY(float amount)
    {
        CollisionRect = new Rectangle((Owner.Pos + Offset).ToPoint(), Size);
        CollisionRect.Y += (int)Math.Round(amount);
        
        foreach (var nearCollisionTileLayers in Map.GetNearCollisionTiles(CollisionRect.Location.ToVector2()))
        {
            foreach (var collisionTile in nearCollisionTileLayers.Value)
            {
                if (collisionTile.Intersects(CollisionRect))
                {
                    if (amount > 0)
                    {
                        CollisionRect.Y = collisionTile.Top - CollisionRect.Height;
                    }
                    if (amount < 0)
                    {
                        CollisionRect.Y = collisionTile.Bottom;
                    }
                }
            }
        }
        Owner.Pos = CollisionRect.Location.ToVector2()-Offset;
    }

    public void Move(Vector2 amount)
    {
        if(amount.X != 0)
            MoveX(amount.X);
        if(amount.Y != 0)
            MoveY(amount.Y);
    }

    public override void Draw(SpriteBatch spriteBatch, float dt)
    {
        base.Draw(spriteBatch, dt);
        if (!GameData.DebugMode)
            return;
      
        DebugRenderer.DrawHollow(CollisionRect,Color.Aquamarine);
        foreach (var nearCollisionTileLayers in Map.GetNearCollisionTiles(CollisionRect.Location.ToVector2()))
        {
            foreach (var collisionTile in nearCollisionTileLayers.Value)
            {
                
                DebugRenderer.DrawHollow(collisionTile,Color.Green,4);
            }
        }
       
    }
}