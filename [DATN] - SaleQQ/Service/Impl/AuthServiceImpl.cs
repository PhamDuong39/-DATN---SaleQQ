using _DATN____SaleQQ.Common.Util;
using _DATN____SaleQQ.Dto.Request.AuthRequest;
using _DATN____SaleQQ.Dto.Response;
using _DATN____SaleQQ.Dto.Response.AuthResponse;
using _DATN____SaleQQ.Entity;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace _DATN____SaleQQ.Service.Impl
{
    public class AuthServiceImpl : AuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly TokenUtil tokenUtil;

        public AuthServiceImpl(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            tokenUtil = new TokenUtil();
            _configuration = configuration;
        }

        #region Login + Logout
        public async Task<BaseResponse<LoginResponse>> Login(LoginRequest loginRequest)
        {
            bool isEmail = Regex.IsMatch(loginRequest.UsernameOrEmail, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");

            var user = isEmail 
                ? await _userManager.FindByEmailAsync(loginRequest.UsernameOrEmail) 
                : await _userManager.FindByNameAsync(loginRequest.UsernameOrEmail);

            if (user != null && await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                if (user is { EmailConfirmed : false})
                {
                    return new BaseResponse<LoginResponse>()
                    {
                        Message = "Email is not comfirmed ! Please check your registed email !!",
                        StatusCode = StatusCodes.Status400BadRequest,
                        Success = false
                    };
                }
                IList<String> userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                // Generate Token
                JwtSecurityToken accessToken = tokenUtil.GenerateAccessToken(authClaims);
                String refreshToken = tokenUtil.GenerateRefreshToken();
                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                await _userManager.UpdateAsync(user);
                return new BaseResponse<LoginResponse>
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = new LoginResponse()
                    {
                        AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                        RefreshToken = refreshToken,
                        Expiration = accessToken.ValidTo
                    }
                };
            }
            return new BaseResponse<LoginResponse>
            {
                Success = false,
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Thất bại!"
            };
        }

        public async Task<BaseResponse<string>> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return new BaseResponse<string>
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Logout successfuly"
                };
            }
            catch(Exception ex)
            {
                return new BaseResponse<string>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Logout failed !" + ex.Message
                };
            }
        }

        #endregion

        public Task<BaseResponse<ResetPasswordRequest>> ChangePassword(ResetPasswordRequest resetPasswordRequest)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<ResetPasswordRequest>> ForgotPassword(string email)
        {
            throw new NotImplementedException();
        }

        #region Register + confirm email

        public Task<BaseResponse<ApplicationUser>> Register(RegisterRequest registerRequest)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<ConfirmEmailRequest>> ConfirmEmail(ConfirmEmailRequest confirmEmailRequest)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
