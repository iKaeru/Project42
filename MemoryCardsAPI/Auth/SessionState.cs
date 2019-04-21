namespace MemoryCardsAPI.Auth
{
        using System;

    public class SessionState
    {
            public SessionState(string sessionId, Guid userId)
            {
                if (sessionId == null)
                {
                    throw new ArgumentNullException(nameof(sessionId));
                }

                this.SessionId = sessionId;
                this.UserId = userId;
            }

            public string SessionId { get; }

            public Guid UserId { get; }
        }
}