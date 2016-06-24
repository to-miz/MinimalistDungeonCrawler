using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace MinimalisticDungeonCrawler.Map
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Map : Entity
    {
        class RoomEntry
        {
            public bool right = false;
            public bool up = false;
            public bool down = false;
            public bool left = false;
            public int x = 0;
            public int y = 0;

            public Room room = null;

            public bool visited = false;
            public bool ingameVisited = false;
        }

        List<RoomEntry> rooms = new List<RoomEntry>();
        Player player;
        Room currentRoom;
        Room nextRoom;
        bool isTransitioningNew = true;
        Vector2 oldPlayerPos;
        Vector2 toPlayerPos;

        readonly int width = 8;
        readonly int height = 6;
        int roomsToCreate = 15;

        List<string> roomNames = new List<string>()
	    {
	        @"Content\rooms\testRoom.txt",
	        @"Content\rooms\1.txt",
	        @"Content\rooms\2.txt",
            @"Content\rooms\3.txt",
            @"Content\rooms\4.txt"
	    };

        int floor = 0;

        public Map(Screens.ScreenManager screenManager, Player player, int floor)
            : base(screenManager)
        {
            // TODO: Construct any child components here
            //currentRoom = new Room(screenManager, 0, 0, @"Content\rooms\testRoom.txt", player, this);
            //nextRoom = new Room(screenManager, 0, 0, @"Content\rooms\testRoom.txt", player, this);
            this.player = player;
            this.floor = floor;
            generateMap();
        }

        void generateMap()
        {
            rooms.Clear();

            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    RoomEntry entry = new RoomEntry();
                    entry.x = j;
                    entry.y = i;
                    rooms.Add(entry);
                }
            }

            int centerX = width / 2;
            int centerY = height / 2;

            Random rnd = new Random();
            deepSearch(rooms[centerX + centerY*width], rnd);
            rooms[centerX + centerY * width].room = new Room(screenManager, centerX, centerY, @"Content\rooms\startRoom.txt", player, this, floor);
            currentRoom = rooms[centerX + centerY * width].room;

            foreach (RoomEntry entry in rooms)
            {
                if (entry.visited && entry.room != rooms[centerX + centerY * width].room)
                {
                    entry.room = new Room(screenManager, entry.x, entry.y, @"Content\rooms\itemRoom.txt", player, this, floor);
                    break;
                }
            }

            for (int i = rooms.Count - 1; i >= 0; --i)
            {
                if (rooms[i].visited && rooms[i].room != rooms[centerX + centerY * width].room)
                {
                    rooms[i].room = new Room(screenManager, rooms[i].x, rooms[i].y, @"Content\rooms\bossRoom.txt", player, this, floor);
                    break;
                }
            }
        }

        void Shuffle<T>(IList<T> list)
        {
            Random rnd = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public bool GoToNextFloor
        {
            get { return currentRoom.GoToNextFloor; }
        }

        void deepSearch( RoomEntry entry, Random rnd )
        {
            if (roomsToCreate <= 0 || entry.visited)
                return;

            entry.visited = true;
            --roomsToCreate;

            string roomName = roomNames[rnd.Next(roomNames.Count)];
            entry.room = new Room(screenManager, entry.x, entry.y, roomName, player, this, floor);

            List<RoomEntry> neighbors = new List<RoomEntry>();
            if (entry.x - 1 > 0)
                neighbors.Add(rooms[entry.x - 1 + entry.y * width]);

            if (entry.x + 1 < width)
                neighbors.Add(rooms[entry.x + 1 + entry.y * width]);

            if (entry.y - 1 > 0)
                neighbors.Add(rooms[entry.x + (entry.y - 1) * width]);

            if (entry.y + 1 < height)
                neighbors.Add(rooms[entry.x + (entry.y + 1) * width]);

            Shuffle<RoomEntry>(neighbors);

            for (int i = 0; i < neighbors.Count && roomsToCreate > 0; ++i)
            {
                if ((entry.x - 1 > 0) && neighbors[i] == rooms[entry.x - 1 + entry.y * width])
                {
                    entry.left = true;
                    neighbors[i].right = true;
                }
                else if ((entry.x + 1 < width) && neighbors[i] == rooms[entry.x + 1 + entry.y * width])
                {
                    entry.right = true;
                    neighbors[i].left = true;
                }
                else if ((entry.y - 1 > 0) && neighbors[i] == rooms[entry.x + (entry.y - 1) * width])
                {
                    entry.up = true;
                    neighbors[i].down = true;
                }
                else if ((entry.y + 1 < height) && neighbors[i] == rooms[entry.x + (entry.y + 1) * width])
                {
                    entry.down = true;
                    neighbors[i].up = true;
                }

                deepSearch(neighbors[i], rnd);
            }
        }

        public override void loadContent(ContentManager content)
        {
            foreach (RoomEntry entry in rooms)
            {
                if (entry.room != null)
                {
                    entry.room.loadContent(content);
                    entry.room.loadRoom(content, entry.left, entry.up, entry.right, entry.down);
                }
            }
        }

        public void update(float dt)
        {
            if( currentRoom != null )
                rooms[currentRoom.X + currentRoom.Y * width].ingameVisited = true;

            if (currentRoom.IsTransitioning)
            {
                if (isTransitioningNew)
                {
                    switch (currentRoom.TransitionRoom)
                    {
                        case Room.NextRoom.Left:
                            nextRoom = rooms[currentRoom.X-1 + currentRoom.Y * width].room;
                            break;

                        case Room.NextRoom.Up:
                            nextRoom = rooms[currentRoom.X + (currentRoom.Y - 1) * width].room;
                            break;

                        case Room.NextRoom.Right:
                            nextRoom = rooms[currentRoom.X + 1 + currentRoom.Y * width].room;
                            break;

                        case Room.NextRoom.Down:
                            nextRoom = rooms[currentRoom.X + (currentRoom.Y + 1) * width].room;
                            break;

                        default:
                            throw new ArgumentException();
                    }

                    nextRoom.setTransitionFrom(currentRoom.TransitionRoom);
                    oldPlayerPos = player.Position;
                    toPlayerPos = nextRoom.getEntryTransition(currentRoom.TransitionRoom);
                    isTransitioningNew = false;
                }
                
                nextRoom.transition(dt);
                float blend = currentRoom.TransitionProgress;
                player.Position = oldPlayerPos * (1f - blend) + toPlayerPos * blend;
                if (currentRoom.transition(dt))
                {
                    player.Position = nextRoom.getEntryAs(currentRoom.TransitionRoom);
                    currentRoom.stopTransition();
                    player.ParentRoom = nextRoom;
                    Room temp = currentRoom;
                    currentRoom = nextRoom;
                    nextRoom = temp;
                    currentRoom.stopTransition();
                    isTransitioningNew = true;
                }
                return;
            }

            player.updateHorizontal(dt);
            updateHorizontal(dt);
            currentRoom.checkHorizontalCollisions();

            player.updateVertical(dt);
            updateVertical(dt);
            currentRoom.checkVerticalCollisions();

            currentRoom.checkAttacks();
        }

        public override void updateHorizontal(float dt)
        {
            currentRoom.updateHorizontal(dt);
        }

        public override void updateVertical(float dt)
        {
            currentRoom.updateVertical(dt);
        }

        static Rectangle[] roomRects = new Rectangle[] { new Rectangle(0 * 16, 7 * 16, 16, 16), new Rectangle(0 * 16, 8 * 16, 16, 16), new Rectangle(1 * 16, 8 * 16, 16, 16), new Rectangle(2 * 16, 8 * 16, 16, 16) };
        static Rectangle[] pathRects = new Rectangle[] { new Rectangle(1 * 16, 7 * 16, 16, 16), new Rectangle(2 * 16, 7 * 16, 16, 16), new Rectangle(3 * 16, 7 * 16, 16, 16), new Rectangle(4 * 16, 7 * 16, 16, 16) };

        public override void draw(GameTime gameTime)
        {
            if (currentRoom.IsTransitioning && nextRoom != null)
                nextRoom.draw(gameTime);
            currentRoom.draw(gameTime);
        }

        public void drawMinimap()
        {
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            Rectangle spriteRect = new Rectangle();
            spriteRect.X = 19 * 32 + 16;
            spriteRect.Y = 200;
            spriteRect.Width = 32;
            spriteRect.Height = 32;

            spriteBatch.DrawString(screenManager.Font, "Floor " + floor, new Vector2(19 * 32 + 16, 200-32), Color.Black, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
            
            foreach (RoomEntry entry in rooms)
            {
                if (!entry.visited || !entry.ingameVisited || entry.room == null)
                    continue;

                spriteRect.X = 19 * 32 + 16 + entry.x * 12;
                spriteRect.Y = 200 + entry.y * 12;
                if( entry.room != currentRoom )
                    spriteBatch.Draw(screenManager.SpriteSheet, spriteRect, roomRects[0], Color.White);
                else
                    spriteBatch.Draw(screenManager.SpriteSheet, spriteRect, roomRects[3], Color.White);

                if (entry.up)
                    spriteBatch.Draw(screenManager.SpriteSheet, spriteRect, pathRects[0], Color.White);
                if (entry.right)
                    spriteBatch.Draw(screenManager.SpriteSheet, spriteRect, pathRects[1], Color.White);
                if (entry.down)
                    spriteBatch.Draw(screenManager.SpriteSheet, spriteRect, pathRects[2], Color.White);
                if (entry.left)
                    spriteBatch.Draw(screenManager.SpriteSheet, spriteRect, pathRects[3], Color.White);
            }

            spriteBatch.End();
        }

        public override void onCollision(int pushX, int pushY)
        {
        }

        public override void handleInput(Input.InputState inputState)
        {
        }

        public Room CurrentRoom
        {
            get { return currentRoom; }
        }
    }
}
