using AppointmentScheduling.Core.Model.ScheduleAggregate;
using Machine.Specifications;

namespace AppointmentScheduling.Specs
{
    [Subject(typeof(Schedule))]
    public class When_loading_the_schedule : BaseScheduleContext
    {
        Establish context = () => CreateScheduleWithConflictingAppointments();

        It Should_mark_conflicting_appointments = () =>
        {
            testAppointment1.IsPotentiallyConflicting.ShouldBeTrue();
            testAppointment2.IsPotentiallyConflicting.ShouldBeTrue();
        };
    }
}