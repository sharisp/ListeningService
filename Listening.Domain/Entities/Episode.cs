using Domain.SharedKernel.HelperFunctions;
using Listening.Domain.Enums;
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
        public AISubtitleStatusEnum AISubtitleStatus { get; private set; } = AISubtitleStatusEnum.Waiting;

        private Episode() { }

        public void ChangeAudioUrl(Uri audioUrl)
        {
            this.AudioUrl = audioUrl;
            if (audioUrl != this.AudioUrl)
            {
                AddDomainEvent(new Events.UpdateAudioUrlEvent(this));
            }
        }
        public void ChangeAlbumId(long albumId)
        {
            this.AlbumId = albumId;
        }
        public void ChangeAISubtitleStatus(AISubtitleStatusEnum status)
        {
            this.AISubtitleStatus = status;
        }
        public void ChangeDurationInSeconds(long durationInSeconds)
        {
            this.DurationInSeconds = durationInSeconds;
        }
        public void ChangeSubtitleContent(string subtitleType, string subtitleContent, long durationInSeconds)
        {
            this.SubtitleContent = ParseSubtitleStr(subtitleType, subtitleContent, durationInSeconds);
        }
        public Episode(long alumId, string title, string subtitleType, string subtitleContent, Uri audioUrl, long durationInSeconds, int sequenceNumber, Uri? coverImgUrl) : base(title, sequenceNumber, coverImgUrl, false)
        {

            this.AlbumId = alumId;

            if (subtitleType=="AI_Generate")
            {

                this.AISubtitleStatus = AISubtitleStatusEnum.Waiting;
                this.SubtitleContent = "";
            }
            else
            {
                this.AISubtitleStatus = AISubtitleStatusEnum.Manual;
                this.SubtitleContent = ParseSubtitleStr(subtitleType, subtitleContent, durationInSeconds);
            }

            this.AudioUrl = audioUrl;
            this.DurationInSeconds = durationInSeconds;

            AddDomainEvent(new Events.AddEpisodeEvent(this));

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
            if (subtitleType == "json")
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
