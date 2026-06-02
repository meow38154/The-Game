namespace UI.Interaction.Interface
{
    public interface IInteractionEvent
    {
        public IInteractionTrigger InteractionTrigger { get; set; }
        public void Start();
    }
}