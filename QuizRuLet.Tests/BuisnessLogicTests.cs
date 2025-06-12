

using System.Threading.Tasks;

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
    
    
    [TestMethod]
    public async Task SendRequest_AiApiProvider_ReturnString()
    {
        //arrange
        var prompt = "Привет! Если ты меня понимаешь, скажи привет";

        //act
        var response = (await AiApiProvider.SendRequestAndGetResult(prompt));
        var result = response.Result;
        var error = response.Error;

        //assert
        Assert.IsTrue(string.IsNullOrEmpty(error));
        Assert.IsTrue(!string.IsNullOrEmpty(result));
    }
}
