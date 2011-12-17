﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Mediaportal.TV.Server.TVDatabase.Entities;
using Mediaportal.TV.Server.TVDatabase.Entities.Enums;
using Mediaportal.TV.Server.TVDatabase.EntityModel.Interfaces;

namespace Mediaportal.TV.Server.TVDatabase.EntityModel.Repositories
{
  public class RecordingRepository : GenericRepository<Model>, IRecordingRepository, IDisposable
  {
    public RecordingRepository()    
    {
    }

    public RecordingRepository(Model context)
      : base(context)
    {
    }    

    public Recording GetRecording (int idRecording)
    {      
      Recording recording =  GetQuery<Recording>(c => c.idRecording == idRecording)
        .Include(r => r.Channel)
        .Include(r => r.RecordingCredits)
        .Include(r => r.Schedule)
        .Include(r => r.ProgramCategory)        
        .FirstOrDefault();
      return recording;     
    }

    public IQueryable<Recording> ListAllRecordingsByMediaType(MediaTypeEnum mediaType)
    {
      IQueryable<Recording> recordings = GetQuery<Recording>(r => r.mediaType == (int)mediaType)
        .Include(r => r.Channel)
        .Include(r => r.RecordingCredits)
        .Include(c => c.Schedule)        
        .Include(r => r.ProgramCategory);
      return recordings;     
    }

    public IQueryable<Recording> IncludeAllRelations(IQueryable<Recording> query)
    {
      var includeRelations = query.Include(r => r.Channel)
                                  .Include(r => r.RecordingCredits)
                                  .Include(c => c.Schedule)
                                  .Include(r => r.ProgramCategory);
      return includeRelations;
    }
  }
}
