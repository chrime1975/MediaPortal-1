﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mediaportal.TV.Server.TVDatabase.Entities.Enums;

namespace Mediaportal.TV.Server.TVDatabase.Entities.Factories
{
  public static class ScheduleFactory
  {
    public static DateTime MinSchedule = new DateTime(2000, 1, 1);
    public static readonly int HighestPriority = Int32.MaxValue;

    public static Schedule Clone(Schedule source)
    {
      return CloneHelper.DeepCopy<Schedule>(source);

      /*Schedule schedule = new Schedule();
                                       IdChannel, idParentSchedule, scheduleType, ProgramName, StartTime, EndTime,
                                       MaxAirings, Priority,
                                       Directory, Quality, KeepMethod, KeepDate, PreRecordInterval, PostRecordInterval,
                                       Canceled);

      schedule.idChannel = source.idChannel;
      schedule.idParentSchedule = source.idParentSchedule;
      schedule.scheduleType = source.scheduleType;
      schedule.programName = source.programName;
      schedule.startTime = source.startTime;
      schedule.endTime = source.endTime;
      schedule.maxAirings = schedule.maxAirings;
      schedule.priority = source.priority;
      schedule.directory = source.directory;
      schedule.quality = source.quality;
      schedule.keepMethod = source.keepMethod;w
      schedule.

      schedule.series = source.series;
      schedule.id_Schedule = source.id_Schedule;

      return schedule;*/
    }

    public static Schedule CreateSchedule(int idChannel, string programName, DateTime startTime, DateTime endTime)
    {
      var schedule = new Schedule
                       {
                         idChannel = idChannel,
                         idParentSchedule = null,
                         programName = programName,
                         canceled = MinSchedule,
                         directory = "",
                         endTime = endTime,
                         keepDate = MinSchedule,
                         keepMethod = (int) KeepMethodType.UntilSpaceNeeded,
                         maxAirings = Int32.MaxValue,
                         postRecordInterval = 0,
                         preRecordInterval = 0,
                         priority = 0,
                         quality = 0,
                         series = false,
                         startTime = startTime
                       };


      return schedule;
    }

   
     
      
  }
}
