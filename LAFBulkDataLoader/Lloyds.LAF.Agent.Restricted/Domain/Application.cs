namespace Lloyds.LAF.Agent.Restricted.Domain
{
    public sealed class Application
    {
        public int ApplicationId { get; set; }
	    public int TermsAndConditionsId { get; set; }
	    public int ApplicationGroupingId { get; set; }
	    public int RememberMeLevelId { get; set; }
	    public int SlidingSessionRefreshIntervalId { get; set; }
	    public string Name { get; set; }
	    public string Description { get; set; }
	    public string ApplicationUrl { get; set; }
	    public string TellMeMoreUrl { get; set; }
	    public bool IsLive { get; set; }
	    public string UserAccessRequestMailbox { get; set; }
	    public bool AllowAuthenticatedUsersAccessRegardlessOfCredentials { get; set; }

    }
}
