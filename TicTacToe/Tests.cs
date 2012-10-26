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

		[Test]
		[ExpectedException(typeof(Exception))]
		public void PlayerCannotPlayTwiceInARow ()
		{
			char lastTurn = 'X';
			char turn = 'X';

			if (lastTurn == turn) 
			{
				throw new Exception();
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
			int boardLeftmostCoordinate = 1;
			
			if (moveX <  boardLeftMostCoordinate)
				throw new Exception();
		}
		
		[Test]
		[ExpectedException(typeof(Exception))]
		public void CannotPlayMarkRightOfTheBoardEdges ()
		{
			int moveX = 4;
		}
	}
}

