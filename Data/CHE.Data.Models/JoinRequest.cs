namespace CHE.Data.Models
{
    using Common.Models;

    public class JoinRequest : BaseDeletableModel<string>
    {
        public string Content { get; set; }

        public string ParentSenderId { get; set; }

        public Parent ParentSender { get; set; }

        public string TeacherReceiverId { get; set; }

        public Teacher TeacherReceiver { get; set; }

        public string CoopSenderId { get; set; }

        public Cooperative CoopSender { get; set; }

        public string CoopReceiverId { get; set; }

        public Cooperative CoopReceiver { get; set; }
    }
}