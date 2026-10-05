using Domain.Contracts;
using Domain.Models;
using Domain.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServiceAbstraction;
using Shared.DTOs;
using Shared.DTOs.Auth;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Settings;
using Shared.SharedResources;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Service;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    IUnitOfWork unitOfWork,
    IOptions<AppSettings> appSettings,
    IMailingService emailService,
    IConfiguration configuration,
    IHostEnvironment env,
    IStringLocalizer<SharedResources> localizer)
    : IAuthService
{
    public async Task<Result<AuthResult>> LoginAsync(LoginRequest request)
    {
        var user = await userManager.Users
            .FirstOrDefaultAsync(
                u => u.Email == request.Email &&
                     u.EmailConfirmed);

        if (user == null ||
            !await userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new UnauthorizedException(
                localizer[SharedResourcesKeys.InvalidCredentials]);
        }

        var roles = await userManager.GetRolesAsync(user);

        var tokens = await GenerateTokensAsync(user);

        var authResult = new AuthResult
        {
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken,
            UserRoles = roles.ToList()
        };

        return Result<AuthResult>.Success(
            authResult,
            localizer[SharedResourcesKeys.LoginSuccessful]);
    }

    public async Task<Result<AuthResult>> ChangePasswordAsync(
        ChangePassRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.OldPassword) ||
            string.IsNullOrWhiteSpace(request.NewPassword))
        {
            throw new BadRequestException(
                localizer[SharedResourcesKeys.CredentialsAreRequired]);
        }

        var user = await userManager.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null)
        {
            throw new NotFoundException(
                localizer[SharedResourcesKeys.UserNotFound]);
        }

        var isOldPasswordCorrect =
            await userManager.CheckPasswordAsync(
                user,
                request.OldPassword);

        if (!isOldPasswordCorrect)
        {
            throw new BadRequestException(
                localizer[SharedResourcesKeys.OldPassIsIncorrect]);
        }

        var result = await userManager.ChangePasswordAsync(
            user,
            request.OldPassword,
            request.NewPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join(
                "; ",
                result.Errors.Select(e => e.Description));

            throw new BadRequestException(
                $"Failed to change password: {errors}");
        }

        var newTokens = await GenerateTokensAsync(user);

        var authResult = new AuthResult
        {
            AccessToken = newTokens.AccessToken,
            RefreshToken = newTokens.RefreshToken
        };

        return Result<AuthResult>.Success(
            authResult,
            localizer[SharedResourcesKeys.PasswordChangedSuccessfully]);
    }

    public async Task<Result> ForgetPasswordAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            throw new NotFoundException(
                localizer[SharedResourcesKeys.UserNotFound]);
        }

        await SendOtpAsync(user.Id, email);

        return Result.Success(
            localizer[SharedResourcesKeys.OtpSentSuccessfully]);
    }

    public async Task<Result> VerifyResetPasswordOtpAsync(
        VerifyOtpRequest dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);

        if (user == null)
        {
            throw new NotFoundException(
                localizer[SharedResourcesKeys.UserNotFound]);
        }

        var repo =
            unitOfWork.GetRepository<EmailVerificationCode, int>();

        var record = await repo.GetFirstOrDefaultAsync(
            x => x.ApplicationUserId == user.Id &&
                 x.Code == dto.Otp);

        if (record == null ||
            record.ExpirationTime < DateTime.Now)
        {
            throw new BadRequestException(
                localizer[SharedResourcesKeys.InvalidOrExpiredOtp]);
        }

        repo.Remove(record);
        await unitOfWork.SaveChangesAsync();

        var token =
            await userManager.GeneratePasswordResetTokenAsync(user);

        var encodedToken =
            WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(token));

        var resetLink =
            $"{appSettings.Value.ResetPasswordUrl}" +
            $"?email={Uri.EscapeDataString(user.Email!)}" +
            $"&token={encodedToken}";

        var filePath = Path.Combine(
            env.ContentRootPath,
            "wwwroot",
            "HTML",
            "ResetPassword.html");

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(
                localizer[SharedResourcesKeys.AnErrorOccured]);
        }

        var body = await File.ReadAllTextAsync(filePath);

        var baseUrl =
            appSettings.Value.BaseUrl?.TrimEnd('/');

        var logoUrl =
            $"{baseUrl?.TrimEnd('/')}/Images/Logo.png";

        body = body
            .Replace("{{user_name}}", user.UserName)
            .Replace("{{logo_url}}", logoUrl)
            .Replace("{{reset_link}}", resetLink);

        await emailService.SendEmailAsync(
            user.Email!,
            localizer[SharedResourcesKeys.OtpEmailSubject],
            body);

        return Result.Success(
            localizer[SharedResourcesKeys.ResetEmailSentSuccessfully]);
    }

    public async Task<Result<AuthResult>> ResetPasswordAsync(
        ResetPasswordRequest dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) ||
            string.IsNullOrWhiteSpace(dto.Token) ||
            string.IsNullOrWhiteSpace(dto.NewPassword))
        {
            throw new BadRequestException(
                localizer[SharedResourcesKeys.CredentialsAreRequired]);
        }

        var user = await userManager.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
        {
            throw new NotFoundException(
                localizer[SharedResourcesKeys.UserNotFound]);
        }

        var decodedTokenBytes =
            WebEncoders.Base64UrlDecode(dto.Token);

        var decodedToken =
            Encoding.UTF8.GetString(decodedTokenBytes);

        var result = await userManager.ResetPasswordAsync(
            user,
            decodedToken,
            dto.NewPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join(
                "; ",
                result.Errors.Select(e => e.Description));

            throw new BadRequestException(
                $"Failed to change password: {errors}");
        }

        var newTokens = await GenerateTokensAsync(user);

        var authResult = new AuthResult
        {
            AccessToken = newTokens.AccessToken,
            RefreshToken = newTokens.RefreshToken
        };

        return Result<AuthResult>.Success(
            authResult,
            localizer[SharedResourcesKeys.PasswordChangedSuccessfully]);
    }

    public async Task<Result> RegisterAsync(
        RegisterRequest dto)
    {
        var existingUser = await userManager.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (existingUser != null &&
            existingUser.EmailConfirmed)
        {
            throw new ConflictException(
                localizer[SharedResourcesKeys.EmailAlreadyExists]);
        }

        if (existingUser != null &&
            !existingUser.EmailConfirmed)
        {
            await SendOtpAsync(
                existingUser.Id,
                existingUser.Email!);

            return Result.Success(
                localizer[SharedResourcesKeys.OtpSentSuccessfully]);
        }

        var user = new ApplicationUser
        {
            Email = dto.Email,
            UserName = dto.Email,
            EmailConfirmed = false,
        };

        var result = await userManager.CreateAsync(
            user,
            dto.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(
                "; ",
                result.Errors.Select(e => e.Description));

            throw new BadRequestException(
                string.Format(
                    localizer[SharedResourcesKeys.RegistrationFailed],
                    errors));
        }

        await userManager.AddToRoleAsync(
            user,
            nameof(Roles.User));

        await SendOtpAsync(
            user.Id,
            user.Email!);

        return Result.Success(
            localizer[SharedResourcesKeys.OtpSentSuccessfully]);
    }

    public async Task<Result<AuthResult>> VerifyRegistrationOtpAsync(
        VerifyOtpRequest dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);

        if (user == null)
        {
            throw new NotFoundException(
                localizer[SharedResourcesKeys.UserNotFound]);
        }

        var repo =
            unitOfWork.GetRepository<EmailVerificationCode, int>();

        var record = await repo.GetFirstOrDefaultAsync(
            x => x.ApplicationUserId == user.Id &&
                 x.Code == dto.Otp);

        if (record == null ||
            record.ExpirationTime < DateTime.Now)
        {
            throw new BadRequestException(
                localizer[SharedResourcesKeys.InvalidOrExpiredOtp]);
        }

        user.EmailConfirmed = true;

        repo.Remove(record);

        await unitOfWork.SaveChangesAsync();

        await userManager.UpdateAsync(user);

        var tokens = await GenerateTokensAsync(user);

        var authResult = new AuthResult
        {
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken,
        };

        return Result<AuthResult>.Success(
            authResult,
            localizer[SharedResourcesKeys.AccountVerifiedSuccessfully]);
    }

    public async Task<Result<AuthResult>> RefreshTokenAsync(
        RefreshTokenRequest refreshTokenRequest)
    {
        var refreshRepo =
            unitOfWork.GetRepository<RefreshToken, int>();

        var existingToken = await refreshRepo
            .GetAllQueryable()
            .Include(r => r.ApplicationUser)
            .FirstOrDefaultAsync(
                r => r.Token == refreshTokenRequest.RefreshToken);

        if (existingToken == null ||
            !existingToken.IsActive)
        {
            throw new UnauthorizedException(
                localizer[SharedResourcesKeys.InvalidRefreshToken]);
        }

        var newAccessToken =
            await GenerateJwtTokenAsync(
                existingToken.ApplicationUser);

        var authResult = new AuthResult
        {
            AccessToken = newAccessToken,
            RefreshToken = existingToken.Token
        };

        return Result<AuthResult>.Success(
            authResult,
            localizer[SharedResourcesKeys.TokenRefreshedSuccessfully]);
    }

    private async Task SendOtpAsync(
        string userId,
        string email)
    {
        var code =
            new Random().Next(100000, 999999).ToString();

        var expiry =
            DateTime.Now.AddMinutes(10);

        var repo =
            unitOfWork.GetRepository<EmailVerificationCode, int>();

        var existingVerification =
            await repo.GetFirstOrDefaultAsync(
                v => v.ApplicationUserId == userId);

        if (existingVerification != null)
        {
            repo.Remove(existingVerification);
            await unitOfWork.SaveChangesAsync();
        }

        var verification = new EmailVerificationCode
        {
            Code = code,
            ExpirationTime = expiry,
            ApplicationUserId = userId
        };

        await repo.AddAsync(verification);
        await unitOfWork.SaveChangesAsync();

        var filePath = Path.Combine(
            env.ContentRootPath,
            "wwwroot",
            "HTML",
            "VerificationOtp.html");

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(
                localizer[SharedResourcesKeys.AnErrorOccured]);
        }

        var body =
            await File.ReadAllTextAsync(filePath);

        var baseUrl =
            appSettings.Value.BaseUrl?.TrimEnd('/');

        var logoUrl =
            $"{baseUrl}/Images/Logo.jpeg";

        body = body
            .Replace("{{logo_url}}", logoUrl)
            .Replace("{{otp_code}}", code);

        await emailService.SendEmailAsync(
            email,
            localizer[SharedResourcesKeys.OtpEmailSubject],
            body);
    }

    private async Task<GenerateTokenResult>
        GenerateTokensAsync(ApplicationUser user)
    {
        var accessToken =
            await GenerateJwtTokenAsync(user);

        var handler =
            new JwtSecurityTokenHandler();

        var jwtObj =
            handler.ReadJwtToken(accessToken);

        var jwtId = jwtObj.Id;

        var refreshToken =
            await GenerateRefreshTokenAsync(
                user,
                jwtId);

        return new GenerateTokenResult
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token
        };
    }

    private async Task<string>
        GenerateJwtTokenAsync(ApplicationUser user)
    {
        var jwtSettings =
            configuration.GetSection("JWT");

        var roles =
            await userManager.GetRolesAsync(user);

        var jwtId =
            Guid.NewGuid().ToString();

        var authClaims = new List<Claim>
        {
            new(
                ClaimTypes.NameIdentifier,
                user.Id),

            new(
                ClaimTypes.Name,
                user.Email!),

            new(
                "Email",
                user.Email!),

            new(
                JwtRegisteredClaimNames.Jti,
                jwtId),

            new(
                "SecurityStamp",
                user.SecurityStamp ?? string.Empty)
        };

        foreach (var role in roles)
        {
            authClaims.Add(
                new Claim(
                    ClaimTypes.Role,
                    role));

            authClaims.Add(
                new Claim(
                    "Role",
                    role));
        }

        var authSigningKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    jwtSettings["SecretKey"]!));

        var jwtToken = new JwtSecurityToken(
            issuer: jwtSettings["ValidIssuer"],
            audience: jwtSettings["ValidAudience"],
            expires: DateTime.UtcNow.AddDays(
                Convert.ToDouble(
                    jwtSettings["DurationInDays"])),
            claims: authClaims,
            signingCredentials:
                new SigningCredentials(
                    authSigningKey,
                    SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler()
            .WriteToken(jwtToken);
    }

    private async Task<RefreshToken>
        GenerateRefreshTokenAsync(
            ApplicationUser user,
            string jwtId)
    {
        var randomBytes =
            RandomNumberGenerator.GetBytes(64);

        var token =
            WebEncoders.Base64UrlEncode(
                randomBytes);

        var refreshToken = new RefreshToken
        {
            JwtId = jwtId,
            IsRevoked = false,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddMonths(1),
            Token = token
        };

        var refreshTokenRepo =
            unitOfWork.GetRepository<RefreshToken, int>();

        await refreshTokenRepo.AddAsync(refreshToken);
        await unitOfWork.SaveChangesAsync();

        return refreshToken;
    }
}