using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using PeerReviewWebsite.Classes.Data.Review;
using System;
using System.Threading.Tasks;

namespace PeerReviewWebsite.Pages.Download {

    public class DownloadModel : PageModel {
        ReviewService p;
        public DownloadModel(ReviewService a) {
            p = a;
        }

        public async Task<IActionResult> OnGetAsync(int docId) {
            Document doc = await p.GetDocumentAsync(docId);
            byte[] data = doc.Content;

            FileExtensionContentTypeProvider typeProvider = new();
            typeProvider.TryGetContentType(doc.FileName, out string contentType);

            return File(data, contentType, doc.FileName);
        }
    }
}
