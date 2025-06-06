using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.EntitySystem;
using MonogameLibrary.Utils;


namespace MainGame.Components.Weapons;

public class Sword:WeaponBase
{
    public Rectangle HitBox;
    public Point HitBoxSize;
    public List<Entity> Enemies ;
    private HurtBox _hurtBox;
    private Controller _controller;
    public Sword(List<Entity> enemies)
    {
        Enemies = enemies;
        HitBoxSize = new Point(16,48);
    }
    public override void Init()
    {
        base.Init();
        _hurtBox = Owner.GetComponent<HurtBox>();
        _controller = Owner.GetComponent<Controller>();
        HitBox = GetHitBoxFacingRight();
    }

    private Rectangle GetHitBoxFacingRight()
    {
        return new Rectangle(_hurtBox.Box.Center.X,(int)_hurtBox.Center.Y - HitBoxSize.Y/2,
            HitBoxSize.X,HitBoxSize.Y);
    }
    private Rectangle GetHitBoxFacingLeft()
    {
        return new Rectangle(_hurtBox.Box.Center.X-HitBoxSize.X,(int)_hurtBox.Center.Y-HitBoxSize.Y/2,
            HitBoxSize.X,HitBoxSize.Y);
    }
    private Rectangle GetHitBoxFacingUp()
    {
        return new Rectangle((int)_hurtBox.Center.X-HitBoxSize.Y/2,_hurtBox.Box.Center.Y-HitBoxSize.X,
            HitBoxSize.Y,HitBoxSize.X);
    }
    private Rectangle GetHitBoxFacingDown()
    {
        return new Rectangle((int)_hurtBox.Center.X-HitBoxSize.Y/2,_hurtBox.Box.Center.Y,
            HitBoxSize.Y,HitBoxSize.X);
    }

    private Rectangle GetHitBox()
    {
        return _controller.Direction switch
        {
            Direction.Down => GetHitBoxFacingDown(),
            Direction.Up => GetHitBoxFacingUp(),
            Direction.Left => GetHitBoxFacingLeft(),
            _ => GetHitBoxFacingRight(),
        };
    }
   public override void Attack()
   {
       //TODO: Set the Direction faced by player

       HitBox = GetHitBox();
       foreach (var entity in Enemies)
       {
           if (entity.IsAlive && entity.TryGetComponent(out HurtBox hurtBox))
           {
               if (hurtBox.Box.Intersects(HitBox) || hurtBox.Box.Contains(HitBox))
               {
                   entity.Disable();
               }
           }
       }
       
   }
   public override void Draw(SpriteBatch spriteBatch, float dt)
   {
       base.Draw(spriteBatch, dt);
       if (GameData.DebugMode)
       {
           DebugRenderer.DrawSolid(GetHitBox(),Color.Bisque,transparency:.5f);
            DebugRenderer.DrawHollow(GetHitBox(),Color.Bisque,1);
       }
   }
      
   
}
