using System;
using Xunit;
namespace GameEngine.Tests
{
    public class NonPlayerCharacterShould
    {
        [Theory]
        [HealthDamageData]
        public void TakeDamage(int damage, int expectedHealth)
        {
            PlayerCharacter sut = new PlayerCharacter();
            sut.TakeDamage(damage);
            Assert.Equal(expectedHealth, sut.Health);
        }
    }
}
