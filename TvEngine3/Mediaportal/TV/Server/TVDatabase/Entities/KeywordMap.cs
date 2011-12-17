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
    [KnownType(typeof(Keyword))]
    [KnownType(typeof(ChannelGroup))]
    public partial class KeywordMap: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int idKeywordMap
        {
            get { return _idKeywordMap; }
            set
            {
                if (_idKeywordMap != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'idKeywordMap' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _idKeywordMap = value;
                    OnPropertyChanged("idKeywordMap");
                }
            }
        }
        private int _idKeywordMap;
    
        [DataMember]
        public int idKeyword
        {
            get { return _idKeyword; }
            set
            {
                if (_idKeyword != value)
                {
                    ChangeTracker.RecordOriginalValue("idKeyword", _idKeyword);
                    if (!IsDeserializing)
                    {
                        if (Keyword != null && Keyword.idKeyword != value)
                        {
                            Keyword = null;
                        }
                    }
                    _idKeyword = value;
                    OnPropertyChanged("idKeyword");
                }
            }
        }
        private int _idKeyword;
    
        [DataMember]
        public int idChannelGroup
        {
            get { return _idChannelGroup; }
            set
            {
                if (_idChannelGroup != value)
                {
                    ChangeTracker.RecordOriginalValue("idChannelGroup", _idChannelGroup);
                    if (!IsDeserializing)
                    {
                        if (ChannelGroups != null && ChannelGroups.idGroup != value)
                        {
                            ChannelGroups = null;
                        }
                    }
                    _idChannelGroup = value;
                    OnPropertyChanged("idChannelGroup");
                }
            }
        }
        private int _idChannelGroup;

        #endregion
        #region Navigation Properties
    
        [DataMember]
        public Keyword Keyword
        {
            get { return _keyword; }
            set
            {
                if (!ReferenceEquals(_keyword, value))
                {
                    var previousValue = _keyword;
                    _keyword = value;
                    FixupKeyword(previousValue);
                    OnNavigationPropertyChanged("Keyword");
                }
            }
        }
        private Keyword _keyword;
    
        [DataMember]
        public ChannelGroup ChannelGroups
        {
            get { return _channelGroups; }
            set
            {
                if (!ReferenceEquals(_channelGroups, value))
                {
                    var previousValue = _channelGroups;
                    _channelGroups = value;
                    FixupChannelGroups(previousValue);
                    OnNavigationPropertyChanged("ChannelGroups");
                }
            }
        }
        private ChannelGroup _channelGroups;

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
            Keyword = null;
            ChannelGroups = null;
        }

        #endregion
        #region Association Fixup
    
        private void FixupKeyword(Keyword previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.KeywordMaps.Contains(this))
            {
                previousValue.KeywordMaps.Remove(this);
            }
    
            if (Keyword != null)
            {
                if (!Keyword.KeywordMaps.Contains(this))
                {
                    Keyword.KeywordMaps.Add(this);
                }
    
                idKeyword = Keyword.idKeyword;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("Keyword")
                    && (ChangeTracker.OriginalValues["Keyword"] == Keyword))
                {
                    ChangeTracker.OriginalValues.Remove("Keyword");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("Keyword", previousValue);
                }
                if (Keyword != null && !Keyword.ChangeTracker.ChangeTrackingEnabled)
                {
                    Keyword.StartTracking();
                }
            }
        }
    
        private void FixupChannelGroups(ChannelGroup previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.KeywordMap.Contains(this))
            {
                previousValue.KeywordMap.Remove(this);
            }
    
            if (ChannelGroups != null)
            {
                if (!ChannelGroups.KeywordMap.Contains(this))
                {
                    ChannelGroups.KeywordMap.Add(this);
                }
    
                idChannelGroup = ChannelGroups.idGroup;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("ChannelGroups")
                    && (ChangeTracker.OriginalValues["ChannelGroups"] == ChannelGroups))
                {
                    ChangeTracker.OriginalValues.Remove("ChannelGroups");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("ChannelGroups", previousValue);
                }
                if (ChannelGroups != null && !ChannelGroups.ChangeTracker.ChangeTrackingEnabled)
                {
                    ChannelGroups.StartTracking();
                }
            }
        }

        #endregion
    }
}
