﻿using Domain.Entities;

namespace Application.Interfaces
{
    public interface IJwtGenerator
    {
        string GenerateToken(User user);
    }
}
