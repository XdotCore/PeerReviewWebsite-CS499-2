namespace PeerReviewWebsite.Classes.Data.Review {
    // TODO: add document subsection
    public class Comment {
        public int Id { get; set; }
        public CommentStatus Status { get; set; }
        public string Content { get; set; }
    }

    public enum CommentStatus {
        Uploaded,
        Approved,
        Resolved
    }
}
