using System;
using PeerReviewWebsite.Classes.Data.Account;

// <summary>
// Save the state of the User Class
// <summary>
public class MyStateContainer
{
    public User User { get; set; }
    public event Action OnStateChange;
    public void UpdateUser(User value)
    {
        this.User = value;
        NotifyStateChanged();
    }
    private void NotifyStateChanged() => OnStateChange?.Invoke();
}