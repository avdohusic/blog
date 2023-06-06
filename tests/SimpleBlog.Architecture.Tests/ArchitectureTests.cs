namespace Architecture.Tests;

public class ArchitectureTests
{
    private const string DomainNamespace = "SimpleBlog.Domain";
    private const string ApplicationNamespace = "SimpleBlog.Application";
    private const string InfrastructureNamespace = "SimpleBlog.Infrastructure";
    private const string ApiNamespace = "SimpleBlog.Api";

    [Fact]
    public void Domain_ShouldNot_HaveDependenciesOnOtherProjects()
    {
        var assembly = typeof(SimpleBlog.Domain.DomainAssembly).Assembly;

        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(ApplicationNamespace, InfrastructureNamespace, ApiNamespace)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Application_ShouldNot_HaveDependenciesOnOtherProjects()
    {
        var assembly = typeof(SimpleBlog.Application.ApplicationAssembly).Assembly;

        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(InfrastructureNamespace, ApiNamespace)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Handlers_Should_HaveDependencyOnDomain()
    {
        var assembly = typeof(SimpleBlog.Application.ApplicationAssembly).Assembly;

        var result = Types
            .InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Handler")
            .Should()
            .HaveDependencyOn(DomainNamespace)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Infrastructure_ShouldNot_HaveDependenciesOnApiProject()
    {
        var assembly = typeof(SimpleBlog.Infrastructure.InfrastructureAssembly).Assembly;

        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(ApiNamespace)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void ApiControllers_Should_HaveDependenciesOnMediatR()
    {
        var assembly = typeof(SimpleBlog.Api.ApiAssembly).Assembly;

        var result = Types
            .InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Controller")
            .And()
            .DoNotHaveName("BaseController")
            .Should()
            .HaveDependencyOn("MediatR")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Api_ShouldNot_HaveDependenciesOnDbContext()
    {
        var assembly = typeof(SimpleBlog.Api.ApiAssembly).Assembly;

        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOn("SimpleBlog.Infrastructure.DbContexts")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Api_ShouldNot_HaveDependenciesOnRepositories()
    {
        var assembly = typeof(SimpleBlog.Api.ApiAssembly).Assembly;

        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny("SimpleBlog.Infrastructure.Repositories",
                                 "SimpleBlog.Domain.Repositories")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
