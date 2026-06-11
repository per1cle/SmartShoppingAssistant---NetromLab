using Microsoft.Agents.AI;

namespace SmartShoppingAssistant.BussinesLogic.Agents
{
    public interface ISuggestionComposer
    {
        ChatClientAgent Build(string cartJson, string categoriesJson); //, string promotionAnalysisJson);
    }
}
