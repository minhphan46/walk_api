﻿namespace WalkProject.Middlewares.Users
{
    public class UserAttribute : GlobalStateAttribute
    {
        public UserAttribute() : base(UserMiddleware.USER_CONTEXT_DATA_KEY)
        {
        }
    }
}
