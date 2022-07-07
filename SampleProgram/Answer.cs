using System;
using System.Collections.Generic;
using System.Linq;
using ChessLib;

namespace SampleProgram
{
    public static class Common
    {
        public static HashSet<Position> occupied = new HashSet<Position>();  // save the occupied positions on board
    }

    public class Pieces
    {
        public Position currPos { get; set; }
        public Pieces(Position currPosition)
        {
            currPos = currPosition;
        }
        public virtual int[,] getMoves()
        {
            return new int[,] {};
        }
        public virtual IEnumerable<Position> ValidMovesFor()
        {
            var Moves = getMoves();
            var newX = currPos.X;
            var newY = currPos.Y;
            var i = 0;
            var k = 1;
            while ((newX > 0 || newX <= 8 || newY > 0 || newY <= 8) && i <= Moves.GetUpperBound(0))
            {
                newX = currPos.X + Moves[i, 0] * k;
                newY = currPos.Y + Moves[i, 1] * k;
                if (newX > 0 && newX <= 8 && newY > 0 && newY <= 8)
                {
                    k++;
                    yield return new Position(newX, newY);
                }
                else
                {
                    i++;
                    k = 1;
                }
            }
        }
    }

    /// <summary>
    /// Bishop: Moves diagonally, any distance within board boundaries
    /// </summary>
    public class Bishop: Pieces
    {
        public static readonly int[,] moves = new[,] { { -1, -1}, { -1, 1}, { 1, 1}, { 1, -1}};
        public Bishop(Position currPosition) : base(currPosition)
        {
        }
        public override int[,] getMoves()
        {
            return moves;
        }

    }
    /// <summary>
    /// Queen: Moves diagonally, horizontally or vertically, any distance within board boundaries 
    /// </summary>
    public class Queen : Pieces
    {
        public static readonly int[,] moves = new[,] { { -1, -1 }, { -1, 1 }, { 1, 1 }, { 1, -1 }, { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };
        public Queen(Position currPosition) : base(currPosition)
        {
        }
        public override int[,] getMoves()
        {
            return moves;
        }
        
    }
    
    public class Knight : Pieces
    {
        public Knight(Position currPosition) : base(currPosition)
        {
        }
        public override IEnumerable<Position> ValidMovesFor()
        {
            var kightmoves = new ChessLib.KnightMove();
            return kightmoves.ValidMovesFor(currPos);
        }
    }
    public class ComplexGame
    {
        private List<Pieces> chess = new List<Pieces>();
        private readonly Random _rnd = new Random();
        public void Setup()
        {
            // TODO: Set up the state of the game here
            // ---------------------- Create five roles on the board: 1 Queen, 2 Bishops, 2 Knights ----------
            var tmp = new Position(1, 4);
            var queen = new Queen(tmp);
            Common.occupied.Add(tmp);
            chess.Add(queen);

            tmp = new Position(1, 3);
            var bishop1 = new Bishop(tmp);
            Common.occupied.Add(tmp);
            chess.Add(bishop1);

            tmp = new Position(1, 6);
            var bishop2 = new Bishop(tmp);
            Common.occupied.Add(tmp);
            chess.Add(bishop2);

            tmp = new Position(1, 2);
            var knight1 = new Knight(tmp);
            Common.occupied.Add(tmp); 
            chess.Add(knight1);

            tmp = new Position(1, 7);
            var knight2 = new Knight(tmp);
            Common.occupied.Add(tmp);
            chess.Add(knight2);
        }

        public void Play(int moves)
        {
            // TODO: Play the game moves here
            // Show the Start position of all pieces on the board
            Console.WriteLine("The Initial Position of All Pieces on Board:");
            for (var i = 0; i < chess.Count; i++)
            {
                Console.WriteLine("0: The position of {0}-{1} is {2}", chess[i].GetType().Name, i, chess[i].currPos);
            }
            
            // Randomly select the chess to start
            var idx = _rnd.Next(chess.Count);
            var pos = chess[idx].currPos;
            
            Console.WriteLine("Randomly Selected: The position of {0} is {1}", idx, pos);

            for (var move = 1; move <= moves; move++)
            {
                var role = chess[idx];
                var possibleMoves = role.ValidMovesFor().ToArray();
                
                // After get the possible moves, remove the elements that presenting in occupied set.
                possibleMoves = possibleMoves.Where(x => !Common.occupied.Contains(x)).ToArray();
                var newPos = possibleMoves[_rnd.Next(possibleMoves.Length)];

                // Update the occupied Hashset and original _chess list.
                Common.occupied.Remove(role.currPos);
                Common.occupied.Add(newPos);
                chess[idx].currPos = newPos;
                Console.WriteLine("{0}: The position of {1}-{2} is {3}", move, chess[idx].GetType().Name, idx, newPos);

                idx = _rnd.Next(chess.Count);
                pos = chess[idx].currPos;
            }
        }
    }

}
