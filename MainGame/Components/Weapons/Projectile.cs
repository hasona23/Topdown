using System;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary;
using MonogameLibrary.EntitySystem;
using MonogameLibrary.Tiled;

namespace MainGame.Components.Weapons;

public class Projectile(TiledMap map,Entity[] targets,int size) : Component
{
    public Rectangle Bounds = new Rectangle(0,0,size,size);
    public Entity[] Targets = targets;
    public override void Update(float dt)
    {
        base.Update(dt);
        Owner.Pos += Owner.Vel;
        Bounds.Location = Owner.Pos.ToPoint();
        foreach (var nearCollisionTileLayer in map.GetNearCollisionTiles(Bounds.Location.ToVector2()))
        {
            foreach (var rectangle in nearCollisionTileLayer.Value)
            {
                if (rectangle.Intersects(Bounds))
                {
                    Console.WriteLine($"COLLIDE MAP AT:{rectangle.Location}");
                    Owner.Disable();
                  
                    
                }
            }
        }
        foreach (var target in Targets)
        {
            //TODO: CHANGE TO HURTBOX INSTEAD OF COLLIDER FOR ENEMY COLLISIONS
            if (target.IsAlive && target.TryGetComponent<Collider>(out var collider))
            {
                if (collider.CollisionRect.Intersects(Bounds))
                {
                    Owner.Disable();
                    target.Disable();
                   
                }
            }
        }
    }

    public override void Draw(SpriteBatch spriteBatch, float dt)
    {
        base.Draw(spriteBatch, dt);
        spriteBatch.Draw(Core.Pixel,new Rectangle(Owner.Pos.ToPoint(),new Point(size)),Color.MonoGameOrange);
    }
}