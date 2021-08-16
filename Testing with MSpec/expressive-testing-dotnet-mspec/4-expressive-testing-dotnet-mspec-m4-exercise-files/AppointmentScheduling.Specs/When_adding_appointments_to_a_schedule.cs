using System;
using AppointmentScheduling.Core.Model.Events;
using AppointmentScheduling.Core.Model.ScheduleAggregate;
using FrontDesk.SharedKernel;
using Machine.Specifications;

namespace AppointmentScheduling.Specs
{
    [Subject(typeof(Schedule))]
    public class When_adding_an_appointment_to_a_schedule : BaseScheduleContext
    {
        static Guid TestScheduleId = Guid.NewGuid();
        static Appointment TestAppointment;
        static int TestClientId = 1;
        static int TestPatientId = 1;
        static int TestRoomId1 = 1;
        static int TestRoomId2 = 2;
        static DateTime TestStartTime = DateTime.Now;
        static DateTime TestEndTime = DateTime.Now.AddHours(1);
        static int TestDoctorId = 1;
        static Guid TestAppointmentId = Guid.Empty;

        Establish context = () =>
        {
            CreateEmptySchedule();
            DomainEvents.Register<AppointmentScheduledEvent>(ase => TestAppointmentId = TestAppointment.Id);

            TestAppointment = Appointment.Create(TestScheduleId, TestClientId, TestPatientId, TestRoomId1, TestStartTime, TestEndTime, 1, TestDoctorId, "Fido");
            schedule.AddNewAppointment(TestAppointment);
        };

        It Should_add_the_appointment_to_the_list_of_appointments_for_the_schedule = () =>
            schedule.Appointments.ShouldContain(a => a.Equals(TestAppointment));

        It Should_notify_the_rest_of_the_system_that_an_appointment_was_scheduled = () =>
            TestAppointment.Id.ShouldEqual(TestAppointmentId);

        class When_adding_the_same_appointment_to_the_schedule_again
        {
            static Exception ExpectedException;

            Establish context = () => ExpectedException = Catch.Exception(() => schedule.AddNewAppointment(TestAppointment));

            It Should_not_allow_the_duplicate_appointment_to_be_added = () => ExpectedException.ShouldBeOfExactType<ArgumentException>();
        }

        class When_adding_the_same_patient_to_another_room_at_the_same_time
        {
            static Appointment TestAppointment2;

            Establish context = () =>
            {
                TestAppointment2 = Appointment.Create(TestScheduleId, TestClientId, TestPatientId, TestRoomId2, TestStartTime, TestEndTime, 1, TestDoctorId, "Spot");
                schedule.AddNewAppointment(TestAppointment2);
            };

            It Should_mark_the_appointments_as_conflicted = () =>
            {
                TestAppointment.IsPotentiallyConflicting.ShouldBeTrue();
                TestAppointment2.IsPotentiallyConflicting.ShouldBeTrue();
            };
        }
    }
}