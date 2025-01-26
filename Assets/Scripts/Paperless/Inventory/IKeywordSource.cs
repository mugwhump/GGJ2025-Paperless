namespace GGJ2025_Paperless.Assets.Scripts.Paperless.Inventory
{
    public interface IKeywordSource
    {
        Keyword GetKeyword();
        void SetKeyword(Keyword keyword);
        
        string GetDialogueTemplate();
    }
}