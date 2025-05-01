using System;
using QuizRuLet.Core.Models;

namespace QuizRuLet.Core.Abstractions;

public interface ICardsSetCreater
{
    public List<Card> Create();
}
