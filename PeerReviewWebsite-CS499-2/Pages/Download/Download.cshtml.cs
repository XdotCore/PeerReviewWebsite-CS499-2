using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.StaticFiles;
using PeerReviewWebsite.Classes.Data.Review;
using System.Threading.Tasks;

namespace PeerReviewWebsite.Pages.Download {

    public class DownloadModel : PageModel {
        ReviewService Service { get; set; }

        // Taking the suggestion of making this a primary constructor breaks this
        // Probably due to how it interacts with c# reflection
#pragma warning disable IDE0290 // Use primary constructor
        public DownloadModel(ReviewService service) {
            Service = service;
        }
#pragma warning restore IDE0290 // Use primary constructor

        public async Task<IActionResult> OnGetAsync(int docId) {
            Document doc = await Service.GetDocumentAsync(docId);
            byte[] data = doc.Content;

            FileExtensionContentTypeProvider typeProvider = new();
            typeProvider.TryGetContentType(doc.FileName, out string contentType);

            return File(data, contentType, doc.FileName);
        }
    }
}
