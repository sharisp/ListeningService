using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Domain.Entities
{
    public class Kind : ListeningBaseEntity
    {
       private Kind()
        {
        }

        public Kind(string title, int sequenceNumber, Uri? coverImgUrl) : base(title, sequenceNumber, coverImgUrl, true)
        {
        }
    }
}
