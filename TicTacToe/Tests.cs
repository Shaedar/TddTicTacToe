using System;
using NUnit.Framework;

namespace TicTacToe
{
	[TestFixture]
	public class Tests
	{
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
		public void PlayerCannotPlayOutsideBoard ()
		{
			int boardHeight = 3;
			int boardWidth = 3;

		}
	}
}

