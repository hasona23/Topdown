global using Microsoft.Xna.Framework;
using System.IO;
namespace MainGame;

public static class GameData
{
    public static string MapDir => Path.GetFullPath("../../../Content/Tiled");
    public static string ArenaMapFile => Path.Combine(MapDir, "ArenaMap.tmx");
    public const int TileSize = 32;
    public const string Name = "Cyber Arena";
   
    public const int Width = 480;
    public const int Height = 270;
    private static float _dt = 1;
    public static bool DebugMode => true;
    public static float TimeScale { get; set; } = 1;
    public static float DeltaTime => TimeScale * _dt;

    public static void Update(GameTime gameTime)
    {
        _dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
}