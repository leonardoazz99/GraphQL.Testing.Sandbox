using MasterData.Contracts;
using MediatR;

namespace MasterData.Application.Queries;

public sealed record GetAllPartGroupsQuery : IRequest<IEnumerable<PartGroupDto>>;
