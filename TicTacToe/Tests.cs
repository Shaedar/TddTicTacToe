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

		[SetUp]
		public void Setup ()
		{
			turns = new List<Turn>();
		}

		void play (char mark, int x, int y)
		{
			play(new Turn(mark, x, y));
		}

        void play(Turn turn)
        {
            ThrowIfFirstTurnIsO(turn.Mark);
            ThrowIfPreviousTurnWasBySameThePlayer(turn.Mark);
            
            turns.Add(turn);
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

		
		void ThrowIfHeightNotAllowed (int moveY)
		{
			if (!IsHeightAllowed (moveY)) {
				throw new Exception ();
			}
		}
		
		bool IsHeightAllowed (int moveY)
		{
			return allowedHeights.Contains(moveY);
		}
		
		void ThrowIfWidthNotAllowed (int moveX)
		{
			if (!IsWidthAllowed (moveX)) 
			{
				throw new Exception ();
			}
		}
		
		bool IsWidthAllowed (int moveX)
		{
			return allowedWidths.Contains (moveX);
		}

		[Test]
		[ExpectedException(typeof(Exception))]
		public void PlayerCannotPlayTwiceInARow ()
		{
			play ('X', 1, 1);
			play ('X', 1, 2);
		}

		[Test]
		[ExpectedException(typeof(Exception))]
		public void CannotPlayMarkAboveTheBoardEdges ()
		{
			int moveY = 4;

			ThrowIfHeightNotAllowed (moveY);
		}

		[Test]
		[ExpectedException(typeof(Exception))]
		public void CannotPlayMarkBelowTheBoardEdges ()
		{
			int moveY = 0;

			ThrowIfHeightNotAllowed(moveY);
		}

		[Test]
		[ExpectedException(typeof(Exception))]
		public void CannotPlayMarkLeftOfTheBoardEdges ()
		{
			int moveX = 0;

			ThrowIfWidthNotAllowed(moveX);
		}
		
		[Test]
		[ExpectedException(typeof(Exception))]
		public void CannotPlayMarkRightOfTheBoardEdges ()
		{
			int moveX = 4;

			ThrowIfWidthNotAllowed(moveX);
		}

		[Test]
		[ExpectedException(typeof(Exception))]
		public void CannotPlayMarkOverAlreadyPlayedMark ()
		{
			int previousMarkHeight = 2;
			int previousMarkWidth = 2;

			int nextMarkHeight = 2;
			int nextMarkWidth = 2;

			if (previousMarkHeight == nextMarkHeight && previousMarkWidth == nextMarkWidth)
				throw new Exception();
		}

		[Test]
		[ExpectedException(typeof(Exception))]
		public void ODoesNotStartTheGame ()
		{
			play('O', 1, 1);
		}

		[Test]
		public void XPlaysFirstAndTheMarkIsSaved ()
		{
		    Turn turn = new Turn('X', 1, 1);

            play(turn);

			Assert.That (GetLastTurn().Mark, Is.EqualTo(turn.Mark));
		}

	    private Turn GetLastTurn()
	    {
	        return turns.Last();
	    }

	    [Test]
		public void XPlaysFirstAndTheXCoordinateIsSaved()
		{
            Turn turn = new Turn('X', 2, 2);

			play(turn);

            Assert.That(GetLastTurn().X, Is.EqualTo(turn.X));
		}

        [Test]
        public void XPlaysFirstAndTheYCoordinateIsSaved()
        {
            Turn turn = new Turn('X', 2, 2);

            play(turn);

            Assert.That(GetLastTurn().Y, Is.EqualTo(turn.Y));
        }

	}
}

