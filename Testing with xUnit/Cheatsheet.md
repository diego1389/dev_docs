* Test code sits with production code.
* Categories:
    1. Unit test (more depth and less breadth).
    2. Integration (less depth and more breadth, db, services).
    3. Sucutaneous test.
    4. UI (greates breadth).
* Testing the behaviour of the class (the public interface of the class, public methods) rather than the internal implementation (private methods).
* If you decide to test private methods change it to internal instead of public.
* AssemblyInfo.cs: [assembly: InternalVisibleTo("GameEngine.Tests")].
* Three logical phases: Arrange (set things up), Act (execute production code, call methods, set properties) and Assert (check results).
```cs
public class PlayerCharacterShould{
    [Fact]
    public void IncreaseHealthAfterSleeping(){
        Player sut = new PlayerCharacter();//Arrange

        sut.Sleep();//Act
        
        Assert.InRange(sut.Health, 101, 200);//Assert
    }

    [Fact]
    public void AnotherTest(){
        ...
    }

    [Theory]
    public void ADataDrivenTest(){...}
}
```
* Use xUnit.net's Theory attribute to create data-driven tests.
* Add project -> XUnit Test Project.
* You can create a new xUnit Tests project using the command line.
* xunit.runner.visualstudio nuget to be able to run tests on TestExplorer (the xunit project adds it by default).
* dotnet test //on the test project directory this command run the tests
* Without asserts it always passes.
* One test can have more than one Assert as long as it is testing the same behaviour.
* Assert booleans:
    ```cs
    [Fact]
    public void BeInexperienceWhenNew()
    {
        PlayerCharacter sut = new PlayerCharacter();
        Assert.True(sut.IsNoob);
    }
    ```
* Making asserts against strings.
    ```cs
    [Fact]
    public void CalculateFullName()
    {
        layerCharacter sut = new PlayerCharacter();
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

        Assert.StartsWith("Sarah", sut.FullName);
    }

    /*Ignore case:*/
     [Fact]
    public void CalculateFullNameIgnoreCase()
    {
        PlayerCharacter sut = new PlayerCharacter();
        sut.FirstName = "Erwin";
        sut.LastName = "Smith";

        Assert.Equal("erwin smith", sut.FullName, ignoreCase : true);
    }
    
    /*Regular expression*/
    [Fact]
    public void CalculateFullNameMatchesTitleCase()
    {
        PlayerCharacter sut = new PlayerCharacter();
        sut.FirstName = "Erwin";
        sut.LastName = "Smith";

        Assert.Matches("[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+", sut.FullName);
    }
    ```

* Asserts with numeric values:
    ```cs
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
    ```
    * Testing on floating point values:
        ```cs
        [Fact]
        public void HaveCorrectPower()
        {
            BossEnemy sut = new BossEnemy();

            Assert.Equal(166.667, sut.TotalSpecialAttackPower, 3);
        }
        ```
    * Asserting null values:
        ```cs
        [Fact]
        public void NotHaveNicknameByDefault()
        {
            PlayerCharacter sut = new PlayerCharacter();

            Assert.Null(sut.Nickname); //There is also a NotNull assert
        }
        ```
    * Asserting with collections.
        ```cs
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
        ```