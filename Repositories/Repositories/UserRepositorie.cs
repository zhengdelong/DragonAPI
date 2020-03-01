using Domain;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories
{
    public class UserRepositorie : EFCoreBase<User>,IUserRepositories, IBaseRepository
    {
        public UserRepositorie(DragonDBContext dragonDBContext) : base(dragonDBContext)
        {

        }
    }
}
