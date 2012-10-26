using System;
using NUnit.Framework;

namespace TicTacToe
{
	[TestFixture]
	public class Tests
	{
		const int boardHeight = 3;
		const int boardWidth = 3;

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

		[Test]
		[ExpectedException(typeof(Exception))]
		public void CannotPlayMarkAboveTheBoardEdges ()
		{
			int moveX = 4;

			if (moveX > boardHeight)
				throw new Exception();
		}

		[Test]
		[ExpectedException(typeof(Exception))]
		public void CannotPlayMarkBelowTheBoardEdges ()
		{
			int moveX = 0;

		}

	}
}

