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
        * The mock is automatically substituted in for any tests!
        * Create the __mocks__ directory in the same level of the node_modules.
        * If a file in your __mocks__ folder that is directly adjacent to the npm modules has the exact same name as an npm module, it will be loades instead of the whole module when you require it in your tests. 
        ```js
        //fetch-question-saga.js
        import { takeEvery, put } from 'redux-saga/effects'
        import fetch from 'isomorphic-fetch';

        export default function * () {
            /**
            * Every time REQUEST_FETCH_QUESTION, fork a handleFetchQuestion process for it
            */
            yield takeEvery(`REQUEST_FETCH_QUESTION`,handleFetchQuestion);
        }

        /**
        * Fetch question details from the local proxy API
        */
        export function * handleFetchQuestion ({question_id}) {
            const raw = yield fetch(`/api/questions/${question_id}`);
            const json = yield raw.json();
            const question = json.items[0];
            /**
            * Notify application that question has been fetched
            */
            yield put({type:`FETCHED_QUESTION`,question});
        }

        //isomorphic-fetch.js (mock)
        let __value = 42;
        const isomorphicFetch = jest.fn(()=> __value); //creates a spy function that returns __value
        isomorphicFetch.__setValue = v=> __value = v; //equals to a method that changes this value property thats here on the global scope of this module
        export default isomorphicFetch;

        //fetch-question-saga.spec.js
        import {handleFetchQuestion} from './fetch-question-saga';
        import fetch from 'isomorphic-fetch'

        describe("Fetch questions saga", ()=>{

            beforeAll(()=> {
                fetch.__setValue([{question_id : 42}]);
            })

            it("should fetch the questions", async ()=> {
                const gen = handleFetchQuestion({question_id : 42});
                const  {value} = await gen.next();

                expect(value).toEqual([{question_id : 42}]);
                expect(fetch).toHaveBeenCalledWith('/api/questions/42');
            })
        });

        ```
        * A snapshot is a JSON file with a record of a component's output.
        * Commited along with other modules and tests to the application repo.
        * The first time it just creates the snapshot, the second one it compares it with previous snapshots.
        ```js
        import React from 'react';
        import TagsList from './TagsList';
        import renderer from 'react-test-renderer';

        describe("Teh tags list", () => {
            it("renders as expected", () => {
                const tree = renderer
                    .create(<TagsList tags={['css', 'html', 'go']}/>)
                    .toJSON();

                console.log(tree);
                expect(tree).toMatchSnapshot();
            })
        });
        ```
        * To update the snapshot with --update flag (after checking is not a regression of course).
            jest TagsList -u
