using System;
using AppointmentScheduling.Core.Model.Events;
using AppointmentScheduling.Core.Model.ScheduleAggregate;
using FrontDesk.SharedKernel;
using Machine.Specifications;

namespace AppointmentScheduling.Specs
{
    [Subject(typeof(Appointment))]
    public class When_updating_the_time_on_an_appointment : BaseScheduleContext
    {
        static DateTimeRange NewTimeRange;
        static Guid UpdatedAppointmentId = Guid.Empty;

        Establish context = () =>
        {
            DomainEvents.Register<AppointmentUpdatedEvent>(e => UpdatedAppointmentId = e.AppointmentUpdated.Id);

            NewTimeRange = new DateTimeRange(testAppointment1.TimeRange.Start.AddDays(1), testAppointment1.TimeRange.End.AddDays(1));
            CreateScheduleWithConflictingAppointments();
        };

        Because of = () => testAppointment1.UpdateTime(NewTimeRange);

        It Should_unmark_appointments_which_no_longer_conflict = () =>
        {
            testAppointment1.IsPotentiallyConflicting.ShouldBeFalse();
            testAppointment2.IsPotentiallyConflicting.ShouldBeFalse();
        };

        It Should_update_the_time_range_on_the_appointment = () =>
            testAppointment1.TimeRange.ShouldEqual(NewTimeRange);

        It Should_notify_the_system_that_the_time_was_updated = () =>
            UpdatedAppointmentId.ShouldEqual(testAppointment1.Id);
    }

    [Subject(typeof(Appointment))]
    public class When_updating_the_time_on_an_appointment_to_the_same_time : BaseScheduleContext
    {
        static Guid UpdatedAppointmentId = Guid.Empty;

        Establish context = () =>
        {
            DomainEvents.Register<AppointmentUpdatedEvent>(e => UpdatedAppointmentId = e.AppointmentUpdated.Id);

            CreateScheduleWithNonConflictingAppointments();
        };

        Because of = () => testAppointment1.UpdateTime(testAppointment1.TimeRange);

        It Should_not_notify_the_system_that_the_time_was_updated = () =>
            UpdatedAppointmentId.ShouldEqual(Guid.Empty);
    }
}