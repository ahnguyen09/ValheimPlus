namespace ValheimPlus.Configurations.Sections
{
    public class GameClockConfiguration : ServerSyncConfig<GameClockConfiguration>
    {
        public bool useAMPM { get; set; } = false;

        public float textRedChannelRatio { get; set; } = 0.9725f;
        public float textGreenChannelRatio { get; set; } = 0.4118f;
        public float textBlueChannelRatio { get; set; } = 0;
        public float textTransparencyChannelRatio { get; set; } = 1f;
    }
}