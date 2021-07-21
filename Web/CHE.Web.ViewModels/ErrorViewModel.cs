namespace CHE.Web.ViewModels
{
    public class ErrorViewModel
    {
        public string RequestId { get; init; }

        public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);
    }
}