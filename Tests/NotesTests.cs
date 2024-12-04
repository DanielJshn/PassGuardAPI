using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace apief
{
    public class TestNotes
    {
        [Fact]
        public async Task PostNote_ShouldReturnOk_WhenNoteIsCreatedSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var noteDto = new NoteDto
            {
                noteId = Guid.NewGuid(),
                title = "Test Note",
                description = "Test Description",
                backgroundColorHex = "#FFFFFF",
                categoryId = "1"
            };

            var createdNoteResponse = new NoteResponseDto
            {
                noteId = noteDto.noteId,
                title = noteDto.title,
                description = noteDto.description,
                backgroundColorHex = noteDto.backgroundColorHex,
                categoryId = noteDto.categoryId,
                createdTime = DateTime.UtcNow.ToString()
            };

            var mockNotesService = new Mock<INoteService>();
            mockNotesService
                .Setup(service => service.CreateNoteAsync(It.IsAny<NoteDto>(), It.IsAny<Guid>()))
                .ReturnsAsync(createdNoteResponse);

            var mockIdentityService = new Mock<IIdentityUser>();
            mockIdentityService
                .Setup(service => service.GetUserByTokenAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User { id = userId });

            var controller = new NoteController(mockNotesService.Object, mockIdentityService.Object);

            // Act
            var result = await controller.PostNote(noteDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);

            Assert.True(apiResponse.Success);
            Assert.Equal(createdNoteResponse, apiResponse.Data);
        }

        [Fact]
        public async Task PostNote_ShouldReturnBadRequest_WhenUserIdIsEmpty()
        {
            // Arrange
            var emptyUserId = Guid.Empty; // Пустой идентификатор пользователя
            var noteDto = new NoteDto
            {
                noteId = Guid.NewGuid(),
                title = "Test Note",
                description = "Test Description",
                backgroundColorHex = "#FFFFFF",
                categoryId = "1"
            };

            var mockIdentityService = new Mock<IIdentityUser>();
            mockIdentityService
                .Setup(service => service.GetUserByTokenAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User { id = emptyUserId });

            var mockNotesService = new Mock<INoteService>();
            var controller = new NoteController(mockNotesService.Object, mockIdentityService.Object);

            // Act
            var result = await controller.PostNote(noteDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse>(badRequestResult.Value);

            Assert.False(apiResponse.Success);
            Assert.Equal("Invalid user ID", apiResponse.Message); // Проверяем сообщение об ошибке
        }


    }
}