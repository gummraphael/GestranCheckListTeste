using GestranChecklist.Application.Dtos;
using GestranChecklist.Core.Enum;
using Moq;

public class ChecklistServiceTests
{
    private readonly Mock<IChecklistRepository> _checklistRepositoryMock;
    private readonly ChecklistService _checklistService;

    public ChecklistServiceTests()
    {
        _checklistRepositoryMock = new Mock<IChecklistRepository>();
        _checklistService = new ChecklistService(_checklistRepositoryMock.Object);
    }

    [Fact]
    public async Task CriarChecklist_ShouldReturnError_WhenChecklistAlreadyExists()
    {
        // Arrange
        var dto = new ChecklistDto { PlacaVeiculo = "ABC1234", Status = StatusEnum.EmExecucao, Itens = new List<ChecklistItemDto>() };
        _checklistRepositoryMock.Setup(repo => repo.ExisteChecklistParaVeiculo(dto.PlacaVeiculo)).ReturnsAsync(true);

        // Act
        var result = await _checklistService.CriarChecklist(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Já existe um checklist em aberto para este veículo.", result.Message);
    }

    [Fact]
    public async Task CriarChecklist_ShouldReturnError_WhenChecklistIsConcludedAndItemsAreEmpty()
    {
        // Arrange
        var dto = new ChecklistDto { PlacaVeiculo = "ABC1234", Status = StatusEnum.Concluido, Itens = new List<ChecklistItemDto>() };
        _checklistRepositoryMock.Setup(repo => repo.ExisteChecklistParaVeiculo(dto.PlacaVeiculo)).ReturnsAsync(false);

        // Act
        var result = await _checklistService.CriarChecklist(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Não é possível concluir um Checklist sem itens para ser verificados.", result.Message);
    }

    [Fact]
    public async Task CriarChecklist_ShouldReturnSuccess_WhenChecklistIsValid()
    {
        // Arrange
        var dto = new ChecklistDto { PlacaVeiculo = "ABC1234", Status = StatusEnum.EmExecucao, Itens = new List<ChecklistItemDto> { new ChecklistItemDto { Nome = "Item 1" } } };
        _checklistRepositoryMock.Setup(repo => repo.ExisteChecklistParaVeiculo(dto.PlacaVeiculo)).ReturnsAsync(false);
        _checklistRepositoryMock.Setup(repo => repo.AdicionarChecklist(It.IsAny<Checklist>())).ReturnsAsync(new Checklist {});

        // Act
        var result = await _checklistService.CriarChecklist(dto);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task AdicionarItemAoChecklist_ShouldReturnError_WhenChecklistNotFound()
    {
        // Arrange
        var checklistId = 1;
        var itemDto = new ChecklistItemDto();
        _checklistRepositoryMock.Setup(repo => repo.ObterChecklistPorId(checklistId)).ReturnsAsync((Checklist)null);

        // Act
        var result = await _checklistService.AdicionarItemAoChecklist(checklistId, itemDto, "executorId");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Checklist não encontrado.", result.Message);
    }

    [Fact]
    public async Task ObterChecklist_ShouldReturnError_WhenChecklistNotFound()
    {
        // Arrange
        var checklistId = 1;
        _checklistRepositoryMock.Setup(repo => repo.ObterChecklistPorId(checklistId)).ReturnsAsync((Checklist)null);

        // Act
        var result = await _checklistService.ObterChecklist(checklistId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Checklist não encontrado.", result.Message);
    }

    [Fact]
    public async Task AprovarChecklist_ShouldReturnError_WhenChecklistNotFound()
    {
        // Arrange
        var checklistId = 1;
        _checklistRepositoryMock.Setup(repo => repo.ObterChecklistPorId(checklistId)).ReturnsAsync((Checklist)null);

        // Act
        var result = await _checklistService.AprovarChecklist(checklistId, "supervisorId");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Checklist não encontrado.", result.Message);
    }
}
