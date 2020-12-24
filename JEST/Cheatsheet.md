* A suite of tests is an application which checks your appliication.
* Composed of assertions about how your code will execute.
* Test files are commited to the repo with application code.
* Suite is run quickly and routinely by CI tools.
* Unit test: a single function or service (Mocha / Jest).
    - You can write them before the actual code (TDD).
    
* Component test: a single component (functionality, Jest / Enzyme).
* Snapshot test: a single component (regression), Jest. It protects a component from regression.
* End-to-End Test: interaction between multiple components (Cypress).
