using Domain.SharedKernel.BaseEntity;

namespace Listening.Domain.Entities
{
    public class ListeningBaseEntity : BaseAuditableEntity
    {
        public int SequenceNumber { get;  set; }
        public string Title { get; private set; }
        public Uri? CoverImgUrl { get; private set; }

        public bool IsShow { get; private set; }

        public void Hide()
        {
            this.IsShow = false;
        }
        public void Show()
        {
            this.IsShow = true;
        }

        public void ChangeSequenceNumber(int sequenceNumber)
        {
            this.SequenceNumber = sequenceNumber;
        }
        public void ChangeTitle(string title)
        {
            this.Title = title;
        }
        public void ChangeCoverImgUrl(Uri coverImgUrl)
        {
            this.CoverImgUrl = coverImgUrl;
        }
        protected ListeningBaseEntity(){}
        public ListeningBaseEntity(string title, int sequenceNumber, Uri? coverImgUrl = null, bool isShow = true)
        {
            this.Title = title;
            this.SequenceNumber = sequenceNumber;
            this.CoverImgUrl = coverImgUrl;
            this.IsShow = isShow; // Default to true when created
        }

    }
}
