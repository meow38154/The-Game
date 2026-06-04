namespace UI.Quest
{
    public readonly struct QuestCountRemoveMessage
    {
        public readonly int c;
        
        public QuestCountRemoveMessage(int c)
        {
            this.c = c;
        }
    }    
    
    public readonly struct QuestCountAddMessage
    {
        public readonly int c;
        
        public QuestCountAddMessage(int c)
        {
            this.c = c;
        }
    }    
    
    public readonly struct QuestCountClearMessage
    {
        public readonly int c;
        
        public QuestCountClearMessage(int c)
        {
            this.c = c;
        }
    }
}