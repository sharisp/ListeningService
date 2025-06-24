using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Domain.ValueObjects
{
    public record Sentence
    {
        public TimeSpan StartTime { get; set; }
        public string Content { get; set; }
        public TimeSpan EndTime { get; set; }

        public Sentence(TimeSpan startTime, TimeSpan endTime, string content)
        {
            StartTime = startTime;
            EndTime = endTime;
            Content = content;
        }
    }
}
