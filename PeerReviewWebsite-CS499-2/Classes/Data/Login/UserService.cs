﻿using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace PeerReviewWebsite.Classes.Data.Login {
    /// <summary>
    /// A helper class that allows for retrieval and manipulation of <see cref="User"/> accounts
    /// </summary>
    /// <param name="context">The database context for the website</param>
    public class UserService(WebsiteDbContext context) {
        /// <summary>
        /// Gets the <see cref="User"/> with the given username
        /// </summary>
        /// <param name="email">The username of the <see cref="User"/></param>
        /// <returns>The <see cref="User"/> with the given username, <see langword="null"/> if not found</returns>
        public async Task<User> GetUserAsync(string email) =>
            await context.Users.Where(d => d.Email == email).AsNoTracking().FirstOrDefaultAsync();

        /// <summary>
        /// Adds the <see cref="User"/> account
        /// </summary>
        /// <param name="user">The object representation</param>
        public Task<User> CreateUserAsync(User user)
        {
            var profile = new TaskCompletionSource<User>();
            context.Users.Add(user);
            context.SaveChanges();
            User addedUser = new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password
            };
            profile.SetResult(addedUser);
            return profile.Task;
        }
    }
}
