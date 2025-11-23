using FootballLeague.Data.Repository;
using FootballLeague.Domain.Models;
using FootballLeague.Services.DTOs;
using FootballLeague.Services.Services;
using FootballLeague.Services.Strategies;
using Moq;
using Match = FootballLeague.Domain.Models.Match;

namespace FootballLeague.UnitTests;

public class MatchServiceTests
{
    private readonly Mock<IMatchRepository> _mockMatchRepo;
    private readonly Mock<ITeamRepository> _mockTeamRepo;
    private readonly Mock<IScoringStrategy> _mockStrategy;
    private readonly MatchService _service;

    public MatchServiceTests()
    {
        _mockMatchRepo = new Mock<IMatchRepository>();
        _mockTeamRepo = new Mock<ITeamRepository>();
        _mockStrategy = new Mock<IScoringStrategy>();

        _service = new MatchService(
            _mockMatchRepo.Object,
            _mockTeamRepo.Object,
            _mockStrategy.Object
        );
    }

    [Fact]
    public async Task RegisterMatchAsync_ShouldReturnSuccess_WhenTeamsExist()
    {
        var home = Team.Create("Home").Value;
        var away = Team.Create("Away").Value;
        var outcome = new MatchOutcome(home.Id, away.Id, 2, 1);

        _mockTeamRepo.Setup(r => r.GetManyByIdsAsync(It.IsAny<List<Guid>>()))
            .ReturnsAsync([home, away]);

        var result = await _service.RegisterMatchAsync(outcome);

        Assert.True(result.IsSuccess);

        _mockStrategy.Verify(s => s.Calculate(home, away, 2, 1), Times.Once);
        _mockMatchRepo.Verify(r => r.Add(It.IsAny<Match>()), Times.Once);
        _mockMatchRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task RegisterMatchAsync_ShouldFail_WhenOneTeamIsMissing()
    {
        var home = Team.Create("Home").Value;
        var outcome = new MatchOutcome(home.Id, Guid.NewGuid(), 1, 0);

        _mockTeamRepo.Setup(r => r.GetManyByIdsAsync(It.IsAny<List<Guid>>()))
            .ReturnsAsync([home]);

        var result = await _service.RegisterMatchAsync(outcome);

        Assert.True(result.IsFailure);
        Assert.Equal("Error.NotFound", result.Error.Code);
        _mockMatchRepo.Verify(r => r.Add(It.IsAny<Match>()), Times.Never);
    }

    [Fact]
    public async Task DeleteMatchAsync_ShouldRevertPointsAndReturnSuccess()
    {
        var home = Team.Create("Home").Value;
        var away = Team.Create("Away").Value;

        var match = Match.Create(home.Id, away.Id, 3, 0).Value;

        typeof(Match).GetProperty(nameof(Match.HomeTeam))?.SetValue(match, home);
        typeof(Match).GetProperty(nameof(Match.AwayTeam))?.SetValue(match, away);

        _mockMatchRepo.Setup(r => r.GetByIdWithTeamsAsync(match.Id)).ReturnsAsync(match);

        var result = await _service.DeleteMatchAsync(match.Id);

        Assert.True(result.IsSuccess);

        _mockStrategy.Verify(s => s.RevertPoints(home, away, 3, 0), Times.Once);
        _mockMatchRepo.Verify(r => r.Delete(match), Times.Once);
        _mockMatchRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
