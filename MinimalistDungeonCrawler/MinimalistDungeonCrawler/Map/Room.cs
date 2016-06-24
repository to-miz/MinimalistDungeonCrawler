using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace MinimalisticDungeonCrawler.Map
{
    public class Room : Entity
    {
        static readonly int width = 19;
        static readonly int height = 15;

        public static readonly int cellWidth = 32;
        public static readonly int cellHeight = 32;

        public int X = 0;
        public int Y = 0;

        private List<Tile> tiles = new List<Tile>();
        string roomName;

        private List<EnemyEntity> enemies = new List<EnemyEntity>();
        private List<EnemyEntity> enemiesToAdd = new List<EnemyEntity>();
        private List<EnemyEntity> enemiesToRemove = new List<EnemyEntity>();
        private List<Item> items = new List<Item>();
        private List<Item> itemsToAdd = new List<Item>();
        private List<Item> itemsToRemove = new List<Item>();

        Vector2 playerPos = new Vector2();
        Player player;

        public Matrix TransitionMatrix;
        bool isTransitioning = false;
        Vector2 transitionVector = Vector2.Zero;
        float transitionProgress = 0f;
        float transitionSpeed = 0.01f;
        bool isTransitioningFrom = false;
        protected bool isRoomCleared = false;
        public bool IsRoomCleared
        {
            get { return isRoomCleared; }
        }

        public bool GoToNextFloor = false;

        int enemyCount = 0;

        public enum NextRoom
        {
            Left,
            Up,
            Right,
            Down
        }
        NextRoom nextRoom = NextRoom.Down;

        public bool IsTransitioning
        {
            get { return isTransitioning; }
        }

        public NextRoom TransitionRoom
        {
            get { return nextRoom; }
        }

        public float TransitionProgress
        {
            get { return transitionProgress; }
        }

        private Tile leftDoor, topDoor, rightDoor, bottomDoor;
        private Tile exit;

        private Map parentMap;
        int floor = 0;

        public Room(Screens.ScreenManager screenManager, int X, int Y, string roomName, Player player, Map map, int floor)
            : base(screenManager)
        {
            this.roomName = roomName;
            this.player = player;
            TransitionMatrix = Matrix.Identity;
            parentMap = map;
            this.X = X;
            this.Y = Y;
            this.floor = floor;
        }

        public void loadRoom(ContentManager content, bool left, bool top, bool right, bool bottom)
        {
            tiles.Clear();
            enemies.Clear();
            leftDoor = null;
            topDoor = null;
            rightDoor = null;
            bottomDoor = null;

            int i = 0, j = 0;

            //create room corners
            try
            {
                using (StreamReader reader = new StreamReader(@"Content\rooms\corners.txt"))
                {
                    while (reader.Peek() >= 0 && j < height)
                    {
                        char character = (char)reader.Read();
                        /*if (i + 1 >= width)
                            ++j;

                        if (j >= height)
                            break;*/
                        Tile tile = null;
                        switch (character)
                        {
                            case '0':
                                tile = new Tile(screenManager, new AABB(i * cellWidth, j * cellHeight, cellWidth, cellHeight), Tile.LeftTop, this);
                                break;

                            case '1':
                                tile = new Tile(screenManager, new AABB(i * cellWidth, j * cellHeight, cellWidth, cellHeight), Tile.Top, this);
                                break;

                            case '2':
                                tile = new Tile(screenManager, new AABB(i * cellWidth, j * cellHeight, cellWidth, cellHeight), Tile.RightTop, this);
                                break;

                            case '3':
                                tile = new Tile(screenManager, new AABB(i * cellWidth, j * cellHeight, cellWidth, cellHeight), Tile.Right, this);
                                break;

                            case '4':
                                tile = new Tile(screenManager, new AABB(i * cellWidth, j * cellHeight, cellWidth, cellHeight), Tile.RightBottom, this);
                                break;

                            case '5':
                                tile = new Tile(screenManager, new AABB(i * cellWidth, j * cellHeight, cellWidth, cellHeight), Tile.Bottom, this);
                                break;

                            case '6':
                                tile = new Tile(screenManager, new AABB(i * cellWidth, j * cellHeight, cellWidth, cellHeight), Tile.LeftBottom, this);
                                break;

                            case '7':
                                tile = new Tile(screenManager, new AABB(i * cellWidth, j * cellHeight, cellWidth, cellHeight), Tile.Left, this);
                                break;

                            case ' ':
                                tile = new Tile(screenManager, new AABB(i * cellWidth, j * cellHeight, cellWidth, cellHeight), Tile.Ground, this);
                                tile.Solid = false;
                                break;

                            case '\r':
                                i = 0;
                                ++j;
                                continue;

                            case 'A':
                                if (top)
                                {
                                    tile = new Tile(screenManager, new AABB(i * cellWidth, j * cellHeight, cellWidth, cellHeight), Tile.DoorClosedUp, this);
                                    tile.Solid = true;
                                    topDoor = tile;
                                }
                                else
                                {
                                    tile = new Tile(screenManager, new AABB(i * cellWidth, j * cellHeight, cellWidth, cellHeight), Tile.Top, this);
                                }
                                
                                break;

                            case 'B':
                                if (right)
                                {
                                    tile = new Tile(screenManager, new AABB(i * cellWidth, j * cellHeight, cellWidth, cellHeight), Tile.DoorClosedRight, this);
                                    tile.Solid = true;
                                    rightDoor = tile;
                                }
                                else
                                {
                                    tile = new Tile(screenManager, new AABB(i * cellWidth, j * cellHeight, cellWidth, cellHeight), Tile.Right, this);
                                }                                
                                break;

                            case 'C':
                                if (bottom)
                                {
                                    tile = new Tile(screenManager, new AABB(i * cellWidth, j * cellHeight, cellWidth, cellHeight), Tile.DoorClosedDown, this);
                                    tile.Solid = true;
                                    bottomDoor = tile;
                                }
                                else
                                {
                                    tile = new Tile(screenManager, new AABB(i * cellWidth, j * cellHeight, cellWidth, cellHeight), Tile.Bottom, this);
                                }
                                break;

                            case 'D':
                                if (left)
                                {
                                    tile = new Tile(screenManager, new AABB(i * cellWidth, j * cellHeight, cellWidth, cellHeight), Tile.DoorClosedLeft, this);
                                    tile.Solid = true;
                                    leftDoor = tile;
                                }
                                else
                                {
                                    tile = new Tile(screenManager, new AABB(i * cellWidth, j * cellHeight, cellWidth, cellHeight), Tile.Left, this);
                                }
                                break;

                            case '\n':
                                continue;

                            default:
                                throw new NotImplementedException();
                        }

                        if (tile != null)
                        {
                            tile.loadContent(content);
                            tiles.Add(tile);
                        }                        

                        i = (i + 1) % width;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            i = 0;
            j = 0;

            try
            {
                using (StreamReader reader = new StreamReader(roomName))
                {
                    while (reader.Peek() >= 0 && j < height)
                    {
                        char character = (char)reader.Read();
                        /*if (i + 1 >= width)
                            ++j;

                        if (j >= height)
                            break;*/
                        Tile tile = null;
                        Item item = null;
                        EnemyEntity enemy = null;
                        switch (character)
                        {
                            case '0':
                                tile = new Tile(screenManager, new AABB((i + 1) * cellWidth, (j + 1) * cellHeight, cellWidth, cellHeight), Tile.Ground, this);
                                tile.loadContent(content);
                                tiles.Add(tile);
                                break;

                            case 'B':
                                tile = new Tile(screenManager, new AABB((i + 1) * cellWidth, (j + 1) * cellHeight, cellWidth, cellHeight), Tile.BigBoulder, this);
                                tile.loadContent(content);
                                tiles.Add(tile);
                                break;

                            case ' ':
                                break;

                            case '\r':
                                i = 0;
                                ++j;
                                continue;

                            case '\n':
                                continue;

                            case '1':
                                playerPos.X = i * cellWidth;
                                playerPos.Y = j * cellHeight;
                                break;

                            case '2':
                                enemy = new Slime(screenManager, new Vector2((i + 1) * cellWidth, (j + 1) * cellHeight), this, player);
                                enemy.loadContent(content);
                                enemies.Add(enemy);
                                ++enemyCount;
                                break;

                            case '3':
                                tile = new Tile(screenManager, new AABB((i + 1) * cellWidth, (j + 1) * cellHeight, cellWidth, cellHeight), Tile.BigBoulder, this);
                                tile.Solid = false;
                                tiles.Add(tile);
                                item = new RandomEffectItem(screenManager, new Vector2((i + 1) * cellWidth, (j + 1) * cellHeight), this, screenManager.SpriteSheet);
                                items.Add(item);
                                break;

                            case '4':
                                enemy = new Bat(screenManager, new Vector2((i + 1) * cellWidth, (j + 1) * cellHeight), this, player);
                                enemy.loadContent(content);
                                enemies.Add(enemy);
                                ++enemyCount;
                                break;

                            case '5':
                                enemy = new Boss(screenManager, new Vector2((i + 1) * cellWidth, (j + 1) * cellHeight), this, player, floor);
                                enemy.loadContent(content);
                                enemies.Add(enemy);
                                ++enemyCount;
                                tile = new Tile(screenManager, new AABB((i + 1) * cellWidth, (j + 1) * cellHeight, cellWidth, cellHeight), Tile.Empty, this);
                                tile.Solid = false;
                                tiles.Add(tile);
                                exit = tile;
                                break;

                            case '6':
                                enemy = new HorizontalSpike(screenManager, new Vector2((i + 1) * cellWidth, (j + 1) * cellHeight), this, player);
                                enemy.loadContent(content);
                                enemies.Add(enemy);
                                break;

                            default:
                                throw new NotImplementedException();
                        }

                        i = (i + 1) % (width-1);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            if (enemyCount <= 0)
            {
                isRoomCleared = true;
                openDoors();
            }
        }

        public void checkHorizontalCollisions()
        {
            foreach (Tile tile in tiles)
            {
                if (!tile.Solid)
                    continue;

                if (tile.CollisionBox.intersects(player.CollisionBox))
                {
                    int depth = tile.CollisionBox.getHorizontalIntersection(player.CollisionBox);
                    player.onCollision(depth, 0);

                    if( tile == rightDoor && IsRoomCleared )
                    {
                        isTransitioning = true;
                        transitionProgress = 0f;
                        transitionVector.X = (float)-width * cellWidth;
                        transitionVector.Y = 0;
                        nextRoom = NextRoom.Right;
                    }
                    else if (tile == leftDoor && isRoomCleared)
                    {
                        isTransitioning = true;
                        transitionProgress = 0f;
                        transitionVector.X = (float)width * cellWidth;
                        transitionVector.Y = 0;
                        nextRoom = NextRoom.Left;
                    }
                    else if (tile == exit && isRoomCleared)
                    {
                        GoToNextFloor = true;
                    }
                }

                foreach (EnemyEntity enemy in enemies)
                {
                    if (tile.CollisionBox.intersects(enemy.CollisionBox))
                    {
                        int depth = tile.CollisionBox.getHorizontalIntersection(enemy.CollisionBox);
                        enemy.onCollision(depth, 0);
                    }
                }
            }
        }

        public void checkVerticalCollisions()
        {
            foreach (Tile tile in tiles)
            {
                if (!tile.Solid)
                    continue;

                if (tile.CollisionBox.intersects(player.CollisionBox))
                {
                    int depth = tile.CollisionBox.getVerticalIntersection(player.CollisionBox);
                    player.onCollision(0, depth);

                    if (tile == topDoor && IsRoomCleared)
                    {
                        isTransitioning = true;
                        transitionProgress = 0f;
                        transitionVector.X = 0;
                        transitionVector.Y = (float)height * cellHeight;
                        nextRoom = NextRoom.Up;
                    }
                    else if (tile == bottomDoor && isRoomCleared)
                    {
                        isTransitioning = true;
                        transitionProgress = 0f;
                        transitionVector.X = 0;
                        transitionVector.Y = (float)-height * cellHeight;
                        nextRoom = NextRoom.Down;
                    }
                    else if (tile == exit && isRoomCleared)
                    {
                        GoToNextFloor = true;
                    }
                }

                foreach (EnemyEntity enemy in enemies)
                {
                    if (tile.CollisionBox.intersects(enemy.CollisionBox))
                    {
                        int depth = tile.CollisionBox.getVerticalIntersection(enemy.CollisionBox);
                        enemy.onCollision(0, depth);
                    }
                }
            }
        }

        public void checkAttacks()
        {
            foreach (Item item in items)
            {
                if (item.CollisionBox.intersects(player.CollisionBox))
                    item.onCollection(player);
            }

            foreach (EnemyEntity enemy in enemies)
            {
                if (enemy.IsHurtingOnContact
                    && !player.IsInvincible
                    && enemy.CollisionBox.intersects(player.CollisionBox))
                {
                    player.onGettingHit(enemy);
                }

                if (player.IsAttacking
                    && !enemy.IsInvincible
                    && enemy.IsHittable
                    && !player.AttackRect.isEmpty()
                    && player.AttackRect.intersects(enemy.CollisionBox))
                {
                    player.onHitting(enemy);
                }

                if (!player.IsInvincible
                    && enemy.IsAttacking
                    && !enemy.AttackRect.isEmpty()
                    && enemy.AttackRect.intersects(player.CollisionBox))
                {
                    player.onGettingHit(enemy);
                }
            }
        }

        public override void loadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            //loadRoom(content,  false, false, false, false);
        }

        public override void updateHorizontal(float dt)
        {
            foreach (EnemyEntity enemy in enemies)
            {
                enemy.updateHorizontal(dt);
            }

            foreach (Item item in items)
            {
                item.updateHorizontal(dt);
            }
        }

        public override void updateVertical(float dt)
        {
            foreach (EnemyEntity enemy in enemies)
            {
                enemy.updateVertical(dt);
            }

            foreach (Item item in items)
            {
                item.updateVertical(dt);
            }

            foreach (EnemyEntity enemy in enemiesToAdd)
            {
                enemies.Add(enemy);
            }

            foreach (EnemyEntity enemy in enemiesToRemove)
            {
                enemies.Remove(enemy);
            }

            enemiesToAdd.Clear();
            enemiesToRemove.Clear();

            foreach (Item item in itemsToAdd)
            {
                items.Add(item);
            }

            foreach (Item item in itemsToRemove)
            {
                items.Remove(item);
            }

            itemsToAdd.Clear();
            itemsToRemove.Clear();
        }

        public override void draw(GameTime gameTime)
        {
            foreach (Tile tile in tiles)
            {
                tile.draw(gameTime);
            }

            foreach (Item item in items)
            {
                item.draw(gameTime);
            }

            foreach (EnemyEntity enemy in enemies)
            {
                enemy.draw(gameTime);
            }            
        }

        public override void onCollision(int pushX, int pushY)
        {
        }

        public override void handleInput(Input.InputState inputState)
        {
        }

        public Vector2 PlayerPos
        {
            get { return playerPos; }
        }

        public void addEnemy(EnemyEntity enemy)
        {
            if (enemy.CountTowardsEnemyCount)
                ++enemyCount;
            enemiesToAdd.Add(enemy);
        }

        public void removeEnemy(EnemyEntity enemy)
        {
            if (enemy.CountTowardsEnemyCount)
                --enemyCount;
            if (enemyCount <= 0)
            {
                isRoomCleared = true;
                openDoors();
            }

            enemiesToRemove.Add(enemy);
        }

        void openDoors()
        {
            if (leftDoor != null)
                leftDoor.sourceRect = Tile.DoorOpenLeft;
            if (rightDoor != null)
                rightDoor.sourceRect = Tile.DoorOpenRight;
            if (topDoor != null)
                topDoor.sourceRect = Tile.DoorOpenUp;
            if (bottomDoor != null)
                bottomDoor.sourceRect = Tile.DoorOpenDown;
            if (exit != null)
            {
                exit.sourceRect = Tile.Exit;
                exit.Solid = true;
            }
        }

        public void addItem(Item item)
        {
            itemsToAdd.Add(item);
        }

        public void removeItem(Item item)
        {
            itemsToRemove.Add(item);
        }

        public bool transition(float dt)
        {
            if (isTransitioning)
            {
                if (!isTransitioningFrom)
                {
                    transitionProgress += transitionSpeed * dt;
                    if (transitionProgress >= 1f)
                        return true;
                }
                else
                {
                    transitionProgress -= transitionSpeed * dt;
                    if (transitionProgress <= 0f)
                        return true;
                }

                TransitionMatrix = Matrix.CreateTranslation(transitionVector.X * transitionProgress, transitionVector.Y * transitionProgress, 0);
                return false;
            }

            return true;
        }

        public void setTransitionFrom(NextRoom from)
        {
            if (isTransitioning)
                return;

            isTransitioning = true;
            isTransitioningFrom = true;
            transitionVector = Vector2.Zero;
            transitionProgress = 1f;
            switch (from)
            {
                case NextRoom.Left:
                    transitionVector.X = (float)-width * cellWidth;
                    break;

                case NextRoom.Up:
                    transitionVector.Y = (float)-height * cellHeight;
                    break;

                case NextRoom.Right:
                    transitionVector.X = (float)width * cellWidth;
                    break;

                case NextRoom.Down:
                    transitionVector.Y = (float)height * cellHeight;
                    break;
            }
        }

        public Vector2 getEntryTransition(NextRoom from)
        {
            switch (from)
            {
                case NextRoom.Left:
                    return new Vector2(-64f, cellHeight * (height - 1) / 2f);

                case NextRoom.Up:
                    return new Vector2(cellWidth * (width - 1) / 2f, -64f);

                case NextRoom.Right:
                    return new Vector2(cellWidth * width + 32f, cellHeight * (height - 1) / 2f);

                case NextRoom.Down:
                    return new Vector2(cellWidth * (width - 1) / 2f, cellHeight * height + 32f);

                default:
                    throw new ArgumentException();
            }
        }

        public Vector2 getEntryAs(NextRoom from)
        {
            switch (from)
            {
                case NextRoom.Left:
                    return new Vector2(cellWidth * width - 64f, cellHeight * (height - 1) / 2f);

                case NextRoom.Up:
                    return new Vector2(cellWidth * (width-1) / 2f, cellHeight * height - 64f);

                case NextRoom.Right:
                    return new Vector2(32f, cellHeight * (height - 1) / 2f);

                case NextRoom.Down:
                    return new Vector2(cellWidth * (width - 1) / 2f, 32f);

                default:
                    throw new ArgumentException();
            }
        }

        public void stopTransition()
        {
            isTransitioning = false;
            isTransitioningFrom = false;
            transitionVector = Vector2.Zero;
            transitionProgress = 0f;
            TransitionMatrix = Matrix.Identity;
        }
    }
}
