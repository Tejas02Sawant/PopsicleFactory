using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PopsicleFactory.DataProvider.Models;
using PopsicleFactory.WebApi.Controllers.V1;
using PopsicleFactory.WebApi.Models;
using PopsicleFactory.WebApi.Services;

namespace PopsicleFactory.UnitTest;

public class PopsicleControllerTests
{
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IPopsicleService> _mockService;
    private readonly PopsicleController _controller;

    private static Popsicle popsicle1 = new Popsicle { Id = Guid.NewGuid(), Flavor = "Strawberry", Color = "Red", Quantity = 50, Price = 2 };
    private static Popsicle popsicle2 = new Popsicle { Id = Guid.NewGuid(), Flavor = "Blue Raspberry", Color = "Blue", Quantity = 30, Price = 2.49m };
    private static Popsicle popsicle3 = new Popsicle { Id = Guid.NewGuid(), Flavor = "Lime", Color = "Green", Quantity = 20, Price = 1.49m };
    private static Popsicle popsicle4 = new Popsicle { Id = Guid.NewGuid(), Flavor = "Mango", Color = "Orange", Quantity = 40, Price = 3 };

    private readonly IList<Popsicle> popsicles = [popsicle1, popsicle2, popsicle3, popsicle4];

    private static PopsicleReadDto popsicle1ReadDto = new PopsicleReadDto
    {
        Id = popsicle1.Id,
        Flavor = popsicle1.Flavor,
        Color = popsicle1.Color,
        Quantity = popsicle1.Quantity,
        Price = popsicle1.Price
    };

    private static PopsicleReadDto popsicle2ReadDto = new PopsicleReadDto
    {
        Id = popsicle1.Id,
        Flavor = popsicle1.Flavor,
        Color = popsicle1.Color,
        Quantity = popsicle1.Quantity,
        Price = popsicle1.Price
    };

    private static PopsicleReadDto popsicle3ReadDto = new PopsicleReadDto
    {
        Id = popsicle1.Id,
        Flavor = popsicle1.Flavor,
        Color = popsicle1.Color,
        Quantity = popsicle1.Quantity,
        Price = popsicle1.Price
    };

    private static PopsicleReadDto popsicle4ReadDto = new PopsicleReadDto
    {
        Id = popsicle1.Id,
        Flavor = popsicle1.Flavor,
        Color = popsicle1.Color,
        Quantity = popsicle1.Quantity,
        Price = popsicle1.Price
    };

    private static IList<PopsicleReadDto> popsicleReadDtos = [popsicle1ReadDto, popsicle1ReadDto, popsicle1ReadDto, popsicle1ReadDto];

    public PopsicleControllerTests()
    {
        _mockService = new Mock<IPopsicleService>();
        _mapper = new Mock<IMapper>();

        _controller = new PopsicleController(_mockService.Object, _mapper.Object);
    }

    [Fact]
    public async void GetAll_ReturnsOk_WithListOfPopsicles()
    {
        _mockService.Setup(s => s.GetAllPopsicles()).ReturnsAsync(popsicles);
        _mapper.Setup(m => m.Map<IEnumerable<PopsicleReadDto>>(popsicles)).Returns(popsicleReadDtos);
        var result = await _controller.GetAllPopsicles();

        Assert.IsType<ActionResult<IEnumerable<PopsicleReadDto>>>(result);
        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        var resultValue = okResult.Value as IEnumerable<PopsicleReadDto>;
        Assert.NotNull(resultValue);
        Assert.Equal(popsicles.Count, resultValue.Count());
    }

    [Fact]
    public async void GetAll_ReturnsOk_WithEmptyListOfPopsicles()
    {
        _mockService.Setup(s => s.GetAllPopsicles()).ReturnsAsync(new List<Popsicle>());
        _mapper.Setup(m => m.Map<IEnumerable<PopsicleReadDto>>(It.IsAny<IEnumerable<Popsicle>>())).Returns(new List<PopsicleReadDto>());
        var result = await _controller.GetAllPopsicles();

        Assert.IsType<ActionResult<IEnumerable<PopsicleReadDto>>>(result);
        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        var resultValue = okResult.Value as IEnumerable<PopsicleReadDto>;
        Assert.NotNull(resultValue);
        Assert.Empty(resultValue);
    }

