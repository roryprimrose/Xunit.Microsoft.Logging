﻿namespace Divergic.Logging.Xunit.UnitTests
{
    using System;
    using FluentAssertions;
    using global::Xunit;
    using global::Xunit.Abstractions;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using NSubstitute;

    public class LoggingBuilderExtensionsTests
    {
        private readonly ITestOutputHelper _output;

        public LoggingBuilderExtensionsTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void AddXunitAddsProviderToBuilder()
        {
            var services = Substitute.For<IServiceCollection>();
            var builder = Substitute.For<ILoggingBuilder>();

            builder.Services.Returns(services);

            builder.AddXunit(_output);

            services.Received().Add(Arg.Is<ServiceDescriptor>(x => x.ServiceType == typeof(ILoggerProvider)));
            services.Received()
                .Add(Arg.Is<ServiceDescriptor>(x => x.ImplementationInstance is TestOutputLoggerProvider));
        }

        [Fact]
        public void AddXunitAddsProviderWithFormatterToBuilder()
        {
            var services = Substitute.For<IServiceCollection>();
            var builder = Substitute.For<ILoggingBuilder>();

            builder.Services.Returns(services);

            builder.AddXunit(_output, Formatters.MyCustomFormatter);

            services.Received().Add(Arg.Is<ServiceDescriptor>(x => x.ServiceType == typeof(ILoggerProvider)));
            services.Received()
                .Add(Arg.Is<ServiceDescriptor>(x => x.ImplementationInstance is TestOutputLoggerProvider));
        }

        [Fact]
        public void AddXunitThrowsExceptionWithNullBuilder()
        {
            ILoggingBuilder builder = null;

            Action action = () => builder.AddXunit(_output);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AddXunitThrowsExceptionWithNullOutput()
        {
            var builder = Substitute.For<ILoggingBuilder>();

            Action action = () => builder.AddXunit(null);

            action.Should().Throw<ArgumentNullException>();
        }
    }
}