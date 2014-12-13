using System;
using Microsoft.TeamFoundation.Client;
using Moq;
using Xunit;

namespace QBuild.Core.Tests.BuildServiceTest
{
    public class When_calling_build
    {
        [Fact]
        public void Should_throw_if_tfs_project_collection_is_not_provided()
        {
            var connectionMock = new Mock<TfsTeamProjectCollection>(
                new Uri("http://tfs.foobar.com"));

            IBuildService service = new BuildService(connectionMock.Object);

            IBuildRequest request = new BuildRequest();

            Assert.Throws<ArgumentException>(() => service.Build(request, response => { }));
        }

        [Fact]
        public void Should_throw_if_tfs_project_is_not_provided()
        {
            var connectionMock = new Mock<TfsTeamProjectCollection>(
                new Uri("http://tfs.foobar.com"));

            IBuildService service = new BuildService(connectionMock.Object);

            IBuildRequest request = new BuildRequest
            {
                ProjectName = "Project-Name"
            };

            Assert.Throws<ArgumentException>(() => service.Build(request, response => { }));
        }

        [Fact]
        public void Should_throw_if_build_definition_is_not_provided()
        {
            var connectionMock = new Mock<TfsTeamProjectCollection>(
                new Uri("http://tfs.foobar.com"));

            IBuildService service = new BuildService(connectionMock.Object);

            IBuildRequest request = new BuildRequest
            {
                ProjectCollection = "http://tfs.foobar.com",
                ProjectName = "Project-Name"
            };

            Assert.Throws<ArgumentException>(() => service.Build(request, response => { }));
        }

        [Fact]
        public void Should_throw_if_build_version_is_not_provided()
        {
            var connectionMock = new Mock<TfsTeamProjectCollection>(
                new Uri("http://tfs.foobar.com"));

            IBuildService service = new BuildService(connectionMock.Object);

            IBuildRequest request = new BuildRequest
            {
                ProjectCollection = "http://tfs.foobar.com",
                ProjectName = "Project-Name",
                BuildDefinition = "Build-Definition-Name"
            };

            Assert.Throws<ArgumentException>(() => service.Build(request, response => { }));
        }
    }
}
