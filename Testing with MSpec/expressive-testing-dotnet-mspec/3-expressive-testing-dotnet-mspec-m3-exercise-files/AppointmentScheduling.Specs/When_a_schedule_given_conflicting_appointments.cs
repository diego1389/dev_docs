using System;
using System.Collections.Generic;
using AppointmentScheduling.Core.Model.ScheduleAggregate;
using FrontDesk.SharedKernel;
using Machine.Specifications;

namespace AppointmentScheduling.Specs
{
    public class When_a_schedule_given_conflicting_appointments
    {
        protected static int testClinicId = 1;
        protected static DateTimeRange testDateTimeRange;
        protected static Appointment testAppointment1;
        protected static Appointment testAppointment2;
        protected static Schedule schedule;

        Establish context = () =>
        {
            DomainEvents.ClearCallbacks();

            Guid testScheduleId = Guid.NewGuid();
            int testPatientId = 123;
            int testClientId = 456;
            int testRoomId = 567;
            int testAppointmentTypeId = 1;
            int testDoctorId = 2;
            DateTime testStartTime = new DateTime(2014, 6, 9, 9, 0, 0);
            DateTime testEndTime = new DateTime(2014, 6, 9, 9, 30, 0);

            testDateTimeRange = new DateTimeRange(new DateTime(2014, 6, 9), new DateTime(2014, 6, 16));

            testAppointment1 = Appointment.Create(testScheduleId,
                testClientId, testPatientId, testRoomId, testStartTime, testEndTime,
                testAppointmentTypeId, testDoctorId, "testAppointment1");

            testAppointment2 = Appointment.Create(testScheduleId,
                testClientId, testPatientId, testRoomId, testStartTime, testEndTime,
                testAppointmentTypeId, testDoctorId, "testAppointment2");

            schedule = new Schedule(Guid.NewGuid(), testDateTimeRange, testClinicId, new List<Appointment>() { testAppointment1, testAppointment2 });
        };

        It Should_mark_conflicting_appointments = () =>
        {
            testAppointment1.IsPotentiallyConflicting.ShouldBeTrue();
            testAppointment2.IsPotentiallyConflicting.ShouldBeTrue();
        };

        class When_updating_one_of_the_conflicting_appointments_to_an_open_time
        {
            Because of = () => testAppointment1.UpdateTime(new DateTimeRange(testAppointment1.TimeRange.Start.AddDays(1), testAppointment1.TimeRange.End.AddDays(1)));

            It Should_unmark_appointments_which_no_longer_conflict = () =>
            {
                testAppointment1.IsPotentiallyConflicting.ShouldBeFalse();
                testAppointment2.IsPotentiallyConflicting.ShouldBeFalse();
            };
        }
    }
}