namespace PeerReviewWebsite.Classes.Data.Review {
    // TODO: add document subsection
    public class Comment {
        public int Id { get; set; }
        public CommentStatus Status { get; set; }
        public string Content { get; set; }

        /// <summary>
        /// The default constructor
        /// </summary>
        public Comment() { }

        /// Initializes a new instance with the values of the other given <see cref="Comment"/>
        /// </summary>
        /// <param name="copy">The other instance with the values to initialize with</param>
        public Comment(Comment copy) {
            Id = copy.Id;
            Status = copy.Status;
            Content = copy.Content;
        }
    }

    public enum CommentStatus {
        Uploaded,
        Approved,
        Resolved
    }
}
