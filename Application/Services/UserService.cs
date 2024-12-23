
using ApplicationProduct.Application.Interfaces;
using ApplicationProduct.Domain.Entities;

namespace ApplicationProduct.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            user.Id = Guid.NewGuid();
            user.Enable = true;
            user.Password = _passwordHasher.HashPassword(user.Password);

            await _userRepository.AddAsync(user);
            return user;
        }
        public async Task<User> VerifyUserCredentialsAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);

            if (user == null || !user.Enable)
            {
                return null;
            }

            // Verificar a senha utilizando o BCrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
            if (!isPasswordValid)
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
    }
}