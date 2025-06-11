using QuizRuLet.Application.Services;
using QuizRuLet.Core.Abstractions;
using QuizRuLet.Core.Models;

namespace QuizRuLet.Tests;

[TestClass]
public sealed class BuisnessLogicTests
{
    [TestMethod]
    public void GetCardsSet_CardSetCreation_ReturnListCards()
    {
        // arrange
        var inputData = "termin1,opred1;termin2,opred2;";
        var pairSep = ",";
        var lineSep = ";";

        var expectFront1 = "termin1";
        var expectFront2 = "termin2";
        var expectBack1 = "opred1";
        var expectBack2 = "opred2";

        // act

        var act = new CardSetCreationService();

        var result = act.Create(inputData, pairSep, lineSep);

        // assert
        Assert.AreEqual(expectFront1, result[0].FrontSide);
        Assert.AreEqual(expectFront2, result[1].FrontSide);
        Assert.AreEqual(expectBack1, result[0].BackSide);
        Assert.AreEqual(expectBack2, result[1].BackSide);
    }
}
