namespace UI.Quest
{
    public readonly struct QuestCountRemoveMessage
    {
        public readonly QuestListEnum quest;
        public readonly int c;
        
        public QuestCountRemoveMessage(int c, QuestListEnum quest)
        {
            this.quest = quest;
            this.c = c;
        }
    }    
    
    public readonly struct QuestCountAddMessage
    {
        public readonly QuestListEnum quest;
        public readonly int c;
        
        public QuestCountAddMessage(int c, QuestListEnum quest)
        {
            this.quest = quest;
            this.c = c;
        }
    }    
    
    public readonly struct QuestCountClearMessage
    {
        public readonly QuestListEnum quest;
        public readonly int c;
        
        public QuestCountClearMessage(int c, QuestListEnum quest)
        {
            this.quest = quest;
            this.c = c;
        }
    }
}