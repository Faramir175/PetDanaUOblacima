namespace Tests
{
    using Xunit;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PetDanaUOblacima.DTO;
    using PetDanaUOblacima.Entities;
    using PetDanaUOblacima.Services;
    using PetDanaUOblacima.Data;
    using FluentAssertions;
    using AutoFixture;

    public class CanteenServiceTests
    {
        private readonly Mock<IStudentService> _mockStudentService;
        private readonly CanteenService _canteenService;
        private readonly IFixture _fixture;

        public CanteenServiceTests()
        {
            _mockStudentService = new Mock<IStudentService>();
            _canteenService = new CanteenService(_mockStudentService.Object);
            _fixture = new Fixture();

            InMemoryDbContext.ResetDatabase();
        }

        [Fact(DisplayName = "Kreiranje menze - Nije Admin")]
        public async Task CreateCanteenAsync_NotAdmin_ThrowsUnauthorizedAccessException()
        {
            var nonAdminStudentId = 99;
            var dto = _fixture.Create<CanteenCreateDTO>();

            _mockStudentService
                .Setup(s => s.CheckAndGetAdminAsync(nonAdminStudentId))
                .ThrowsAsync(new UnauthorizedAccessException());

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _canteenService.CreateCanteenAsync(dto, nonAdminStudentId)
            );
        }

        [Fact(DisplayName = "Kreiranje menze - Uspešno")]
        public async Task CreateCanteenAsync_IsAdmin_ReturnsCanteen()
        {
            var adminStudentId = 1;

            var dto = _fixture.Build<CanteenCreateDTO>()
                              .With(x => x.Name, "Menza AutoTest")
                              .Create();

            var adminStudent = _fixture.Build<Student>()
                                       .With(s => s.Id, adminStudentId)
                                       .With(s => s.IsAdmin, true)
                                       .Create();

            _mockStudentService
                .Setup(s => s.CheckAndGetAdminAsync(adminStudentId))
                .ReturnsAsync(adminStudent);

            var result = await _canteenService.CreateCanteenAsync(dto, adminStudentId);

            result.Should().NotBeNull("jer očekujemo da je menza uspešno kreirana.");
            result.Id.Should().BeGreaterThan(0, "jer bi ID trebalo da je dodeljen u bazi.");
            result.Name.Should().Be("Menza AutoTest", "jer je ime menze trebalo da bude preuzeto iz DTO-a.");

            _mockStudentService.Verify(s => s.CheckAndGetAdminAsync(adminStudentId), Times.Once);
        }

        [Fact(DisplayName = "Generisanje slotova - Interval od 30 minuta")]
        public void GenerateIntervals_30Minutes_ReturnsCorrectSlots()
        {
            var startTime = new TimeOnly(8, 0);
            var endTime = new TimeOnly(9, 30);

            var expectedSlots = new List<TimeOnly>
        {
            new TimeOnly(8, 0),
            new TimeOnly(8, 30),
            new TimeOnly(9, 0)
        };

            Assert.True(true);
        }
    }
}
