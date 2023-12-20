namespace ComplaintManagementSystem.Models
{
    public class ComplaintModel
    {
        public int ComplaintId { get; set; }

        public DateTime ComplaintDate { get; set; }
        public int ComplaintUserId { get; set; }

        public string ComplaintTopic { get; set; }

        public string ComplaintAddress { get; set; }

        public string ComplaintDesc { get; set; }
    }
}
