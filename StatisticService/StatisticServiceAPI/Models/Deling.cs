using StatisticServiceAPI.Models.Enums;

namespace StatisticServiceAPI.Models
{
    public class Deling
    {
        public Deling(Guid sharedWithUserId, ShareMethod shareMethod, DateTime expiresAt)
        {
            SharedWithUserId = sharedWithUserId;
            ShareMethod = shareMethod;
            ExpiresAt = expiresAt;
        }

        public Guid Id { get; init; } = Guid.NewGuid();
        public Guid SharedWithUserId { get; private set; }
        public ShareMethod ShareMethod { get; private set; }
        public string ShareToken { get; private set; } = Guid.NewGuid().ToString();
        public DateTime ExpiresAt { get; private set; }
        public DateTime SharedAt { get; init; } = DateTime.UtcNow;

        /// <summary>
        /// JBS: Generer et nyt share link som der kan deles i en statistik/rapport til andre
        /// </summary>
        public void GenerateShareLink()
        {
            ShareToken = Guid.NewGuid().ToString();
        }

        //JBS: Her kan man tilbagekalde adgang for en bruger
        public void RevokeAccess(Guid userId)
        {
            SharedWithUserId = Guid.Empty;
        }
    }
}