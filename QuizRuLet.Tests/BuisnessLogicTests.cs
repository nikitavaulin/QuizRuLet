

using System.Threading.Tasks;
using NuGet.ContentModel;

namespace QuizRuLet.Tests;

[TestClass]
public sealed class BuisnessLogicTests
{
    [TestMethod]
    public void GetCardsSet_CardSetCreation_ReturnListCards()
    {
        // arrange
        var inputData = "termin1\topred1\ntermin2\topred2\n\n";
        var pairSep = "\t";
        var lineSep = "\n";

        // act
        var act = new CardSetCreationService();

        var (Cards, Error) = act.Create(inputData, pairSep, lineSep);

        // assert
        Assert.IsTrue(string.IsNullOrEmpty(Error));
        Assert.AreEqual(2, Cards?.Count ?? 0);
    }
    
    
    // [TestMethod]
    // public async Task SendRequest_AiApiProvider_ReturnString()
    // {
    //     //arrange
    //     var prompt = "Привет! Если ты меня понимаешь, скажи привет";

    //     IAiApiProvider aiProvider = new AiApiProvider();

    //     //act
    //     var response = (await aiProvider.SendRequestAndGetResult(prompt));
    //     var result = response.Result;
    //     var error = response.Error;

    //     //assert
    //     Assert.IsTrue(string.IsNullOrEmpty(error));
    //     Assert.IsTrue(!string.IsNullOrEmpty(result));
    // }
    
    // [TestMethod]
    // public async Task CreateAiCardSet_CardSetAiCreationService_ReturnString()
    // {
    //     //arrange
    //     ICardSetAiCreationService service = new CardSetAiCreationService();
    //     var data = "Пётр I Алексе́евич (назван Вели́ким, 30 мая [9 июня] 1672 — 28 января [8 февраля] 1725) — последний царь всея Руси (с 1682 года) и первый Император Всероссийский (с 1721 года). Петра I считают одним из наиболее выдающихся государственных деятелей, определившим направление развития России в XVIII веке. Представитель династии Романовых, он был провозглашён царём в десятилетнем возрасте (1682) при регентше Софье Алексеевне, стал править самостоятельно с 1689 года. Формальным соправителем Петра был его брат Иван (до своей смерти в 1696 году). Биография Родился 30 мая (9 июня по новому стилю) 1672 года (точное место его рождения осталось неизвестным) в семье царя Алексея Михайловича, имевшего много детей. Пётр был четырнадцатым его ребёнком, но первым от второй жены — царицы Натальи Кирилловны Нарышкиной. 29 июня — в день святых апостолов Петра и Павла — царевич был крещён. Первое время после рождения Пётр был с матерью, затем его отдали на воспитание нянькам. В 1676 году умер его отец, и опекуном Петра стал его единокровный брат — крёстный отец и новый царь Фёдор Алексеевич. По неизвестной причине Пётр получил слабое образование, и до конца жизни писал с ошибками. Вероятно, это произошло от того, что патриарх московский Иоаким, боровшийся с иноземным влиянием, отстранил от царского двора Симеона Полоцкого, который обучал старших братьев Петра. Обучением царевича занимались менее образованные дьяки Никита Зотов и Афанасий Нестеров. Дьяки обучали молодого Петра грамоте с 1676 по 1680 год. Недостатки своего базового образования Пётр впоследствии устранить не смог.";

    //     //act
    //     var response = await service.Create(data, 5);
    //     var result = response.Cards;
    //     var error = response.Error;

    //     //assert
    //     Assert.IsTrue(string.IsNullOrEmpty(error));
    //     Assert.IsTrue(result is not null);
    // }
    
    
    
    
}
