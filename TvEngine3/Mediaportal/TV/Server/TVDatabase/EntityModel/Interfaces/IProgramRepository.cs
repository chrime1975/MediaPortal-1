using System;
using System.Collections.Generic;
using System.Linq;
using Mediaportal.TV.Server.TVDatabase.Entities;
using Mediaportal.TV.Server.TVDatabase.Entities.Enums;

namespace Mediaportal.TV.Server.TVDatabase.EntityModel.Interfaces
{
  public interface IProgramRepository : IRepository<Model>, IDisposable
  {
    IQueryable<Program> GetNowAndNextProgramsForChannels(IList<Channel> channels);
    void DeleteAllProgramsWithChannelId(int idChannel);
    IQueryable<Program> FindAllProgramsByChannelId(int idChannel);
    IQueryable<Program> GetProgramsByStartEndTimes(DateTime startTime, DateTime endTime);
    IQueryable<Program> GetNowAndNextProgramsForChannel(int idChannel);

    Program GetProgramAt(DateTime date, int idChannel);
    Program GetProgramAt(DateTime date, string title);
    IQueryable<Program> GetProgramsByTitle(IQueryable<Program> query, string searchCriteria, StringComparisonEnum stringComparison);
    IQueryable<Program> GetProgramsByCategory(IQueryable<Program> query, string searchCriteria, StringComparisonEnum stringComparison);
    IQueryable<Program> GetProgramsByDescription(IQueryable<Program> query, string searchCriteria, StringComparisonEnum stringComparison);
    IQueryable<Program> GetProgramsByTimesInterval(DateTime startTime, DateTime endTime);
    IQueryable<Program> IncludeAllRelations(IQueryable<Program> query);
  }
}