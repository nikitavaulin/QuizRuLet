// using Microsoft.Extensions.ObjectPool;
// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using Moq;
// using System.Collections.Generic;
// using System.Threading.Tasks;

// [TestClass]
// public class CardSetAiCreationServiceTests
// {
//     private Mock<ICardSetCreationService> _mockCreationService;
//     private Mock<IAiApiProvider> _mockAiProvider;

//     [TestInitialize]
//     public void Setup()
//     {
//         _mockCreationService = new Mock<ICardSetCreationService>();
//         _mockAiProvider = new Mock<IAiApiProvider>();
//     }

//     [TestMethod]
//     public async Task Create_ValidData_ReturnsCards()
//     {
//         // Arrange
//         var expectedCards = new List<Card>
//         {
//             new Card { Id = 1, Text = "Card 1" },
//             new Card { Id = 2, Text = "Card 2" }
//         };

//         var aiResponse = new AiApiResponse { Result = "Valid AI Response", Error = null };
//         _mockAiProvider.Setup(ai => ai.GetResponseAsync(It.IsAny<string>())).ReturnsAsync(aiResponse);
//         _mockCreationService.Setup(cs => cs.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
//                             .Returns(expectedCards);

//         var service = new CardSetAiCreationService(_mockCreationService.Object, _mockAiProvider.Object);

//         // Act
//         var (cards, error) = await service.Create("test data", 5);

//         // Assert
//         Assert.IsNotNull(cards);
//         Assert.AreEqual(0, string.Compare(error, "", StringComparison.Ordinal));
//         Assert.AreEqual(expectedCards.Count, cards.Count);
//         Assert.AreEqual(expectedCards[0].Id, cards[0].Id);
//         Assert.AreEqual(expectedCards[1].Text, cards[1].Text);
//     }

//     [TestMethod]
//     public async Task Create_AiError_ReturnsError()
//     {
//         // Arrange
//         (object?, string) aiResponse = (null, "AI Error");
//         _mockAiProvider.Setup(ai => ai.SendRequestAndGetResult(It.IsAny<string>())).ReturnsAsync(aiResponse);

//         var service = new CardSetAiCreationService(_mockCreationService.Object, _mockAiProvider.Object);

//         // Act
//         var (cards, error) = await service.Create("test data", 5);

//         // Assert
//         Assert.IsNull(cards);
//         Assert.AreEqual("AI Error", error);
//     }
// }