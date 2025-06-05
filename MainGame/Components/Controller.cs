using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameLibrary;
using MonogameLibrary.EntitySystem;
using MonogameLibrary.Timers;

namespace MainGame.Components;

public class Controller:Component
{
    public Direction Direction { get; private set; }
    private readonly float _dashFactor;
    private readonly Timer _dashTimer ;
    private bool _canDash;
    private readonly Timer _dashCooldownTimer;
    private bool _isDashing;

    public Controller()
    {
        _dashFactor = 10;
        _dashTimer = new Timer(250);
        _dashTimer.OnFinish += () => _isDashing = false;
        _canDash = true;
        _dashCooldownTimer = new Timer(1500);
        _dashCooldownTimer.OnFinish += () => _canDash = true;
        _isDashing = false;
    }
    public override void Update( float dt)
    {
        base.Update( dt);
       
        Owner.Vel = Input.Keyboard.GetMovementWasd();
        if (Input.Keyboard.IsKeyPressed(Keys.Space) && !_isDashing && _canDash)
        {
          
            _dashTimer.Start();
            _dashCooldownTimer.Start();
      
            _isDashing = true;
            _canDash = false;
            
        }
        _dashCooldownTimer.Update(dt);
        _dashTimer.Update(dt);
        if (Owner.Vel.LengthSquared() > 0)
        {
            Owner.Vel = Vector2.Normalize(Owner.Vel);
            Owner.Vel *= _isDashing? _dashFactor : 5;
            
            if (Owner.Vel.X != 0)
            {
                Direction = Owner.Vel.X >0 ? Direction.Right : Direction.Left;
            }
            else
            {
                if (Owner.Vel.Y != 0)
                {
                    Direction = Owner.Vel.Y > 0 ? Direction.Down : Direction.Up;
                }
            }
        }
    }
}

public enum Direction
{
    Right,
    Left,
    Up,
    Down
}