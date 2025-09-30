using Listening.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Domain.Events
{
    public record AddEpisodeEvent(Episode episode) : INotification;
    public record UpdateEpisodeEvent(Episode episode) : INotification;
    public record UpdateAudioUrlEvent(Episode episode) : INotification;
    public record DeleteEpisodeEvent(Episode episode) : INotification;
}
