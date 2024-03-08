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
    public enum Permission {
        None            = 0,
        All             = int.MaxValue & ~ReadOnly,
        ReadOnly        = 1 << 0,
        UploadDocument  = 1 << 1,
        UpdateDocument  = 1 << 2,
        ApproveDocument = 1 << 3,
        CloseDocument   = 1 << 4,
        SelectReviewer  = 1 << 5,
        Comment         = 1 << 6,
        Respond         = 1 << 7,
        ResolveComment  = 1 << 8,
        EditPermissions = 1 << 9,
        EditRoles       = 1 << 10,
    }
}
