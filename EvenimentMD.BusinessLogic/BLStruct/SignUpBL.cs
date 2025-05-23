﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvenimentMD.BusinessLogic.Core;
using EvenimentMD.BusinessLogic.DatabaseContext;
using EvenimentMD.BusinessLogic.Interface;
using EvenimentMD.Domain.Model;

namespace EvenimentMD.BusinessLogic.BLStruct
{
    public class SignUpBL : UserAPI, ISignUp
    {
        public string SignUpLogic(UserSignUpData data)
        {
            return UserSignUpLogic(data);
        }

        public bool EmailExists(string email)
        {
            return EmailExistsLogic(email);
        }

        public bool PhoneNumberExists(string phoneNumber)
        {
            return PhoneNumberExistsLogic(phoneNumber);
        }
    }
}
