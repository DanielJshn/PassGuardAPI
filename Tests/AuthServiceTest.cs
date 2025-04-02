using AutoMapper;
using Moq;
using Xunit;

namespace apief
{
    public class AuthServiceTest
    {
        private readonly Mock<IAuthRepository> _authRepositoryMock;
        private readonly Mock<ILog> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AuthService _userService;
        public AuthServiceTest()
        {
            _authRepositoryMock = new Mock<IAuthRepository>();
            _loggerMock = new Mock<ILog>();
            _mapperMock = new Mock<IMapper>();

            _userService = new AuthService(_authRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CreateNewAccountAsync_ShouldCreateUserSuccessfully()
        {
            // Arrange
            var userDto = new UserDataRegistrationDto
            {
                email = "test@example.com",
                hashedPK = "hashedPK",
                hashedPKSalt = "hashedPKSalt",
                encryptedSK = "encryptedSK",
                hashedRK = "hashedRK",
                recoverySK = "recoverySK"
            };

            var userModel = new UserData();
            _mapperMock.Setup(m => m.Map<UserData>(userDto)).Returns(userModel);
            _authRepositoryMock.Setup(r => r.AddUserDataAsync(It.IsAny<UserData>())).Returns(Task.CompletedTask);

            // Act
            var result = await _userService.CreateNewAccountAsync(userDto);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.id);
            _authRepositoryMock.Verify(r => r.AddUserDataAsync(It.IsAny<UserData>()), Times.Once);
        }
    }
}