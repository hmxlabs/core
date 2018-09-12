using System;
using System.Xml;
using FaqatSafi.Core.Base;
using FaqatSafi.Core.AppUser;

namespace FaqatSafi.Core.Serialization.Xml.AppUser
{
    public static class XmlActionLogSerializer
    {
        public class Tags
        {
            public const string Root = "ActionLog";
            public const string UserId = "userId";
            public const string Timestamp = "timestamp";
        }

        public static void Serialise(IActionLog actionLog_, XmlWriter writer_)
        {
            Serialise(actionLog_, writer_, Tags.Root);
        }

        public static void Serialise(IActionLog actionLog_, XmlWriter serialiser_, string rootTag_)
        {
            if (null == serialiser_) { return; } // if no data then nothing to serialise, just return
            if (null == actionLog_) { throw new ArgumentNullException("actionLog_"); }
            if (string.IsNullOrEmpty(rootTag_)) { throw new ArgumentNullException("rootTag_"); }

            serialiser_.WriteStartElement(rootTag_);
            serialiser_.WriteAttributeString(Tags.UserId, actionLog_.UserId);
            serialiser_.WriteAttributeString(Tags.Timestamp, actionLog_.Timestamp.ToIsoDateTimeString());
            serialiser_.WriteEndElement();
        }

        public static ActionLog Deserialise(XmlReader reader_)
        {
            if (null == reader_) { throw new ArgumentNullException("reader_"); }

            reader_.MoveToContent();
            var elemName = reader_.Name;
            if (string.IsNullOrEmpty(elemName))
                { throw new XmlException("Attempt to deserialise an ActionLog without a root element name"); }

            var userId = reader_.GetAttribute(Tags.UserId);
            var timestamp = FsDateTime.ParseIsoDateTimeString(reader_.GetAttribute(Tags.Timestamp));

            return new ActionLog(userId, timestamp);
        }
    }
}
