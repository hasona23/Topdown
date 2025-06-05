using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameLibrary;
using MonogameLibrary.EntitySystem;
using MonogameLibrary.Utils;


namespace MainGame.Components;

public class Sword:Component
{
    public Rectangle HitBox;
    public Point HitBoxSize;
    public List<Entity> Enemies ;
    private Collider _collider;
    private Controller _controller;
    private float _dotThreshHold = .7f;
    private float _distance = 48;
    public Sword(List<Entity> enemies)
    {
        Enemies = enemies;
        HitBoxSize = new Point(8,48);
    }
    public override void Init()
    {
        base.Init();
        _collider = Owner.GetComponent<Collider>();
        _controller = Owner.GetComponent<Controller>();
    }

   /* private Rectangle GetHurtBoxFacingRight()
    {
        return new Rectangle(_collider.CollisionRect.Right,_collider.CollisionRect.Top+Offset,
            HitBoxSize.X,HitBoxSize.Y);
    }

     private int Offset => (_collider.CollisionRect.Height - HitBoxSize.Y) / 2;
    private Rectangle GetHurtBoxFacingLeft()
    {
        return new Rectangle(_collider.CollisionRect.Left-HitBoxSize.X,_collider.CollisionRect.Top+ Offset,
            HitBoxSize.X,HitBoxSize.Y);
    }
    private Rectangle GetHurtBoxFacingUp()
    {
        return new Rectangle(_collider.CollisionRect.Left+Offset,_collider.CollisionRect.Top-HitBoxSize.X,
            HitBoxSize.Y,HitBoxSize.X);
    }
    private Rectangle GetHurtBoxFacingDown()
    {
        return new Rectangle(_collider.CollisionRect.Left+Offset,_collider.CollisionRect.Bottom,
            HitBoxSize.Y,HitBoxSize.X);
    }*/
   public void Attack()
   {
       //TODO: Set the Direction faced by player


       foreach (var entity in Enemies)
       {
           Vector2 dir = entity.GetComponent<Collider>().CollisionRect.Center.ToVector2() -
                         Owner.GetComponent<Collider>().CollisionRect.Center.ToVector2();
           if (entity.IsAlive && dir.Length() < _distance)
           {
               float dot = Vector2.Dot(Vector2.Normalize(dir),
                   Vector2.Normalize(MainGame.WorldMousePosition -
                                     Owner.GetComponent<Collider>().CollisionRect.Center.ToVector2()));
               if (dot > _dotThreshHold)
               {
                   if (Input.Keyboard.IsKeyPressed(Keys.E))
                   {
                       entity.Disable();
                   }
               }
           }
       }
   }

   public override void Update(float dt)
    {
        base.Update(dt);
        if (Input.Keyboard.IsKeyPressed(Keys.E))
        {
            Attack();
        }
        
    }

    public override void Draw(SpriteBatch spriteBatch, float dt)
    {
        base.Draw(spriteBatch, dt);
        
      
        
        DrawMeleeRange(spriteBatch,Owner.GetComponent<Collider>().CollisionRect.Center.ToVector2(),MainGame.WorldMousePosition,_distance,_dotThreshHold
        );
    }
 
        public void DrawMeleeRange(SpriteBatch spriteBatch, Vector2 playerCenter, Vector2 mousePos, 
            float maxDistance, float dotThreshold)
        {
            Vector2 playerToMouse = Vector2.Normalize(mousePos - playerCenter);
            float centerAngle = (float)Math.Atan2(playerToMouse.Y, playerToMouse.X);
        
            // Convert dot product to angle
            float totalAngleRadians = (float)Math.Acos(dotThreshold) * 2f; // Full cone angle
            float halfArcAngle = totalAngleRadians * 0.5f;
        
            // Draw the attack cone
            DrawArc(spriteBatch, playerCenter, maxDistance, 
                centerAngle - halfArcAngle, centerAngle + halfArcAngle, 
                Color.Yellow * 0.4f);
        
            // Draw center line to mouse
            Vector2 mouseDir = playerToMouse * maxDistance;
            DrawLine(spriteBatch, playerCenter, playerCenter + mouseDir, Color.Green);
        }
    void DrawArc(SpriteBatch spriteBatch, Vector2 center, float radius, float startAngle, float endAngle, Color color)
    {
        int segments = 20;
        for (int i = 0; i < segments; i++)
        {
            float angle1 = MathHelper.Lerp(startAngle, endAngle, (float)i / segments);
           
            Vector2 p1 = center + new Vector2((float)Math.Cos(angle1), (float)Math.Sin(angle1)) * radius;
            
            DrawLine(spriteBatch, center, p1, color);
            
        }
    }
    void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color)
    {
        Vector2 edge = end - start;
        float angle = (float)Math.Atan2(edge.Y, edge.X);
    
        spriteBatch.Draw(Core.Pixel, 
            new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), 1),
            null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
    }
}
