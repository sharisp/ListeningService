using Domain.SharedKernel.HelperFunctions;
using Listening.Domain.Helper;
using Listening.Domain.Subtitles;
using Listening.Domain.ValueObjects;

namespace Listening.Domain.Entities
{
    public class Episode : ListeningBaseEntity
    {

        public long AlbumId { get; private set; }
        public string SubtitleContent { get; private set; }
        public Uri AudioUrl { get; private set; }
        public long DurationInSeconds { get; private set; }

        private Episode() { }


        public void ChangeAlbumId(long albumId)
        {
            this.AlbumId = albumId;
        }
        public Episode(long alumId, string title, string subtitleType, string subtitleContent, Uri audioUrl, long durationInSeconds, int sequenceNumber, Uri? coverImgUrl) : base(title, sequenceNumber, coverImgUrl, true)
        {

            this.AlbumId = alumId;
            this.SubtitleContent = ParseSubtitleStr(subtitleType, subtitleContent, durationInSeconds);
            this.AudioUrl = audioUrl;
            this.DurationInSeconds = durationInSeconds;

        }

        public IEnumerable<Sentence> ParseSubtitle(string subtitleType, string subtitleContent, long durationSeconds)
        {
            var parser = SubtitleParserFactory.GetParser(subtitleType);
            if (parser == null)
            {
                throw new Exception($"{subtitleType} not supported");
            }
            return parser.Parse(subtitleContent, durationSeconds);
        }
        public string ParseSubtitleStr(string subtitleType, string subtitleContent, long durationSeconds)
        {
            //correct subtitleContent,just return it
            if (subtitleType=="json")
            {
                return subtitleContent;
            }
            return ParseSubtitle(subtitleType, subtitleContent, durationSeconds).ToJsonString();
        }

        public void ChangeSubtitle(string subtitleType, string subtitleContent, long durationSeconds)
        {
            this.SubtitleContent = ParseSubtitleStr(subtitleType, subtitleContent, durationSeconds);
        }
        public string GetEncodedSubtitle()
        {            
            return Base64Helper.Encode(this.SubtitleContent);
        }

    }
}
