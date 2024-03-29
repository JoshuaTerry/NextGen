﻿using DDI.EFAudit.Exceptions;
using DDI.Shared.Models.Client.Audit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DDI.EFAudit.History
{
    public class Change<TValue, TPrincipal> : IChange<TValue, TPrincipal>
    {
        public TValue Value { get; private set; }
        public IObjectChange<TPrincipal> ObjectChange { get; private set; }

        public Change(TValue value, IObjectChange<TPrincipal> objectChange, IEnumerable<Exception> errors = null)
        {
            Value = value;
            ObjectChange = objectChange;
            Errors = errors ?? new List<Exception>();
        }

        public DateTime Timestamp
        {
            get
            {
                { return ObjectChange.ChangeSet.Timestamp; }
            }
        }
        public TPrincipal User
        {
            get
            {
                return ObjectChange.ChangeSet.User;
            }
        }
        
        public IEnumerable<Exception> Errors { get; private set; }
        public bool ProblemsRetrievingData
        {
            get
            {
                return Errors.Any();
            }
        }

        public override bool Equals(object obj)
        {
            var other = obj as Change<TValue, TPrincipal>;

            if (other == null)
                return false;

            return object.Equals(User, other.User)
                && object.Equals(Timestamp, other.Timestamp)
                && object.Equals(Value, other.Value);                
        }
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
        public override string ToString()
        {
            return string.Format("{0}:{1}:{2}", User, Timestamp, Value);
        }
    }

    /// <summary>
    /// Static helper methods for the generic Change type
    /// </summary>
    public static class Change
    {
        public static Change<TValue, TPrincipal> FromObjectChange<TValue, TPrincipal>(TValue value, 
            IObjectChange<TPrincipal> objectChange, IEnumerable<Exception> errors = null)
        {
            var changeSet = objectChange.ChangeSet;

            if (changeSet == null)
            {
                throw new InvalidAuditLogRecordException("IObjectChange '{0}'has a null ChangeSet property", objectChange);
            }

            return new Change<TValue, TPrincipal>(value, objectChange, errors: errors);
        }
    }
}