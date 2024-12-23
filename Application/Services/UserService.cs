
using ApplicationProduct.Application.DTOs;
using ApplicationProduct.Application.Interfaces;
using ApplicationProduct.Domain.Entities;

namespace ApplicationProduct.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public UserService(IUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            user.Id = Guid.NewGuid();
            user.Enable = true;
            user.Password = _authService.ComputeSha256Hash(user.Password);

            await _userRepository.AddAsync(user);
            return user;
        }
        public async Task<User> VerifyUserCredentialsAsync(string username, string password)
        {
            password = _authService.ComputeSha256Hash(password);
            var user = await _userRepository.GetByUsernameAndPassword(username, password);

            if (user == null || !user.Enable)
            {
                return null;
            }
            return user;
        }
        public async Task<User> UpdateUserAsync(User user)
        {
            var existingUser = await _userRepository.GetByIdAsync(user.Id) ?? throw new Exception("Usuário não encontrado.");
            existingUser.Name = user.Name;
            existingUser.Username = user.Username;
            existingUser.Password = user.Password;

            await _userRepository.UpdateAsync(existingUser);
            return existingUser;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return await _userRepository.GetByIdAsync(id)
                ?? throw new Exception("Usuário não encontrado.");
        }

        public async Task<IEnumerable<User>> GetUsersByNameAsync(string name)
        {
            return await _userRepository.FindByNameAsync(name);
        }

        public async Task<User> DisableUserAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new Exception("Usuário não encontrado.");

            user.Enable = false;
            await _userRepository.UpdateAsync(user);
            return user;
        }

        public async Task<User> UpdatePassWord(EditPasswordDto dto)
        {
            dto.password = _authService.ComputeSha256Hash(dto.password);
            var user = await _userRepository.GetByUsernameAndPassword(dto.username, dto.password);

            if (user == null || !user.Enable)
            {
                return null;
            }

            user.Password = _authService.ComputeSha256Hash(dto.newpassword);

            await _userRepository.UpdateAsync(user);
            return user;
        }
    }
}