using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using PeerReviewWebsite.Classes.Data.Review;
using Document = PeerReviewWebsite.Classes.Data.Review.Document;

namespace PeerReviewWebsite.Classes.Data.Account {
    /// <summary>
    /// A helper class that allows for retrieval and manipulation of <see cref="User"/> accounts
    /// </summary>
    /// <param name="context">The database context for the website</param>
    public class AccountService(WebsiteDbContext context) {
        /// <summary>
        /// Gets the <see cref="User"/> with the given username
        /// </summary>
        /// <param name="id">The id if the <see cref="User"/></param>
        /// <returns>The <see cref="User"/> with the given id, <see langword="null"/> if not found</returns>
        public async Task<User> GetUserAsync(int id) =>
            await context.Users.Where(user => user.Id == id).AsNoTracking().FirstOrDefaultAsync();

        /// <summary>
        /// Gets the <see cref="User"/> with the given username
        /// </summary>
        /// <param name="email">The username of the <see cref="User"/></param>
        /// <returns>The <see cref="User"/> with the given username, <see langword="null"/> if not found</returns>
        public async Task<User> GetUserAsync(string email) =>
            await context.Users.Where(user => user.Email == email).AsNoTracking().FirstOrDefaultAsync();

        /// <summary>
        /// Adds the <see cref="User"/> account
        /// </summary>
        /// <param name="user">The object representation</param>
        /// <returns>The added <see cref="User"/></returns>
        public Task<User> CreateUserAsync(User user)
        {
            TaskCompletionSource<User> result = new();
            context.Users.Add(user);
            context.SaveChanges();
            User addedUser = new(user);
            result.SetResult(addedUser);
            return result.Task;
        }

        /// <summary>
        /// Gets the <see cref="Role"/> with the given id
        /// </summary>
        /// <param name="id">The id of the <see cref="Role"/></param>
        /// <returns>The <see cref="Role"/> with the given id, <see langword="null"/> if not found</returns>
        public async Task<Role> GetRoleAsync(int id) =>
            await context.Roles.Where(role => role.Id == id).AsNoTracking().FirstOrDefaultAsync();

        /// <summary>
        /// Gets the <see cref="Role"/> with the given name
        /// </summary>
        /// <param name="name">The namde of the <see cref="Role"/></param>
        /// <returns>The <see cref="Role"/> with the given name, <see langword="null"/> if not found</returns>
        public async Task<Role> GetRoleAsync(string name) =>
            await context.Roles.Where(role => role.Name == name).AsNoTracking().FirstOrDefaultAsync();

        /// <summary>
        /// Adds the <see cref="Role"/>
        /// </summary>
        /// <param name="role">The object representation</param>
        /// <returns>The added <see cref="Role"/></returns>
        public Task<Role> CreateRoleAsync(Role role) {
            TaskCompletionSource<Role> result = new();
            context.Roles.Add(role);
            context.SaveChanges();
            Role addedRole = new(role);
            result.SetResult(addedRole);
            return result.Task;
        }

        /// <summary>
        /// Gets the combined <see cref="Permission"/>s that the <see cref="User"/> has
        /// </summary>
        /// <param name="user">The <see cref="User"/> to get the <see cref="Permission"/>s from</param>
        /// <returns>The combined <see cref="Permission"/>s of the <see cref="User"/></returns>
        public async Task<Permission> GetUserPermissions(User user) {
            Permission result = Permission.None;

            foreach (int roleId in user.Roles) {
                Role role = await GetRoleAsync(roleId);
                result |= role.Permissions;
            }

            return result;
        }

        /// <summary>
        /// Adds the given <see cref="Role"/> to the given <see cref="User"/>
        /// </summary>
        /// <param name="user">The <see cref="User"/> to add the <see cref="Role"/> to</param>
        /// <param name="role">The <see cref="Role"/> to add to the <see cref="User"/></param>
        /// <returns>True if the role was added or already added, false otherwise</returns>
        public async Task<bool> AddRoleToUserAsync(User user, Role role) {
            User dbUser = await context.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();
            if (dbUser is null)
                return false;

            dbUser.Roles.Add(role.Id);
            context.Users.Update(dbUser);
            context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Gets if the <see cref="User"/> has all of the given <see cref="Permission"/>s
        /// </summary>
        /// <param name="user">The <see cref="User"/> to check against</param>
        /// <param name="permissions">The <see cref="Permission"/>s to check against</param>
        /// <returns><see langword="true"/> if the <see cref="User"/> has every given <see cref="Permission"/>, <see langword="false"/> otherwise</returns>
        public async Task<bool> UserHasPermissions(User user, Permission permissions) {
            Permission userPerms = await GetUserPermissions(user);

            return (userPerms & permissions) == permissions;
        }

        /// <summary>
        /// Gets if the <see cref="User"/> has any of the given <see cref="Permission"/>s
        /// </summary>
        /// <param name="user">The <see cref="User"/> to check against</param>
        /// <param name="permissions">The <see cref="Permission"/>s to check against</param>
        /// <returns><see langword="true"/> if the <see cref="User"/> has any given <see cref="Permission"/>, <see langword="false"/> otherwise</returns>
        public async Task<bool> UserHasAnyPermissions(User user, Permission permissions) {
            Permission userPerms = await GetUserPermissions(user);

            return (userPerms & permissions) != Permission.None;
        }

        /// <summary>
        /// Gets if the <see cref="User"/> has any <see cref="Permission"/> that would give them access to the moderator tab
        /// </summary>
        /// <param name="user">The given <see cref="User"/> to check against</param>
        /// <returns><see langword="true"/> if the <see cref="User"/> has any of the right <see cref="Permission"/>s, <see langword="false"/> otherwise</returns>
        public async Task<bool> UserIsModerator(User user) {
            return await UserHasAnyPermissions(user, Permission.ApproveDocument |
                                                     Permission.ApproveComment | 
                                                     Permission.SelectReviewer | 
                                                     Permission.EditPermissions | 
                                                     Permission.EditRoles);
        }

        /// <summary>
        /// Adds the given <see cref="Document"/> to the given <see cref="User"/>
        /// </summary>
        /// <param name="user">The <see cref="User"/> to add the <see cref="Document"/> to</param>
        /// <param name="doc">The <see cref="Document"/> to add to the <see cref="User"/></param>
        /// <returns><see langword="true"/> if the document was successfully added to the user, <see langword="false"/> otherwise</returns>
        public async Task<bool> AddDocumentToUserAsync(User user, Document doc) {
            User dbUser = await context.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();
            if (dbUser is null)
                return false;

            Document dbDoc = await context.Documents.Where(d => d.Id == doc.Id).FirstOrDefaultAsync();
            if (dbDoc is null)
                return false;

            dbUser.OwnedDocuments.Add(dbDoc.Id);
            context.Users.Update(dbUser);
            dbDoc.Author = dbUser.Id;
            context.Documents.Update(dbDoc);
            context.SaveChanges();
            return true;
        }
    }
}