    [Fact]
    public async void Get_WithExistingId_ReturnsOk()
    {
        _mockService.Setup(s => s.GetPopsicleById(popsicle1.Id)).ReturnsAsync(popsicle1);
        _mapper.Setup(m => m.Map<PopsicleReadDto>(popsicle1)).Returns(popsicle1ReadDto);

        var result = await _controller.Get(popsicle1.Id);

        Assert.IsType<ActionResult<PopsicleReadDto>>(result);
        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        Assert.Equal(popsicle1ReadDto, okResult.Value);
    }

    [Fact]
    public async void Get_WithExistingId_ReturnsNotFound()
    {
        var result = await _controller.Get(Guid.NewGuid());

        Assert.IsType<ActionResult<PopsicleReadDto>>(result);
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.NotNull(notFoundResult);
        Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
    }

    [Fact]
    public async void Create_ValidPopsicle_ReturnsCreatedAtAction()
    {
        var id = Guid.NewGuid();
        var newPopsicle = new Popsicle { Id = id, Flavor = "Kiwi", Color = "Green", Price = 2.49m };
        var popsicleCreateDto = new PopsicleCreateDto { Flavor = "Kiwi", Color = "Green", Price = 2.49m };
        _mockService.Setup(s => s.CreatePopsicle(newPopsicle)).ReturnsAsync(newPopsicle);
        _mapper.Setup(m => m.Map<Popsicle>(popsicleCreateDto)).Returns(newPopsicle);

        var result = await _controller.CreatePopsicle(popsicleCreateDto);

        Assert.IsType<ActionResult<PopsicleReadDto>>(result);
        var createdResult = result.Result as CreatedAtActionResult;
        Assert.NotNull(createdResult);
        Assert.Equal((int)HttpStatusCode.Created, createdResult.StatusCode);
        Assert.Equal(newPopsicle, createdResult.Value);
        _mockService.Verify(x => x.CreatePopsicle(It.IsAny<Popsicle>()), Times.Once);
    }

    [Fact]
    public async Task Update_ExistingData_ReturnsOk()
    {
        var update = new Popsicle { Flavor = "Watermelon", Color = "Green", Price = 2.49m };
        var popsicleCreateDto = new PopsicleCreateDto { Flavor = "Watermelon", Color = "Green", Price = 2.49m };

        _mapper.Setup(m => m.Map<Popsicle>(popsicleCreateDto)).Returns(update);
        _mockService.Setup(s => s.UpdateInformation(popsicle1.Id, update)).ReturnsAsync(update);
        _mapper.Setup(m => m.Map<PopsicleReadDto>(update)).Returns(popsicle1ReadDto);

        var result = await _controller.Update(popsicle1.Id, popsicleCreateDto);

        Assert.IsType<ActionResult<PopsicleReadDto>>(result);
        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        Assert.Equal(popsicle1ReadDto, okResult.Value);
    }

