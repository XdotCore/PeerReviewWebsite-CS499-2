using PeerReviewWebsite.Classes.Data.Account;
using PeerReviewWebsite.Classes.Data.Review;

namespace PeerReviewWebsite.Classes.Data {

    // <summary>
    // Current state of the program
    // <summary>
    public class MyStateContainer {
        public User User { get; set; }
        public Document CurrentDoc { get; set; }
    }
}