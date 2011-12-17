﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Mediaportal.TV.Server.TVControl.Events;
using Mediaportal.TV.Server.TVControl.Interfaces.Events;
using Mediaportal.TV.Server.TVControl.Interfaces.Services;
using Mediaportal.TV.Server.TVLibrary.Interfaces.CiMenu;

namespace Mediaportal.TV.Server.TVService.Services
{
  class Subscriber
  {
    public string UserName { get; set; }
    public IServerEventCallback ServerEventCallback { get; set; }
    //public ServerEventEnum ServerEventEnum { get; set; }
  }

  [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
  public class EventService : IEventService, IDisposable
  {
    private static readonly object _subscribersLock = new object();
    private static readonly IDictionary<string, Subscriber> _subscribers = new ConcurrentDictionary<string, Subscriber>();    

    //private static readonly ICollection<string> _tvServerEventsSubscribers = new HashSet<string>();
    //private static readonly ICollection<string> _heartbeatEventsSubscribers = new HashSet<string>();
    //private static readonly ICollection<string> _ciMenuEventsSubscribers = new HashSet<string>();

    //private static readonly object _tvServerEventsSubscribersLock = new object();
    //private static readonly object _heartbeatEventsSubscribersLock = new object();
    //private static readonly object _ciMenuEventsSubscribersLock = new object();

    public delegate void UserDisconnectedFromServiceDelegate(string username);        
    public static UserDisconnectedFromServiceDelegate UserDisconnectedFromService;

    #region IDisposable

    ~EventService()
    {
      Dispose();
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <filterpriority>2</filterpriority>
    public void Dispose()
    {
      //if (OperationContext.Current != null)
      {
        lock (_subscribersLock)
        {
          foreach (Subscriber subscriber in _subscribers.Values)
          {
            IServerEventCallback eventCallback = subscriber.ServerEventCallback;
            var obj = eventCallback as ICommunicationObject;
            if (obj != null)
            {
              obj.Close();
            }
          }
          _subscribers.Clear();
        }
      }  
    }

    #endregion

    public void Subscribe(string username/*, ServerEventEnum eventEnum*/)
    {
      bool alreadySubscribed;
      lock (_subscribersLock)
      {
        alreadySubscribed = _subscribers.ContainsKey(username);
      }

      if (!alreadySubscribed)
      {
        var eventCallback = OperationContext.Current.GetCallbackChannel<IServerEventCallback>();

        var obj = eventCallback as ICommunicationObject;
        if (obj != null)
        {          
          obj.Faulted += EventService_Faulted;
          obj.Closed += EventService_Closed;
        }
        var subscriber = new Subscriber { UserName = username, ServerEventCallback = eventCallback/*, ServerEventEnum = eventEnum*/ };
        lock (_subscribersLock)
        {
          _subscribers[username] = subscriber;
        }
      }
      /*else
      {
        _subscribers[username].ServerEventEnum |= eventEnum;
      }*/
      
      /*bool isTvServerEventEnum = (eventEnum & ServerEventEnum.TvServerEventEnum) == ServerEventEnum.TvServerEventEnum;
      bool isHeartbeatEventEnum = (eventEnum & ServerEventEnum.HeartbeatEventEnum) == ServerEventEnum.HeartbeatEventEnum;
      bool isCiMenuEventEnum = (eventEnum & ServerEventEnum.CiMenuEventEnum) == ServerEventEnum.CiMenuEventEnum;*/
    }

    public void Unsubscribe(string username/*, ServerEventEnum eventEnum*/)
    {
      Subscriber subscriber;
      bool found;
      lock (_subscribersLock)
      {
        found = _subscribers.TryGetValue(username, out subscriber);
      }
      if (found)
      {
        FireUserDisconnectedFromService(subscriber.ServerEventCallback);
      }
      lock (_subscribersLock)
      {
        if (_subscribers.ContainsKey(username))
        {
          _subscribers.Remove(username);
        }
      }


      /*Subscriber subscriber;
      bool foundUser = _subscribers.TryGetValue(username, out subscriber);
      if (foundUser)
      {
        subscriber.ServerEventEnum = subscriber.ServerEventEnum | eventEnum;
        if (subscriber.ServerEventEnum == ServerEventEnum.None)
        {
          _subscribers.Remove(username); 
        }
        else
        {
          _subscribers[username] = subscriber;
        }
      }*/
    }

    private void EventService_Faulted(object sender, EventArgs e)
    {
      FireUserDisconnectedFromService(sender as IServerEventCallback);
    }

    private void EventService_Closed(object sender, EventArgs e)
    {
      FireUserDisconnectedFromService(sender as IServerEventCallback);
    }

    private void FireUserDisconnectedFromService(IServerEventCallback eventCallback)
    {      
      if (eventCallback != null)
      {
        if (UserDisconnectedFromService != null)
        {
          Subscriber subscriber;
          lock (_subscribersLock)
          {
            subscriber = _subscribers.Values.FirstOrDefault(c => c.ServerEventCallback == eventCallback);
          }
          if (subscriber != null)
          {
            string username = subscriber.UserName;            
            UserDisconnectedFromService(username);
            lock (_subscribersLock)
            {
              if (_subscribers.ContainsKey(username))
              {
                _subscribers.Remove(username);
              }
            }
          }
        }        
      }
    }    

    private static bool IsConnectionReady(ICommunicationObject callback)
    {
      bool connectionReady = callback != null && (callback.State == CommunicationState.Opened);
      return connectionReady;
    }

    #region public static methods

    public static void CallbackTvServerEvent(string username, TvServerEventArgs eventArgs)
    {
      Subscriber subscriber;
      bool userFound;
      lock (_subscribersLock)
      {
        userFound = _subscribers.TryGetValue(username, out subscriber);
      }
      //foreach (Subscriber subscriber in _subscribers.Values)
      {
        //bool isTvServerEventEnum = (subscriber.ServerEventEnum & ServerEventEnum.TvServerEventEnum) == ServerEventEnum.TvServerEventEnum;
        //if (isTvServerEventEnum)
        if (userFound)
        {
          if (IsConnectionReady(subscriber.ServerEventCallback as ICommunicationObject))
          {
            subscriber.ServerEventCallback.CallbackTvServerEvent(eventArgs);
          }
        }
      }
    }

    public static void CallbackCiMenuEvent(string username, CiMenu eventArgs)
    {
      Subscriber subscriber;
      bool userFound;
      lock (_subscribersLock)
      {
        userFound = _subscribers.TryGetValue(username, out subscriber);
      }

      //foreach (Subscriber subscriber in _subscribers.Values)
      {
        //bool isCiMenuEventEnum = (subscriber.ServerEventEnum & ServerEventEnum.CiMenuEventEnum) == ServerEventEnum.CiMenuEventEnum;
        //if (isCiMenuEventEnum)
        if (userFound)
        {
          if (IsConnectionReady(subscriber.ServerEventCallback as ICommunicationObject))
          {
            subscriber.ServerEventCallback.CiMenuCallback(eventArgs);
          }
        }
      }
    }

    public static bool CallbackRequestHeartbeat(string username)
    {
      bool heartbeatSent = false;
      Subscriber subscriber;
      bool userFound;
      lock (_subscribersLock)
      {
        userFound = _subscribers.TryGetValue(username, out subscriber);
      }

      //foreach (Subscriber subscriber in _subscribers.Values)
      {
        //bool isHeartbeatEventEnum = (subscriber.ServerEventEnum & ServerEventEnum.HeartbeatEventEnum) == ServerEventEnum.HeartbeatEventEnum;
        //if (isHeartbeatEventEnum)
        if (userFound)
        {
          if (IsConnectionReady(subscriber.ServerEventCallback as ICommunicationObject))
          {
            subscriber.ServerEventCallback.HeartbeatRequestReceived();
            heartbeatSent = true;
          }
        }
        return heartbeatSent;
      }
    }

    #endregion

    
  }
}
