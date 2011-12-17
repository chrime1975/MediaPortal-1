﻿using System.Collections.Generic;
using Mediaportal.TV.Server.TVControl.Interfaces;
using Mediaportal.TV.Server.TVControl.Interfaces.Services;
using Mediaportal.TV.Server.TVDatabase.Entities;
using Mediaportal.TV.Server.TVService.Interfaces.Services;

namespace Mediaportal.TV.Server.TVService.Services
{
  public class CardService : ICardService
  {    

    public IList<Card> ListAllCards()
    {
      var listAllCards = TVDatabase.TVBusinessLayer.CardManagement.ListAllCards();
      return listAllCards;
    }

    public Card GetCardByDevicePath(string cardDevice)
    {
      var cardByDevicePath = TVDatabase.TVBusinessLayer.CardManagement.GetCardByDevicePath(cardDevice);
      return cardByDevicePath;
    }

    public Card SaveCard(Card card)
    {
      return TVDatabase.TVBusinessLayer.CardManagement.SaveCard(card);
    }

    public void DeleteCard(int idCard)
    {
      TVDatabase.TVBusinessLayer.CardManagement.DeleteCard(idCard);
    }

    public DisEqcMotor SaveDisEqcMotor(DisEqcMotor motor)
    {
      return TVDatabase.TVBusinessLayer.CardManagement.SaveDisEqcMotor(motor);
    }

    public Card GetCard(int idCard)
    {
      return TVDatabase.TVBusinessLayer.CardManagement.GetCard(idCard);
    }

    public CardGroup SaveCardGroup(CardGroup @group)
    {
      return TVDatabase.TVBusinessLayer.CardManagement.SaveCardGroup(@group);
    }

    public void DeleteCardGroup(int idCardGroup)
    {
      TVDatabase.TVBusinessLayer.CardManagement.DeleteCardGroup(idCardGroup);
    }

    public IList<CardGroup> ListAllCardGroups()
    {
      return TVDatabase.TVBusinessLayer.CardManagement.ListAllCardGroups();
    }

    public IList<SoftwareEncoder> ListAllSofwareEncodersVideo()
    {
      return TVDatabase.TVBusinessLayer.CardManagement.ListAllSofwareEncodersVideo();
    }
    public IList<SoftwareEncoder> ListAllSofwareEncodersAudio()
    {
      return TVDatabase.TVBusinessLayer.CardManagement.ListAllSofwareEncodersAudio();
    }

    public IList<Satellite> ListAllSatellites()
    {
      return TVDatabase.TVBusinessLayer.CardManagement.ListAllSatellites();
    }

    public Satellite SaveSatellite(Satellite satellite)
    {
      return TVDatabase.TVBusinessLayer.CardManagement.SaveSatellite(satellite);
    }

    public SoftwareEncoder SaveSoftwareEncoder(SoftwareEncoder encoder)
    {
      return TVDatabase.TVBusinessLayer.CardManagement.SaveSoftwareEncoder(encoder);
    }

    public void DeleteGroupMap(int idMap)
    {
      TVDatabase.TVBusinessLayer.CardManagement.DeleteGroupMap(idMap);
    }

    public CardGroupMap SaveCardGroupMap(CardGroupMap map)
    {
      return TVDatabase.TVBusinessLayer.CardManagement.SaveCardGroupMap(map);
    }
  }
}
