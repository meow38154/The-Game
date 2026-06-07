using System.Collections.Generic;

namespace UI.Quest
{
    public interface IQuestCountManager
    {
        List<QuestListEnum> QuestAddEnumLs { get; }
        List<QuestListEnum> QuestRemoveEnumLs { get; }
        List<QuestListEnum> QuestClearEnumLs { get; }
    }
}