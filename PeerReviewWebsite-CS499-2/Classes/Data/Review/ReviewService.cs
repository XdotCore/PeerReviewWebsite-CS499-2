using Microsoft.EntityFrameworkCore;
using PeerReviewWebsite.Classes.Data.Account;
using System.Linq;
using System.Threading.Tasks;

namespace PeerReviewWebsite.Classes.Data.Review {
    public class ReviewService(WebsiteDbContext context) {
        /// <summary>
        /// Gets the <see cref="Document"/> with the given id
        /// </summary>
        /// <param name="id">The id of the <see cref="Document"/></param>
        /// <returns><see langword="true"/> if the <see cref="Document"/> with the id exists, <see langword="false"/> otherwise</returns>
        public async Task<Document> GetDocumentAsync(int id) =>
            await context.Documents.Where(doc => doc.Id == id).AsNoTracking().FirstOrDefaultAsync();

        /// <summary>
        /// Adds the <see cref="Document"/>
        /// </summary>
        /// <param name="doc">The object representation</param>
        /// <returns>The added <see cref="Document"/></returns>
        public Task<Document> CreateDocumentAsync(Document doc) {
            TaskCompletionSource<Document> result = new();
            context.Documents.Add(doc);
            context.SaveChanges();
            Document addedDoc = new(doc);
            result.SetResult(addedDoc);
            return result.Task;
        }

        /// <summary>
        /// Gets the <see cref="Comment"/> with the given id
        /// </summary>
        /// <param name="id">The id of the <see cref="Comment"/></param>
        /// <returns><see langword="true"/> if the <see cref="Comment"/> with the id exists, <see langword="false"/> otherwise</returns>
        public async Task<Comment> GetCommentAsync(int id) =>
            await context.Comments.Where(com => com.Id == id).AsNoTracking().FirstOrDefaultAsync();

        /// <summary>
        /// Adds the <see cref="Comment"/>
        /// </summary>
        /// <param name="com">The object representation</param>
        /// <returns>The added <see cref="Comment"/></returns>
        public Task<Comment> CreateCommentAsync(Comment com) {
            TaskCompletionSource<Comment> result = new();
            context.Comments.Add(com);
            context.SaveChanges();
            Comment addedComment = new(com);
            result.SetResult(addedComment);
            return result.Task;
        }
    }
}
