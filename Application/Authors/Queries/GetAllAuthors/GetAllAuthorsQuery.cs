﻿using Application.Common;
using Application.DTOs;
using MediatR;

namespace Application.Authors.Queries.GetAllAuthors
{
    public class GetAllAuthorsQuery : IRequest<OperationResult<IEnumerable<AuthorDto>>>
    {
    }
}
