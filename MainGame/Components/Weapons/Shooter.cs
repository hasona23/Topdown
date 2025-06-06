using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameLibrary;
using MonogameLibrary.EntitySystem;
using MonogameLibrary.Tiled;
using MonogameLibrary.Utils;

namespace MainGame.Components.Weapons;

public class Shooter(Cam2D cam,Entity bulletGroup,params Entity[] targets):WeaponBase
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
    public override void Attack()
    {
            bulletGroup.Children.Add(CreateBullet());
    }

   
}

