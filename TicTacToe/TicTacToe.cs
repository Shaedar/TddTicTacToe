using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe
{
    public class TicTacToe
    {
        class Turn
        {
            public Turn(char mark, int x, int y)
            {
                Mark = mark;
                X = x;
                Y = y;
            }

            public char Mark { get; private set; }
            public int X { get; private set; }
            public int Y { get; private set; }
        }

        const int BoardHeight = 3;
        const int BoardWidth = 3;
        readonly IEnumerable<int> allowedHeights = Enumerable.Range(1, BoardHeight);
        readonly IEnumerable<int> allowedWidths = Enumerable.Range(1, BoardWidth);
        readonly IList<Turn> turns;

        readonly Func<Turn, bool> turnIsOnDiagonal = t => t.X == t.Y;
        readonly Func<Turn, bool> turnIsOnInvertedDiagonal = t => t.Y == BoardHeight - t.X + 1;

        public TicTacToe()
        {
            turns = new List<Turn>();
        }

        IEnumerable<Func<Turn, bool>> VerticalLinePredicates
        {
            get
            {
                return allowedWidths.Select(w => new Func<Turn, bool>(t => t.X == w));
            }
        }

        IEnumerable<Func<Turn, bool>> HorizontalLinePredicates
        {
            get
            {
                return allowedHeights.Select(h => new Func<Turn, bool>(t => t.Y == h));
            }
        }

        void ThrowIfGameHasEnded()
        {
            char winner = GetWinner();

            if (winner == 'X' || winner == 'O')
            {
                throw new Exception();
            }
        }

        void ThrowIfCoordinatesAlreadyPlayed(int x, int y)
        {
            if (turns.Any(t => t.X == x && t.Y == y))
            {
                throw new Exception();
            }
        }

        void ThrowIfPreviousTurnWasBySameThePlayer(char turn)
        {
            if (turn == turns.Select(t => t.Mark).LastOrDefault())
            {
                throw new Exception();
            }
        }

        void ThrowIfFirstTurnIsO(char turn)
        {
            if (!turns.Any() && turn == 'O')
            {
                throw new Exception();
            }
        }

        void ThrowIfHeightNotAllowed(int y)
        {
            if (!IsHeightAllowed(y))
            {
                throw new Exception();
            }
        }

        bool IsHeightAllowed(int y)
        {
            return allowedHeights.Contains(y);
        }

        void ThrowIfWidthNotAllowed(int x)
        {
            if (!IsWidthAllowed(x))
            {
                throw new Exception();
            }
        }

        bool IsWidthAllowed(int x)
        {
            return allowedWidths.Contains(x);
        }

        public char GetWinner()
        {
            List<Func<Turn, bool>> predicates = new List<Func<Turn, bool>>();

            predicates.Add(turnIsOnDiagonal);
            predicates.Add(turnIsOnInvertedDiagonal);
            predicates.AddRange(VerticalLinePredicates);
            predicates.AddRange(HorizontalLinePredicates);

            return GetWinner(predicates);
        }

        private char GetWinner(IEnumerable<Func<Turn, bool>> predicates)
        {
            foreach (var predicate in predicates)
            {
                if (HasWinner(predicate))
                {
                    return GetWinner(predicate);
                }
            }

            return 'Å';
        }

        private bool HasWinner(Func<Turn, bool> predicate)
        {
            var filteredTurns = turns.Where(predicate);

            return filteredTurns.Count() == 3
                   && GetDistinctMarks(filteredTurns).Count() == 1;
        }

        private char GetWinner(Func<Turn, bool> predicate)
        {
            var filteredTurns = turns.Where(predicate);

            return GetDistinctMarks(filteredTurns).Single();
        }

        private IEnumerable<char> GetDistinctMarks(IEnumerable<Turn> turns)
        {
            return turns.Select(t => t.Mark).Distinct();
        }

        public void Play(char mark, int x, int y)
        {
            ThrowIfGameHasEnded();
            ThrowIfFirstTurnIsO(mark);
            ThrowIfPreviousTurnWasBySameThePlayer(mark);
            ThrowIfCoordinatesAlreadyPlayed(x, y);
            ThrowIfHeightNotAllowed(y);
            ThrowIfWidthNotAllowed(x);

            Turn turn = new Turn(mark, x, y);

            turns.Add(turn);
        }
    }
}