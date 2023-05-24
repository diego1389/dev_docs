## Getting started with xUnit

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
        public void IncreaseHealthAfterSleeping()
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
    * Makings asserts againts object types:
        * IsType validation is strict, meaning it will fail if you ask if BossEnemy is type enemy, even if it inherits from it. For a non-strict validation use IsAssignableFrom.
        ```cs
        [Fact]
        public void CreateNormalEnemy()
        {
            EnemyFactory sut = new EnemyFactory();
            Enemy enemy = sut.Create("Zombie");
            Assert.IsType<NormalEnemy>(enemy);
        }

        [Fact]
        public void CreateNormalEnemy_NotTypeExample()
        {
            EnemyFactory sut = new EnemyFactory();
            Enemy enemy = sut.Create("Zombie");
            Assert.IsNotType<DateTime>(enemy);
        }

        [Fact]
        public void CreateBossEnemy()
        {
            EnemyFactory sut = new EnemyFactory();
            Enemy enemy = sut.Create("Zombie King", true);
            Assert.IsType<BossEnemy>(enemy);
        }

        [Fact]
        public void CreateBossEnemy_NonStrict()
        {
            EnemyFactory sut = new EnemyFactory();
            Enemy enemy = sut.Create("Zombie King", true);
            Assert.IsAssignableFrom<Enemy>(enemy);
        }
        ```
    * Asserting object instances
        ```cs
        [Fact]
        public void CreateSeparateInstances()
        {
            EnemyFactory sut = new EnemyFactory();
            Enemy enemy1 = sut.Create("Zombie");
            Enemy enemy2 = sut.Create("Zombie");

            Assert.NotSame(enemy1, enemy2);
        }
        ```
    * Asserting that code throws exceptions. There is also a ThrowsAsync version for asynchoronous methods.
        ```cs
        [Fact]
        public void NotAllowNullName()
        {
            EnemyFactory sut = new EnemyFactory();

            //Assert.Throws<ArgumentNullException>(() => sut.Create(null));

            //You can also pass the argument that raises the exception
            Assert.Throws<ArgumentNullException>("name", () => sut.Create(null));
        }

        [Fact]
        public void OnlyAllowKingOrQueenBosses()
        {
            EnemyFactory sut = new EnemyFactory();

            EnemyCreationException ex = Assert.Throws<EnemyCreationException>(() => sut.Create("Zombie", true));

            Assert.Equal("Zombie", ex.RequestedEnemyName);
        }
        ```
    * Asserting that events are raised
        ```cs
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
        ```
    * Categorize tests through Trait attribute.
        ```cs
        [Fact]
        [Trait("Category", "Enemy")]
        public void HaveCorrectPower()
        {
            BossEnemy sut = new BossEnemy();

            Assert.Equal(166.667, sut.TotalSpecialAttackPower, 3);
        }
        ```
    * Trait attribute can also be at class level.
    * On the command line under the tests folder:
        ```bash
        dotnet tests --filter Category=Enemy
        dotnet tests --filter "Category=Enemy|Category=Boss"
        ```
    * To skip a test: 
        ```cs
        [Fact(Skip = "Don't need to run this")]
        public void OnlyAllowKingOrQueenBosses()
        {
            EnemyFactory sut = new EnemyFactory();

            EnemyCreationException ex = Assert.Throws<EnemyCreationException>(() => sut.Create("Zombie", true));

            Assert.Equal("Zombie", ex.RequestedEnemyName);
        }
        ```
    * Write additional info to the output console:
        ```cs
        using Xunit.Abstractions;

        namespace GameEngine.Tests
        {
            public class EnemyFactoryShould
            {
                private readonly ITestOutputHelper _output;

                public EnemyFactoryShould(ITestOutputHelper output)
                {
                    this._output = output;
                }

                [Fact]
                public void CreateNormalEnemy()
                {
                    EnemyFactory sut = new EnemyFactory();
                    _output.WriteLine("Creating an enemy and validating type");
                    Enemy enemy = sut.Create("Zombie");
                    Assert.IsType<NormalEnemy>(enemy);
                }
            }
        }
        ```
    *  Get additional info from tests in the command line (this creates an xml file on the folder called TestResults):
        ```bash
         dotnet tests --filter Category=Enemy --logger:trx
        ```
    * REduce code duplication (a global instances instead of one per method).
    * To share context between test methods because instance creation is expensive.
    * Create new class GameStateFixture:
        ```cs
        public class GameStateFixture : IDisposable
        {
            public GameState State { get; private set; }

            public GameStateFixture()
            {
                State = new GameState();
            }

            public void Dispose()
            {
                // Cleanup
            }
        }
        ```
    * Implement IClassFixture interface, implementing it we tell xUnit we want to supply an instance of the GameStateFixture before the first test executes and then dispose after all tests are executed.
    * We get the same instance injected for every method.
    * Be aware the actions performed under the shared instance don't have side effects.
        ```cs
        using Xunit;
        using Xunit.Abstractions;

        namespace GameEngine.Tests
        {
            public class GameStateShould : IClassFixture<GameStateFixture>
            {
                private readonly GameStateFixture _gameStateFixture;
                private readonly ITestOutputHelper _output;

                public GameStateShould(GameStateFixture gameStateFixture,
                                    ITestOutputHelper output)
                {
                    _gameStateFixture = gameStateFixture;

                    _output = output;
                }

                [Fact]
                public void DamageAllPlayersWhenEarthquake()
                {
                    _output.WriteLine($"GameState ID={_gameStateFixture.State.Id}");

                    var player1 = new PlayerCharacter();
                    var player2 = new PlayerCharacter();

                    _gameStateFixture.State.Players.Add(player1);
                    _gameStateFixture.State.Players.Add(player2);

                    var expectedHealthAfterEarthquake = player1.Health - GameState.EarthquakeDamage;

                    _gameStateFixture.State.Earthquake();

                    Assert.Equal(expectedHealthAfterEarthquake, player1.Health);
                    Assert.Equal(expectedHealthAfterEarthquake, player2.Health);
                }

                [Fact]
                public void Reset()
                {
                    _output.WriteLine($"GameState ID={_gameStateFixture.State.Id}");

                    var player1 = new PlayerCharacter();
                    var player2 = new PlayerCharacter();

                    _gameStateFixture.State.Players.Add(player1);
                    _gameStateFixture.State.Players.Add(player2);

                    _gameStateFixture.State.Reset();

                    Assert.Empty(_gameStateFixture.State.Players);            
                }
            }
        }
        ```
    * To share the same instance across multiple classes:
        * Create a new class and implement ICollectionFixture<GameStateFixture>.
        ```cs
        using Xunit;

        namespace GameEngine.Tests
        {   
        [CollectionDefinition("GameState collection")] //collection name
        public class GameStateCollection : ICollectionFixture<GameStateFixture> {}
        }

        ```
    * Add collection attribute at class level of the tests classes (we dont need to implement any interface in the classes).
    ```cs
    using Xunit;
    using Xunit.Abstractions;

    namespace GameEngine.Tests
    {
        [Collection("GameState collection")]
        public class TestClass1
        {
            private readonly GameStateFixture _gameStateFixture;
            private readonly ITestOutputHelper _output;

            public TestClass1(GameStateFixture gameStateFixture, ITestOutputHelper output)
            {
                _gameStateFixture = gameStateFixture;

                _output = output;
            }

            [Fact]
            public void Test1()
            {
                _output.WriteLine($"GameState ID={_gameStateFixture.State.Id}");
            }

            [Fact]
            public void Test2()
            {
                _output.WriteLine($"GameState ID={_gameStateFixture.State.Id}");
            }
        }
    }
    ```
    * Data-driven tests: when you have to execute the same test method but with different data. Test case n.
    * Test data sources: inline attribute, property, field attribute, external data.
    *  [Theory] attribute means the test should be executed it multiple times. The test has to be provide it with some test data (that we pass as parameters). With inline we cannot share data across multiple test classes or tests.
        ```cs
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
        ```
    * To share test data create a new class.
        ```cs
             public class InternalHealthDamageTestData
        {
            /*private static readonly List<object[]> Data = new List<object[]>()
            {
                new object[]{0, 100},
                new object[]{1, 99},
                new object[]{50,50},
                new object[]{101, 1}
            };

            public static IEnumerable<object[]> TestData => Data;*/

            public static IEnumerable<object[]> TestData
            {
                get
                {
                    yield return new object[] { 0, 100 };
                    yield return new object[] { 1, 99 };
                    yield return new object[] { 50, 50 };
                    yield return new object[] { 101, 1 };
                }   
            }
        }
        ```
    * Now use it in the tests:
        ```cs
        [Theory]
        [MemberData(nameof(InternalHealthDamageTestData.TestData), MemberType = typeof(InternalHealthDamageTestData))]
        public void TakeDamage(int damage, int expectedHealth)
        {
            PlayerCharacter sut = new PlayerCharacter();
            sut.TakeDamage(damage);
            Assert.Equal(expectedHealth, sut.Health);
        }
        ```
    * We can use custom attributes to share test data accross tests.
    * Add new class:
        ```cs
        using System;
        using System.Collections.Generic;
        using System.Reflection;
        using Xunit.Sdk;

        namespace GameEngine.Tests
        {
            public class HealthDamageDataAttribute : DataAttribute
            {
                public override IEnumerable<object[]> GetData(MethodInfo testMethod)
                {
                    yield return new object[] { 0, 100 };
                    yield return new object[] { 1, 99 };
                    yield return new object[] { 50, 50 };
                    yield return new object[] { 101, 1 };
                }
            }
        }
        ```
    * Use the new custom attribute:
        ```cs
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
        ```
