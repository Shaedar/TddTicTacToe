using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace TicTacToe
{
    public class Turn
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

    public class TicTacToe
    {
        const int boardHeight = 3;
        const int boardWidth = 3;
        IEnumerable<int> allowedHeights = Enumerable.Range(1, boardHeight);
        IEnumerable<int> allowedWidths = Enumerable.Range(1, boardWidth);
        IList<Turn> turns;

        Func<Turn, bool> turnIsOnFirstHorizontalLine = t => t.Y == 1;
        Func<Turn, bool> turnIsOnDiagonal = t => t.X == t.Y;
        Func<Turn, bool> turnIsOnInvertedDiagonal = t => t.Y == boardHeight - t.X + 1;

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

        public Turn GetLastTurn()
        {
            return turns.Last();
        }

        public char GetWinner()
        {
            List<Func<Turn, bool>> predicates = new List<Func<Turn, bool>>();

            predicates.Add(turnIsOnFirstHorizontalLine);
            predicates.Add(turnIsOnDiagonal);
            predicates.Add(turnIsOnInvertedDiagonal);
            predicates.AddRange(VerticalLinePredicates);

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

	[TestFixture]
	public class Tests
	{
	    private TicTacToe sut;


	    [SetUp]
		public void Setup ()
	    {
	        sut = new TicTacToe();
	    }

	    void Play(char mark, int x, int y)
	    {
	        sut.Play(mark, x, y);
	    }

        Turn GetLastTurn()
        {
            return sut.GetLastTurn();
        }

        char GetWinner()
        {
            return sut.GetWinner();
        }

		[Test]
		public void PlayerCannotPlayTwiceInARow ()
		{
			Play ('X', 1, 1);

			Assert.Throws<Exception>(() => Play ('X', 1, 2));
		}

		[Test]
		public void CannotPlayMarkAboveTheBoardEdges ()
		{
			Assert.Throws<Exception>(() => Play ('X', 1, 4));
		}

		[Test]
		public void CannotPlayMarkBelowTheBoardEdges ()
		{
			Assert.Throws<Exception>(() => Play ('X', 1, 0));
		}

		[Test]
		public void CannotPlayMarkLeftOfTheBoardEdges ()
		{
			Assert.Throws<Exception>(() => Play ('X', 0, 1));
		}
		
		[Test]
		public void CannotPlayMarkRightOfTheBoardEdges ()
		{
			Assert.Throws<Exception>(() => Play ('X', 4, 1));
		}

		[Test]
		public void CannotPlayMarkOverAlreadyPlayedMark ()
		{
			Play ('X', 2, 2);

			Assert.Throws<Exception>(() => Play ('O', 2, 2));
		}

		[Test]
		public void ODoesNotStartTheGame ()
		{
			Assert.Throws<Exception>(() => Play('O', 1, 1));
		}

		[Test]
		public void XPlaysFirstAndTheMarkIsSaved ()
		{
            Play('X', 1, 1);

			Assert.That (GetLastTurn().Mark, Is.EqualTo('X'));
		}

	    [Test]
		public void XPlaysFirstAndTheXCoordinateIsSaved()
		{
            Play('X', 2, 3);

            Assert.That(GetLastTurn().X, Is.EqualTo(2));
		}

        [Test]
        public void XPlaysFirstAndTheYCoordinateIsSaved()
        {
            Play('X', 3, 2);

            Assert.That(GetLastTurn().Y, Is.EqualTo(2));
        }

	    [Test]
		public void XPlaysTheFirstHorizontalLineAndWins ()
		{
			Play ('X', 1, 1);
			Play ('O', 2, 2);
			Play ('X', 2, 1);
			Play ('O', 3, 3);
			Play ('X', 3, 1);

			Assert.That (GetWinner(), Is.EqualTo('X'));
	    }

        [Test]
        public void XPlaysTheSecondHorizontalLineAndWins()
        {
            Play('X', 1, 2);
            Play('O', 2, 1);
            Play('X', 2, 2);
            Play('O', 3, 3);
            Play('X', 3, 2);

            Assert.That(GetWinner(), Is.EqualTo('X'));
        }

		[Test]
		public void OPlaysThreeInDiagonalAndWins ()
		{
			Play ('X', 1, 2);
			Play ('O', 1, 1);
			Play ('X', 1, 3);
			Play ('O', 2, 2);
			Play ('X', 3, 1);
			Play ('O', 3, 3);

			Assert.That (GetWinner(), Is.EqualTo('O'));
		}

		[Test]
		public void XPlaysThreeInInvertedDiagonalAndWins()
		{
			Play ('X', 3, 1);
			Play ('O', 1, 1);
			Play ('X', 2, 2);
			Play ('O', 3, 3);
			Play ('X', 1, 3);

			Assert.That(GetWinner(), Is.EqualTo('X'));
		}

		[Test]
		public void XPlaysThreeInLastVerticallLineAndWins()
		{
			Play ('X', 3, 1);
			Play ('O', 2, 2);
			Play ('X', 3, 2);
			Play ('O', 2, 3);
			Play ('X', 3, 3);

			Assert.That (GetWinner(), Is.EqualTo('X'));
		}

		[Test]
		public void XWinsSoTheGameEndsAndNoMoreTurnsCanBePlayed()
		{
			Play ('X', 3, 1);
			Play ('O', 2, 2);
			Play ('X', 3, 2);
			Play ('O', 2, 3);
			Play ('X', 3, 3);

			Assert.Throws<Exception>(() => Play ('O', 1, 1));
		}

		[Test]
		public void XPlaysTheSecondVerticalLineAndWins()
		{
			Play ('X', 2, 1);
			Play ('O', 1, 1);
			Play ('X', 2, 2);
			Play ('O', 1, 2);
			Play ('X', 2, 3);

			Assert.That(GetWinner(), Is.EqualTo('X'));
		}

		[Test]
		public void XPlaysTheFirstVerticalLineAndWins()
		{
			Play ('X', 1, 1);
			Play ('O', 2, 1);
			Play ('X', 1, 2);
			Play ('O', 3, 2);
			Play ('X', 1, 3);
			
			Assert.That(GetWinner(), Is.EqualTo('X'));
		}

	}
}

       