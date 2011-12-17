//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;

namespace Mediaportal.TV.Server.TVDatabase.Entities
{
    [DataContract(IsReference = true)]
    [KnownType(typeof(Channel))]
    public partial class ChannelLinkageMap: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int idMapping
        {
            get { return _idMapping; }
            set
            {
                if (_idMapping != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'idMapping' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _idMapping = value;
                    OnPropertyChanged("idMapping");
                }
            }
        }
        private int _idMapping;
    
        [DataMember]
        public int idPortalChannel
        {
            get { return _idPortalChannel; }
            set
            {
                if (_idPortalChannel != value)
                {
                    ChangeTracker.RecordOriginalValue("idPortalChannel", _idPortalChannel);
                    if (!IsDeserializing)
                    {
                        if (ChannelPortal != null && ChannelPortal.idChannel != value)
                        {
                            ChannelPortal = null;
                        }
                    }
                    _idPortalChannel = value;
                    OnPropertyChanged("idPortalChannel");
                }
            }
        }
        private int _idPortalChannel;
    
        [DataMember]
        public int idLinkedChannel
        {
            get { return _idLinkedChannel; }
            set
            {
                if (_idLinkedChannel != value)
                {
                    ChangeTracker.RecordOriginalValue("idLinkedChannel", _idLinkedChannel);
                    if (!IsDeserializing)
                    {
                        if (ChannelLink != null && ChannelLink.idChannel != value)
                        {
                            ChannelLink = null;
                        }
                    }
                    _idLinkedChannel = value;
                    OnPropertyChanged("idLinkedChannel");
                }
            }
        }
        private int _idLinkedChannel;
    
        [DataMember]
        public string displayName
        {
            get { return _displayName; }
            set
            {
                if (_displayName != value)
                {
                    _displayName = value;
                    OnPropertyChanged("displayName");
                }
            }
        }
        private string _displayName;

        #endregion
        #region Navigation Properties
    
        [DataMember]
        public Channel ChannelLink
        {
            get { return _channelLink; }
            set
            {
                if (!ReferenceEquals(_channelLink, value))
                {
                    var previousValue = _channelLink;
                    _channelLink = value;
                    FixupChannelLink(previousValue);
                    OnNavigationPropertyChanged("ChannelLink");
                }
            }
        }
        private Channel _channelLink;
    
        [DataMember]
        public Channel ChannelPortal
        {
            get { return _channelPortal; }
            set
            {
                if (!ReferenceEquals(_channelPortal, value))
                {
                    var previousValue = _channelPortal;
                    _channelPortal = value;
                    FixupChannelPortal(previousValue);
                    OnNavigationPropertyChanged("ChannelPortal");
                }
            }
        }
        private Channel _channelPortal;

        #endregion
        #region ChangeTracking
    
        protected virtual void OnPropertyChanged(String propertyName)
        {
            if (ChangeTracker.State != ObjectState.Added && ChangeTracker.State != ObjectState.Deleted)
            {
                ChangeTracker.State = ObjectState.Modified;
            }
            if (_propertyChanged != null)
            {
                _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    
        protected virtual void OnNavigationPropertyChanged(String propertyName)
        {
            if (_propertyChanged != null)
            {
                _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged{ add { _propertyChanged += value; } remove { _propertyChanged -= value; } }
        private event PropertyChangedEventHandler _propertyChanged;
        private ObjectChangeTracker _changeTracker;
    
        [DataMember]
        public ObjectChangeTracker ChangeTracker
        {
            get
            {
                if (_changeTracker == null)
                {
                    _changeTracker = new ObjectChangeTracker();
                    _changeTracker.ObjectStateChanging += HandleObjectStateChanging;
                }
                return _changeTracker;
            }
            set
            {
                if(_changeTracker != null)
                {
                    _changeTracker.ObjectStateChanging -= HandleObjectStateChanging;
                }
                _changeTracker = value;
                if(_changeTracker != null)
                {
                    _changeTracker.ObjectStateChanging += HandleObjectStateChanging;
                }
            }
        }
    
        private void HandleObjectStateChanging(object sender, ObjectStateChangingEventArgs e)
        {
            if (e.NewState == ObjectState.Deleted)
            {
                ClearNavigationProperties();
            }
        }
    
        protected bool IsDeserializing { get; private set; }
    
        [OnDeserializing]
        public void OnDeserializingMethod(StreamingContext context)
        {
            IsDeserializing = true;
        }
    
        [OnDeserialized]
        public void OnDeserializedMethod(StreamingContext context)
        {
            IsDeserializing = false;
            ChangeTracker.ChangeTrackingEnabled = true;
        }
    
        // This entity type is the dependent end in at least one association that performs cascade deletes.
        // This event handler will process notifications that occur when the principal end is deleted.
        internal void HandleCascadeDelete(object sender, ObjectStateChangingEventArgs e)
        {
            if (e.NewState == ObjectState.Deleted)
            {
                this.MarkAsDeleted();
            }
        }
    
        protected virtual void ClearNavigationProperties()
        {
            ChannelLink = null;
            ChannelPortal = null;
        }

        #endregion
        #region Association Fixup
    
        private void FixupChannelLink(Channel previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.ChannelLinkMaps.Contains(this))
            {
                previousValue.ChannelLinkMaps.Remove(this);
            }
    
            if (ChannelLink != null)
            {
                if (!ChannelLink.ChannelLinkMaps.Contains(this))
                {
                    ChannelLink.ChannelLinkMaps.Add(this);
                }
    
                idLinkedChannel = ChannelLink.idChannel;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("ChannelLink")
                    && (ChangeTracker.OriginalValues["ChannelLink"] == ChannelLink))
                {
                    ChangeTracker.OriginalValues.Remove("ChannelLink");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("ChannelLink", previousValue);
                }
                if (ChannelLink != null && !ChannelLink.ChangeTracker.ChangeTrackingEnabled)
                {
                    ChannelLink.StartTracking();
                }
            }
        }
    
        private void FixupChannelPortal(Channel previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.ChannelPortalMaps.Contains(this))
            {
                previousValue.ChannelPortalMaps.Remove(this);
            }
    
            if (ChannelPortal != null)
            {
                if (!ChannelPortal.ChannelPortalMaps.Contains(this))
                {
                    ChannelPortal.ChannelPortalMaps.Add(this);
                }
    
                idPortalChannel = ChannelPortal.idChannel;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("ChannelPortal")
                    && (ChangeTracker.OriginalValues["ChannelPortal"] == ChannelPortal))
                {
                    ChangeTracker.OriginalValues.Remove("ChannelPortal");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("ChannelPortal", previousValue);
                }
                if (ChannelPortal != null && !ChannelPortal.ChangeTracker.ChangeTrackingEnabled)
                {
                    ChannelPortal.StartTracking();
                }
            }
        }

        #endregion
    }
}
