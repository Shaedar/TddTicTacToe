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

	[TestFixture]
	public class Tests
	{
		const int boardHeight = 3;
		const int boardWidth = 3;
		IEnumerable<int> allowedHeights = Enumerable.Range(1, boardHeight);
		IEnumerable<int> allowedWidths = Enumerable.Range (1, boardWidth);
		IList<Turn> turns;

		Func<Turn, bool> turnIsOnFirstHorizontalLine = t => t.Y == 1;
		Func<Turn, bool> turnIsOnDiagonal = t => t.X == t.Y;

		[SetUp]
		public void Setup ()
		{
			turns = new List<Turn>();
		}

		void play (char mark, int x, int y)
		{
            ThrowIfFirstTurnIsO(mark);
            ThrowIfPreviousTurnWasBySameThePlayer(mark);
			ThrowIfCoordinatesAlreadyPlayed(x, y);
			ThrowIfHeightNotAllowed(y);
			ThrowIfWidthNotAllowed(x);
            
            Turn turn = new Turn(mark, x, y);
            
		    turns.Add(turn);
		}

		void ThrowIfCoordinatesAlreadyPlayed (int x, int y)
		{
			if (turns.Any (t => t.X == x && t.Y == y)) 
			{
				throw new Exception ();
			}
		}

	    void ThrowIfPreviousTurnWasBySameThePlayer (char turn)
		{
			if (turn == turns.Select(t => t.Mark).LastOrDefault ()) 
			{
				throw new Exception();
			}
		}

		void ThrowIfFirstTurnIsO (char turn)
		{
			if (!turns.Any () && turn == 'O') {
				throw new Exception ();
			}
		}

		
		void ThrowIfHeightNotAllowed (int y)
		{
			if (!IsHeightAllowed (y)) {
				throw new Exception ();
			}
		}
		
		bool IsHeightAllowed (int y)
		{
			return allowedHeights.Contains(y);
		}
		
		void ThrowIfWidthNotAllowed (int x)
		{
			if (!IsWidthAllowed (x)) 
			{
				throw new Exception ();
			}
		}
		
		bool IsWidthAllowed (int x)
		{
			return allowedWidths.Contains (x);
		}

		private Turn GetLastTurn()
		{
			return turns.Last();
		}

		private char GetWinner ()
		{
			return GetWinner(turnIsOnFirstHorizontalLine, turnIsOnDiagonal);
		}

		private char GetWinner (params Func<Turn, bool>[] predicates)
		{
			foreach(var predicate in predicates)
			{
				var filteredTurns = turns.Where(predicate);

				if(WinnerExists(filteredTurns))
				{
					return GetWinnerFromTurns(filteredTurns);
				}
			}

			return 'Å';
		}

		private bool WinnerExists (IEnumerable<Turn> turns)
		{
			return GetDistinctMarks(turns).Count() == 1;
		}

		private char GetWinnerFromTurns (IEnumerable<Turn> turns)
		{
			return GetDistinctMarks (turns).Single();
		}

		private IEnumerable<char> GetDistinctMarks (IEnumerable<Turn> turns)
		{
			return turns.Select(t => t.Mark).Distinct();
		}

		[Test]
		public void PlayerCannotPlayTwiceInARow ()
		{
			play ('X', 1, 1);

			Assert.Throws<Exception>(() => play ('X', 1, 2));
		}

		[Test]
		public void CannotPlayMarkAboveTheBoardEdges ()
		{
			Assert.Throws<Exception>(() => play ('X', 1, 4));
		}

		[Test]
		public void CannotPlayMarkBelowTheBoardEdges ()
		{
			Assert.Throws<Exception>(() => play ('X', 1, 0));
		}

		[Test]
		public void CannotPlayMarkLeftOfTheBoardEdges ()
		{
			Assert.Throws<Exception>(() => play ('X', 0, 1));
		}
		
		[Test]
		public void CannotPlayMarkRightOfTheBoardEdges ()
		{
			Assert.Throws<Exception>(() => play ('X', 4, 1));
		}

		[Test]
		public void CannotPlayMarkOverAlreadyPlayedMark ()
		{
			play ('X', 2, 2);

			Assert.Throws<Exception>(() => play ('O', 2, 2));
		}

		[Test]
		public void ODoesNotStartTheGame ()
		{
			Assert.Throws<Exception>(() => play('O', 1, 1));
		}

		[Test]
		public void XPlaysFirstAndTheMarkIsSaved ()
		{
            play('X', 1, 1);

			Assert.That (GetLastTurn().Mark, Is.EqualTo('X'));
		}

	    [Test]
		public void XPlaysFirstAndTheXCoordinateIsSaved()
		{
            play('X', 2, 3);

            Assert.That(GetLastTurn().X, Is.EqualTo(2));
		}

        [Test]
        public void XPlaysFirstAndTheYCoordinateIsSaved()
        {
            play('X', 3, 2);

            Assert.That(GetLastTurn().Y, Is.EqualTo(2));
        }

	    [Test]
		public void XPlaysThreeInHorizontalLineAndWins ()
		{
			play ('X', 1, 1);
			play ('O', 2, 2);
			play ('X', 2, 1);
			play ('O', 3, 3);
			play ('X', 3, 1);

			Assert.That (GetWinner(), Is.EqualTo('X'));
	    }

		[Test]
		public void OPlaysThreeInDiagonalAndWins ()
		{
			play ('X', 1, 2);
			play ('O', 1, 1);
			play ('X', 1, 3);
			play ('O', 2, 2);
			play ('X', 3, 1);
			play ('O', 3, 3);

			Assert.That (GetWinner(), Is.EqualTo('O'));
		}

	}
}

