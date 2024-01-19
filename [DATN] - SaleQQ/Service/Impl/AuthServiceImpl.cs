using _DATN____SaleQQ.Common.Config;
using _DATN____SaleQQ.Common.Util;
using _DATN____SaleQQ.Dto.Request.AuthRequest;
using _DATN____SaleQQ.Dto.Response;
using _DATN____SaleQQ.Dto.Response.AuthResponse;
using _DATN____SaleQQ.Entity;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Crypto.Engines;
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
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly TokenUtil tokenUtil;

        public AuthServiceImpl(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, EmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            tokenUtil = new TokenUtil();
            _configuration = configuration;
            _emailService = emailService;
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

        public async Task<BaseResponse<ApplicationUser>> Register(RegisterRequest registerRequest)
        {
            var userExits = await _userManager.FindByNameAsync(registerRequest.Username);
            if (userExits != null)
            {
                return new BaseResponse<ApplicationUser>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "User is already exits !"
                };
            }

            var emailExits = await _userManager.FindByEmailAsync(registerRequest.Email);
            if (userExits != null)
            {
                return new BaseResponse<ApplicationUser>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Email is already register !"
                };
            }

            ApplicationUser applicationUser = new ApplicationUser()
            {
                Email = registerRequest.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerRequest.Username,
                Address = registerRequest.Address,
                FullName = registerRequest.Fullname,
                CreatedAt = DateTime.Now,
                UpdateAt = DateTime.Now
            };

            var result = await _userManager.CreateAsync(applicationUser);
            if (!result.Succeeded)
            {
                return new BaseResponse<ApplicationUser>             
                {
                    Success = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Failed to register new account!"
                };
            }

            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                var adminRole = new ApplicationRole { Name = "Admin", AuthorityName = "Admin", CreatedAt = DateTime.Now };
                await _roleManager.CreateAsync(adminRole);
            }

            if (!await _roleManager.RoleExistsAsync("User"))
            {
                var userRole = new ApplicationRole { Name = "User", AuthorityName = "User", CreatedAt = DateTime.Now };
                await _roleManager.CreateAsync(userRole);
            }

            if (!await _roleManager.RoleExistsAsync("Employee"))
            {
                var employeeRole = new ApplicationRole { Name = "Employee", AuthorityName = "Employee", CreatedAt = DateTime.Now };
                await _roleManager.CreateAsync(employeeRole);
            }

            if (await _roleManager.RoleExistsAsync("User"))
            {
                await _userManager.AddToRoleAsync(applicationUser, "User");
            }

            // Thêm token cho Xác thực Email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
            var tokenEncoded = Uri.EscapeDataString(token);
            var confirmationLink = $"https://localhost:7046/api/auth/confirm-email?UserNameOrEmail={applicationUser.Email}&Token={tokenEncoded}";
            var htmlContent = $@"
                <html>
                <body>
                    <p>Xin chào,</p>
                    <p>Vui lòng bấm vào nút bên dưới để xác nhận Email!</p>
                    <a href='{confirmationLink}'>Xác nhận email</a>
                </body>
                </html>";


            var message = new EmailRequest(new string[] { applicationUser.Email! }, "Xác nhận email", htmlContent);
            _emailService.SendEmail(message);

            return new BaseResponse<ApplicationUser>
            {
                Success = true,
                StatusCode = StatusCodes.Status201Created,
                Message = $"Đăng ký thành công và Email xác nhận đã được gửi đến {applicationUser.Email}!",
                Data = applicationUser
            };
        }

        public async Task<BaseResponse<ConfirmEmailRequest>> ConfirmEmail(ConfirmEmailRequest confirmEmailRequest)
        {
            bool isEmail = Regex.IsMatch(confirmEmailRequest.UserNameOrEmail, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
            
            var user = isEmail 
                ? await _userManager.FindByEmailAsync(confirmEmailRequest.UserNameOrEmail)
                : await _userManager.FindByNameAsync(confirmEmailRequest.UserNameOrEmail);

            if (user == null)
            {
                return new BaseResponse<ConfirmEmailRequest>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Email was never registed"
                };
            }

            var result = await _userManager.ConfirmEmailAsync(user, confirmEmailRequest.Token);
            user.UpdateAt = DateTime.Now;
            await _userManager.UpdateAsync(user);
            
            if (!result.Succeeded)
            {
                return new BaseResponse<ConfirmEmailRequest>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Confirm Email failed. Please Try again!"
                };
            }
            return new BaseResponse<ConfirmEmailRequest>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = "Email confirm successfuly"
            };
        }

        #endregion
    }
}
