namespace AdvantShop.App.Landing.Domain.Settings
{
    public class LpSettings
    {
        public int Id { get; set; }
        public int LandingId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public LpSettings()
        {
        }

        public LpSettings(int landingId, string name, string value)
        {
            LandingId = landingId;
            Name = name;
            Value = value;
        }
    }
}
