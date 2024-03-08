using System.Collections.Generic;

namespace PeerReviewWebsite.Classes.Data.Account {
    public class User {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        /// <summary>
        /// Ids for the roles of the user
        /// </summary>
        public HashSet<int> Roles { get; set; } = [];

        /// <summary>
        /// Ids for the documents the user owns
        /// </summary>
        public HashSet<int> OwnedDocuments { get; set; } = [];

        /// <summary>
        /// Ids for the comments the user owns
        /// </summary>
        public HashSet<int> OwnedComments { get; set; } = [];

        /// <summary>
        /// The default constructor
        /// </summary>
        public User() { }

        /// <summary>
        /// Initializes a new instance with the values of the other given <see cref="User"/>
        /// </summary>
        /// <param name="copy">The other instance with the values to initialize with</param>
        public User(User copy) {
            Id = copy.Id;
            FirstName = copy.FirstName;
            LastName = copy.LastName;
            Email = copy.Email;
            Password = copy.Password;
            Roles.UnionWith(copy.Roles);
            OwnedDocuments.UnionWith(copy.OwnedDocuments);
            OwnedComments.UnionWith(copy.OwnedComments);
        }
    }
}
