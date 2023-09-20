using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API.Dtos;
using API.Helpers;
using API.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class UserService : IUserService
{
    private readonly JWT _jwt;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher<User> _passwordHasher;
    public UserService(IUnitOfWork unitOfWork, IOptions<JWT> jwt, IPasswordHasher<User> passwordHasher)
    {
        _jwt = jwt.Value;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }
    public async Task<string> RegisterAsync(RegisterDto registerDto)
    {
        var user = new User
        {
            Email = registerDto.Email,
            UserName = registerDto.UserName,
            IdenNumber = registerDto.IdenNumber,
        };
        user.Password = _passwordHasher.HashPassword(user, registerDto.Password); //Encrypt password
        var existingUser = _unitOfWork.Users
                                    .Find(u => u.UserName.ToLower() == registerDto.UserName.ToLower() || u.IdenNumber == registerDto.IdenNumber)
                                    .FirstOrDefault();
        if (existingUser == null)
        {
            var rolDefault = _unitOfWork.Roles
                                    .Find(u => u.Name == Authorization.rol_default.ToString())
                                    .First();
            try
            {
                user.Roles.Add(rolDefault);
                _unitOfWork.Users.Add(user);
                await _unitOfWork.SaveAsync();

                return $"User  {registerDto.UserName} has been registered successfully";
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return $"Error: {message}";
            }
        }
        else
        {
            return $"User {registerDto.UserName} already registered.";
        }
    }
    public async Task<string> RegisterAdmiAsync(RegisterAdmiDto registerAdmiDto)
    {
        var user = new User
        {
            Email = registerAdmiDto.Email,
            UserName = registerAdmiDto.UserName
        };

        user.Password = _passwordHasher.HashPassword(user, registerAdmiDto.Password); //Encrypt password

        var existingUser = _unitOfWork.Users
                                    .Find(u => u.UserName.ToLower() == registerAdmiDto.UserName.ToLower())
                                    .FirstOrDefault();

        if (existingUser == null)
        {
            var rolDefault = _unitOfWork.Roles
                                    .Find(u => u.Name == Authorization.Roles.Administrator.ToString())
                                    .First();
            try
            {
                user.Roles.Add(rolDefault);
                _unitOfWork.Users.Add(user);
                await _unitOfWork.SaveAsync();

                return $"User  {registerAdmiDto.UserName} has been registered successfully";
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return $"Error: {message}";
            }
        }
        else
        {
            return $"User {registerAdmiDto.UserName} already registered.";
        }
    }
    public async Task<string> RegisterEmployeeAsync(RegisterEmployeeDto registerEmployeeDto)
    {
        var user= CreateUser(registerEmployeeDto.Email, registerEmployeeDto.UserName, registerEmployeeDto.Password);

        if (user != null)
        {
            var existingEmployee = _unitOfWork.Employees
                                              .Find(u => u.IdenNumber == registerEmployeeDto.IdenNumber)
                                              .FirstOrDefault();
            if (existingEmployee == null)
            {
                var rolDefault = _unitOfWork.Roles
                                   .Find(u => u.Name == Authorization.Roles.Employee.ToString())
                                   .First();
                try
                {
                    user.Roles.Add(rolDefault);
                    _unitOfWork.Users.Add(user);
                    await _unitOfWork.SaveAsync();
                    //var idUser = _unitOfWork.Users.GetIDUserAsync(user.UserName);
                    var userLook = _unitOfWork.Users.GetByUserNameAsync(user.UserName);
                    var employee = new Employee
                    {
                        Name = registerEmployeeDto.Name,
                        Position = registerEmployeeDto.Position,
                        IdenNumber = registerEmployeeDto.IdenNumber,
                        DateContract = registerEmployeeDto.DateContract,
                        UserId = userLook.Id

                    };
                   
                    _unitOfWork.Employees.Add(employee);
                    await _unitOfWork.SaveAsync();

                    return $"User  {registerEmployeeDto.UserName} has been registered successfully";

                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    return $"Error: {message}";
                }
            }
            else
            {
                return $"User {registerEmployeeDto.IdenNumber} already registered.";
            }
        }
        else
        {
            return $"User {registerEmployeeDto.UserName} already registered.";
        }
    }

    public async Task<string> RegisterPatientAsync(RegisterPatientDto registerPatientDto)
    {
         var user= CreateUser(registerPatientDto.Email, registerPatientDto.UserName, registerPatientDto.Password);

        if (user != null)
        {
            var existingPatient = _unitOfWork.Patients
                                              .Find(u => u.IdenNumber == registerPatientDto.IdenNumber)
                                              .FirstOrDefault();
            if (existingPatient == null)
            {
                var rolDefault = _unitOfWork.Roles
                                   .Find(u => u.Name == Authorization.Roles.Patient.ToString())
                                   .First();
                try
                {
                    user.Roles.Add(rolDefault);
                    _unitOfWork.Users.Add(user);
                    await _unitOfWork.SaveAsync();
                    //var idUser = _unitOfWork.Users.GetIDUserAsync(user.UserName);
                    var userLook = _unitOfWork.Users.GetByUserNameAsync(user.UserName);
                    var patient = new Patient
                    {
                        Name = registerPatientDto.Name,
                        Address = registerPatientDto.Address,
                        IdenNumber = registerPatientDto.IdenNumber,
                        PhoneNumber = registerPatientDto.PhoneNumber,
                        UserId = userLook.Id

                    };
                   
                    _unitOfWork.Patients.Add(patient);
                    await _unitOfWork.SaveAsync();

                    return $"User  {registerPatientDto.UserName} has been registered successfully";

                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    return $"Error: {message}";
                }
            }
            else
            {
                return $"User with Identificacion Number {registerPatientDto.IdenNumber} already registered.";
            }
        }
        else
        {
            return $"User {registerPatientDto.UserName} already registered.";
        }
    } 
    public async Task<DataUserDto> GetTokenAsync(LoginDto model)
    {
        DataUserDto dataUserDto = new DataUserDto();
        var user = await _unitOfWork.Users
                    .GetByUserNameAsync(model.UserName);

        if (user == null)
        {
            dataUserDto.IsAuthenticated = false;
            dataUserDto.Message = $"User does not exist with UserName {model.UserName}.";
            return dataUserDto;
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

        if (result == PasswordVerificationResult.Success)
        {
            dataUserDto.IsAuthenticated = true;
            JwtSecurityToken jwtSecurityToken = CreateJwtToken(user);
            dataUserDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            dataUserDto.Email = user.Email;
            dataUserDto.UserName = user.UserName;
            dataUserDto.Roles = user.Roles
                                            .Select(u => u.Name)
                                            .ToList();

            if (user.RefreshTokens.Any(a => a.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.Where(a => a.IsActive == true).FirstOrDefault();
                dataUserDto.RefreshToken = activeRefreshToken.Token;
                dataUserDto.RefreshTokenExpiration = activeRefreshToken.Expires;
            }
            else
            {
                var refreshToken = CreateRefreshToken();
                dataUserDto.RefreshToken = refreshToken.Token;
                dataUserDto.RefreshTokenExpiration = refreshToken.Expires;
                user.RefreshTokens.Add(refreshToken);
                _unitOfWork.Users.Update(user);
                await _unitOfWork.SaveAsync();
            }

            return dataUserDto;
        }
        dataUserDto.IsAuthenticated = false;
        dataUserDto.Message = $"Credenciales incorrectas para el usuario {user.UserName}.";
        return dataUserDto;
    }
    public async Task<string> AddRoleAsync(AddRoleDto model)
    {
        var user = await _unitOfWork.Users
                    .GetByUserNameAsync(model.UserName);
        if (user == null)
        {
            return $"User {model.UserName} does not exists.";
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

        if (result == PasswordVerificationResult.Success)
        {
            var rolExists = _unitOfWork.Roles
                                        .Find(u => u.Name.ToLower() == model.Role.ToLower())
                                        .FirstOrDefault();
            if (rolExists != null)
            {
                var userHasRole = user.Roles
                                        .Any(u => u.Id == rolExists.Id);

                if (userHasRole == false)
                {
<<<<<<< HEAD
<<<<<<< HEAD
                    if (rolExists.Name == Authorization.Roles.Employee.ToString())
                    {
                        var existPosition = _unitOfWork.Positions
                                                 .Find(u => u.Name.ToLower() == model.Position.ToLower())
                                                 .FirstOrDefault();

                        if (!string.IsNullOrEmpty(model.Name) && !string.IsNullOrEmpty(model.Position) && existPosition != null)
=======
                    if (rolExists.Name == Authorization.Roles.Employee.ToString())
                    {
                        if (!string.IsNullOrEmpty(model.Name) && !string.IsNullOrEmpty(model.Position))
>>>>>>> f6a347b (addrole casi check jeje)
                        {
                            var employee = new Employee
                            {
                                Name = model.Name,
<<<<<<< HEAD
                                PositionId = existPosition.Id,
=======
                                Position = model.Position,
>>>>>>> f6a347b (addrole casi check jeje)
                                UserId = user.Id
                            };
                            _unitOfWork.Employees.Add(employee);
                            await _unitOfWork.SaveAsync();
                        }
                        else
                        {
                            return $"Register employees needs all data (Name, Position)";
                        }

                    }
<<<<<<< HEAD
                    var withoutRole = user.Roles.FirstOrDefault(u => u.Name == Authorization.Roles.WithoutRol.ToString());
                    if (withoutRole != null && model.Role.ToLower() != Authorization.Roles.WithoutRol.ToString().ToLower())
                    {
                        user.Roles.Remove(withoutRole);
                    }
=======
                    //if(rolExists.Name == Authorization.Roles.Employee.ToString()){}
                    //en proceso jeje
>>>>>>> e85a095 (feat: :alembic: cambios y experimentos con roles y usuarios jeje)
=======
>>>>>>> f6a347b (addrole casi check jeje)
                    user.Roles.Add(rolExists);
                    _unitOfWork.Users.Update(user);
                    await _unitOfWork.SaveAsync();
                    return $"Role {model.Role} added to user {model.UserName} successfully.";
                }
<<<<<<< HEAD
                else
                {
                    return $"Role {model.Role} ya está asignado al usuario.";
                }
            }
            else
            {
=======
                else{
                    return $"Role {model.Role} ya está asignado al usuario.";
                }
            }
            else{
>>>>>>> f6a347b (addrole casi check jeje)
                return $"Role {model.Role} was not found.";
            }

        }
<<<<<<< HEAD
        else
        {
=======
        else{
>>>>>>> f6a347b (addrole casi check jeje)
            return $"Invalid Credentials";
        }

    }
    public async Task<DataUserDto> RefreshTokenAsync(string refreshToken)
    {
        var dataUserDto = new DataUserDto();

        var usuario = await _unitOfWork.Users
                        .GetByRefreshTokenAsync(refreshToken);

        if (usuario == null)
        {
            dataUserDto.IsAuthenticated = false;
            dataUserDto.Message = $"Token is not assigned to any user.";
            return dataUserDto;
        }

        var refreshTokenBd = usuario.RefreshTokens.Single(x => x.Token == refreshToken);

        if (!refreshTokenBd.IsActive)
        {
            dataUserDto.IsAuthenticated = false;
            dataUserDto.Message = $"Token is not active.";
            return dataUserDto;
        }
        //Revoque the current refresh token and
        refreshTokenBd.Revoked = DateTime.UtcNow;
        //generate a new refresh token and save it in the database
        var newRefreshToken = CreateRefreshToken();
        usuario.RefreshTokens.Add(newRefreshToken);
        _unitOfWork.Users.Update(usuario);
        await _unitOfWork.SaveAsync();
        //Generate a new Json Web Token
        dataUserDto.IsAuthenticated = true;
        JwtSecurityToken jwtSecurityToken = CreateJwtToken(usuario);
        dataUserDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        dataUserDto.Email = usuario.Email;
        dataUserDto.UserName = usuario.UserName;
        dataUserDto.Roles = usuario.Roles
                                        .Select(u => u.Name)
                                        .ToList();
        dataUserDto.RefreshToken = newRefreshToken.Token;
        dataUserDto.RefreshTokenExpiration = newRefreshToken.Expires;
        return dataUserDto;
    }
    private RefreshToken CreateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var generator = RandomNumberGenerator.Create())
        {
            generator.GetBytes(randomNumber);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                Expires = DateTime.UtcNow.AddMinutes(2),
                Created = DateTime.UtcNow
            };
        }
    }
    private JwtSecurityToken CreateJwtToken(User usuario)
    {
        var roles = usuario.Roles;
        var roleClaims = new List<Claim>();
        foreach (var role in roles)
        {
            roleClaims.Add(new Claim("roles", role.Name));
        }
        var claims = new[]
        {
                                new Claim(JwtRegisteredClaimNames.Sub, usuario.UserName),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                                new Claim("uid", usuario.Id.ToString())
                        }
        .Union(roleClaims);
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.DurationOnMinutes),
            signingCredentials: signingCredentials);
        return jwtSecurityToken;
    }

    private User CreateUser(string email, string username, string password)
    {
        var user = new User
        {
            Email = email,
            UserName = username
        };

        user.Password = _passwordHasher.HashPassword(user, password); //Encrypt password

        var existingUser = _unitOfWork.Users
                                    .Find(u => u.UserName.ToLower() == username.ToLower())
                                    .FirstOrDefault();

        if (existingUser != null)
        {
            return null;
        }
        return user;
    }

}
