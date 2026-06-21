using UI.Interaction.Events;

namespace UI.Quest
{
    public readonly struct QuestAddMessage
    {
        public readonly QuestListEnum title;
        public readonly string questTitle;

        public QuestAddMessage(QuestListEnum title, string questTitle)
        {
            this.questTitle = questTitle;
            this.title = title;
        }
    }    
    
    public readonly struct QuestRemoveMessage
    {
        public readonly QuestListEnum title;

        public QuestRemoveMessage(QuestListEnum title)
        {
            this.title = title;
        }
    }    
    
    public readonly struct QuestClearMessage
    {
        public readonly QuestListEnum title;

        public QuestClearMessage(QuestListEnum title)
        {
            this.title = title;
        }
    }    
    
    public readonly struct QuestAllRemoveMessage
    {
        public readonly QuestListEnum title;

        public QuestAllRemoveMessage(QuestListEnum title)
        {
            this.title = title;
        }
    }
}