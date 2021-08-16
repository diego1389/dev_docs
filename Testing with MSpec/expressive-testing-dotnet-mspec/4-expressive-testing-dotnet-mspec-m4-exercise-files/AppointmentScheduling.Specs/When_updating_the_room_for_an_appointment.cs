using System;
using AppointmentScheduling.Core.Model.Events;
using AppointmentScheduling.Core.Model.ScheduleAggregate;
using FrontDesk.SharedKernel;
using Machine.Specifications;

namespace AppointmentScheduling.Specs
{
    [Subject(typeof(Appointment))]
    public class When_updating_the_room_for_an_appointment : BaseScheduleContext
    {
        static Guid updatedAppointmentId = Guid.Empty;

        Establish context = () =>
        {
            DomainEvents.Register<AppointmentUpdatedEvent>(e => updatedAppointmentId = e.AppointmentUpdated.Id);

            CreateScheduleWithNonConflictingAppointments();
        };

        Because of = () => testAppointment3.UpdateRoom(999);

        It Should_notify_the_system_that_it_was_updated = () =>
            updatedAppointmentId.ShouldEqual(testAppointment3.Id);

        It Should_update_the_room_on_the_appointment = () =>
            testAppointment3.RoomId.ShouldEqual(999);
    }

    [Subject(typeof(Appointment))]
    public class When_updating_the_room_for_an_appointment_to_the_same_room : BaseScheduleContext
    {
        static Guid updatedAppointmentId = Guid.Empty;

        Establish context = () =>
        {
            DomainEvents.Register<AppointmentUpdatedEvent>(e => updatedAppointmentId = e.AppointmentUpdated.Id);

            CreateScheduleWithNonConflictingAppointments();
        };

        Because of = () => testAppointment3.UpdateRoom(testAppointment3.RoomId);

        It Should_not_notify_the_system_that_it_was_updated = () =>
            updatedAppointmentId.ShouldEqual(Guid.Empty);
    }
}