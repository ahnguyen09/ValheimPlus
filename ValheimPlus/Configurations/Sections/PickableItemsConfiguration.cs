namespace ValheimPlus.Configurations.Sections
{
    public class PickableItemsConfiguration : ServerSyncConfig<PickableItemsConfiguration>
    {
        public int dandelionAmount { get; set; } = 1;
        public int mushroomAmount { get; set; } = 1;
        public int mushroomBlueAmount { get; set; } = 1;
        public int mushroomYelloAmount { get; set; } = 1;
        public int thistleAmount { get; set; } = 1; 
    }
}