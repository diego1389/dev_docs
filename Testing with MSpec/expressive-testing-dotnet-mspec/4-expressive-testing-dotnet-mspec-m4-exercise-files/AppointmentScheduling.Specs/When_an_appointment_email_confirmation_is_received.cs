using System;
using AppointmentScheduling.Core.Interfaces;
using AppointmentScheduling.Core.Model.ApplicationEvents;
using AppointmentScheduling.Core.Model.ScheduleAggregate;
using AppointmentScheduling.Core.Services;
using FrontDesk.SharedKernel;
using Machine.Fakes;
using Machine.Specifications;

namespace AppointmentScheduling.Specs
{
    [Subject("Confirming an appointment")]
    public class When_an_appointment_email_confirmation_is_received : WithSubject<EmailConfirmationHandler>
    {
        static Schedule TestSchedule;
        static Appointment AppointmentToBeConfirmed;
        static AppointmentConfirmedEvent AppointmentConfirmedEvent;
        static Appointment ConfirmedAppointment;

        Establish context = () =>
        {
            DomainEvents.ClearCallbacks();
            DomainEvents.Register<Core.Model.Events.AppointmentConfirmedEvent>(e => ConfirmedAppointment = e.AppointmentUpdated);

            const int testClinicId = 3;
            var testScheduleId = Guid.NewGuid();
            var testDate = DateTime.Parse("1/1/2015 10:00:00");
            var testDateTimeRange = new DateTimeRange(DateTime.Parse("1/1/2015"), DateTime.Parse("1/2/2015"));
            
            AppointmentToBeConfirmed = Appointment.Create(testScheduleId, 1, 2, 3, testDate, testDate.AddHours(1), 1, 1, "Test");
            var appointment1 = Appointment.Create(testScheduleId, 5, 6, 2, testDate.AddHours(4), testDate.AddHours(5), 2, 2, "Test1");
            var appointment2 = Appointment.Create(testScheduleId, 6, 7, 1, testDate.AddHours(-2), testDate.AddHours(-1), 1, 1, "Test2");
            TestSchedule = new Schedule(testScheduleId, testDateTimeRange, testClinicId, new[] { appointment1, AppointmentToBeConfirmed, appointment2 } );
            
            The<IApplicationSettings>().WhenToldTo(s => s.ClinicId).Return(testClinicId);
            The<IApplicationSettings>().WhenToldTo(s => s.TestDate).Return(testDate);

            The<IScheduleRepository>().WhenToldTo(r => r.GetScheduleForDate(testClinicId, testDate)).Return(TestSchedule);
            AppointmentConfirmedEvent = new AppointmentConfirmedEvent
            {
                AppointmentId = AppointmentToBeConfirmed.Id,
                DateTimeEventOccurred = DateTime.Parse("12/31/2014 12:34:56")
            };
        };

        Because of = () => Subject.Handle(AppointmentConfirmedEvent);

        It Should_confirm_the_appointment = () =>
            ConfirmedAppointment.ShouldBeTheSameAs(AppointmentToBeConfirmed);

        It Should_update_the_schedule = () =>
            The<IScheduleRepository>().WasToldTo(r => r.Update(TestSchedule));
    }
}