using System;
using AppointmentScheduling.Core.Model.Events;
using AppointmentScheduling.Core.Model.ScheduleAggregate;
using FrontDesk.SharedKernel;
using Machine.Specifications;

namespace AppointmentScheduling.Specs
{
    [Subject(typeof(Appointment))]
    public class When_an_appointment_is_confirmed
    {
        static Appointment AppointmentToConfirm;
        static readonly DateTime TestConfirmationTime = DateTime.Now.AddDays(1);
        static Guid ConfirmedAppointmentId = Guid.Empty;

        Establish context = () =>
        {
            DomainEvents.ClearCallbacks();
            DomainEvents.Register<AppointmentConfirmedEvent>(ace => ConfirmedAppointmentId = AppointmentToConfirm.Id);

            AppointmentToConfirm = Appointment.Create(Guid.NewGuid(), 1, 1, 1, DateTime.Now, DateTime.Now.AddHours(0.5), 1, 1, "testAppointment1");
            AppointmentToConfirm.Confirm(TestConfirmationTime);
        };

        It Should_notify_the_system_that_it_was_confirmed = () => ConfirmedAppointmentId.ShouldEqual(AppointmentToConfirm.Id);

        class When_reconfirming_the_already_confirmed_appointment
        {
            Establish context = () =>
            {
                ConfirmedAppointmentId = Guid.Empty;
                AppointmentToConfirm.Confirm(DateTime.Now.AddDays(2));
            };

            It Should_not_notify_the_system_of_the_redundant_confirmation = () => ConfirmedAppointmentId.ShouldEqual(Guid.Empty);
        }
    }
}