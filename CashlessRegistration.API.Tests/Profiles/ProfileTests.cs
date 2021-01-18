using AutoMapper;
using CashlessRegistration.API.Profiles;
using Xunit;

namespace CashlessRegistration.API.Tests.Profiles
{
    public class ProfileTests
    {
        [Fact]
        public void CardProfile_WhenConfigured_ShouldAssertConfigurationIsValid()
        {
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<CardProfile>());

            mapperConfiguration.AssertConfigurationIsValid<CardProfile>();
        }

        [Fact]
        public void AuthenticationProfile_WhenConfigured_ShouldAssertConfigurationIsValid()
        {
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<AuthenticationProfile>());

            mapperConfiguration.AssertConfigurationIsValid<AuthenticationProfile>();
        }
    }
}
