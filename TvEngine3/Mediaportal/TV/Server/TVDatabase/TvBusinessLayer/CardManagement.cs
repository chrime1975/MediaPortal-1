﻿using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using Mediaportal.TV.Server.TVDatabase.Entities;
using Mediaportal.TV.Server.TVDatabase.EntityModel.Interfaces;
using Mediaportal.TV.Server.TVDatabase.EntityModel.ObjContext;
using Mediaportal.TV.Server.TVDatabase.EntityModel.Repositories;
using Mediaportal.TV.Server.TVLibrary.Interfaces.Logging;
using Channel = Mediaportal.TV.Server.TVDatabase.Entities.Channel;

namespace Mediaportal.TV.Server.TVDatabase.TVBusinessLayer
{
  public static class CardManagement
  {   
    public static IList<Card> ListAllCards()
    {
      using (ICardRepository cardRepository = new CardRepository())
      {
        IQueryable<Card> query = cardRepository.GetAll<Card>();
        query = cardRepository.IncludeAllRelations(query);
        return query.ToList(); 
      }      
    }

    /// <summary>
    /// Checks if a card can view a specific channel
    /// </summary>
    /// <param name="channelId">Channel id</param>
    /// <param name="card"></param>
    /// <returns>true/false</returns>
    public static bool CanViewTvChannel(Card card, int channelId)
    {
      IList<ChannelMap> cardChannels = card.ChannelMaps;
      return cardChannels.Any(cmap => channelId == cmap.idChannel && !cmap.epgOnly);
    }

    /// <summary>
    /// Checks if a card can tune a specific channel
    /// </summary>
    /// <param name="card"></param>
    /// <param name="channelId">Channel id</param>
    /// <returns>true/false</returns>
    public static bool CanTuneTvChannel(Card card, int channelId)
    {
      IList<ChannelMap> cardChannels = card.ChannelMaps;
      return cardChannels.Any(cmap => channelId == cmap.idChannel);
    }

    public static Card GetCardByDevicePath(string devicePath)
    {
      using (ICardRepository cardRepository = new CardRepository())
      {
        var query = cardRepository.GetQuery<Card>(c => c.devicePath == devicePath);
        query = cardRepository.IncludeAllRelations(query);
        Card card = query.ToList().FirstOrDefault(); 
        return card;
      }            
    }

    public static Card SaveCard(Card card)
    {
      using (ICardRepository cardRepository = new CardRepository())
      {
        cardRepository.AttachEntityIfChangeTrackingDisabled(cardRepository.ObjectContext.Cards, card);
        cardRepository.ApplyChanges(cardRepository.ObjectContext.Cards, card);
        cardRepository.UnitOfWork.SaveChanges();
        card.AcceptChanges();
        return card;
      }    
    }

    public static void DeleteCard(int idCard)
    {
      using (ICardRepository cardRepository = new CardRepository())
      {
        cardRepository.Delete<Card>(p => p.idCard == idCard);
        cardRepository.UnitOfWork.SaveChanges();
      }
    }

    public static IList<CardGroup> ListAllCardGroups()
    {
      using (ICardRepository cardRepository = new CardRepository())
      {
        var listAllCardGroups = cardRepository.GetAll<CardGroup>().ToList();
        return listAllCardGroups;
      }
    }

    public static DisEqcMotor SaveDisEqcMotor(DisEqcMotor motor)
    {
      using (ICardRepository cardRepository = new CardRepository())
      {
        cardRepository.AttachEntityIfChangeTrackingDisabled(cardRepository.ObjectContext.DisEqcMotors, motor);
        cardRepository.ApplyChanges(cardRepository.ObjectContext.DisEqcMotors, motor);
        cardRepository.UnitOfWork.SaveChanges();
        motor.AcceptChanges();
        return motor;
      }  
    }

    public static Card GetCard(int idCard)
    {
      using (ICardRepository cardRepository = new CardRepository())
      {
        IQueryable<Card> query = cardRepository.GetQuery<Card>(c => c.idCard == idCard);
        query = cardRepository.IncludeAllRelations(query);
        Card card = query.ToList().FirstOrDefault();
        return card;
      }  
    }

    public static CardGroup SaveCardGroup(CardGroup @group)
    {
      using (ICardRepository cardRepository = new CardRepository())
      {
        cardRepository.AttachEntityIfChangeTrackingDisabled(cardRepository.ObjectContext.CardGroups, @group);
        cardRepository.ApplyChanges(cardRepository.ObjectContext.CardGroups, @group);
        cardRepository.UnitOfWork.SaveChanges();
        @group.AcceptChanges();
        return @group;
      }  
    }

    public static void DeleteCardGroup(int idCardGroup)
    {
      using (ICardRepository cardRepository = new CardRepository())
      {
        cardRepository.Delete<CardGroup>(p => p.idCardGroup == idCardGroup);
        cardRepository.UnitOfWork.SaveChanges();
      }
    }

    public static IList<SoftwareEncoder> ListAllSofwareEncodersVideo()
    {
      using (ICardRepository cardRepository = new CardRepository())
      {
        return cardRepository.GetQuery<SoftwareEncoder>(s=>s.type == 0).OrderBy(s=>s.priority).ToList();                
      }
    }

    public static IList<SoftwareEncoder> ListAllSofwareEncodersAudio()
    {
      using (ICardRepository cardRepository = new CardRepository())
      {
        return cardRepository.GetQuery<SoftwareEncoder>(s => s.type == 1).OrderBy(s => s.priority).ToList();
      }
    }

    public static IList<Satellite> ListAllSatellites()
    {
      using (ICardRepository cardRepository = new CardRepository())
      {
        return cardRepository.GetAll<Satellite>().ToList();
      }
    }

    public static Satellite SaveSatellite(Satellite satellite)
    {
      using (ICardRepository cardRepository = new CardRepository())
      {
        cardRepository.AttachEntityIfChangeTrackingDisabled(cardRepository.ObjectContext.Satellites, satellite);
        cardRepository.ApplyChanges(cardRepository.ObjectContext.Satellites, satellite);
        cardRepository.UnitOfWork.SaveChanges();
        satellite.AcceptChanges();
        return satellite;
      }  
    }

    public static SoftwareEncoder SaveSoftwareEncoder(SoftwareEncoder encoder)
    {
      using (ICardRepository cardRepository = new CardRepository())
      {
        cardRepository.AttachEntityIfChangeTrackingDisabled(cardRepository.ObjectContext.SoftwareEncoders, encoder);
        cardRepository.ApplyChanges(cardRepository.ObjectContext.SoftwareEncoders, encoder);
        cardRepository.UnitOfWork.SaveChanges();
        encoder.AcceptChanges();
        return encoder;
      }  
    }

    public static void DeleteGroupMap(int idMap)
    {
      using (ICardRepository cardRepository = new CardRepository())
      {
        cardRepository.Delete<CardGroupMap>(p => p.idMapping == idMap);
        cardRepository.UnitOfWork.SaveChanges();
      }
    }

    public static CardGroupMap SaveCardGroupMap(CardGroupMap map)
    {
      using (ICardRepository cardRepository = new CardRepository())
      {
        cardRepository.AttachEntityIfChangeTrackingDisabled(cardRepository.ObjectContext.CardGroupMaps, map);
        cardRepository.ApplyChanges(cardRepository.ObjectContext.CardGroupMaps, map);
        cardRepository.UnitOfWork.SaveChanges();
        map.AcceptChanges();
        return map;
      }  
    }
   
  }
}
