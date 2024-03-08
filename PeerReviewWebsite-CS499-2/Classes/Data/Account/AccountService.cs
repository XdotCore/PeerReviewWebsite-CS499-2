using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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
        /// Gets the <see cref="Role"/> with the given id
        /// </summary>
        /// <param name="id">The id of the <see cref="Role"/></param>
        /// <returns>The <see cref="Role"/> with the given id, <see langword="null"/> if not found</returns>
        public async Task<Role> GetRole(int id) =>
            await context.Roles.Where(role => role.Id == id).AsNoTracking().FirstOrDefaultAsync();

        /// <summary>
        /// Adds the <see cref="Role"/>
        /// </summary>
        /// <param name="role">The object representation</param>
        public Task<Role> CreateRoleAsync(Role role) {
            TaskCompletionSource<Role> result = new();
            context.Roles.Add(role);
            context.SaveChanges();
            Role addedRole = new(role);
            result.SetResult(addedRole);
            return result.Task;
        }
    }
}
