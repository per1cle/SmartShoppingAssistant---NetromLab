using Microsoft.Agents.AI;

namespace SmartShoppingAssistant.BussinesLogic.Agents;

public interface IPromotionCheckerAgent
{
    ChatClientAgent Build(string cartJson);
}