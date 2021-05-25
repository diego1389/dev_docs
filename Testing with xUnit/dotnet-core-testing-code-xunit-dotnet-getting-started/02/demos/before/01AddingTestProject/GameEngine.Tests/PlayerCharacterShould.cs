using System;
using Xunit;

namespace GameEngine.Tests
{
    public class PlayerCharacterShould
    {
        [Fact]
        public void BeInexperienceWhenNew()
        {
            PlayerCharacter sut = new PlayerCharacter();

            Assert.True(sut.IsNoob);
        }

        [Fact]
        public void CalculateFullName()
        {
            PlayerCharacter sut = new PlayerCharacter();
            sut.FirstName = "Erwin";
            sut.LastName = "Smith";

            Assert.Equal("Erwin Smith", sut.FullName);
        }


        [Fact]
        public void HaveFullnameStartingWithFirstName()
        {
            PlayerCharacter sut = new PlayerCharacter();
            sut.FirstName = "Erwin";
            sut.LastName = "Smith";

            Assert.StartsWith("Erwin", sut.FullName);
        }

        [Fact]
        public void CalculateFullNameIgnoreCase()
        {
            PlayerCharacter sut = new PlayerCharacter();
            sut.FirstName = "Erwin";
            sut.LastName = "Smith";

            Assert.Equal("erwin smith", sut.FullName, ignoreCase: true);
        }

        [Fact]
        public void CalculateFullNameMatchesTitleCase()
        {
            PlayerCharacter sut = new PlayerCharacter();
            sut.FirstName = "Erwin";
            sut.LastName = "Smith";

            Assert.Matches("[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+", sut.FullName);
        }

        [Fact]
        public void StartWithDefaultHealth()
        {
            PlayerCharacter sut = new PlayerCharacter();
            Assert.Equal(100, sut.Health);
        }

        [Fact]
        public void StartWithDefaultHealth_NonZero()
        {
            PlayerCharacter sut = new PlayerCharacter();
            Assert.NotEqual(0, sut.Health);
        }

        [Fact]
        public void IncreaseHealthAfterSpeeling()
        {
            PlayerCharacter sut = new PlayerCharacter();
            sut.Sleep();

            Assert.InRange(sut.Health, 101, 200);
        }

        [Fact]
        public void NotHaveNicknameByDefault()
        {
            PlayerCharacter sut = new PlayerCharacter();

            Assert.Null(sut.Nickname);
        }

        [Fact]
        public void HaveALongBow()
        {
            PlayerCharacter sut = new PlayerCharacter();

            Assert.Contains("Long Bow", sut.Weapons);
        }

        [Fact]
        public void NotHaveAStaffOfWonder()
        {
            PlayerCharacter sut = new PlayerCharacter();

            Assert.DoesNotContain("Staff of Wonder", sut.Weapons);
        }

        [Fact]
        public void ContainsOneTypeOfSword()
        {
            PlayerCharacter sut = new PlayerCharacter();

            Assert.Contains(sut.Weapons, weapon => weapon.Contains("Sword")); //collection, verify if matches predicate
        }

        [Fact]
        public void HaveAllExpectedWeapons()
        {
            PlayerCharacter sut = new PlayerCharacter();

            var expectedWeapons = new[]
            {
                "Long Bow",
                "Short Bow",
                "Short Sword",
            };

            Assert.Equal(expectedWeapons, sut.Weapons);
        }

        [Fact]
        public void HaveNoEmptyDefaultWeapons()
        {
            PlayerCharacter sut = new PlayerCharacter();

            Assert.All(sut.Weapons, weapon => Assert.False(string.IsNullOrEmpty(weapon)));
        }

        [Fact]
        public void RaiseSleptEvent()
        {
            PlayerCharacter sut = new PlayerCharacter();
            Assert.Raises<EventArgs>(
                handler => sut.PlayerSlept += handler, //attach to the event
                handler => sut.PlayerSlept -= handler,  //detach from the event
                () => sut.Sleep()
                );
        }

        [Fact]
        public void RaisePropertyChangedEvent()
        {
            PlayerCharacter sut = new PlayerCharacter();

            Assert.PropertyChanged(sut, "Health", () => sut.TakeDamage(10));/*PlayerCharacter has to implementINofityPropertyChanged interface and call OnPropertyChanged() on the properties set*/
        }

        [Fact]
        public void TakeZeroDamage()
        {
            PlayerCharacter sut = new PlayerCharacter();
            sut.TakeDamage(0);
            Assert.Equal(100, sut.Health);
        }

        [Theory]
        [InlineData(0,100)]
        [InlineData(1, 99)]
        [InlineData(50, 50)]
        [InlineData(101, 1)]
        public void TakeDamage(int damage, int expectedHealth)
        {
            PlayerCharacter sut = new PlayerCharacter();
            sut.TakeDamage(damage);
            Assert.Equal(expectedHealth, sut.Health);
        }
    }
}
