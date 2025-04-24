using System;
using Application.Activities.DTO;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities.Queries;

public class GetComments
{
    public class Querie : IRequest<Result<List<CommentDto>>>
    {
        public required string ActivityId { get; set; }
    }

    public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Querie, Result<List<CommentDto>>>
    {
        public async Task<Result<List<CommentDto>>> Handle(Querie request, CancellationToken cancellationToken)
        {
            var comments = await context.Comments
               .Where(x => x.ActivityId == request.ActivityId)
               .OrderByDescending(x => x.CreatedAt)
               .ProjectTo<CommentDto>(mapper.ConfigurationProvider)
               .ToListAsync(cancellationToken);

            return Result<List<CommentDto>>.Success(comments);

        }
    }
}
