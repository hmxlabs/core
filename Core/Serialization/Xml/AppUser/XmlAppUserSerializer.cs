using System;
using System.Xml;
using FaqatSafi.Core.AppUser;

namespace FaqatSafi.Core.Serialization.Xml.AppUser
{
    public class XmlAppUserSerializer : XmlSerializer<IAppUser>
    {
        public static class Tags
        {
            public const string Root = "AppUser";
            public const string FirstName = "firstName";
            public const string LastName = "lastName";
            public const string Id = "id";
            public const string Role = "role";
        }

        public override string TypeKey { get { return Tags.Root; } }

        public override void Serialize(IAppUser appUser_, XmlWriter writer_)
        {
            if (null == appUser_)
                throw new ArgumentNullException("appUser_");

            if (null == writer_)
                throw new ArgumentNullException("writer_");

            SerializeAppUser(appUser_, writer_);
        }

        public override IAppUser Deserialize(XmlReader reader_)
        {
            if (null == reader_)
                throw new ArgumentNullException("reader_");

            return DeserializeAppUser(reader_);
        }

        private void SerializeAppUser(IAppUser appUser_, XmlWriter writer_)
        {
            writer_.WriteStartElement(Tags.Root);
            
            if (!string.IsNullOrEmpty(appUser_.FirstName))
                writer_.WriteAttributeString(Tags.FirstName, appUser_.FirstName);

            if (!string.IsNullOrEmpty(appUser_.LastName))
                writer_.WriteAttributeString(Tags.LastName, appUser_.LastName);

            if (!string.IsNullOrEmpty(appUser_.Id))
                writer_.WriteAttributeString(Tags.Id, appUser_.Id);

            if (!string.IsNullOrEmpty(appUser_.Role))
                writer_.WriteAttributeString(Tags.Role, appUser_.Role);

            writer_.WriteEndElement();
        }

        private IAppUser DeserializeAppUser(XmlReader reader_)
        {
            reader_.MoveToContent();
            if (!reader_.IsStartElement())
                throw new XmlException("XmlReader not positioned on Xml Element. Unable to start parsing");

            if (!Tags.Root.Equals(reader_.Name))
                throw new XmlException("XmlReader positioned on an element that is not an AppUser");

            return ParseAppUserFromXml(reader_);
        }

        private IAppUser ParseAppUserFromXml(XmlReader reader_)
        {
            string id = null;
            string firstName = null;
            string lastName = null;
            string role = null;
            while (reader_.MoveToNextAttribute())
            {
                switch (reader_.Name)
                {
                    case Tags.Id:
                        id = reader_.Value;
                        break;

                    case Tags.FirstName:
                        firstName = reader_.Value;
                        break;

                    case Tags.LastName:
                        lastName = reader_.Value;
                        break;

                    case Tags.Role:
                        role = reader_.Value;
                        break;
                }
            }

            if (string.IsNullOrEmpty(id))
                throw new XmlException("Unable to parse ID for App User");

            return new ApplicationUser(id, firstName, lastName, role);
        }
    }
}
