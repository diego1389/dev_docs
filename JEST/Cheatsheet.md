* A suite of tests is an application which checks your appliication.
* Composed of assertions about how your code will execute.
* Test files are commited to the repo with application code.
* Suite is run quickly and routinely by CI tools.
* Unit test: a single function or service (Mocha / Jest).
    - You can write them before the actual code (TDD).
    
* Component test: a single component (functionality, Jest / Enzyme).
 * Great defense against regression.
 * Does not verify interactions between two components.
* Snapshot test: a single component (regression), Jest. It protects a component from regression.
 * A subtype of a component test.
 * Generated automatically.
 * Verifies output matches a past record.
* End-to-End Test: interaction between multiple components (Cypress).
 * Measures the functionaloty of the whole applications.
 * virtual or headless browser.
* Peformance tests (not JEST). 
 * Idenfity bottlenecks.
* Coverage Tests
 * Tests for your tests.
 * Measures application code whi9ch is visited but not verified during tests. 

## Jest
 * Javascript library.
 * Test runners but with handy extra features (Jasmine is also of test runner).
 * Enzyme works with Jest.
 * Jasmine / Mocha (Jest is built on top of). Organize tests into groups "describe" and "it" blocks. It checks all the assertions inside tests are verified whenever the test-runner is invoked.
 *  Jests adds snapshot testing, mocking and other feature sto mocha/jasmine.
 * Includes superiors assertion library and CLI.
 * Works with or without React. 
 * Jest has spies (mock functions) and snapshot testings and moduce mocking that Jest has but Mocha doesnt. 
 * Jest version has to match Jest CLI version. 
 * To get the latest version:
    npm install -g jest-cli
    npm install --save jest@version

* To execute the tests type:
    jest

* All files inside _tests_ are considered tests, as well as *.spec.js or *.tests.js files.
* Jest globals (describe and it).
    * "It" (test) method that you pass a function to, that function is executed as a block of tests by the test runner.
    * "Describe" (suite) optional method for grouping any number of "it" statements.
    ```js
    describe('The Question Detail Component', ()=>{
        it('Should not regress', () => {
             expect(2+2).toEqual(4);
        });
    });
    ```
* In **Watch mode** tests are run automatically as files change.
    * Only tests pertaining to changed files are run.
    jest --watch
* **BeforeEach** runs a block of code before each test (setting up db, mock instances, etc).
* **BeforeAll** runs code just once, before the first test.
* **AfterEach** opposite of BeforeEach
* **AfterAll** end of the last test (closing open connections).

    ```js
    describe("The question list", ()=>{
        beforeEach(()=> {
            console.log("Before each!");
        });

        beforeAll(()=> {
            console.log("Before all!");
        });

        it("should display a list of items", () => {
            expect(2+2).toEqual(4);
        })

        it("should be a different test", () => {
            expect(2+2).toEqual(4);
        })

        afterAll(()=>{
            console.log("after all this time? Always.");
        });

        afterEach(()=>{
            console.log("after each");
        });
    });
    ```

    * Mark a test as skip to skip it from jest running it. 
        ```js
        describe("The question list", ()=>{
            it("should display a list of items", () => {
                expect(2+2).toEqual(4);
            })

            it.skip("should be a different test", () => {
                expect(2+2).toEqual(5);
            })
        });
        ```
    * Only to indicate to run only this test.
        ```js
        describe("The question list", ()=>{
        it.only("should display a list of items", () => {
            expect(2+2).toEqual(4);
        })

        it("should be a different test", () => {
            expect(2+2).toEqual(5);
        })
    });
        ```
    * Asynchronous tests in different ways:
        * invoke the done() callback that is passed to the test.
        * Return a promise from a test
        * Pass an async function

        ```js
        import delay from 'redux-saga';

        it("async test 1", done =>{
            setTimeout(done, 100);
        });

        it ("async test 2", () =>
        {
            return new Promise(resolve => setTimeout(resolve, 1555));
        });

        it("async test 3", async() =>{
            await delay(100);
        });
        ```
    * To execute only the tests of the App spec:
        jest app //a regex after the jest.
    * Mocks:
        * Reduce dependencies required by tests
        * Prevent side-effects during testing.
        * A convincing duplicate of an object with no internal workings.
        * Has a same API as original, but no side-effects.
        * Spy methods with the same names.
        * Mock functions:
            * Spies.
            * No side-effects.
            * Counts function calls.
        * Appropiately named npm mocks are loaded automatically.
        * Mocks must reside in a __mocks__ folder next to mocked module.
        * NPM modules and local modules can both be mocked.
        * 