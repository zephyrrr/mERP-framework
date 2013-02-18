/*
Copyright © 2005, Peter Kellner
All rights reserved.
http://peterkellner.net

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions
are met:

- Redistributions of source code must retain the above copyright
notice, this list of conditions and the following disclaimer.

- Neither Peter Kellner, nor the names of its
contributors may be used to endorse or promote products
derived from this software without specific prior written 
permission. 

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
"AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE 
COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES INCLUDING,
BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; 
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER 
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT 
LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
POSSIBILITY OF SUCH DAMAGE.
*/


using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.ComponentModel;


namespace MembershipUtilities
{

    

    /// <summary>
    /// Summary description for MembershipUserWrapper
    /// This class is inherited from MembershipUser 
    /// Using the sytax public class ClassName (..) : base(initializers...) allows for calling the
    /// contstructor of the base class.  In this case MembershipUser.
    /// </summary>
    /// 
    public class MembershipUserWrapper : MembershipUser
    {

        /// <summary>
        /// This constructor is used to create a MembershipUserWrapper from a MembershipUser object.  MembershipUser is a default type used
        /// in the Membership API provided with ASP.NET 2.0
        /// </summary>
        /// <param name="mu">MembershipUser object</param>
        public MembershipUserWrapper(MembershipUser mu)
            : base(mu.ProviderName, mu.UserName, mu.ProviderUserKey, mu.Email, mu.PasswordQuestion,
            mu.Comment, mu.IsApproved, mu.IsLockedOut, mu.CreationDate, mu.LastLoginDate, mu.LastActivityDate, 
            mu.LastPasswordChangedDate, mu.LastLockoutDate)
        {
        }


        /// <summary>
        /// This calls the base class UserName property.  It is here so we can tag
        /// this property as the primary key so that datakeynames attribute gets set in the data control.
        /// </summary>
        /// 
        [DataObjectField(true)]
        public override string UserName
        {
            get { return base.UserName; }
        }


        
        /// <summary>
        /// This constructor is used to create a MembershipUserWrapper from individual parameters values.  
        /// For details of what each parameter means, see the Microsoft Membership class.
        /// </summary>
        /// <param name="comment">Passes to MembershipUser.comment</param>
        /// <param name="creationDate">Passes to MembershipUser.creationDate</param>
        /// <param name="email">Passes to MembershipUser.email</param>
        /// <param name="isApproved">Passes to MembershipUser.isApproved</param>
        /// <param name="lastActivityDate">Passes to MembershipUser.lastActivityDate</param>
        /// <param name="lastLoginDate">Passes to MembershipUser.lastLoginDate</param>
        /// <param name="passwordQuestion">Passes to MembershipUser.passwordQuestion</param>
        /// <param name="providerUserKey">Passes to MembershipUser.providerUserKey</param>
        /// <param name="userName">Passes to MembershipUser.userName</param>
        /// <param name="lastLockoutDate">Passes to MembershipUser.lastLockoutDate</param>
        /// <param name="providerName">Passes to MembershipUser.providerName</param>
        /// 
        public MembershipUserWrapper(string comment, DateTime creationDate, string email,
                    bool isApproved, DateTime lastActivityDate, DateTime lastLoginDate,
                    string passwordQuestion, object providerUserKey, string userName,
                    DateTime lastLockoutDate, string providerName)
            : base(providerName, userName, providerUserKey, email, passwordQuestion, comment,
            isApproved, false, creationDate, lastLoginDate, lastActivityDate, DateTime.Now, lastLockoutDate)
        {
            // This calls a constructor of MembershipUser automatically because of the base reference above
        }

    }

}

