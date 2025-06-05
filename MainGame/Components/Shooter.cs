using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameLibrary;
using MonogameLibrary.EntitySystem;
using MonogameLibrary.Tiled;
using MonogameLibrary.Utils;

namespace MainGame.Components;

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
public class Shooter(Cam2D cam,Entity bulletGroup,params Entity[] targets):Component
{
    public const float BulletSpeed = 10F;
    public Vector2 PosOffset;
    public int BulletSize = 2;
    private TiledMap _map;
    public override void Init()
    {
        base.Init();
        bulletGroup.Children = new List<Entity>(32);
        PosOffset = Owner.GetComponent<Collider>().CollisionRect.Size.ToVector2()/2;
        _map = Owner.GetComponent<Collider>().Map;
    }

    private Entity CreateBullet()
    {
        Entity newBullet =  new Entity("Bullet", Owner.Pos+PosOffset).AddComponent(new Projectile(_map,targets,2));
        newBullet.Vel = Vector2.Normalize(MainGame.WorldMousePosition - newBullet.Pos) * BulletSpeed;
        return newBullet;
    }
    public override void Update(float dt)
    {
        base.Update(dt);
        if (Input.Keyboard.IsKeyPressed(Keys.Enter))
        {
            bulletGroup.Children.Add(CreateBullet());
        }
    }


}

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
                    Console.WriteLine("HI");
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