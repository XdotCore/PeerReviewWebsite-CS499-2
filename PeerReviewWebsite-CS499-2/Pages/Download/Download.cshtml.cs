using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.StaticFiles;
using PeerReviewWebsite.Classes.Data;
using PeerReviewWebsite.Classes.Data.Review;

namespace PeerReviewWebsite.Pages.Download {

    public class DownloadModel : PageModel {
        MyStateContainer State { get; set; }

        // Taking the suggestion of making this a primary constructor breaks this
        // Probably due to how it interacts with c# reflection
#pragma warning disable IDE0290 // Use primary constructor
        public DownloadModel(MyStateContainer state) {
            State = state;
        }
#pragma warning restore IDE0290 // Use primary constructor

        public IActionResult OnGet() {
            Document doc = State.CurrentDoc;
            byte[] data = doc.Content;

            FileExtensionContentTypeProvider typeProvider = new();
            typeProvider.TryGetContentType(doc.FileName, out string contentType);

            return File(data, contentType, doc.FileName);
        }
    }
}
