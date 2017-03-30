namespace Lloyds.LAF.Agent.Restricted.Domain.Search
{
    public class CredentialValueSearchOptions
    {
        public int? ApplicationId { get; set; }

        public string Value { get; set; }

        public string Urn { get; set; }

        public int? SourceApplicationId { get; set; }
    }
}
