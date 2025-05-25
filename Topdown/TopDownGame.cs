using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameLibrary;

using System;
namespace Topdown;

public class TopDownGame : Core
{

    public TopDownGame() : base("TopDownGame", 320, 180)
    {

        Console.WriteLine(Window.Title);
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {

    }

    protected override void Update(GameTime gameTime)
    {


        if (Input.Keyboard.IsKeyPressed(Keys.I))
            Console.WriteLine("Hello mohsen");
        base.Update(gameTime);

    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(WindowResolutionHandler.Screen);
        GraphicsDevice.Clear(Color.MonoGameOrange);

        // TODO: Add your drawing code here

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        SpriteBatch.Draw(Pixel, new Rectangle(10, 10, 16, 16), Color.AntiqueWhite);
        SpriteBatch.End();


        //Render the Actual Screen
        GraphicsDevice.SetRenderTarget(null);
        GraphicsDevice.Clear(Color.Black);
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        SpriteBatch.Draw(WindowResolutionHandler.Screen, WindowResolutionHandler.DestinationRect, Color.White);
        SpriteBatch.End();

        base.Draw(gameTime);

    }
    public override void DrawGui(GameTime gameTime)
    { }

}
