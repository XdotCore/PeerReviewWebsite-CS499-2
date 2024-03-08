namespace PeerReviewWebsite.Classes.Data.Review {
    // TODO: finish this stub class
    public class Document {
        public int Id { get; set; }
        public DocumentStatus Status { get; set; }
        public string FileName { get; set; }
    }

    public enum DocumentStatus {
        Uploaded,
        Approved,
        Closed
    }
}
