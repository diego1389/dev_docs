using Machine.Specifications;

namespace AppointmentScheduling.Specs
{
    public class When_loading_the_schedule : BaseScheduleContext
    {
        Establish context = () => CreateSchedule();

        It Should_mark_conflicting_appointments = () =>
        {
            testAppointment1.IsPotentiallyConflicting.ShouldBeTrue();
            testAppointment2.IsPotentiallyConflicting.ShouldBeTrue();
        };
    }
}