using System;
using NUnit.Framework;

namespace TicTacToe
{
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

		[Test]
		public void ADrawIsPlayedSoNoWinnerIsFound ()
		{
			Play ('X', 1, 1);
			Play ('O', 1, 2);
			Play ('X', 1, 3);
			Play ('O', 2, 1);
			Play ('X', 3, 2);			
			Play ('O', 2, 2);
			Play ('X', 3, 1);	
			Play ('O', 2, 3);
			Play ('X', 3, 3);

			Assert.That (GetWinner(), Is.EqualTo('-'));
		}

	}
}

       