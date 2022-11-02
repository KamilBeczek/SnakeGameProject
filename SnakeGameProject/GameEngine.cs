using System;
using System.Collections.Generic;
using System.Media;
using System.Windows;

namespace SnakeGameProject
{
    public class GameEngine
    {

        private int Rows;
        private int Columns;
        public int Score;
        public bool GameOver;
        public GridValue[,] GameBoard;
        public SnakeDirection Dir { get; set; }

        public LinkedList<ObjectPosition> snakeBody = new LinkedList<ObjectPosition>();
        private readonly LinkedList<SnakeDirection> directionHisotry = new LinkedList<SnakeDirection>();
        readonly Random Random = new Random();


        public GameEngine(int rows, int cols)
        {
            Rows = rows;
            Columns = cols;
            GameBoard = new GridValue[rows, cols];
            Dir = SnakeDirection.Right;
            Music.DeadSound.Load();
            Music.EatSound.Load();
            GameStart();
            SnakeRespawn();
            SpawnFood();
        }

        public void GameStart()
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    GameBoard[r, c] = GridValue.EMPTY;

                }
            }
        }

        public void SnakeRespawn()
        {
            int row = Rows/2;

            for (int column = 1; column <= 3; column++)
            {
                GameBoard[row, column] = GridValue.SNAKE;
                snakeBody.AddFirst(new ObjectPosition(row, column));
            }
        }

        public void SpawnFood()
        {
            List<ObjectPosition> emptySpace = new List<ObjectPosition>();

            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    if (GameBoard[row, column] == GridValue.EMPTY)
                    {
                        emptySpace.Add(new ObjectPosition(row, column));

                    }
                }
            }
            int lenghtOfEmptySpaces = Random.Next(1, emptySpace.Count);
            ObjectPosition position = emptySpace[lenghtOfEmptySpaces];
            GameBoard[position.PostionRow, position.PostionColumn] = GridValue.FOOD;
            emptySpace.Clear();
        }

        public ObjectPosition SnakeHeadPosition()
        {
            return snakeBody.First.Value;
        }

        public ObjectPosition SnakeTailPosition()
        {
            return snakeBody.Last.Value;
        }

        private void AddHead(ObjectPosition move)
        {
            snakeBody.AddFirst(move);
            GameBoard[move.PostionRow, move.PostionColumn] = GridValue.SNAKE;
        }

        private void RemoveTail()
        {
            ObjectPosition snakeTail = snakeBody.Last.Value;
            GameBoard[snakeTail.PostionRow, snakeTail.PostionColumn] = GridValue.EMPTY;
            snakeBody.RemoveLast();
        }

        public SnakeDirection GetLastDirection()
        {
            if (directionHisotry.Count == 0)
            {
                return Dir;
            }

            return directionHisotry.Last.Value;
        }

        private bool CanChangeDirection(SnakeDirection newDir)
        {
            SnakeDirection lastDir = GetLastDirection();
            SnakeDirection lastDirOpposite = lastDir.Opposite();
            return newDir != lastDir && newDir != lastDirOpposite;
        }

        public void ChangeDirection(SnakeDirection dir)
        {
            if (CanChangeDirection(dir))
            {
                directionHisotry.AddLast(dir);
            }
        }

        private bool IsOutsideGrid(ObjectPosition pos)
        {
            return pos.PostionRow < 0 || pos.PostionRow >= Rows || pos.PostionColumn < 0 || pos.PostionColumn >= Columns;
        }

        private GridValue WillHit(ObjectPosition nextMove)
        {
            if (IsOutsideGrid(nextMove))
            {
                return GridValue.BORDER;
            }

            if (nextMove == SnakeTailPosition())
            {
                return GridValue.EMPTY;
            }
            return GameBoard[nextMove.PostionRow, nextMove.PostionColumn];
        }

        public void Move()
        {
            if (directionHisotry.Count > 0)
            {
                Dir = directionHisotry.First.Value;
                directionHisotry.RemoveFirst();
            }

            ObjectPosition nextMove = SnakeHeadPosition().Translate(Dir);
            GridValue checkNextMovePosition = WillHit(nextMove);

            if (checkNextMovePosition == GridValue.BORDER || checkNextMovePosition == GridValue.SNAKE)
            {
                DeadSound();
                GameOver = true;
                
            }

            else if (checkNextMovePosition == GridValue.EMPTY)
            {
                AddHead(nextMove);
                RemoveTail();
            }

            else if (checkNextMovePosition == GridValue.FOOD)
            {
                EatSound();
                AddHead(nextMove);
                Score++;
                SpawnFood();
            }
        }

        public void EatSound()
        {
            Music.EatSound.Play();
        }

        public void DeadSound()
        {
            Music.DeadSound.Play();
        }
    }
}