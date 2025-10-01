using EasyNetQ;
using Listening.Domain.Entities;
using Listening.Domain.Enums;
using Listening.Domain.Events;
using Listening.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Listening.Infrastructure.Repository
{
    public class CommonQuery(AppDbContext dbContext, IBus bus)
    {
        public IQueryable<T> Query<T>() where T : class
        {
            return dbContext.Set<T>().AsQueryable();
        }
        public IQueryable<T> ApplyQueryWithPermission<T>(bool isAnd = false) where T : class
        {
            return dbContext.Set<T>().ApplyRowPermission(isAnd);
        }
        public async Task MqPublish<T>(T model, CancellationToken cancellationToken = default)
        {
            if (model == null) return;


            var exchange = await bus.Advanced.ExchangeDeclareAsync("episode_exchange", ExchangeType.Topic);

            var message = new Message<T>(model);

            await bus.Advanced.PublishAsync(
                exchange,
                "episode.subtitle",
                mandatory: true,
                publisherConfirms: true,
                message,
                cancellationToken
            );


        }
    }
}

