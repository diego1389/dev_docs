using System;
using AppointmentScheduling.Core.Model.ScheduleAggregate;
using Machine.Specifications;

namespace AppointmentScheduling.Specs
{
    [Subject("Appointments")]
    public class When_creating_an_appointment
    {
        It Should_be_invalid_if_given_an_invalid_client = () =>
        {
            var exception = Catch.Exception(() => Appointment.Create(Guid.NewGuid(), -1, 1, 2, DateTime.Now, DateTime.Now.AddHours(1), 1, 1, "Fido"));
            exception.ShouldBeOfExactType<ArgumentOutOfRangeException>();
            exception.Message.ShouldContain("clientId");
        };

        It Should_be_invalid_if_given_an_invalid_patient = () =>
        {
            var exception = Catch.Exception(() => Appointment.Create(Guid.NewGuid(), 1, 0, 2, DateTime.Now, DateTime.Now.AddHours(1), 1, 1, "Fido"));
            exception.ShouldBeOfExactType<ArgumentOutOfRangeException>();
            exception.Message.ShouldContain("patientId");
        };

        It Should_be_invalid_if_given_an_invalid_room = () =>
        {
            var exception = Catch.Exception(() => Appointment.Create(Guid.NewGuid(), 1, 1, -99, DateTime.Now, DateTime.Now.AddHours(1), 1, 1, "Fido"));
            exception.ShouldBeOfExactType<ArgumentOutOfRangeException>();
            exception.Message.ShouldContain("roomId");
        };

        It Should_be_invalid_if_given_an_invalid_appointment_type = () =>
        {
            var exception = Catch.Exception(() => Appointment.Create(Guid.NewGuid(), 1, 2, 3, DateTime.Now, DateTime.Now.AddHours(1), -5, 1, "Fido"));
            exception.ShouldBeOfExactType<ArgumentOutOfRangeException>();
            exception.Message.ShouldContain("appointmentTypeId");
        };

        It Should_be_invalid_if_given_an_invalid_title = () =>
        {
            var exception = Catch.Exception(() => Appointment.Create(Guid.NewGuid(), 1, 2, 4, DateTime.Now, DateTime.Now.AddHours(1), 1, 1, ""));
            exception.ShouldBeOfExactType<ArgumentOutOfRangeException>();
            exception.Message.ShouldContain("title");
        };
    }
}