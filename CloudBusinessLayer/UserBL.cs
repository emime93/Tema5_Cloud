using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudModel;

namespace CloudBusinessLayer
{
    public class UserBL
    {
        public bool SignUp(User user)
        {
            using (var context = new CloudModel.LeModelContainer())
            {
                if (!user.Username.Trim().Equals("") ||
                    !user.Password.Trim().Equals(""))
                {
                    user.Status = true;
                    context.Users.Add(user);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public bool SignIn(User user)
        {
            using (var context = new CloudModel.LeModelContainer())
            {
                if (!user.Username.Trim().Equals("") ||
                   !user.Password.Trim().Equals(""))
                {                  
                    foreach (var user1 in context.Users)
                    {
                        if (user1.Username.Equals(user.Username) &&
                            user1.Password.Equals(user.Password))
                        {
                            user1.Status = true;
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public bool SignOut(User user)
        {
            using (var context = new CloudModel.LeModelContainer())
            {
                if (!user.Username.Trim().Equals("") ||
                   !user.Password.Trim().Equals(""))
                {
                    foreach (var user1 in context.Users)
                    {
                        if (user1.Username.Equals(user.Username) &&
                            user1.Password.Equals(user.Password))
                        {
                            user1.Status = false;
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public bool AddDocumentToUser(Document doc, User user)
        {
            user.Documents.Add(doc);
            return true;
        }

        public ICollection<Document> GetDocumentsForUser(User user)
        {
            return user.Documents;
        }
    }
}
