namespace UI.Quest
{
    public readonly struct QuestCountListMessage
    {
        public readonly IQuestCountManager quest;
        
        public QuestCountListMessage(IQuestCountManager quest)
        {
            this.quest = quest;
        }
    }   
}