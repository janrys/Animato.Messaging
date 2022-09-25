namespace Animato.Messaging.Application.Features.Queues;
using Animato.Messaging.Application.Features.Queues.Contracts;
using Animato.Messaging.Domain.Entities;
using AutoMapper;

public class QueueProfile : Profile
{
    public QueueProfile() => CreateMap<CreateQueueModel, Queue>();
}
