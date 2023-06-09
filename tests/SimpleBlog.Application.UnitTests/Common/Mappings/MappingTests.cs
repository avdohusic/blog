using AutoMapper;
using SimpleBlog.Application.Common.Mappings;
using SimpleBlog.Application.Dtos;
using SimpleBlog.Domain.Entities;
using SimpleBlog.Domain.Entities.Identity;
using System.Runtime.Serialization;

namespace Application.UnitTests.Common.Mappings;

public class MappingTests
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;

    public MappingTests()
    {
        _configuration = new MapperConfiguration(config =>
        {
            config.AddProfile<BlogProfile>();
            config.AddProfile<UserProfile>();
        });

        _mapper = _configuration.CreateMapper();
    }

    [Fact]
    public void ShouldHaveValidConfiguration()
    {
        _configuration.AssertConfigurationIsValid();
    }

    [Theory]
    [InlineData(typeof(Blog), typeof(BlogDto))]
    [InlineData(typeof(Blog), typeof(BlogExcelDto))]
    [InlineData(typeof(UserIdentity), typeof(UserDto))]
    public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
    {
        var instance = GetInstanceOf(source);

        _mapper.Map(instance, source, destination);
    }

    private object GetInstanceOf(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) != null)
            return Activator.CreateInstance(type)!;

        // Type without parameterless constructor
        return FormatterServices.GetUninitializedObject(type);
    }
}