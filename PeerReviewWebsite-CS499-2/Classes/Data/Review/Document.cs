using System.Collections.Generic;

namespace PeerReviewWebsite.Classes.Data.Review {
    // TODO: finish this stub class
    public class Document {
        public int Id { get; set; }
        public DocumentStatus Status { get; set; } = DocumentStatus.Uploaded;
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public int Author { get; set; }
        public HashSet<int> Comments { get; set; } = [];
        public string Title { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// The default constructor
        /// </summary>
        public Document() { }

        /// Initializes a new instance with the values of the other given <see cref="Document"/>
        /// </summary>
        /// <param name="copy">The other instance with the values to initialize with</param>
        public Document(Document copy) {
            Id = copy.Id;
            Status = copy.Status;
            FileName = copy.FileName;
            Author = copy.Author;
            Comments.UnionWith(copy.Comments);
            Title = copy.Title;
            Description = copy.Description;
        }
    }

    public enum DocumentStatus {
        Uploaded,
        Approved,
        Closed
    }
}
