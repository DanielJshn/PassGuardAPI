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

            var result = await _userService.CreateNewAccountAsync(userDto);

            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.id);
            _authRepositoryMock.Verify(r => r.AddUserDataAsync(It.IsAny<UserData>()), Times.Once);
        }


        [Fact]
        public async Task CreateNewAccountAsync_ShouldThrowException_WhenEmailIsMissing()
        {
            var userDto = new UserDataRegistrationDto
            {
                email = "",
                hashedPK = "hashedPK",
                hashedPKSalt = "hashedPKSalt",
                encryptedSK = "encryptedSK",
                hashedRK = "hashedRK",
                recoverySK = "recoverySK"
            };

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _userService.CreateNewAccountAsync(userDto));
            Assert.Contains("Email is required.", exception.Message);
        }


        [Fact]
        public async Task CreateNewAccountAsync_ShouldThrowException_WhenMultipleFieldsAreMissing()
        {
            var userDto = new UserDataRegistrationDto
            {
                email = "",
                hashedPK = "",
                hashedPKSalt = "",
                encryptedSK = "",
                hashedRK = "",
                recoverySK = ""
            };

            string expectedMessage = "Email is required. HashedPK is required. HashedPKSalt is required. EncryptedSK is required. HashedRK is required. RecoverySK is required.";

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _userService.CreateNewAccountAsync(userDto));

            Assert.Equal(expectedMessage, exception.Message);
        }


        [Fact]
        public async Task CreateNewAccountAsync_ShouldThrowException_WhenDatabaseFails()
        {
            var userDto = new UserDataRegistrationDto
            {
                email = "test@example.com",
                hashedPK = "hashedPK",
                hashedPKSalt = "hashedPKSalt",
                encryptedSK = "encryptedSK",
                hashedRK = "hashedRK",
                recoverySK = "recoverySK"
            };

            _mapperMock.Setup(m => m.Map<UserData>(userDto)).Returns(new UserData());

            _authRepositoryMock.Setup(r => r.AddUserDataAsync(It.IsAny<UserData>())).ThrowsAsync(new Exception("DB error"));

            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.CreateNewAccountAsync(userDto));

            Assert.Equal("An error occurred while saving the user data.", exception.Message);
        }
    }
}