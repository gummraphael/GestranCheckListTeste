using GestranChecklist.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using Moq;

public class ChecklistControllerTests
{
    private readonly Mock<IChecklistService> _checklistServiceMock;
    private readonly ChecklistController _checklistController;

    public ChecklistControllerTests()
    {
        _checklistServiceMock = new Mock<IChecklistService>();
        _checklistController = new ChecklistController(_checklistServiceMock.Object);
    }

    [Fact]
    public async Task CriarChecklist_ShouldReturnBadRequest_WhenServiceReturnsError()
    {
        // Arrange
        var checklistDto = new ChecklistDto();
        _checklistServiceMock.Setup(service => service.CriarChecklist(checklistDto))
            .ReturnsAsync(ResultViewModel<ChecklistDto>.Error("Dados do checklist inválidos."));

        // Act
        var result = await _checklistController.CriarChecklist(checklistDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var resultViewModel = Assert.IsType<ResultViewModel<ChecklistDto>>(badRequestResult.Value);
        Assert.Equal("Dados do checklist inválidos.", resultViewModel.Message);
    }

    [Fact]
    public async Task CriarChecklist_ShouldReturnOk_WhenServiceReturnsSuccess()
    {
        // Arrange
        var checklistDto = new ChecklistDto();
        _checklistServiceMock.Setup(service => service.CriarChecklist(checklistDto))
            .ReturnsAsync(ResultViewModel<ChecklistDto>.Success(checklistDto));

        // Act
        var result = await _checklistController.CriarChecklist(checklistDto);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task AdicionarItemAoChecklist_ShouldReturnBadRequest_WhenServiceReturnsError()
    {
        // Arrange
        var itemDto = new ChecklistItemDto();
        var checklistId = 1;
        _checklistServiceMock.Setup(service => service.AdicionarItemAoChecklist(checklistId, itemDto, It.IsAny<string>()))
            .ReturnsAsync(ResultViewModel.Error("Erro ao adicionar item."));

        // Act
        var result = await _checklistController.AdicionarItemAoChecklist(checklistId, itemDto, "executorId");

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Erro ao adicionar item.", badRequestResult.Value);
    }

    [Fact]
    public async Task AdicionarItemAoChecklist_ShouldReturnOk_WhenServiceReturnsSuccess()
    {
        // Arrange
        var itemDto = new ChecklistItemDto();
        var checklistId = 1;
        _checklistServiceMock.Setup(service => service.AdicionarItemAoChecklist(checklistId, itemDto, It.IsAny<string>()))
            .ReturnsAsync(ResultViewModel.Success());

        // Act
        var result = await _checklistController.AdicionarItemAoChecklist(checklistId, itemDto, "executorId");

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task ObterChecklist_ShouldReturnNotFound_WhenServiceReturnsError()
    {
        // Arrange
        var checklistId = 1;
        _checklistServiceMock.Setup(service => service.ObterChecklist(checklistId))
            .ReturnsAsync(ResultViewModel<ChecklistDto>.Error("Checklist não encontrado."));

        // Act
        var result = await _checklistController.ObterChecklist(checklistId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task ObterChecklist_ShouldReturnOk_WhenServiceReturnsSuccess()
    {
        // Arrange
        var checklistId = 1;
        var checklist = new ChecklistDto(); // Certifique-se de que Checklist é um objeto válido
        _checklistServiceMock.Setup(service => service.ObterChecklist(checklistId))
            .ReturnsAsync(ResultViewModel<ChecklistDto>.Success(checklist));

        // Act
        var result = await _checklistController.ObterChecklist(checklistId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var resultValue = Assert.IsType<ResultViewModel<ChecklistDto>>(okResult.Value);
        Assert.Equal(checklist, resultValue.Data);
    }


    [Fact]
    public async Task ObterChecklist_ShouldReturnOk_WhenServiceReturnsSuccess2()
    {
        // Arrange
        var checklistId = 1;
        var checklist = new ChecklistDto();
        _checklistServiceMock.Setup(service => service.ObterChecklist(checklistId))
            .ReturnsAsync(ResultViewModel<ChecklistDto>.Success(checklist));

        // Act
        var result = await _checklistController.ObterChecklist(checklistId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var resultValue = Assert.IsType<ResultViewModel<ChecklistDto>>(okResult.Value);
        Assert.Equal(checklist, resultValue.Data);
    }

    [Fact]
    public async Task AprovarChecklist_ShouldReturnBadRequest_WhenServiceReturnsError()
    {
        // Arrange
        var checklistId = 1;
        var supervisorId = "supervisorId";
        _checklistServiceMock.Setup(service => service.AprovarChecklist(checklistId, supervisorId))
            .ReturnsAsync(ResultViewModel.Error("Erro ao aprovar checklist."));

        // Act
        var result = await _checklistController.AprovarChecklist(checklistId, supervisorId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Erro ao aprovar checklist.", badRequestResult.Value);
    }

    [Fact]
    public async Task AprovarChecklist_ShouldReturnNoContent_WhenServiceReturnsSuccess()
    {
        // Arrange
        var checklistId = 1;
        var supervisorId = "supervisorId";
        _checklistServiceMock.Setup(service => service.AprovarChecklist(checklistId, supervisorId))
            .ReturnsAsync(ResultViewModel.Success());

        // Act
        var result = await _checklistController.AprovarChecklist(checklistId, supervisorId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task ObterTodosChecklists_ShouldReturnOk_WhenServiceReturnsSuccess()
    {
        // Arrange
        var checklistsDto = new List<ChecklistDto> { new ChecklistDto() }; // Crie uma lista de ChecklistDto
        var resultViewModel = ResultViewModel<List<ChecklistDto>>.Success(checklistsDto); // Crie o ResultViewModel com sucesso

        _checklistServiceMock.Setup(service => service.ListarChecklists())
            .ReturnsAsync(resultViewModel); // Configure o mock para retornar o ResultViewModel

        // Act
        var result = await _checklistController.ObterTodosChecklists();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var resultValue = Assert.IsType<ResultViewModel<List<ChecklistDto>>>(okResult.Value);
        Assert.Equal(checklistsDto, resultValue.Data); // Verifique se os dados retornados são os esperados
    }


    [Fact]
    public async Task AtualizarChecklist_ShouldReturnNotFound_WhenServiceReturnsError()
    {
        // Arrange
        var checklistId = 1;
        var checklistDto = new ChecklistDto();

        // Configure o mock para retornar um ResultViewModel com erro
        var resultViewModel = ResultViewModel<ChecklistDto>.Error("Checklist não encontrado.");
        _checklistServiceMock.Setup(service => service.AtualizarChecklist(checklistId, checklistDto))
            .ReturnsAsync(resultViewModel);

        // Act
        var result = await _checklistController.AtualizarChecklist(checklistId, checklistDto);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }


    [Fact]
    public async Task AtualizarChecklist_ShouldReturnNoContent_WhenServiceReturnsSuccess()
    {
        // Arrange
        var checklistId = 1;
        var checklistDto = new ChecklistDto();
        var resultViewModel = ResultViewModel<ChecklistDto>.Success(checklistDto);

        _checklistServiceMock.Setup(service => service.AtualizarChecklist(checklistId, checklistDto))
            .ReturnsAsync(resultViewModel);

        // Act
        var result = await _checklistController.AtualizarChecklist(checklistId, checklistDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}
