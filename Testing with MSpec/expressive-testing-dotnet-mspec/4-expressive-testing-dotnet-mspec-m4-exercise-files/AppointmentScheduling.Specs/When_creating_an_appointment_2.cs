using System;
using AppointmentScheduling.Core.Model.ScheduleAggregate;
using Machine.Specifications;

namespace AppointmentScheduling.Specs
{
    [Subject("Creating an appointment")]
    public class When_creating_an_appointment
    {
        static int ClientId;
        static int PatientId;
        static int RoomId;
        static int AppointmentType;
        static string Title;
        static Exception CreateAppointmentException;

        Establish context = () =>
        {
            ClientId = 1;
            PatientId = 1;
            RoomId = 1;
            AppointmentType = 1;
            Title = "Fido";
        };

        Because of = () =>
            CreateAppointmentException =
                Catch.Exception(() =>Appointment.Create(Guid.NewGuid(), ClientId, PatientId, RoomId, DateTime.Now,DateTime.Now.AddHours(1), AppointmentType, 1, Title));

        [Tags("Slow")]
        class With_an_invalid_client
        {
            Establish context = () => ClientId = -1;
            It Should_not_be_valid = () => CreateAppointmentException.ShouldBeOfExactType<ArgumentOutOfRangeException>();
        }

        [Tags("Slow")]
        class With_an_invalid_patient
        {
            Establish context = () => PatientId = 0;
            It Should_not_be_valid = () => CreateAppointmentException.ShouldBeOfExactType<ArgumentOutOfRangeException>();
        }

        [Tags("Slow")]
        class With_an_invalid_room
        {
            Establish context = () => RoomId = -99;
            It Should_not_be_valid = () => CreateAppointmentException.ShouldBeOfExactType<ArgumentOutOfRangeException>();
        }

        [Tags("Slow")]
        class With_an_invalid_appointment_type
        {
            Establish context = () => AppointmentType = -5;
            It Should_not_be_valid = () => CreateAppointmentException.ShouldBeOfExactType<ArgumentOutOfRangeException>();
        }

        [Tags("Slow")]
        class With_an_invalid_title
        {
            Establish context = () => Title = "";
            It Should_not_be_valid = () => CreateAppointmentException.ShouldBeOfExactType<ArgumentOutOfRangeException>();
        }
    }
}