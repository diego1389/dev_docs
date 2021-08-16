using System;
using System.Collections.Generic;
using AppointmentScheduling.Core.Model.ScheduleAggregate;
using FrontDesk.SharedKernel;
using Machine.Specifications;

namespace AppointmentScheduling.Specs
{
    public class BaseScheduleContext
    {
        protected static int testClinicId = 1;
        protected static DateTimeRange testDateTimeRange;
        protected static Appointment testAppointment1;
        protected static Appointment testAppointment2;
        protected static Appointment testAppointment3;
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

            testAppointment3 = Appointment.Create(testScheduleId,
                testClientId, testPatientId, testRoomId, testStartTime.AddHours(1), testEndTime.AddHours(2),
                testAppointmentTypeId, testDoctorId, "testAppointment3");
        };

        protected static void CreateEmptySchedule()
        {
            schedule = new Schedule(Guid.NewGuid(), testDateTimeRange, testClinicId, new List<Appointment>());
        }

        protected static void CreateScheduleWithConflictingAppointments()
        {
            schedule = new Schedule(Guid.NewGuid(), testDateTimeRange, testClinicId, new List<Appointment>() { testAppointment1, testAppointment2 });
        }

        protected static void CreateScheduleWithNonConflictingAppointments()
        {
            schedule = new Schedule(Guid.NewGuid(), testDateTimeRange, testClinicId, new List<Appointment>() { testAppointment1, testAppointment3 });
        }
    }
}