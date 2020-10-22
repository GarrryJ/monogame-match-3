using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace match_3
{
    public class Board
    {
        private Explotion explotion;
        private const int SPEED = 10;
        private Random random;
        private Texture2D texture;
        private Texture2D textureExplotion;
        private Piece[,] pieces;
        private bool pieceChanged;
        private Bomb replacedBomb;
        private Point changePos;
        private bool isEnableToTap;
        public bool IsInit {get; set;}
        public long GameScore {get; set;}
        

        public Board(Texture2D texture, Texture2D textureExplotion){
            this.texture = texture;
            this.textureExplotion = textureExplotion;
            IsInit = false;
            pieces = new Piece[8, 8];
            random = new Random();
        }

        private Rectangle TextureType(Piece piece)
        {
            Point pointSize = new Point(100, 100);
            int bonus = 0;
            if (piece.hor)
                bonus = 100;
            if (piece.ver)
                bonus = 200;
            switch(piece.type)
            {
                case Type.Yellow:
                    return new Rectangle(new Point(0, 0 + bonus), pointSize);
                case Type.Blue:
                    return new Rectangle(new Point(100, 0 + bonus), pointSize);
                case Type.Red:
                    return new Rectangle(new Point(200, 0 + bonus), pointSize);
                case Type.Green:
                    return new Rectangle(new Point(300, 0 + bonus), pointSize);
                case Type.Purple:
                    return new Rectangle(new Point(400, 0 + bonus), pointSize);
                case Type.Bomb:
                    return new Rectangle(new Point(200, 400), pointSize);
                default:
                    return new Rectangle(new Point(600, 600), pointSize);
            }
        }

        private Type RandomType()
        {
            return (Type)random.Next(0, 5);
        }

        
        private Type TypeChanger(Type type)
        {
            type++;
            if (type == Type.Bomb)
                type = 0;
            return type;
        }
        private bool SwapCheck(Point pos)
        {
            bool result = false;

            if (pos.X > 1)
                result = result || (pieces[pos.X, pos.Y].type == pieces[pos.X - 1, pos.Y].type && pieces[pos.X, pos.Y].type == pieces[pos.X - 2, pos.Y].type);
            if (pos.X > 0 && pos.X < 7)
                result = result || (pieces[pos.X, pos.Y].type == pieces[pos.X - 1, pos.Y].type && pieces[pos.X, pos.Y].type == pieces[pos.X + 1, pos.Y].type);
            if (pos.X < 6)
                result = result || (pieces[pos.X, pos.Y].type == pieces[pos.X + 1, pos.Y].type && pieces[pos.X, pos.Y].type == pieces[pos.X + 2, pos.Y].type);

            if (pos.Y > 1)
                result = result || (pieces[pos.X, pos.Y].type == pieces[pos.X, pos.Y - 1].type && pieces[pos.X, pos.Y].type == pieces[pos.X, pos.Y - 2].type);
            if (pos.Y > 0 && pos.Y < 7)
                result = result || (pieces[pos.X, pos.Y].type == pieces[pos.X, pos.Y - 1].type && pieces[pos.X, pos.Y].type == pieces[pos.X, pos.Y + 1].type);
            if (pos.Y < 6)
                result = result || (pieces[pos.X, pos.Y].type == pieces[pos.X, pos.Y + 1].type && pieces[pos.X, pos.Y].type == pieces[pos.X, pos.Y + 2].type);

            return result;
        }


        private void BoardCheck()
        {
            if (replacedBomb.IsReplaced)
            {   
                BombDestroy(replacedBomb.point);
                replacedBomb.IsReplaced = false;
            }
            int match;
            for (int i = 0; i < 8; i++)
            {
                match = 1;
                for (int y = 1; y < 8; y++)
                {
                    if (pieces[i, y - 1].type == pieces[i, y].type)
                        match++;

                    if (!(pieces[i, y - 1].type == pieces[i, y].type) || y == 7)
                    {
                        if (match > 2 && pieces[i, y - 1].type != Type.Nothing)
                        {
                            if (y == 7 && pieces[i, y - 1].type == pieces[i, y].type)
                                MatchDestroy(new Point(i, y), match, true);
                            else
                                MatchDestroy(new Point(i, y - 1), match, true);
                            BoardCheck();
                            return;
                        }
                        match = 1;
                    }
                }
                match = 1;
                for (int x = 1; x < 8; x++)
                {
                    if (pieces[x - 1, i].type == pieces[x, i].type)
                        match++;
                    
                    if (!(pieces[x - 1, i].type == pieces[x, i].type) || x == 7)
                    {
                        if (match > 2 && pieces[x - 1, i].type != Type.Nothing)
                        {
                            if (x == 7 && pieces[x - 1, i].type == pieces[x, i].type)
                                MatchDestroy(new Point(x , i), match, false);
                            else
                                MatchDestroy(new Point(x - 1, i), match, false);
                            BoardCheck();
                            return;
                        }
                        match = 1;
                    }
                }
            }
        }

        private void Destroy(Point point)
        {
            explotion.IsBoom = true;
            explotion.boomList.Remove(point);
            explotion.boomList.Add(pieces[point.X, point.Y].point);
            if (pieces[point.X, point.Y].type == Type.Bomb)
                BombDestroy(point);
            else
                pieces[point.X, point.Y].type = Type.Nothing;
        }

        private void BombDestroy(Point point)
        {
            pieces[point.X, point.Y].type = Type.Nothing;
            point.X--;
            point.Y--;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (i >= 0 && j >= 0 && i < 8 && j < 8)
                        Destroy(new Point(point.X + i, point.Y + j));
        }

        private void IncGameScore(int b)
        {
            GameScore += (100 * b + (b - 3) * 25);
        }

        private void MatchDestroy(Point point, int match, bool ver)
        {
            if (ver)
            {
                for (int i = 0; i < match; i++)
                {
                    if (match == 4 && i == (match + 1)/2)
                        pieces[point.X, point.Y - i].ver = true;
                    else if (match == 5 && i == (match + 1)/2)
                        pieces[point.X, point.Y - i].type = Type.Bomb;
                    else
                        Destroy(new Point(point.X, point.Y - i));
                }
            }
            else
            {
                for (int i = 0; i < match; i++)
                {
                    if (match == 4 && i == (match + 1)/2)
                        pieces[point.X - i, point.Y].hor = true;
                    else if (match == 5 && i == (match + 1)/2)
                        pieces[point.X - i, point.Y].type = Type.Bomb;
                    else
                        Destroy(new Point(point.X - i, point.Y));
                }
            }
        }

        private void BoardFill()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 1; j < 8; j++)
                {
                    if (pieces[i, j].type == Type.Nothing)
                    {
                        int p = j;
                        
                        while (p > 0)
                        {
                            SimpleSwap(new Point(i, p), new Point(i, p - 1));
                            p--;
                        }
                    }
                }
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (pieces[i, j].type == Type.Nothing)
                        pieces[i, j] = new Piece(RandomType(), new Point(100 + i * 100, 100 - (random.Next(5, 10) * SPEED)), false, false);
        }

        private void SimpleSwap(Point posFirst, Point posSecond)
        {
            Piece piece = pieces[posFirst.X, posFirst.Y];
            pieces[posFirst.X, posFirst.Y] = pieces[posSecond.X, posSecond.Y];
            pieces[posSecond.X, posSecond.Y] = piece;
        }
        private void Swap(Point posFirst, Point posSecond)
        {
            isEnableToTap = false;

            SimpleSwap(posFirst, posSecond);

            if (pieces[posFirst.X, posFirst.Y].type == Type.Bomb)
            {   
                replacedBomb.point = posFirst;
                replacedBomb.IsReplaced = true;
                return;
            }
            else if (pieces[posSecond.X, posSecond.Y].type == Type.Bomb)
            {   
                replacedBomb.point = posSecond;
                replacedBomb.IsReplaced = true;
                return;
            }
            else if ((!(SwapCheck(posFirst) || SwapCheck(posSecond))))
            {
                isEnableToTap = true;
                SimpleSwap(posFirst, posSecond);

                int swapSpeed = SPEED + 40;

                if (pieces[posFirst.X, posFirst.Y].point.X > pieces[posSecond.X, posSecond.Y].point.X)
                {
                    pieces[posFirst.X, posFirst.Y].point.X -= swapSpeed;
                    pieces[posSecond.X, posSecond.Y].point.X += swapSpeed;
                }
                else if (pieces[posFirst.X, posFirst.Y].point.X < pieces[posSecond.X, posSecond.Y].point.X)
                {
                    pieces[posFirst.X, posFirst.Y].point.X += swapSpeed;
                    pieces[posSecond.X, posSecond.Y].point.X -= swapSpeed;
                }
                if (pieces[posFirst.X, posFirst.Y].point.Y > pieces[posSecond.X, posSecond.Y].point.Y)
                {
                    pieces[posFirst.X, posFirst.Y].point.Y -= swapSpeed;
                    pieces[posSecond.X, posSecond.Y].point.Y += swapSpeed;
                }
                else if (pieces[posFirst.X, posFirst.Y].point.Y < pieces[posSecond.X, posSecond.Y].point.Y)
                {
                    pieces[posFirst.X, posFirst.Y].point.Y += swapSpeed;
                    pieces[posSecond.X, posSecond.Y].point.Y -= swapSpeed;
                }
            }


        }

        private void RefreshBoard()
        {
            isEnableToTap = true;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (pieces[i, j].point.X != (i + 1) * 100 || pieces[i, j].point.Y != (j + 1) * 100)
                        isEnableToTap = false;

                    if (pieces[i, j].point.X > i * 100 + 100)
                        pieces[i, j].point.X = pieces[i, j].point.X - SPEED;
                    else if (pieces[i, j].point.X < i * 100 + 100)
                        pieces[i, j].point.X = pieces[i, j].point.X + SPEED;
                    
                    if (pieces[i, j].point.Y > j * 100 + 100)
                        pieces[i, j].point.Y = pieces[i, j].point.Y - SPEED;
                    else if (pieces[i, j].point.Y < j * 100 + 100)
                        pieces[i, j].point.Y = pieces[i, j].point.Y + SPEED;
                }
        }
        public void Init()
        {
            replacedBomb = new Bomb();
            explotion = new Explotion();
            GameScore = 0;
            changePos = new Point(-1, -1);
            pieceChanged = false;
            isEnableToTap = true;
            IsInit = true;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    pieces[i, j] = new Piece(RandomType(), new Point(100 + i * 100, j * 100 - (random.Next(5, 10) * SPEED)), false, false);

                    if (j > 1 && pieces[i, j].type ==  pieces[i, j - 1].type && pieces[i, j].type == pieces[i, j - 2].type)
                    {
                        pieces[i, j].type = TypeChanger(pieces[i, j].type);
                    }
                    if (i > 1 && pieces[i, j].type ==  pieces[i - 1, j].type && pieces[i, j].type == pieces[i - 2, j].type)
                    {
                        pieces[i - 2, j].type = TypeChanger(pieces[i - 2, j].type);
                    }
                }
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (j > 1 && pieces[i, j].type ==  pieces[i, j - 1].type && pieces[i, j].type == pieces[i, j - 2].type)
                    {
                        pieces[i, j].type = TypeChanger(pieces[i, j].type);
                    }
                    if (i > 1 && pieces[i, j].type ==  pieces[i - 1, j].type && pieces[i, j].type == pieces[i - 2, j].type)
                    {
                        pieces[i - 2, j].type = TypeChanger(pieces[i - 2, j].type);
                    }
                }
        }
        public void MouseLeftClick(int x, int y)
        {
            if (x > 100 && x < 900 && y > 100 && y < 900 && isEnableToTap)
            {
                Point newPos = new Point(x/100 - 1, y/100 - 1);
                if (pieceChanged && ( (changePos.X == newPos.X && Math.Abs(changePos.Y - newPos.Y) == 1) || (changePos.Y == newPos.Y && Math.Abs(changePos.X - newPos.X) == 1) ))
                {
                    Swap(changePos, newPos);
                    pieceChanged = false;
                } 
                else 
                {
                    pieceChanged = true;
                    changePos.X = x/100 - 1;
                    changePos.Y = y/100 - 1;
                }
            }
            else
            {
                pieceChanged = false;
            }
        }

        public void MouseRightClick()
        {
            pieceChanged = false;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            RefreshBoard();
            if (isEnableToTap)
            {
                BoardCheck();
                BoardFill();
            }
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    spriteBatch.Draw(texture, new Vector2(100 + i * 100, 100 + j * 100), new Rectangle(0, 500, 100, 100), Color.White);
                    if (i == changePos.X && j == changePos.Y && pieceChanged)
                    {
                        spriteBatch.Draw(texture, new Vector2(100 + i * 100, 100 + j * 100), new Rectangle(100, 300, 100, 100), Color.White);
                    }
                    spriteBatch.Draw(texture, new Vector2(pieces[i, j].point.X, pieces[i, j].point.Y), TextureType(pieces[i,j]), Color.White);
                }
            if (explotion.IsBoom)
            {
                if (explotion.TicForScore())
                    IncGameScore(explotion.boomList.Count);
                Rectangle rectangle = explotion.TextureRect();
                explotion.boomList.ForEach(p => spriteBatch.Draw(textureExplotion, new Vector2(p.X - 20, p.Y - 20), rectangle, Color.White));
            }
        }
    }
}