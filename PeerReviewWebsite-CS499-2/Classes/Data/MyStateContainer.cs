using System;
using PeerReviewWebsite.Classes.Data.Login;

// <summary>
// Save the state of the User Class
// <summary>
public class MyStateContainer
{
    public User Value { get; set; }
    public event Action OnStateChange;
    public void SetValue(User value)
    {
        this.Value = value;
        NotifyStateChanged();
    }
    private void NotifyStateChanged() => OnStateChange?.Invoke();
}