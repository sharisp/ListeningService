using EasyNetQ;
using Listening.Domain.Entities;
using Listening.Domain.Enums;
using Listening.Domain.Events;
using Listening.Infrastructure.Repository;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Infrastructure.EventHandlers
{

    public class EpisodeAddEventHandler : INotificationHandler<AddEpisodeEvent>
    {
        private readonly IBus _bus;
        private readonly CommonQuery _commonQuery;

        public EpisodeAddEventHandler(IBus bus, CommonQuery commonQuery)
        {
            _bus = bus;
            _commonQuery = commonQuery;
        }

        public async Task Handle(AddEpisodeEvent notification, CancellationToken cancellationToken)
        {
            if (notification.episode == null) return;

            if (notification.episode.AISubtitleStatus == AISubtitleStatusEnum.Waiting)
            {
                // await _bus.PubSub.PublishAsync<Episode>(notification.episode, cancellationToken);
                await _commonQuery.MqPublish(notification.episode.Id, cancellationToken);
            }

        }
    }
}
