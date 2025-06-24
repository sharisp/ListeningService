namespace Listening.Domain.Entities
{
    public class Album : ListeningBaseEntity
    {
        public long CategoryId { get; private set; }


        private Album()
        {

        }
        public Album(string title, long categoryId, int sequenceNumber, Uri? coverImgUrl) : base(title, sequenceNumber, coverImgUrl, true)
        {
            this.CategoryId = categoryId;
        }

        public void ChangeCategoryId(long categoryId)
        {
            this.CategoryId = categoryId;
        }
    }
}
