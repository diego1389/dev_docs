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

