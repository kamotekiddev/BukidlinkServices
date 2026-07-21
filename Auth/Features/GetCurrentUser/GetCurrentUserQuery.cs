using MediatR;

namespace Auth.Features.GetCurrentUser;

public record GetCurrentUserQuery() : IRequest<GetCurrentUserResult>;