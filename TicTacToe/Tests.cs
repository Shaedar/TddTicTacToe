using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace TicTacToe
{
	[TestFixture]
	public class Tests
	{
		const int boardHeight = 3;
		const int boardWidth = 3;
		IEnumerable<int> allowedHeights = Enumerable.Range(1, boardHeight);
		IEnumerable<int> allowedWidths = Enumerable.Range (1, boardWidth);
		IList<char> turns;

		[SetUp]
		public void Setup ()
		{
			turns = new List<char>();
		}

		void play (char turn, int x, int y)
		{
			ThrowIfFirstTurnIsO (turn);
			ThrowIfPreviousTurnWasBySameThePlayer(turn);

			turns.Add(turn);
		}

		void ThrowIfPreviousTurnWasBySameThePlayer (char turn)
		{
			if (turn == turns.LastOrDefault ()) 
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

		[Test]
		[ExpectedException(typeof(Exception))]
		public void PlayerCannotPlayTwiceInARow ()
		{
			play ('X', 1, 1);
			play ('X', 1, 2);
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
		public void FirstPlayerWithThreeHorizontalWins ()
		{
			play ('X', 1, 1);
			play ('O', 1, 2);
			play ('X', 2, 1);
			play ('O', 2, 2);
			play ('X', 3, 1);

			Assert.That(XHasWon());
		}

		private bool XHasWon ()
		{
			return false;
		}
	}
}

