﻿using WebAPI.Models.User;
using System.Threading.Tasks;
using System.Web.Http;
using WebAPI.Classes.UserWork;

namespace WebAPI.Controllers
{
    public class UserController : ApiController
    {
        public async Task<UserContainerModel> Post([FromBody]UserModelRequest value)
        {
            return await LoginCheck.CheckUser(value);
        }

        public async Task<UserModel> Get(int id)
        {
            return await new UserCreator().GetUserById(id);
        }
    }
}