## Mocking with Moq and xUnit
* Replacing the actual dependency that would be used at production time, with a test-time only version to enable easier isolation of the code we want to test.
* Improve test execution speed.
* Improve test reliability.
* Support parallel development stream:
    - Real object not yet developed.
* Reduce development/testing costs.
* Unit tests: 
    * Low level testing.
    * Specific part of the system.
    * Quick to execute.
    * Test all the logical paths.
    * A unit doesnt need to test an individual class, it is a situational thing.
    * Units of behaviour over units of implementation.
    * Test double: generic term for replacing a production object for testing purposes.
        * Fake: working implementation of the dependency. EF COre in memory provider f.e.
        * Dummies: passed around never used / accessed. Satisfy parameters.
        * Stubs: provide answers to calls. Property gets. Method return values.
        * Mocks: expect / verify calls, properties, methods.
    * Add a new dependency to CreditCardApplicationEvaluator:
        ```cs
        public class CreditCardApplicationEvaluator
        {
            private readonly IFrequentFlyerNumberValidator _validator;
            private const int AutoReferralMaxAge = 20;
            private const int HighIncomeThreshold = 100_000;
            private const int LowIncomeThreshold = 20_000;


            public CreditCardApplicationEvaluator(IFrequentFlyerNumberValidator validator)
            {
                _validator = validator;
            }

            public CreditCardApplicationDecision Evaluate(CreditCardApplication application)
            {
                if (application.GrossAnnualIncome >= HighIncomeThreshold)
                {
                    return CreditCardApplicationDecision.AutoAccepted;
                }

                var isValidFrequentFlyerNumber = _validator.IsValid(application.FrequentFlyerNumber);

                if (application.Age <= AutoReferralMaxAge)
                {
                    return CreditCardApplicationDecision.ReferredToHuman;
                }

                if (application.GrossAnnualIncome < LowIncomeThreshold)
                {
                    return CreditCardApplicationDecision.AutoDeclined;
                }

                return CreditCardApplicationDecision.ReferredToHuman;
            }       
        }
        ```
    * Modify the tests to pass a mocked implementation:
        ```cs
        [Fact]
        public void ReferYoungApplications()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication() { Age = 19 };

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }
        ```
    * Configure mocks to return specific values (setup method):
        ```cs
        [Fact]
        public void DeclineLowIncomeApplications()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            mockValidator.Setup(x => x.IsValid("x")).Returns(true);
            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication() { GrossAnnualIncome = 19_999, Age = 42, FrequentFlyerNumber= "x" };

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.AutoDeclined, decision);
        }
        ```
    * YOu can use argument matching methods to not specify the parameters directly. 
        ```cs
        [Fact]
        public void DeclineLowIncomeApplications()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            //mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
            //mockValidator.Setup(x => x.IsValid(It.Is<string>(number => number.StartsWith("y")))).Returns(true);
            //mockValidator.Setup(x => x.IsValid(It.IsInRange<string>("a", "z", Moq.Range.Inclusive))).Returns(true);
            //mockValidator.Setup(x => x.IsValid(It.IsIn<string>("abx", "clin", "yay"))).Returns(true);
            mockValidator.Setup(x => x.IsValid(It.IsRegex("[a-z]"))).Returns(true);

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication() { GrossAnnualIncome = 19_999, Age = 42, FrequentFlyerNumber= "yay" };

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.AutoDeclined, decision);
        }
        ```
    * By default mocking is loose. Strict mocking throws an exception if a mocked method is called but has not been setup. Loose mocks use default values.
        ```cs
        [Fact]
        public void ReferInvalidFrequentFlyerApplications()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>(MockBehavior.Strict);
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(false);

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication();

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }
        ```
    * Mocking methods with out parameters.
    * Modify the credit card evaluator and add a new method, it uses the overloaded IsValid that uses an out paraemeter:
        ```cs
        public CreditCardApplicationDecision EvaluateUsingOut(CreditCardApplication application)
        {
            if (application.GrossAnnualIncome >= HighIncomeThreshold)
            {
                return CreditCardApplicationDecision.AutoAccepted;
            }

            _validator.IsValid(application.FrequentFlyerNumber,
                               out var isValidFrequentFlyerNumber);

            if (!isValidFrequentFlyerNumber)
            {
                return CreditCardApplicationDecision.ReferredToHuman;
            }

            if (application.Age <= AutoReferralMaxAge)
            {
                return CreditCardApplicationDecision.ReferredToHuman;
            }

            if (application.GrossAnnualIncome < LowIncomeThreshold)
            {
                return CreditCardApplicationDecision.AutoDeclined;
            }

            return CreditCardApplicationDecision.ReferredToHuman;
        }
        ```
    * Setup a mock method that has an out parameter:
        ```cs
        [Fact]
        public void DeclineLowIncomeApplicationsOutDemo()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            bool isValid = true;
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>(), out isValid));

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication()
            {
                GrossAnnualIncome = 19_000,
                Age = 42
            };

            CreditCardApplicationDecision decision = sut.EvaluateUsingOut(application);

            Assert.Equal(CreditCardApplicationDecision.AutoDeclined, decision);

        }
        ```
    * Matching attribute for reference types:
        ```cs
        var person1 = new Person();
        var person2 = new Person();
        var mockGateway = new Mock<IGateway>();
        mockGateway.Setup(x => x.Execute(ref It.Ref<Person>.IsAny)).Returns(-1);

        var sut = new Processor(mockGateway.Object);
        sut.Process(person1); //IGateway.Execute() returns -1
        sut.Process(person2); //IGateway.Execute() returns -1
        ```
    * Setup mock properties to return a specific value.
        ```cs
        [Fact]
        public void ReferWhenLicenseKeyExpired()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
            mockValidator.Setup(x => x.LicenseKey).Returns("EXPIRED"); /*You can pass a method to the Returns(). F.e GetLicenseExpiryString*/
            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var application = new CreditCardApplication { Age = 42 };
            CreditCardApplicationDecision decision = sut.Evaluate(application);
            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision); 
        }
        ```
    * Working with hierarchies:
        ```cs
        namespace CreditCardApplications
        {
            public interface ILicenseData
            {
                string LicenseKey { get; }
            }

            public interface IServiceInformation
            {
                ILicenseData License { get; }
            }

            public interface IFrequentFlyerNumberValidator
            {
                bool IsValid(string frequentFlyerNumber);
                void IsValid(string frequentFlyerNumber, out bool isValid);
                //string LicenseKey { get; }
                IServiceInformation ServiceInformation { get; }

            }
        }
        ```
    * Modify the tests to work with hierarchy:
        ```cs
        [Fact]
        public void ReferWhenLicenseKeyExpired()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            var mockLicenseData = new Mock<ILicenseData>();

            mockLicenseData.Setup(x => x.LicenseKey).Returns("EXPIRED");

            var mockServiceInfo = new Mock<IServiceInformation>();

            mockServiceInfo.Setup(x => x.License).Returns(mockLicenseData.Object);

            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
            mockValidator.Setup(x => x.ServiceInformation).Returns(mockServiceInfo.Object);

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var application = new CreditCardApplication { Age = 42 };
            CreditCardApplicationDecision decision = sut.Evaluate(application);
            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision); 
        }
        ```
    * Moq has a more succint way to do this:
        ```cs       
        [Fact]
        public void ReferWhenLicenseKeyExpired()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            /*var mockLicenseData = new Mock<ILicenseData>();

            mockLicenseData.Setup(x => x.LicenseKey).Returns("EXPIRED");

            var mockServiceInfo = new Mock<IServiceInformation>();

            mockServiceInfo.Setup(x => x.License).Returns(mockLicenseData.Object);


            mockValidator.Setup(x => x.ServiceInformation).Returns(mockServiceInfo.Object);*/

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("EXPIRED");
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var application = new CreditCardApplication { Age = 42 };
            CreditCardApplicationDecision decision = sut.Evaluate(application);
            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision); 
        }
        ```
    * Specify default value behaviour to avoid breaking the other tests. DefaultValue defines a mocked object instead of null as default.
        ```cs
        [Fact]
        public void ReferYoungApplications()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            mockValidator.DefaultValue = DefaultValue.Mock;

            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication { Age = 19 };

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }
        ```
    * Track changes made to the mock objects (when method changes a value dynamically). 
    * Add new property to the interface (an enum):
        ```cs
        public interface IFrequentFlyerNumberValidator
        {
            bool IsValid(string frequentFlyerNumber);
            void IsValid(string frequentFlyerNumber, out bool isValid);
            //string LicenseKey { get; }
            IServiceInformation ServiceInformation { get; }
            ValidationMode ValidationMode { get; set; }

        }
        //change in the evaluate method:
         _validator.ValidationMode = application.Age >= 30 ? ValidationMode.Detailed : ValidationMode.Quick;
        ```
    * Modify the tests to remember its changes:
        ```cs
        [Fact]
        public void UseDetailedLookupForOlderApplications()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            //mockValidator.Setup(x => x.ValidationMode);
            mockValidator.SetupAllProperties(); /*Before any specific setup otherwise it will override it*/
            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");


            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication { Age = 35 };

            sut.Evaluate(application);

            Assert.Equal(ValidationMode.Detailed, mockValidator.Object.ValidationMode);
        }
        ```
    * State based testing: Test passes mock to the constructor, SUT calls the Mock and returns a state/result to the test for it to assert it. 
    * In behaviour verification tests we create a mock and SUT interacts with the mock object but the test check the mock object (methods called or properties accessed).
        ```cs
        [Fact]
        public void ValidateFrequentFlyerNumberForLowIncomeApplications()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication();

            sut.Evaluate(application);

            mockValidator.Verify(x => x.IsValid(null));

        }
        ```
    * If we comment the IsValid method on Evaluate method it will fail.
        ```cs
        [Fact]
        public void ValidateFrequentFlyerNumberForLowIncomeApplications()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication(){ FrequentFlyerNumber = "q"};

            sut.Evaluate(application);

            mockValidator.Verify(x => x.IsValid(It.IsAny<string>()), Times.Exactly(1));

        }
        ```
    * You can use an overload of the Verify method to specify a custom message when test fails:
        ```cs
        mockValidator.Verify(x => x.IsValid(It.IsAny<string>()), "IsValid method not called");
        ```          
    * Verify if a method is not called:
        ```cs
        [Fact]
        public void ValidateFrequentFlyerNumberForHighIncomeApplications()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication() {GrossAnnualIncome = 100_000 };

            sut.Evaluate(application);

            mockValidator.Verify(x => x.IsValid(It.IsAny<string>()), Times.Never);
        }
        ```
    * Verify if a property's get method is called:
        ```cs
        [Fact]
        public void CheckLicenseKeyForLowIncomeApp()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication() { GrossAnnualIncome = 90_000 };

            sut.Evaluate(application);

            mockValidator.VerifyGet(x => x.ServiceInformation.License.LicenseKey);
        }
        ```
    * Verify that a property was set:
        ```cs
        [Fact]
        public void SetDetailedLookupForOlderApplications()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication() { Age = 30 };

            sut.Evaluate(application);

            mockValidator.VerifySet(x => x.ValidationMode = ValidationMode.Detailed);
        }
        ```
    * VerifyNoOtherCalls() we are telling moq than other than any verifications we already made there should be no other calls.
    * Modify the evaluate method:
        ```cs
         public CreditCardApplicationDecision Evaluate(CreditCardApplication application)
        {
            if (application.GrossAnnualIncome >= HighIncomeThreshold)
            {
                return CreditCardApplicationDecision.AutoAccepted;
            }

            if(_validator.ServiceInformation.License.LicenseKey == "EXPIRED")
            {
                return CreditCardApplicationDecision.ReferredToHuman;
            }

            _validator.ValidationMode = application.Age >= 30 ? ValidationMode.Detailed : ValidationMode.Quick;
            bool isValidFrequentFlyerNumber = false;
            try
            {
                isValidFrequentFlyerNumber =
                _validator.IsValid(application.FrequentFlyerNumber);
            }
            catch (System.Exception ex)
            {
                return CreditCardApplicationDecision.ReferredToHuman;
            }

            if (!isValidFrequentFlyerNumber)
            {
                return CreditCardApplicationDecision.ReferredToHuman;
            }

            if (application.Age <= AutoReferralMaxAge)
            {
                return CreditCardApplicationDecision.ReferredToHuman;
            }

            if (application.GrossAnnualIncome < LowIncomeThreshold)
            {
                return CreditCardApplicationDecision.AutoDeclined;
            }

            return CreditCardApplicationDecision.ReferredToHuman;
        }
        ```
    * Create a test method that simulates an exception when calling the IsValid method:
        ```cs
        [Fact]
        public void ReferWhenFrequentFlyerValidationError()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Throws<Exception>();
            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication() { Age = 42 };

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }
        ```
    * Automatic and manual ways to raise an event using Moq:
        ```cs
        [Fact]
        public void IncrementLookupCount()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>()))
                .Returns(true)
                .Raises(x => x.ValidatorLookupPerformed += null, EventArgs.Empty);
            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication() { FrequentFlyerNumber = "x", Age = 25 };

            sut.Evaluate(application);

            //mockValidator.Raise(x => x.ValidatorLookupPerformed += null, EventArgs.Empty);


            Assert.Equal(1, sut.ValidatorLookupCount);
        }
        ```
    - We can setup a method to return different results when it's called multiple times using SetupSequence and chain multiple return methods.
        ```cs
        [Fact]
        public void IncrementLookupCount()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>()))
                .Returns(true)
                .Raises(x => x.ValidatorLookupPerformed += null, EventArgs.Empty);
            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication() { FrequentFlyerNumber = "x", Age = 25 };

            sut.Evaluate(application);

            //mockValidator.Raise(x => x.ValidatorLookupPerformed += null, EventArgs.Empty);


            Assert.Equal(1, sut.ValidatorLookupCount);
        }
        ```
    - Check a method was called multiple times:
        ```cs
        [Fact]
        public void ReferWhenFrequentFlyer_MultipleCallsSequence()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            var frequenFlyerNumbersPassed = new List<string>();
            mockValidator.Setup(x => x.IsValid(Capture.In(frequenFlyerNumbersPassed)));

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application1 = new CreditCardApplication { Age = 25, FrequentFlyerNumber = "aa"};
            var application2 = new CreditCardApplication { Age = 25, FrequentFlyerNumber = "bb" };
            var application3 = new CreditCardApplication { Age = 25, FrequentFlyerNumber = "cc" };

            sut.Evaluate(application1);
            sut.Evaluate(application2);
            sut.Evaluate(application3);

            Assert.Equal(new List<string> { "aa", "bb", "cc" }, frequenFlyerNumbersPassed);
        }
        ```
    - In order to mock members of concrete classes the methods should be virtual.
        ```cs
        public class FraudLookup
        {
            public virtual bool IsFraudRisk(CreditCardApplication application)
            {
                if(application.LastName == "Smith")
                {
                    return true;
                }
                return false;
            }
            
        }
        /************************TEST**************************/
        [Fact]
        public void ReferFraudRisk()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            Mock<FraudLookup> mockFraudLookup = new Mock<FraudLookup>();

            mockFraudLookup.Setup(x => x.IsFraudRisk(It.IsAny<CreditCardApplication>())).Returns(true);


            var sut = new CreditCardApplicationEvaluator(mockValidator.Object, mockFraudLookup.Object);

            var application = new CreditCardApplication();

            CreditCardApplicationDecision decision = sut.Evaluate(application);


            Assert.Equal(CreditCardApplicationDecision.ReferToHumanFraudRisk, decision);
        }
        ```
    - MOcking virtual protected methods
        ```cs
        public bool IsFraudRisk(CreditCardApplication application)
        {
            return CheckApplication(application);
        }

        protected virtual bool CheckApplication(CreditCardApplication application)
        {
            if (application.LastName == "Smith")
            {
                return true;
            }
            return false;
        }

        /**************************TEST**********************/
        [Fact]
        public void ReferFraudRisk()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            Mock<FraudLookup> mockFraudLookup = new Mock<FraudLookup>();

            mockFraudLookup.Protected()
                .Setup<bool>("CheckApplication", ItExpr.IsAny<CreditCardApplication>()).Returns(true);


            var sut = new CreditCardApplicationEvaluator(mockValidator.Object, mockFraudLookup.Object);

            var application = new CreditCardApplication();

            CreditCardApplicationDecision decision = sut.Evaluate(application);


            Assert.Equal(CreditCardApplicationDecision.ReferToHumanFraudRisk, decision);
        }
        ```
    * Using Linq to Mocks
        ```cs
        [Fact]
        public void LinqToMoqs()
        {
            IFrequentFlyerNumberValidator mockValidator = Mock.Of<IFrequentFlyerNumberValidator>(
                validator =>
                validator.ServiceInformation.License.LicenseKey == "OK" &&
                validator.IsValid(It.IsAny<string>()) == true
            );

            var sut = new CreditCardApplicationEvaluator(mockValidator);

            var application = new CreditCardApplication() {Age = 25};

            var decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.AutoDeclined, decision);
        }
        ```
    - Mocking for generic type:
        ```cs
        mock.Setup(x=> x.IsOdd(It.IsAny<It.IsAnyType>())).Returns(true);
        //also
        mock.Setup(x=> x.IsOdd(It.IsAny<It.IsSubtype<ApplicationException>>())).Returns(true);

        ```
    - Mocking async methods:
        ```cs
        public interface IDemoInterfaceAsync{
            Task StartAsync();
            Task<int> StopAsync();
        }

        var mock = new Mock<IDemoInterfaceAsync>();
        mock.Setup(x => x.StartAsync()).Returns(Task.CompletedTask);
        mock.Setup(x => x.StopAsync()).Returns(Task.FromResult(42));
        //or
        mock.Setup(x=> x.StopAsync()).ReturnsAsync(42);
        ```