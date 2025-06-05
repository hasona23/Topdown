using System.Collections.Generic;
using System.Linq;
using MainGame.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameLibrary;
using MonogameLibrary.EntitySystem;
using MonogameLibrary.Scenes;
using MonogameLibrary.Tiled;

namespace MainGame;

public class GameScene:Scene
{
    public World World;
    public TiledMap Map;
    public GameScene(Core core) : base("Arena", core)
    {
        Texture2D tileSet = Game.Content.Load<Texture2D>("Sprites/Tileset");
        Map = TiledMap.FromXml(GameData.ArenaMapFile);
        Map.Atlas = tileSet;
        Map.SetCollisionLayers((layer)=>layer.Class=="Collision");
        World = new World();
        LoadEnemies();
        LoadPlayer();
        
        Cam.Pos = Player.Pos;
        Cam.Offset = new Vector2(GameData.Width, GameData.Height)/-2;
        
        World.Init();

        void LoadPlayer()
        { 
            TiledObject playerObj = Map.ObjectGroups["Entities"].Objects.First(o => o.Name == "Player");
            World.AddEntity(new Entity("BulletGroup", Vector2.Zero).AddComponent(new BulletManager(Map)));
            Entity player = new Entity("Player", playerObj.Pos)
                .AddComponent(new Collider(Map))
                .AddComponent(new Sword(World["EnemyGroup"].Children))
                .AddComponent(new SpriteRenderer(tileSet,playerObj.Gid))
                .AddComponent(new Controller())
                .AddComponent(new Shooter(Cam,World["BulletGroup"],World["EnemyGroup"].Children.ToArray()));
            World.AddEntity(player);
        }

        void LoadEnemies()
        {
            var enemies = Map.ObjectGroups["Entities"].Objects.Where(o => o.Type == "Enemy").ToArray();
            Entity enemyGroup = new Entity("EnemyGroup", new Vector2(0, 0));
            enemyGroup.Children.EnsureCapacity(enemies.Length);
         
            int i = 0;
            foreach (var enemyObj in enemies)
            {
                Collider collider = new Collider(Map);
                enemyGroup.AddChild(new Entity($"Enemy{i++}",enemyObj.Pos).AddComponent(new SpriteRenderer(tileSet,enemyObj.Gid)).AddComponent(collider));
            }

            World.AddEntity(enemyGroup);
        }
    }

    public Entity Player => World["Player"];
    public override void Update(GameTime gameTime)
    {
        World.Update(gameTime);
      

        Cam.Follow(Player.Pos,2,GameData.DeltaTime);
    }

    public override void Draw(GameTime gameTime)
    {
        
        Game.SpriteBatch.Begin(samplerState:SamplerState.PointClamp, transformMatrix:Cam.GetCamMatrix());
        Map.DrawTiles(Game.SpriteBatch);
        
        World.Draw(Game.SpriteBatch,gameTime);
        Game.SpriteBatch.End();
    }
}