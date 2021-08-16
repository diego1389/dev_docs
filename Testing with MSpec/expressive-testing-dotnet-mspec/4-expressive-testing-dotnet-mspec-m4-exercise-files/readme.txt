This project contains all of the specification code that was written in module 4 of this course, including all specificatins written in previous modules.
The specifications project was added to the FrontDesk solution contained in the downloaded sample code from the "Domain-Driven Design Fundamentals" course by Steve Smith and Julie Lerman.
This course's specifications project was built against the module 7 exercise files from the DDD course.

To build and run the sample specifications in this folder:
 1) Download and unzip the module 7 exercise files from the "Domain-Driven Design Fundamentals" course on Pluralsight.
 2) Copy the AppointmentScheduling.Specs folder into the FrontDeskSolution folder of the DDD sample.
 3) Open the FrontDesk.sln solution file in Visual Studio
 4) Right-click on the "AppointmentScheduling" solution folder in the Solution Explorer and choose Add->Existing Project...
 5) Select the AppointmentScheduling.Specs.csproj that you copied into the FrontDeskSolution folder in step 2.

You should now be able to build and run the specifications. The AppointmentScheduling.Specs project references 3 NuGet packages which should be automatically restored by Visual Studio.
If the NuGet packages do not get restored you will have to install them manually. The packages you will need are as follows:
 - Machine.Specifications (version 0.9.3)
 - Machine.Specifications.Should (version 0.7.2)
 - FakeItEasy (version 1.25.3)
 - Machine.Fakes (version 2.6.0)
 - Machine.Fakes.FakeItEasy (version 2.6.0)

The specifications can be executed using any of the methods demonstrated in module 1.