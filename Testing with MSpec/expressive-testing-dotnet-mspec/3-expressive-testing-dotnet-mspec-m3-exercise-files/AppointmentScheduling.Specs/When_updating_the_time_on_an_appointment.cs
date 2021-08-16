using FrontDesk.SharedKernel;
using Machine.Specifications;

namespace AppointmentScheduling.Specs
{
    public class When_updating_the_time_on_an_appointment : BaseScheduleContext
    {
        Establish context = () => CreateSchedule();

        Because of = () => testAppointment1.UpdateTime(new DateTimeRange(testAppointment1.TimeRange.Start.AddDays(1), testAppointment1.TimeRange.End.AddDays(1)));

        It Should_unmark_appointments_which_no_longer_conflict = () =>
        {
            testAppointment1.IsPotentiallyConflicting.ShouldBeFalse();
            testAppointment2.IsPotentiallyConflicting.ShouldBeFalse();
        };
    }
}