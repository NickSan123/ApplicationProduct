using ApplicationProduct.Application.DTOs;
using ApplicationProduct.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationProduct.Application.Interfaces
{
    public interface ISessionService
    {
        public Task<Session?> LoginSessionAsync(LoginDto dto);
        public Task<Session> LogoutSessionAsync(Guid id);
        public Task<Session?> GetSessionByIdAsync(Guid id);
        public Task<Session?> GetSessionByToken(string token);
        public Task<IEnumerable<Session>> GetAllSessionByUserAsync(Guid userId);
    }
}