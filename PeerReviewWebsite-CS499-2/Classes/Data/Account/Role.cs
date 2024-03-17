using System;

namespace PeerReviewWebsite.Classes.Data.Account {
    public class Role {
        public int Id { get; set; }
        public string Name { get; set; }
        public Permission Permissions { get; set; }

        /// <summary>
        /// The default constructor
        /// </summary>
        public Role() { }

        /// <summary>
        /// Initializes a new instance with the values of the other given <see cref="Role"/>
        /// </summary>
        /// <param name="copy">The other instance with the values to initialize with</param>
        public Role(Role copy) {
            Id = copy.Id;
            Permissions = copy.Permissions;
        }
    }

    [Flags]
    public enum Permission : uint {
        None            = 0,
        All             = ~None & ~ReadOnly,
        ReadOnly        = 1 << 0,
        UploadDocument  = 1 << 1,
        UpdateDocument  = 1 << 2,
        ApproveDocument = 1 << 3,
        CloseDocument   = 1 << 4,
        ApproveComment  = 1 << 5,
        SelectReviewer  = 1 << 6,
        Comment         = 1 << 7,
        Respond         = 1 << 8,
        ResolveComment  = 1 << 9,
        EditPermissions = 1 << 10,
        EditRoles       = 1 << 11,
    }
}
