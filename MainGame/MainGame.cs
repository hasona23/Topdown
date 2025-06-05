using System;
using System.Linq;
using ImGuiNET;
using MainGame.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameLibrary;

namespace MainGame;

public class MainGame : Core
{
    private GameScene _gameScene;
    public MainGame() : base(GameData.Name, GameData.Width, GameData.Height)
    {
        DebugMode = GameData.DebugMode;
        ExitOnEscape = true;
        
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        base.Initialize();
        _gameScene = new GameScene(this);
        IsMouseVisible = false;
    }

    protected override void Update(GameTime gameTime)
    {
        GameData.Update(gameTime);
        WorldMousePosition = Vector2.Transform(WindowResolutionHandler.GetMousePos(),Matrix.Invert(MainCam.GetCamMatrix()));
        base.Update(gameTime);
        _gameScene.Update(gameTime);
    }

    public static Vector2 WorldMousePosition;
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(WindowResolutionHandler.Screen);
        GraphicsDevice.Clear(Color.Black);
        _gameScene.Draw(gameTime);
        SpriteBatch.Begin(transformMatrix:MainCam.GetCamMatrix());
       // DrawLine(_gameScene.World["Player"].GetComponent<Collider>().CollisionRect.Center.ToVector2(),Color.Red,1);
        SpriteBatch.Draw(Pixel,new Rectangle(WorldMousePosition.ToPoint()-new Point(2),new Point(4)),Color.Black*.7f);
        SpriteBatch.End();
        GraphicsDevice.SetRenderTarget(null);
        GraphicsDevice.Clear(Color.Black);
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        
        SpriteBatch.Draw(WindowResolutionHandler.Screen, WindowResolutionHandler.DestinationRect, Color.White);
        SpriteBatch.End();
        base.Draw(gameTime);
    }

    public override void DrawGui(GameTime gameTime)
    {
        base.DrawGui(gameTime);
        ImGui.Begin("Window");
        ImGui.Text($"ENTITIES:{_gameScene.World.Entities.Count}");
        ImGui.Text($"ENEMIES:{_gameScene.World["EnemyGroup"].Children.Count}");
        ImGui.Text($"CAM:{MainCam.Pos}");
        ImGui.Text($"BULLETS ALIVE: {_gameScene.World["BulletGroup"].Children?.Count}");
        ImGui.Text($"RESOLUTION_HANDLER: {WindowResolutionHandler.DestinationRect}");
        ImGui.End();
    }
    public static void DrawLine(SpriteBatch spriteBatch, Vector2 start, Color color, float thickness = 1f)
    {
        Vector2 edge = WorldMousePosition - start;
        float angle = (float)Math.Atan2(edge.Y, edge.X);
        
        spriteBatch.Draw(Pixel,
            new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), (int)thickness),
            null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
    }
}