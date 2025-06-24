using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Domain.Entities
{
    public class Category:ListeningBaseEntity
    {
        public long KindId { get; private set; }

        public Category(string title, int sequenceNumber, long kindId, Uri? coverImgUrl) : base(title, sequenceNumber, coverImgUrl, true)
        {
            this.KindId = kindId;
        }

        public void ChangeKindId(long kindId)
        {
            this.KindId = kindId;
        }
    }
}