    [Fact]
    public async Task Update_ExistingData_ReturnsNotFound()
    {
        var update = new Popsicle { Flavor = "Watermelon", Color = "Green", Price = 2.49m };
        var popsicleCreateDto = new PopsicleCreateDto { Flavor = "Watermelon", Color = "Green", Price = 2.49m };
        _mapper.Setup(m => m.Map<Popsicle>(popsicleCreateDto)).Returns(update);

        var result = await _controller.Update(Guid.NewGuid(), popsicleCreateDto);

        Assert.IsType<ActionResult<PopsicleReadDto>>(result);
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.NotNull(notFoundResult);
        Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task PartialUpdate_ExistingData_ReturnsOk()
    {
        var serializePopsicle = "{ \"Flavor\": \"Apple\" }";
        var updatedElement = JsonSerializer.Deserialize<JsonElement>(serializePopsicle);
        var updatedReadDto = popsicle2ReadDto;
        updatedReadDto.Flavor = "Apple";

        var update = new Popsicle { Flavor = "Apple", Color = popsicle2.Color, Price = popsicle2.Price };

        _mockService.Setup(s => s.PartialUpdate(popsicle1.Id, updatedElement)).ReturnsAsync(update);
        _mapper.Setup(m => m.Map<PopsicleReadDto>(update)).Returns(updatedReadDto);

        var result = await _controller.PartialUpdate(popsicle1.Id, updatedElement);

        Assert.IsType<ActionResult<PopsicleReadDto>>(result);
        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        Assert.Equal(updatedReadDto, okResult.Value);
    }

    [Fact]
    public async Task PartialUpdate_ExistingData_ReturnsNotFound()
    {
        var serializePopsicle = "{ \"Flavor\": \"Orange\" }";
        var updatedElement = JsonSerializer.Deserialize<JsonElement>(serializePopsicle);
        var update = new Popsicle { Flavor = "Orange", Color = "Orange", Price = 2.49m };
        var updatedReadDto = popsicle3ReadDto;
        updatedReadDto.Flavor = "Orange";

        _mapper.Setup(m => m.Map<PopsicleReadDto>(update)).Returns(updatedReadDto);

        var result = await _controller.PartialUpdate(Guid.NewGuid(), updatedElement);

        Assert.IsType<ActionResult<PopsicleReadDto>>(result);
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.NotNull(notFoundResult);
        Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task DeletePopsicle_ReturnsSuccessWithNoContent()
    {
        _mockService.Setup(s => s.DeletePopsicle(popsicle3.Id)).ReturnsAsync(true);

        var result = await _controller.DeletePopsicle(popsicle3.Id);

        Assert.IsType<NoContentResult>(result);
        var successWithNoContent = result as NoContentResult;
        Assert.NotNull(successWithNoContent);
        Assert.Equal((int)HttpStatusCode.NoContent, successWithNoContent.StatusCode);
    }

    [Fact]
    public async Task DeletePopsicle_ReturnsNotFound()
    {
        _mockService.Setup(s => s.DeletePopsicle(Guid.NewGuid())).ReturnsAsync(false);

        var result = await _controller.DeletePopsicle(Guid.NewGuid());

        Assert.IsType<NotFoundObjectResult>(result);
        var notFoundResult = result as NotFoundObjectResult;
        Assert.NotNull(notFoundResult);
        Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task SearchPopsicle_ReturnOk()
    {
        _mockService.Setup(s => s.Search(popsicle1.Flavor, null, null)).ReturnsAsync(popsicles);
        _mapper.Setup(m => m.Map<IEnumerable<PopsicleReadDto>>(popsicles)).Returns(popsicleReadDtos);
        var result = await _controller.Search(popsicle1.Flavor, null, null);

        Assert.IsType<ActionResult<IEnumerable<PopsicleReadDto>>>(result);
        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        var resultValue = okResult.Value as IEnumerable<PopsicleReadDto>;
        Assert.NotNull(resultValue);
        Assert.Equal(popsicles.Count, resultValue.Count());
    }

    [Fact]
    public async void SearchPopsicles_ReturnsOk_WithEmptyListOfPopsicles()
    {
        _mockService.Setup(s => s.Search(null, null, null)).ReturnsAsync(popsicles);
        _mapper.Setup(m => m.Map<IEnumerable<PopsicleReadDto>>(It.IsAny<IEnumerable<Popsicle>>())).Returns(new List<PopsicleReadDto>());
        var result = await _controller.Search(null, null, null);

        Assert.IsType<ActionResult<IEnumerable<PopsicleReadDto>>>(result);
        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        var resultValue = okResult.Value as IEnumerable<PopsicleReadDto>;
        Assert.NotNull(resultValue);
        Assert.Empty(resultValue);
    }
}
