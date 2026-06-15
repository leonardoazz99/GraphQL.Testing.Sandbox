using MasterData.Contracts;
using MediatR;

namespace MasterData.Application.Queries;

public sealed record GetPartGroupByCodeQuery(string Code) : IRequest<PartGroupDto?>;
