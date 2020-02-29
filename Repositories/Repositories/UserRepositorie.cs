using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories
{
    public class UserRepositorie : EFCoreBase<User>,IUserRepositories
    {
        public UserRepositorie(DragonDBContext dragonDBContext) : base(dragonDBContext)
        {

        }
    }
}
