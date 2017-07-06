﻿namespace DDI.Shared.Statics
{
    /// <summary>
    /// Status strings used for ElasticSearch indexing of entities.
    /// </summary>
    public static class EntityStatus
    {
        public static string Deleted => "Deleted";
        public static string Active => "Active";
        public static string Expired => "Expired";
        public static string Approved => "Approved";
        public static string Unapproved => "Unapproved";
        public static string Posted => "Posted";
        public static string Unposted => "Unposted";
        public static string Reversed => "Reversed";
    }
}
