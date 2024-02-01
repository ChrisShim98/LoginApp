using api.Controllers;
using api.DTOs;
using api.Entity;
using api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace api.Tests
{
    [TestFixture]
    public class AccountControllerTests
    {
        private AccountController _accountController;
        private Mock<UserManager<AppUser>> _userManager;
        private Mock<IUserStore<AppUser>> _userStore;
        private Mock<ITokenService> _tokenService;
        private Mock<IMapper> _mapper;
        private RegisterDTO _registerDTO;
        private LoginDTO _loginDTO;

        [SetUp]
        public void Setup()
        {    
            // Mocking the user storage to mock data
            // _userStore = Mock.Of<IUserStore<AppUser>>(); 
            _userStore = new Mock<IUserStore<AppUser>>();

            // This is a mock list of users that would be
            // stored in the mock database 
            var users = new List<AppUser>
            {
                new AppUser { Id = 1, UserName = "test", Email = "test@test.com" },
                new AppUser { Id = 2, UserName = "chris", Email = "chris@test.com" },
                new AppUser { Id = 3, UserName = "sarah", Email = "sarah@test.com" },
            }.AsQueryable();

            // Mocking the ASP core Identity manager which has many
            // classes that inherit from it but we dont need them
            _userManager = new Mock<UserManager<AppUser>>(_userStore.Object, null, null, null, null, null, null, null, null);

            // Setting up that the identtiy manager will return 
            // the list of mock users
            _userManager.Setup(_ => _.Users).Returns(users);

            // These classes are in the account controller
            // so we need them too
            _tokenService = new Mock<ITokenService>();
            _mapper = new Mock<IMapper>();

            // Fetching the account controller, this is not a mock
            _accountController = new AccountController(_userManager.Object, _tokenService.Object, _mapper.Object);
        }

        // ----------------------- Tests for Register Function -----------------------
        [Test]
        [TestCase("chris", "Chris123", "chris@test.com")]
        [TestCase("sarah", "Testing", "sarah@sarah.com")]
        public async Task Register_UsernameTaken_ReturnsBadRequest(string username, string password, string email)
        {
            // Arrange
            // Test case 1 is an exact match of username, password and email
            // Test case 2 is an exact match of username only
            _registerDTO = new RegisterDTO {
                Username = username,
                Password = password,
                Email = email
            };

            // Act
            var result = await _accountController.Register(_registerDTO);

            // Assert
            // Assert that the result is a Bad request
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);

            // Create a variable if result is a BadRequestObjectResult
            var resultBadRequestObject = result.Result as BadRequestObjectResult;

            // Assert that the error message is 
            // "Username is taken!"
            Assert.That(resultBadRequestObject.Value, Is.EquivalentTo("Username is taken!"));
        }

        [Test]
        [TestCase("newuser", "Chris123", "newuser@test.com")]
        public async Task Register_UserWithNoErrors_GetUserDTO(string username, string password, string email)
        {
            // Arrange
            _registerDTO = new RegisterDTO {
                Username = username,
                Password = password,
                Email = email
            };
            
            // Setting the CreateAsync to alway return true
            _userManager.Setup(_ => _.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

            // Setting the AddToRoleAsync to always return true
            _userManager.Setup(_ => _.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

            // Setting _mapper to map registerDTO to userDTO
            // Only username is required, token is not set here
            _mapper.Setup(_ => _.Map<AppUser>(It.IsAny<RegisterDTO>()))
                .Returns((RegisterDTO registerDTO) =>
                {
                    var newUser = new AppUser {
                        UserName = registerDTO.Username,
                        PasswordHash = registerDTO.Password,
                        Email = registerDTO.Email
                    };

                    return newUser;
                });  

            // Fetching the account controller again because
            // new setup on _userManager
            _accountController = new AccountController(_userManager.Object, _tokenService.Object, _mapper.Object);

            // Act
            var result = await _accountController.Register(_registerDTO) as ActionResult<UserDTO>;

            // Assert
            // Assert that the result is a UserDTO
            Assert.IsInstanceOf<UserDTO>(result.Value);
        }

        // ----------------------- Tests for UserExists Function -----------------------
        [Test]
        [TestCase("Chris", ExpectedResult = true)]
        [TestCase("Camilla", ExpectedResult = false)]
        public async Task<bool> UserExists_UsersToCheck_PassAndFailCase(string username) {
            // Arrange
            // Chris should be true because it exists in 
            // mock database and Camilla should be false
            // because it doesnt exist in mock database

            // Act
            var result = await _accountController.UserExists(username);

            // Assert
            return result;
        }

        // ----------------------- Tests for Login Function -----------------------
        [Test]
        public async Task Login_ValidUser_ReturnUserDTO() {
            // Arrange
            _loginDTO = new LoginDTO {
                Username = "chris",
                Password = "Chris123"
            };

            // Setting the CheckPasswordAsync to alway return true
            _userManager.Setup(_ => _.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            .ReturnsAsync(true);

            // Fetching the account controller again because
            // new setup on _userManager
            _accountController = new AccountController(_userManager.Object, _tokenService.Object, _mapper.Object);

            // Act
            var result = await _accountController.Login(_loginDTO) as ActionResult<UserDTO>;

            // Assert
            Assert.IsInstanceOf<UserDTO>(result.Value);
        }

        [Test]
        public async Task Login_InvalidUser_ReturnUnauthorized() {
            // Arrange
            _loginDTO = new LoginDTO {
                Username = "notexisting",
                Password = "Chris123"
            };

            // Act
            var result = await _accountController.Login(_loginDTO);

            // Assert
            // Assert that the result is an Unauthorized request
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result.Result);

            // Create a variable if result is an UnauthorizedObjectResult
            var resultUnauthorizedObject = result.Result as UnauthorizedObjectResult;

            // Assert that the error message is 
            // "Invalid username"
            Assert.That(resultUnauthorizedObject.Value, Is.EquivalentTo("Invalid username"));
        }

        [Test]
        public async Task Login_InvalidPassword_ReturnUnauthorized() {
            // Arrange
            _loginDTO = new LoginDTO {
                Username = "chris",
                Password = "wrongpassword"
            };

            // Setting the CheckPasswordAsync to alway return false
            _userManager.Setup(_ => _.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            .ReturnsAsync(false);

            // Fetching the account controller again because
            // new setup on _userManager
            _accountController = new AccountController(_userManager.Object, _tokenService.Object, _mapper.Object);

            // Act
            var result = await _accountController.Login(_loginDTO);

            // Assert
            // Assert that the result is an Unauthorized request
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result.Result);

            // Create a variable if result is an UnauthorizedObjectResult
            var resultUnauthorizedObject = result.Result as UnauthorizedObjectResult;

            // Assert that the error message is 
            // "Invalid username"
            Assert.That(resultUnauthorizedObject.Value, Is.EquivalentTo("Invalid Password"));
        }
    }
}
