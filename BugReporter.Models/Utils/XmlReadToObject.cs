using BugReporter.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BugReporter.Models.Utils
{
    public static class XmlReadToObject
    {

        public static UserList ReadToObjectUsers()
        {
            XmlReader xmlReader = XmlReader.Create("users.xml");

            UserList users = new UserList();

            while (xmlReader.Read())
            {

                if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "User"))
                {
                    if (xmlReader.HasAttributes)
                    {
                        User user = (User)Activator.CreateInstance(typeof(User));

                        user.GetType().InvokeMember("Username", BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty, Type.DefaultBinder, user, new object[] { xmlReader.GetAttribute("username") });
                        user.GetType().InvokeMember("Password", BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty, Type.DefaultBinder, user, new object[] { xmlReader.GetAttribute("password") });
                        user.GetType().InvokeMember("Type", BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty, Type.DefaultBinder, user, new object[] { (Enums.UserType)Enum.Parse(typeof(Enums.UserType), xmlReader.GetAttribute("type")) });

                        users.Users.Add(user);
                    }
                }
            }

            return users;
        }
    }
}
