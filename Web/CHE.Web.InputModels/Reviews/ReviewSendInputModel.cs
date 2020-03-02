namespace CHE.Web.InputModels.Reviews
{
    public class ReviewSendInputModel
    {
        public string Comment { get; set; }

        public int Rating { get; set; }

        public string ReceiverId { get; set; }
    }
}