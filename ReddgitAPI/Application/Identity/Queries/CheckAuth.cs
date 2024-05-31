using MediatR;

namespace ReddgitAPI.Application.Identity.Queries
{
    public class CheckAuth : IRequestHandler<CheckAuth.Query, string>
    {
        public class Query : IRequest<string>
        { }

        public CheckAuth()
        { }

        public async Task<string> Handle(Query request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            var response = "Authorized";

            return response;
        }

    }
}
