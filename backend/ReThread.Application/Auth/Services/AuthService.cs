using ReThread.Application.Auth.Requests;
using ReThread.Application.Auth.Responses;
using ReThread.Application.Auth.Security;
using ReThread.Application.Auth.Services;
using ReThread.Application.Interfaces;
using ReThread.Application.Auth.Requests;
using ReThread.Application.Auth.Responses;
using ReThread.Application.Auth.Security;
using ReThread.Application.Interfaces;
using ReThreaded.Domain.Entities;
using ReThreaded.Domain.Enums;
using ReThreaded.Domain.Exceptions;

namespace ReThreaded.Application.Auth.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
        _unitOfWork = unitOfWork;
    }

    // REGISTER
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
            throw new DomainException("User already exists");

        // 🔐 ROLE CONTROL — SERVER SIDE
        var allowedRole = request.RequestedRole switch
        {
            UserRole.Buyer => UserRole.Buyer,
            UserRole.Designer => UserRole.Designer,
            _ => throw new DomainException("Invalid role selection")
        };

        var passwordHash = _passwordHasher.Hash(request.Password);

        var user = User.Create(
            request.Email,
            passwordHash,
            request.FirstName,
            request.LastName,
            allowedRole
        );

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        var token = _jwtTokenService.GenerateToken(user);

        return new AuthResponse
        {
            Token = token
        };
    }


    // LOGIN
    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        // 👇 THIS IS “USER LOADED”
        var user = await _userRepository.GetByEmailAsync(request.Email)
            ?? throw new DomainException("Invalid credentials");

        if (!user.IsActive)
            throw new DomainException("User account is deactivated");

        var isPasswordValid = _passwordHasher.Verify(
            request.Password,
            user.PasswordHash
        );

        if (!isPasswordValid)
            throw new DomainException("Invalid credentials");

        var token = _jwtTokenService.GenerateToken(user);

        return new AuthResponse
        {
            Token = token
        };
    }
}
