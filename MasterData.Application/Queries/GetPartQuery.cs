using MasterData.Contracts;
using MediatR;

namespace MasterData.Application.Queries;

public sealed record GetPartQuery(string PartNumber) : IRequest<PartDto?>;
